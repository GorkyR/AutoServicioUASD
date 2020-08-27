using System;
using System.Linq;
using System.Collections.Generic;
using UASD;
using Convert = UASD.Utilities.Convert;

namespace Client.Droid.TestEnvironment
{
	public static class TestData
	{
		public static int generated = 0;

		public static CourseCollection  fakePreloadedSchedule = new CourseCollection {
		new Course() {
			Title     = "Base de Datos I",
			Code      = "INF 4200",
			Section   = "W01",
			NRC       = "76025",
			Professor = "Romery Alberto Monegro",
			Credits   = 5,
		},
		new Course() {
			Title     = "Teoría De Los Compiladores",
			Code      = "INF 4220",
			Section   = "02",
			NRC       = "46396",
			Professor = "Rafael Salvador Escarraman",
			Credits   = 4,
			Schedule  = {
				new CourseClass() {
					Place     = "LABORATORIO DE INFORMATICA 103",
					DayOfWeek = DayOfWeek.Thursday,
					StartTime = Convert.Time("8:00 PM"),
					EndTime   = Convert.Time("9:50 PM"),
					StartDate = Convert.Date("Ene 29, 2018"),
					EndDate   = Convert.Date("May 20, 2018")
				},
				new CourseClass() {
					Place     = "LABORATORIO DE INFORMATICA 102",
					DayOfWeek = DayOfWeek.Saturday,
					StartTime = Convert.Time("5:00 PM"),
					EndTime   = Convert.Time("7:50 PM"),
					StartDate = Convert.Date("Ene 29, 2018"),
					EndDate   = Convert.Date("May 20, 2018")
				}
			}
		},
		new Course() {
			Title     = "Teleproceso",
			Code      = "INF 4250",
			Section   = "03",
			NRC       = "59798",
			Professor = "Rafael Salvador Escarraman",
			Credits   = 4,
			Schedule  = {
				new CourseClass() {
					Place     = "LABORATORIO DE INFORMATICA 107",
					DayOfWeek = DayOfWeek.Monday,
					StartTime = Convert.Time("5:00 PM"),
					EndTime   = Convert.Time("6:50 PM"),
					StartDate = Convert.Date("Ene 29, 2018"),
					EndDate   = Convert.Date("May 20, 2018")
				},
				new CourseClass() {
					Place     = "LABORATORIO DE INFORMATICA 101",
					DayOfWeek = DayOfWeek.Thursday,
					StartTime = Convert.Time("5:00 PM"),
					EndTime   = Convert.Time("6:50 PM"),
					StartDate = Convert.Date("Ene 29, 2018"),
					EndDate   = Convert.Date("May 20, 2018")
				},
				new CourseClass() {
					Place     = "LABORATORIO DE INFORMATICA 104",
					DayOfWeek = DayOfWeek.Thursday,
					StartTime = Convert.Time("7:00 PM"),
					EndTime   = Convert.Time("7:50 PM"),
					StartDate = Convert.Date("Ene 29, 2018"),
					EndDate   = Convert.Date("May 20, 2018")
				},
			}
		},
		new Course() {
			Title     = "Redes De Proc De Datos",
			Code      = "INF 4260",
			Section   = "02",
			NRC       = "46410",
			Professor = "Rafael Salvador Escarraman",
			Credits   = 4,
			Schedule  = {
				new CourseClass() {
					Place     = "LABORATORIO DE INFORMATICA 105",
					DayOfWeek = DayOfWeek.Monday,
					StartTime = Convert.Time("7:00 PM"),
					EndTime   = Convert.Time("9:50 PM"),
					StartDate = Convert.Date("Ene 29, 2018"),
					EndDate   = Convert.Date("May 20, 2018")
				},
				new CourseClass() {
					Place     = "LABORATORIO DE INFORMATICA 102",
					DayOfWeek = DayOfWeek.Tuesday,
					StartTime = Convert.Time("8:00 PM"),
					EndTime   = Convert.Time("9:50 PM"),
					StartDate = Convert.Date("Ene 29, 2018"),
					EndDate   = Convert.Date("May 20, 2018")
				},
			}
		},
		new Course() {
			Title     = "Lenguaje de Programación III",
			Code      = "INF 5160",
			Section   = "11",
			NRC       = "84447",
			Professor = "Angel Asencio M",
			Credits   = 4,
			Schedule  = {
				new CourseClass() {
					Place     = "LOCAL SAN CRISTOBAL EDIFICIO 2 101",
					DayOfWeek = DayOfWeek.Tuesday,
					StartTime = Convert.Time("2:00 PM"),
					EndTime   = Convert.Time("5:50 PM"),
					StartDate = Convert.Date("Ene 29, 2018"),
					EndDate   = Convert.Date("May 20, 2018")
				},
			}
		},
		new Course() {
			Title     = "Lab Lenguaje de Program III",
			Code      = "INF 5170",
			Section   = "11",
			NRC       = "84448",
			Professor = "Angel Asencio M",
			Credits   = 4,
			Schedule  = {
				new CourseClass() {
					Place     = "LABORATORIO INFORMAT CURSCEN 101",
					DayOfWeek = DayOfWeek.Tuesday,
					StartTime = Convert.Time("12:00 PM"),
					EndTime   = Convert.Time("1:50 PM"),
					StartDate = Convert.Date("Ene 29, 2018"),
					EndDate   = Convert.Date("May 20, 2018")
				},
			}
		},
		new Course() {
			Title     = "Cálculo II",
			Code      = "AMT 3570",
			Section   = "03",
			NRC       = "87046",
			Professor = "Juan Manzueta Concepcion",
			Credits   = 5,
			Schedule  = {
				new CourseClass() {
					Place     = "MAXIMO AVILES BLONDA 205",
					DayOfWeek = DayOfWeek.Wednesday,
					StartTime = Convert.Time("10:00 AM"),
					EndTime   = Convert.Time("12:50 PM"),
					StartDate = Convert.Date("Ene 29, 2018"),
					EndDate   = Convert.Date("May 20, 2018")
				},
				new CourseClass() {
					Place     = "CIENCIAS MODERNAS 206",
					DayOfWeek = DayOfWeek.Thursday,
					StartTime = Convert.Time("10:00 AM"),
					EndTime   = Convert.Time("12:50 PM"),
					StartDate = Convert.Date("Ene 29, 2018"),
					EndDate   = Convert.Date("May 20, 2018")
				},
			}
		},
	};
		public static CareerInformation fakePreloadedCareerInformation = new CareerInformation()
		{
			Name = "Informática",
			RequiredCredits = 189,
			Credits = 160,
			RequiredCourses = 52,
			Courses = 46
		};
		public static AcademicReport    fakePreloadedAcademicReport = new AcademicReport()
		{
			Periods = {
			new AcademicPeriod("Primer Semestre 2015") {
				Courses = {
					new GradedCourse() {
						Title = "Int A La Filosofía",
						Code = "FIL 0110",
						State = GradedCourse.CourseState.Absent,
						Credits = 3
					},
					new GradedCourse() {
						Title = "Física Básica",
						Code = "FIS 0180",
						Grade = 85,
						State = GradedCourse.CourseState.Published,
						Credits = 3
					},
					new GradedCourse() {
						Title = "Laboratorio de Física Básica",
						Code = "FIS 0200",
						Grade = 95,
						State = GradedCourse.CourseState.Published,
						Credits = 1
					},
					new GradedCourse() {
						Title = "Lengua Española Básica I",
						Code = "LET 0110",
						Grade = 95,
						State = GradedCourse.CourseState.Published,
						Credits = 3
					},
					new GradedCourse() {
						Title = "Matemática Básica",
						Code = "MAT 0140",
						Grade = 92,
						State = GradedCourse.CourseState.Published,
						Credits = 4
					},
					new GradedCourse() {
						Title = "Orient Institucional y Académ",
						Code = "OSI 0310",
						State = GradedCourse.CourseState.Absent,
						Credits = 2
					},
					new GradedCourse() {
						Title = "Introd A Las Ciencias Sociales",
						Code = "SOC 0100",
						Grade = 90,
						State = GradedCourse.CourseState.Published,
						Credits = 3
					}
				}
			},
			new AcademicPeriod("Verano 2015") {
				IsSummerCourse = true,
				Courses = {
					new GradedCourse() {
						Title = "Educación Física",
						Code = "EFI 0120",
						Grade = 98,
						State = GradedCourse.CourseState.Published,
						Credits = 2
					},
					new GradedCourse() {
						Title = "Int A La Filosofía",
						Code = "FIL 0110",
						Grade = 85,
						State = GradedCourse.CourseState.Published,
						Credits = 3
					},
					new GradedCourse() {
						Title = "Fund De His Social Dominicana",
						Code = "HIS 0110",
						State = GradedCourse.CourseState.Absent,
						Credits = 3
					}
				}
			},
			new AcademicPeriod("Segundo Semestre 2015") {
				Courses = {
					new GradedCourse() {
						Title = "Biología Básica",
						Code = "BIO 0170",
						Grade = 85,
						State = GradedCourse.CourseState.Published,
						Credits = 2
					},
					new GradedCourse() {
						Title = "Laboratorio de Biología Básica",
						Code = "BIO 0180",
						Grade = 93,
						State = GradedCourse.CourseState.Published,
						Credits = 1
					},
					new GradedCourse() {
						Title = "Lengua Española Básica II",
						Code = "LET 0120",
						State = GradedCourse.CourseState.Absent,
						Credits = 3
					},
					new GradedCourse() {
						Title = "Orient Institucional y Académ",
						Code = "OSI 0310",
						Grade = 97,
						State = GradedCourse.CourseState.Published,
						Credits = 2
					},
					new GradedCourse() {
						Title = "LAB Química Básica",
						Code = "QUI 0140",
						Type = Course.CourseType.Laboratory,
						Grade = 24,
						Credits = 3
					},
					new GradedCourse() {
						Title = "Química Básica",
						Code = "QUI 0140",
						Grade = 76,
						State = GradedCourse.CourseState.Published,
						Credits = 3
					}
				}
			},
			new AcademicPeriod("Primer Semestre 2016") {
				Courses = {
					new GradedCourse() {
						Title = "Fund De His Social Dominicana",
						Code = "HIS 0110",
						State = GradedCourse.CourseState.Absent,
						Credits = 3
					},
					new GradedCourse() {
						Title = "Inglés Elemental",
						Code = "IDI 0280",
						Grade = 97,
						State = GradedCourse.CourseState.Published,
						Credits = 5
					},
					new GradedCourse() {
						Title = "Computación Esencial",
						Code = "INF 2060",
						Grade = 93,
						State = GradedCourse.CourseState.Published,
						Credits = 4
					},
					new GradedCourse() {
						Title = "Lab de Computación Esencial",
						Code = "INF 2070",
						Grade = 85,
						State = GradedCourse.CourseState.Published,
						Credits = 1
					},
					new GradedCourse() {
						Title = "Análisis Lineal Y Matricial",
						Code = "MAT 2330",
						Grade = 77,
						State = GradedCourse.CourseState.Published,
						Credits = 5
					}
				}
			},
			new AcademicPeriod("Verano 2016") {
				IsSummerCourse = true,
				Courses = {
					new GradedCourse() {
						Title = "Inglés Técnico",
						Code = "IDI 0130",
						Grade = 100,
						State = GradedCourse.CourseState.Published,
						Credits = 4
					},
					new GradedCourse() {
						Title = "Lengua Española Básica II",
						Code = "LET 0120",
						Grade = 90,
						State = GradedCourse.CourseState.Published,
						Credits = 3
					}
				}
			},
			new AcademicPeriod("Segundo Semestre 2016") {
				Courses = {
					new GradedCourse() {
						Title = "Bioética",
						Code = "BIO 2400",
						Grade = 88,
						State = GradedCourse.CourseState.Published,
						Credits = 2
					},
					new GradedCourse() {
						Title = "Estadística Básica",
						Code = "EST 2110",
						Grade = 84,
						State = GradedCourse.CourseState.Published,
						Credits = 5
					},
					new GradedCourse() {
						Title = "Fund De His Social Dominicana",
						Code = "HIS 0110",
						Grade = 90,
						State = GradedCourse.CourseState.Published,
						Credits = 3
					},
					new GradedCourse() {
						Title = "Organiz y Arq del Computador",
						Code = "INF 2080",
						Grade = 88,
						State = GradedCourse.CourseState.Published,
						Credits = 4
					},
					new GradedCourse() {
						Title = "Introducción a la Programación",
						Code = "INF 5100",
						Grade = 95,
						State = GradedCourse.CourseState.Published,
						Credits = 4
					},
					new GradedCourse() {
						Title = "Lab. de Introd a la Programaci",
						Code = "INF 5110",
						Grade = 95,
						State = GradedCourse.CourseState.Published,
						Credits = 1
					}
				}
			},
			new AcademicPeriod("Primer Semestre 2017") {
				Courses = {
					new GradedCourse() {
						Title = "Física para Informática",
						Code = "FIS 1150",
						Grade = 70,
						State = GradedCourse.CourseState.Published,
						Credits = 3
					},
					new GradedCourse() {
						Title = "Lab de Física para Informática",
						Code = "FIS 1160",
						Grade = 80,
						State = GradedCourse.CourseState.Published,
						Credits = 1
					},
					new GradedCourse() {
						Title = "Lenguaje de Programación I",
						Code = "INF 5120",
						Grade = 96,
						State = GradedCourse.CourseState.Published,
						Credits = 4
					},
					new GradedCourse() {
						Title = "Lab de Lenguaje de Programac I",
						Code = "INF 5130",
						Grade = 100,
						State = GradedCourse.CourseState.Published,
						Credits = 1
					},
					new GradedCourse() {
						Title = "Matemática Financiera",
						Code = "MAT 1430",
						Grade = 95,
						State = GradedCourse.CourseState.Published,
						Credits = 5
					},
					new GradedCourse() {
						Title = "Matemática Discr para Computac",
						Code = "MAT 3920",
						State = GradedCourse.CourseState.Absent,
						Credits = 4
					}
				}
			},
			new AcademicPeriod("Verano 2017") {
				IsSummerCourse = true,
				Courses = {
					new GradedCourse() {
						Title = "Metodología De La Inv Cientif",
						Code = "FIL 1240",
						Grade = 96,
						State = GradedCourse.CourseState.Published,
						Credits = 3
					},
					new GradedCourse() {
						Title = "Matemática Discr para Computac",
						Code = "MAT 3920",
						Grade = 85,
						State = GradedCourse.CourseState.Published,
						Credits = 4
					}
				}
			},
			new AcademicPeriod("Segundo Semestre 2017") {
				Courses = {
					new GradedCourse() {
						Title = "Contabilidad General I",
						Code = "CON 1190",
						State = GradedCourse.CourseState.Absent,
						Credits = 5
					},
					new GradedCourse() {
						Title = "Sistemas Operativos",
						Code = "INF 3240",
						Grade = 90,
						State = GradedCourse.CourseState.Published,
						Credits = 4
					},
					new GradedCourse() {
						Title = "Lenguaje de Programación II",
						Code = "INF 5140",
						Grade = 95,
						State = GradedCourse.CourseState.Published,
						Credits = 4
					},
					new GradedCourse() {
						Title = "Lab de Lenguaje de Program II",
						Code = "INF 5150",
						Grade = 99,
						State = GradedCourse.CourseState.Published,
						Credits = 1
					},
					new GradedCourse() {
						Title = "Estructura De Datos",
						Code = "INF 5260",
						Grade = 100,
						State = GradedCourse.CourseState.Published,
						Credits = 3
					},
					new GradedCourse() {
						Title = "Cálculo I",
						Code = "MAT 3560",
						Grade = 85,
						State = GradedCourse.CourseState.Published,
						Credits = 5
					}
				}
			},
			new AcademicPeriod("Primer Semestre 2018") {
				Courses = {
					new GradedCourse() {
						Title = "Base de Datos I",
						Code = "INF 4200",
						Grade = 93,
						State = GradedCourse.CourseState.Published,
						Credits = 5
					},
					new GradedCourse() {
						Title = "Teoría De Los Compiladores",
						Code = "INF 4220",
						Grade = 92,
						State = GradedCourse.CourseState.Published,
						Credits = 4
					},
					new GradedCourse() {
						Title = "Teleproceso",
						Code = "INF 4250",
						Grade = 91,
						State = GradedCourse.CourseState.Published,
						Credits = 4
					},
					new GradedCourse() {
						Title = "Redes De Proc De Datos",
						Code = "INF 4260",
						Grade = 89,
						State = GradedCourse.CourseState.Published,
						Credits = 4
					},
					new GradedCourse() {
						Title = "Lenguaje de Programación III",
						Code = "INF 5160",
						Grade = 99,
						State = GradedCourse.CourseState.Published,
						Credits = 4
					},
					new GradedCourse() {
						Title = "Lab Lenguaje de Program III",
						Code = "INF 5170",
						Grade = 99,
						State = GradedCourse.CourseState.Published,
						Credits = 1
					},
					new GradedCourse() {
						Title = "Cálculo II",
						Code = "MAT 3570",
						Grade = 80,
						State = GradedCourse.CourseState.Published,
						Credits = 5
					}
				}
			},
			new AcademicPeriod("Verano 2018") {
				IsSummerCourse = true,
				Courses = {
					new GradedCourse() {
						Title = "Método De Optimización",
						Code = "MAT 3470",
						Grade = 90,
						State = GradedCourse.CourseState.Published,
						Credits = 5
					}
				}
			},
			new AcademicPeriod("Segundo Semestre 2018") {
				Courses = {
					new GradedCourse() {
						Title = "Contabilidad General I",
						Code = "CON 1190",
						State = GradedCourse.CourseState.Absent,
						Credits = 5
					},
					new GradedCourse() {
						Title = "Algoritmos Computacionales",
						Code = "INF 3220",
						Grade = 97,
						State = GradedCourse.CourseState.Published,
						Credits = 5
					},
					new GradedCourse() {
						Title = "Análisis y Diseño de Sistema",
						Code = "INF 3290",
						Grade = 77,
						State = GradedCourse.CourseState.Published,
						Credits = 4
					},
					new GradedCourse() {
						Title = "Base de Datos II",
						Code = "INF 5200",
						Grade = 70,
						State = GradedCourse.CourseState.Published,
						Credits = 5
					},
					new GradedCourse() {
						Title = "Adm de Centros de Cómputos",
						Code = "INF 5240",
						Grade = 82,
						State = GradedCourse.CourseState.Published,
						Credits = 4
					}
				}
			},
			new AcademicPeriod("Primer Semestre 2019") {
				Courses = {
					new GradedCourse() {
						Title = "Ingeniería de Software I",
						Code = "INF 5220",
						Grade = 80,
						State = GradedCourse.CourseState.Published,
						Credits = 4
					},
					new GradedCourse() {
						Title = "Auditoría de Sist Informáticos",
						Code = "INF 5230",
						Grade = 86,
						State = GradedCourse.CourseState.Published,
						Credits = 4
					},
					new GradedCourse() {
						Title = "Inteligencia Artificial",
						Code = "INF 5290",
						Grade = 83,
						State = GradedCourse.CourseState.Published,
						Credits = 5
					}
				}
			},
			new AcademicPeriod("Segundo Semestre 2019") {
				Courses = {
					new GradedCourse() {
						Title = "Contabilidad General I",
						Code = "CON 1190",
						Grade = 99,
						State = GradedCourse.CourseState.Published,
						Credits = 5
					},
					new GradedCourse() {
						Title = "Ingeniería de Software II",
						Code = "INF 5250",
						Grade = 80,
						State = GradedCourse.CourseState.Published,
						Credits = 4
					}
				}
			}
		}
		};

