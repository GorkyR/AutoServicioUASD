using System;
using System.Net;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using UASD.Properties;
using HtmlAgilityPack;

namespace UASD
{
	using System.Threading.Tasks;
	using Utilities;
	public partial class Client
	{
		public string Username { get; protected set; }
        public bool IsLoggedIn { get; protected set; } = false;

        public async Task<CourseCollection>       FetchScheduleDetailAsync()
		{
			Task<HtmlDocument> response = ReceiveAsync(Strings.HorarioDetalleURL);
			CourseCollection schedule = new CourseCollection("Current Schedule");
			HtmlDocument responsePage = await response;
            responsePage = await SelectActivePeriod(responsePage, Strings.HorarioDetalleURL, true);

            IList<HtmlNode> displayTables = responsePage.GetElementsByClass("datadisplaytable");
            if (displayTables.Count < 2)
                throw new NoDataReceivedException();

            for (int i = 0; i < displayTables.Count; i += 2)
            {
                string[] identification = displayTables[i].ChildNodes[0].InnerText.Split(" - ");
                string[] details = displayTables[i]
                    .GetElementsByTagName("td")
                    .GetElementsWithAttribute("class", "dddefault")
                    .Select(j => j.InnerText.Trim())
                    .ToArray();

                Course course = new Course()
                {
                    Title = identification[0],
                    Code = identification[1],
                    Section = identification[2],
                    NRC = details[1],
                    Professor = Convert.SimpleEncodeFix(details[3]),
                    Credits = Convert.Credits(details[5])
                };

                IEnumerable<HtmlNode> scheduleRows = displayTables[i + 1].GetElementsByTagName("tr").Skip(1);
                foreach (HtmlNode scheduleRow in scheduleRows)
                {
                    string[] rowCells =
                        (from cell in scheduleRow.GetElementsByTagName("td")
                         select cell.InnerText)
                        .ToArray();
                    foreach (char day in rowCells[2])
                    {
                        string[] hours = rowCells[1].Split('-');
                        string[] boundDates = rowCells[4].Split('-');
                        try
                        {
                            course.Schedule.Add(new CourseClass()
                            {
                                Place     = rowCells[3],
                                DayOfWeek = Convert.Days[day],
                                StartTime = Convert.Time(hours[0]),
                                EndTime   = Convert.Time(hours[1]),
                                StartDate = Convert.Date(boundDates[0]),
                                EndDate   = Convert.Date(boundDates[1]),
                            });
                        }
                        catch { }
                    }
                }

                schedule.Add(course);
            }

            return schedule;
		}
		public async Task<CourseCollection>       FetchCourseProjectionAsync()
		{
			Task<HtmlDocument> response = ReceiveAsync(Strings.ProyeccionUrl);
			CourseCollection projection = new CourseCollection("Proyection");
			HtmlDocument responsePage = await response;
            responsePage = await SelectActivePeriod(responsePage, Strings.ProyeccionUrl);
			if (responsePage.GetTitle().Contains("Period"))
				throw new NoProyectionAvailableException();

			IEnumerable<HtmlNode> tableRows =
				(from node in responsePage.GetElementsByTagName("tr")
				 where node.ParentNode.HasClass("datadisplaytable")
				 select node)
                .Skip(1);

			foreach (HtmlNode tableRow in tableRows)
			{
				string[] courseInfo =
					(from node in tableRow.ChildNodes
					 where !string.IsNullOrWhiteSpace(node.InnerText)
					 select node.InnerText)
                    .ToArray();

				projection.Add(new Course()
				{
					Title   = courseInfo[2],
					Code    = $"{courseInfo[0]} {courseInfo[1]}",
					Credits = Convert.Credits(courseInfo[5])
				});
			}

			return projection;
		}
		public async Task<AcademicReport>         FetchAcademicReportAsync()
		{
			Task<HtmlDocument> response = SubmitAsync(Strings.KardexUrl, "levl=&tprt=INTL");
			AcademicReport historial = new AcademicReport();
			HtmlDocument ResponsePage = await response;

			IList<HtmlNode> tableRowNodes =
				(from node in ResponsePage.GetElementsByTagName("tr")
				 where node.ParentNode.HasClass("datadisplaytable")
				 select node)
                .ToList();

			// Used for separation:
			IEnumerable<HtmlNode> orangeRows =
				from node in ResponsePage.GetElementsByClass("fieldOrangetextbold")
				select node.ParentNode.ParentNode;

			// Set an index to separate inactive courses from courses in progress.
			IList<HtmlNode> incompletePeriods = ResponsePage
				.GetElementsByTagName("a")
				.GetElementsWithAttribute("name", "crses_progress");

			int completedPeriodsLimit =
				incompletePeriods.Count > 0 ?
					  tableRowNodes.IndexOf(incompletePeriods[0].ParentNode.ParentNode)
					  : tableRowNodes.Count;

			foreach (HtmlNode periodStartNode in orangeRows)
			{
				string periodName = periodStartNode.InnerText.Trim().Replace("Period: ", "");
				int periodIndex = tableRowNodes.IndexOf(periodStartNode);
				if (periodIndex < completedPeriodsLimit)
				{
					while (tableRowNodes[periodIndex].ChildNodes.Count < 20)
						periodIndex++;
					AcademicPeriod period = new AcademicPeriod(periodName)
                        { IsSummerCourse = periodName.Contains("Verano") };

					for (; tableRowNodes[periodIndex].ChildNodes.Count > 19; periodIndex++)
					{
						string[] data =
							tableRowNodes[periodIndex]
							.GetElementsByClass("dddefault")
							.Select(i => i.InnerText)
							.ToArray();
						GradedCourse course = new GradedCourse()
						{
							Code = $"{data[0]} {data[1]}",
							Title = data[4],
							Credits = Convert.Credits(data[6])
						};

						if (data[5].Trim() == "AUS")
							course.State = GradedCourse.CourseState.Absent;
						else if (string.IsNullOrWhiteSpace(data[5]))
							course.State = GradedCourse.CourseState.NotPublished;
						else if (data[5].Trim()[0] == 'L')
						{
							course.Grade = Convert.Grade(data[5]);
							course.Credits = 0;
						}
						else
							course.Grade = Convert.Grade(data[5]);
						period.Courses.Add(course);
					}
					historial.Periods.Add(period);
				}
				else
				{
					while (tableRowNodes[periodIndex].ChildNodes.Count < 13)
                        periodIndex++;
					periodIndex++;

					AcademicPeriod periodo =
                        historial.Periods.FirstOrDefault(p => p.Title == periodName) ??
                        new AcademicPeriod(periodName)
                            { IsSummerCourse = periodName.Contains("Verano") };

					for (; tableRowNodes.Count > periodIndex && tableRowNodes[periodIndex].ChildNodes.Count > 12; periodIndex++)
					{
						string[] data = tableRowNodes[periodIndex]
							.GetElementsByClass("dddefault")
							.Select(i => i.InnerText)
							.ToArray();
						periodo.Courses.Add(new GradedCourse()
						{
							Code = data[0] + ' ' + data[1],
							Title = data[4],
							Credits = Convert.Credits(data[5]),
							State = GradedCourse.CourseState.NotPublished
						});
					}
					if (!historial.Periods.Contains(periodo))
						historial.Periods.Add(periodo);
				}
			}

            for (int i = 0; i < historial.Periods.Count; i++)
                if (historial.Periods[i].IsActive)
                    historial.Periods[i] = await FetchGradesAsync(Convert.PeriodCode(historial.Periods[i].Title));

			return historial;
		}
		public async Task<List<CourseCollection>> FetchAvailableCoursesAsync()
		{
			Task<HtmlDocument> response = ReceiveAsync(Strings.BuscarClasesSelectionUrl);
			List<CourseCollection> secciones = new List<CourseCollection>();
			HtmlDocument responsePage = await response;

			string term = responsePage
				.GetElementbyId("term_input_id")
				.GetElementsByTagName("option")
				.Select(i => i.GetAttribute("value"))
				.First(i => i.LastOrDefault().IsEither('0', '5'));

			responsePage = await SubmitAsync(Strings.BuscarClasesSelection2Url, $"p_calling_proc=P_CrseSearch&p_term={term}");

			IEnumerable<string> subjects = responsePage
				.GetElementbyId("subj_id")
				.GetElementsByTagName("option")
				.Select(i => i.GetAttributeValue("value", "-"));

			string campus = responsePage
				.GetElementsByTagName("input")
				.Where(i => i.GetAttribute("name") == "sel_camp")
				.ToArray()[1]
				.GetAttribute("value");

			string dataString = $"term_in={term}";
			foreach (string subj in subjects)
				dataString += $"&sel_subj={subj}";
			dataString += $"&sel_camp={campus}";
			dataString += Strings.BuscarClasesDummyParameters;

			try { responsePage = await SubmitAsync(Strings.BuscarClasesUrl, dataString); }
			catch (NoDataReceivedException) { throw new NoSelectionAvailableException(); }

			foreach (HtmlNode curso in responsePage.GetElementsByClass("datadisplaytable"))
			{
				string[][] info = curso
					.GetElementsByTagName("td")
					.Select(i => i
						.GetElementsByTagName("input")
						.Where(j => j.GetAttribute("name").ToLower().IsEither("sel_subj", "sel_crse"))
						.Skip(1)
						.Select(j => j.GetAttribute("value"))
						.ToArray())
					.Where(i => i.Length > 0)
					.ToArray();

				foreach (string[] matInfo in info)
				{
					try
                    {
                        var collection = await FetchOpenCoursesAsync(term, matInfo[0], matInfo[1]);
                        secciones.Add(collection);
                    }
					catch (NoDataReceivedException)
                    {
                        continue;
                    }
				}
			}

            if (secciones.Count == 0)
                throw new NoDataReceivedException();
			return secciones;
		}
		public async Task<CareerInformation>      FetchCareerInformationAsync()
		{
			HtmlDocument ResponsePage = await SubmitAsync(Strings.EvaluacionSelectionUrl,
				$"term_in={DateTime.Now.Year}{(DateTime.Now.Month >= 6 ? "20" : "10")}");

			HtmlNode reportLink = ResponsePage
				.GetElementsByClass("datadisplaytable")[0]
				.GetElementsByTagName("tr")[1]
				.GetElementsByTagName("a")[0];
			CareerInformation infoCarrera = new CareerInformation() { Name = Convert.SimpleEncodeFix(reportLink.InnerText) };

			ResponsePage = await ReceiveAsync(new Uri(BaseUri, reportLink.GetAttribute("href")));
			int[] infoReq = ResponsePage
				.GetElementsByClass("datadisplaytable").ToList()[1]
				.GetElementsByTagName("tr").ToList()[2]
				.GetElementsByTagName("td")
				.Skip(1)
				.Select(i => (int)float.Parse(i.InnerText, NumberStyles.Any, CultureInfo.InvariantCulture))
				.ToArray();

			infoCarrera.RequiredCredits = infoReq[0];
			infoCarrera.Credits = infoReq[1];
			infoCarrera.RequiredCourses = infoReq[2];
			infoCarrera.Courses = infoReq[3];

			return infoCarrera;
		}