        public static string[] carreras = {
			"Informática",
			"Contabilidad",
			"Economía"
		};
		public static string[] titulosMaterias = {
			"Materia de Ejemplo I",
			"Materia de Ejemplo II",
			"Materia de Ejemplo III",
			"Teoria de Ejemplificacion",
			"Lab Desarrollo de Prueba",
			"Ejemplo Básico",
			"Int Al Ejempo De Prueba",
			"Introd A Las Ciencias Ejemplares",
			"Fund De La Data de Ejemplo",
			"Ingeniería de Ejemplo I",
			"Ingeniería de Ejemplo II",
			"Ejemplo Artificial",
			"Análisis y Diseño de Ejemplo",
		};
		public static string[] codigosMaterias = {
			"EJEM", "TEST", "FAKE"
		};
		public static string[] profesores = {
			"Fulanito Ejemplo Perez",
			"Persona Ejemplo Martinez",
			"Juan de Prueba Cabreras"
		};

		public static T    Choose<T>(this ICollection<T> list, Random random_source) => list.ElementAt(random_source.Next(list.Count));
		public static bool Probability(this Random source, double probability) => source.NextDouble() < probability;
		public static bool CoinFlip(this Random source) => source.Probability(0.5);

		public static Course GenerateRandomCourse(Random random) =>
			new Course()
			{
				Title     = titulosMaterias.Choose(random),
				Code      = $"{codigosMaterias.Choose(random)} {++generated:0000}",
				Section   = random.Next(1, 50).ToString("00"),
				NRC       = $"{random.Next(1, 10)}{random.Next(1, 10)}{random.Next(1, 10)}{random.Next(1, 10)}{random.Next(1, 10)}",
				Professor = profesores.Choose(random),
				Credits   = random.Next(2, 6),
				Type      = Course.CourseType.Theory
			};

		public static GradedCourse ToGradedCourse(this Course course) =>
			new GradedCourse()
			{
				Title     = course.Title,
				Code      = course.Code,
				Section   = course.Section,
				NRC       = course.NRC,
				Professor = course.Professor,
				Credits   = course.Credits,
				Type      = course.Type
			};

		public static OpenCourse ToOpenCourse(this Course course) =>
			new OpenCourse()
			{
				Title     = course.Title,
				Code      = course.Code,
				Section   = course.Section,
				NRC       = course.NRC,
				Professor = course.Professor,
				Credits   = course.Credits,
				Type      = course.Type,
			};

		public static CourseCollection                GenerateFakeCourseSchedule(Random random)
		{
			var limits = new[]
			{
				new[] {  "8:00 AM", "9:50 AM" },
				new[] { "10:00 AM", "1:00 PM" },
				new[] {  "3:00 PM", "6:50 PM" },
				new[] {  "8:00 PM", "9:50 PM" }
			};

			CourseCollection schedule = new CourseCollection("Fake Schedule");
			int number_of_courses_in_schedule = random.Next(2, 5);
			for (int i = 0; i < number_of_courses_in_schedule; i++)
			{
				Course course = GenerateRandomCourse(random);
				int number_of_classes = random.Next(1, 4);
				for (int j = 0; j < number_of_classes; j++)
				{
					CourseClass courseClass = null;
					do
					{
						var limit = limits.Choose(random);
						courseClass = new CourseClass()
						{
							Place     = $"{Convert.Places.Keys.Choose(random)} {random.Next(101, 400)}",
							DayOfWeek = Convert.Days.Values.Choose(random),
							StartTime = Convert.Time(limit[0]),
							EndTime   = Convert.Time(limit[1]),
							StartDate = Convert.Date("Ene 01, 2020"),
							EndDate   = Convert.Date("Dic 31, 2020")
						};
					} while (course.Schedule.Any(cc => cc.CollidesWith(courseClass)));
					course.Schedule.Add(courseClass);
				}
				schedule.Add(course);
			}

			return schedule;
		}
		public static CourseCollection                GenerateFakeCourseProjection(Random random)
		{
			CourseCollection projection = new CourseCollection("Proyección");
			int number_of_courses_to_project = random.Next(3, 7);
			for (int i = 0; i < number_of_courses_to_project; i++)
				projection.Add(GenerateRandomCourse(random));
			return projection;
		}
		public static AcademicReport                  GenerateFakeAcademicReport(Random random, CourseCollection mostRecent)
		{
			string[] period_title_prefixes = { "Primer Semestre ", "Segundo Semestre " };
			AcademicReport report = new AcademicReport();
			for (int year = 2015; year <= 2020; year++)
			{
				int sem_limit = year == 2020 ? 0 : 2;
				for (int semestre = 0; semestre < sem_limit; semestre++)
				{
					AcademicPeriod period = new AcademicPeriod(period_title_prefixes[semestre] + year.ToString());
					int number_of_courses_in_period = random.Next(2, 5);
					for (int i = 0; i < number_of_courses_in_period; i++)
					{
						GradedCourse gradedCourse = GenerateRandomCourse(random).ToGradedCourse();
						gradedCourse.State = random.Probability(0.9) ? GradedCourse.CourseState.Published : GradedCourse.CourseState.Absent;
						gradedCourse.Grade = random.Next(80, 101);
						period.Courses.Add(gradedCourse);
					}
					report.Periods.Add(period);
				}
				if (year == 2020)
				{
					AcademicPeriod lastPeriod = new AcademicPeriod(period_title_prefixes[0] + year.ToString());
					foreach (Course course in mostRecent)
					{
						GradedCourse gradedCourse = course.ToGradedCourse();
						gradedCourse.State = random.CoinFlip() ? GradedCourse.CourseState.Published : GradedCourse.CourseState.NotPublished;
						gradedCourse.Grade = random.Next(80, 101);
						lastPeriod.Courses.Add(gradedCourse);
					}
					report.Periods.Add(lastPeriod);
				}
			}
			return report;
		}
		public static CareerInformation               GenerateFakeCareerInformation(Random random, AcademicReport report)
		{
			var info = new CareerInformation()
			{
				Name = carreras.Choose(random),
				Courses = report.Periods.Select(p => p.Courses.Count).Sum(),
				Credits = report.Credits,
				RequiredCredits = report.Credits
			};
			info.RequiredCourses = info.Courses;
			return info;
		}
		public static List<CourseCollection>          GenerateFakeAvailableCourses(Random random)
		{
			var limits = new[]
			{
				new[] {  "7:00 AM",  "9:50 AM" },
				new[] {  "7:00 AM",  "8:50 AM" },
				new[] {  "8:00 AM",  "9:50 AM" },
				new[] { "10:00 AM", "12:50 PM" },
				new[] { "11:00 AM",  "1:50 PM" },
				new[] {  "1:00 PM",  "2:50 PM" },
				new[] {  "2:00 PM",  "2:50 PM" },
				new[] {  "2:00 PM",  "3:50 PM" },
				new[] {  "3:00 PM",  "5:50 PM" },
				new[] {  "6:00 PM",  "7:50 PM" },
				new[] {  "6:00 PM",  "8:50 PM" },
				new[] {  "8:00 PM",  "9:50 PM" },
			};

			var availableCourses = new List<CourseCollection>();

			int number_of_available_courses = random.Next(3, 6);

			for (int i = 0; i < number_of_available_courses; i++)
			{
				Course templateCourse = null;
				do
				{
					templateCourse = GenerateRandomCourse(random);
				} while (availableCourses.Any(cc => cc.Name == templateCourse.Title));
				CourseCollection collection = new CourseCollection(templateCourse.Title);

				int number_of_open_courses_in_collection = random.Next(5, 25);
				for (int j = 0; j < number_of_open_courses_in_collection; j++)
				{
					var openCourse = GenerateRandomCourse(random).ToOpenCourse();
					openCourse.Title   = templateCourse.Title;
					openCourse.Code    = templateCourse.Code;
					openCourse.Credits = templateCourse.Credits;
					openCourse.Type    = templateCourse.Type;

					openCourse.Capacity = 50;
					openCourse.Vacancy  = random.Next(1, 51);

					int number_of_classes = random.Next(1, 4);
					for (int k = 0; k < number_of_classes; k++)
					{
						CourseClass courseClass = null;
						do
						{
							var limit = limits.Choose(random);
							courseClass = new CourseClass()
							{
								Place     = $"{Convert.Places.Keys.Choose(random)} {random.Next(101, 400)}",
								DayOfWeek = Convert.Days.Values.Choose(random),
								StartTime = Convert.Time(limit[0]),
								EndTime   = Convert.Time(limit[1]),
								StartDate = Convert.Date("Ene 01, 2020"),
								EndDate   = Convert.Date("Dic 31, 2020")
							};
						} while (openCourse.Schedule.Any(cc => cc.CollidesWith(courseClass)));
						openCourse.Schedule.Add(courseClass);
					}

					openCourse.Schedule.Sort((a, b) => (int)(10 * (a.StartTime.TotalHours - b.StartTime.TotalHours)));
					openCourse.Schedule.Sort((a, b) => (int)a.DayOfWeek - (int)b.DayOfWeek);
					collection.Add(openCourse);
				}

				availableCourses.Add(collection);
			}

			return availableCourses;
		}
		public static List<DateTimeRange>             GenerateFakeSelectionCalendar()
		{
			var now = DateTime.Now;
			var nextWeek = (new DateTime(now.Year, now.Month, now.Day)).AddDays(7);

			var selectionCalendar = new List<DateTimeRange>();
			selectionCalendar.Add(new DateTimeRange()
			{
				StartDate = nextWeek.AddHours(10),
				EndDate   = nextWeek.AddHours(13)
			});

			for (int i = 0; i < 5; i++)
			{
				selectionCalendar.Add(new DateTimeRange()
				{
					StartDate = nextWeek.AddDays(i).AddHours(12+7),
					EndDate   = nextWeek.AddDays(i).AddHours(24+7)
				});
			}

			return selectionCalendar;
		}
	}
}