		public async Task RegisterCoursesAsync(params string[] NRCs)
		{
			HtmlDocument ResponsePage = await ReceiveAsync(Strings.RegistroSeleccionUrl);
			try
			{
				string period = ResponsePage.GetElementsByTagName("option").Select(o => o.GetAttribute("value")).First(v => v.LastOrDefault().IsEither('0', '5'));
				ResponsePage = await SubmitAsync(Strings.RegistroSeleccionUrl, $"term_in={period}");
			}
			catch (InvalidOperationException) { }
			IList<HtmlNode> hiddenInputs = ResponsePage
				.GetElementsWithAttribute("name")
				.Where(i => i.HasAttribute("type", "hidden") || string.Equals(i.Name, "select", StringComparison.CurrentCultureIgnoreCase))
				.ToList();

			string dataString = string.Empty;
			try
			{
				foreach (HtmlNode input in hiddenInputs.TakeWhile(i => i.GetAttribute("value") != "RW"))
					dataString += $"{ input.GetAttribute("name") }={ WebUtility.UrlEncode(input.GetAttribute("value")) }&";
				foreach (string nrc in NRCs)
					dataString += $"RSTS_IN=RW&CRN_IN={nrc}&assoc_term_in=&start_date_in=&end_date_in=&";
				dataString += $"regs_row={ hiddenInputs.Reverse().ToList()[2].GetAttribute("value") }&wait_row=0&add_row=10&REG_BTN=Enviar+Cambios";
			}
			catch (ArgumentOutOfRangeException) { throw new NoSelectionAvailableException(); }

			ResponsePage = await SubmitAsync(Strings.RegistroUrl, dataString);

			HtmlNode ErrorTable = ResponsePage.GetElementsByClass("datadisplaytable").Where(n => n.GetAttribute("summary").Contains("Error")).FirstOrDefault();
			if (ErrorTable != null)
			{
				string ErrorMessage = string.Empty;
				foreach (HtmlNode row in ErrorTable.GetElementsByTagName("tr").Skip(1))
				{
					string[] data = row.GetElementsByTagName("td").Select(d => d.InnerText).ToArray();
					ErrorMessage += $"\n\t{data[0]}: {data[2]} {data[3]} - {data[8]}";
				}
				throw new SeleccionErrorsException(ErrorMessage);
			}
		}

        private async Task<AcademicPeriod>   FetchGradesAsync(string periodoOption)
        {
            Task <HtmlDocument> response= SubmitAsync(Strings.CalificacionesUrl, $"term_in={periodoOption}");
            AcademicPeriod period = new AcademicPeriod()
            {
                Title = Convert.PeriodTitle(periodoOption),
                IsSummerCourse = periodoOption.Last() == '5'
            };
            HtmlDocument ResponsePage = await response;

            IList<HtmlNode> displayTables = ResponsePage.GetElementsByClass("datadisplaytable");
            if (displayTables.Count < 2)
                throw new NoDataReceivedException();

            string[][] tableData = displayTables[1].GetElementsByTagName("tr")
                .Skip(1)
                .Select(i => i.GetElementsByClass("dddefault")
                    .Select(j => WebUtility.HtmlDecode(j.InnerText))
                    .ToArray())
                .ToArray();

            foreach (string[] materiaData in tableData)
            {
                GradedCourse course = new GradedCourse()
                {
                    NRC = materiaData[0],
                    Code = materiaData[1] + ' ' + materiaData[2],
                    Section= materiaData[3],
                    Title = materiaData[4],
                    Credits = Convert.Credits(materiaData[7])
                };
                if (materiaData[6].Trim() == "AUS")
                    course.State = GradedCourse.CourseState.Absent;
                else if (string.IsNullOrWhiteSpace(materiaData[6]))
                    course.State = GradedCourse.CourseState.NotPublished;
                else
                    course.Grade= Convert.Grade(materiaData[6]);
                period.Courses.Add(course);
            }

            return period;
        }
        private async Task<CourseCollection> FetchOpenCoursesAsync(string term, string subject, string course)
		{
			string dataString = $"term_in={term}&sel_subj=&sel_subj={subject}&sel_crse={course}";
			dataString += Strings.ClasesDummyParameters;

			HtmlDocument ResponsePage = await SubmitAsync(Strings.BuscarClasesUrl, dataString);

			IEnumerable<HtmlNode> dataRows = ResponsePage
				.GetElementsByClass("datadisplaytable").FirstOrDefault()
				?.GetElementsByTagName("tr");
			if (dataRows is null || dataRows.Count() == 0)
				throw new NoDataReceivedException();

			string[][] info = dataRows
				.Skip(2)
				.Select(i => i
					.GetElementsByTagName("td")
					.Skip(1)
					.Select(j => WebUtility.HtmlDecode(j.InnerText).Trim())
					.ToArray())
				.ToArray();

			CourseCollection secciones = new CourseCollection() { Name = Convert.SimpleEncodeFix(info[0][6]) };

			for (int i = 0; i < info.Length; i++)
			{
				OpenCourse seccion = new OpenCourse()
				{
					NRC = info[i][0],
					Code = info[i][1] + " " + info[i][2],
					Section = info[i][3],
					Credits = Convert.Credits(info[i][5]),
					Title = info[i][6],
					Capacity = int.Parse(info[i][9], NumberStyles.Any, CultureInfo.InvariantCulture),
					Vacancy = int.Parse(info[i][11], NumberStyles.Any, CultureInfo.InvariantCulture),
				};
				for (; ; i++)
				{
					foreach (char dia in info[i][7])
					{
						string[] horas = info[i][8].Split('-');
						string[] fechas = info[i][12].Split('-');
						seccion.Schedule.Add(new CourseClass()
						{
							DayOfWeek = Convert.Days[dia],
							StartTime = Convert.Time(horas[0]),
							EndTime = Convert.Time(horas[1]),
							StartDate = Convert.Date(fechas[0]),
							EndDate = Convert.Date(fechas[1]),
							Place = info[i][13]
						});
					}
					if (i + 1 == info.Length || !String.IsNullOrWhiteSpace(info[i + 1][0]))
						break;
				}
				secciones.Add(seccion);
			}

			return secciones;
		}

        private async Task<HtmlDocument> SelectActivePeriod(HtmlDocument page, string callbackURL, bool retry = false)
        {
            if (page.GetTitle().Contains("Period"))
            {
                string[] periodCodes =
                    (from option in page.GetElementsByTagName("option")
                     where option.ParentNode.Name.Matches("select")
                     select option.GetAttribute("value"))
                    .Where(i => i.LastOrDefault().IsEither('0', '5')).ToArray();

                page = await SubmitAsync(callbackURL, $"term_in={periodCodes[0]}");

                if (retry)
                {
                    IList<HtmlNode> displayTables = page.GetElementsByClass("datadisplaytable");
                    if (displayTables.Count < 2)
                        page = await SubmitAsync(callbackURL, $"term_in={periodCodes[1]}");
                }
            }
            return page;
        }
	}
}
