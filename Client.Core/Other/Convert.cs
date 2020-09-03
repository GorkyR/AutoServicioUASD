using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UASD.Utilities
{
    static public class Convert
    {
        static Regex UsernameRegex = new Regex(@".*\(a\),(.*?),.*");
        static Regex TimeRegex     = new Regex(@"(\d{1,2}):(\d{2})\s+(\w{2})");
        static Regex DateRegex     = new Regex(@"(\w{3})\s+(\d{1,2}),\s+(\d{4})");
        static Regex DateRegex2    = new Regex(@"(\d{2})[/](\d{2})");
        static Regex PlaceRegex    = new Regex(@"(.*)\s(.*)");
        static Regex CreditsRegex  = new Regex(@"([0-9\.]+)(/.*)?");

        static public string Username(string loginresponse)
        {
            HtmlAgilityPack.HtmlDocument a = new HtmlAgilityPack.HtmlDocument();
            a.LoadHtml(loginresponse);
            var match = UsernameRegex.Match(a.GetElementsByTagName("meta").First().GetAttribute("content"));
            if (match.Success)
                return SimpleEncodeFix(match.Groups[1].Value.Replace('+', ' ').Trim());
            else
                return "";
        }
        static public int Credits(string creditoString)
        {
            var creditsMatch = CreditsRegex.Match(creditoString);
            if (creditsMatch.Success)
                return (int)float.Parse(creditsMatch.Groups[1].Value, NumberStyles.Any, CultureInfo.InvariantCulture);
            else
                return 0;
        }
        static public int Grade(string notaString)
        {
            if (notaString[0] == 'L')
                return int.Parse(notaString.Substring(1), NumberStyles.Any, CultureInfo.InvariantCulture);
            return int.Parse(notaString, NumberStyles.Any, CultureInfo.InvariantCulture);
        }
        static public string Time(TimeSpan hora, bool shorten = false)
        {
            if (shorten) {
                int hour = (int)Math.Round(hora.TotalHours);
                if (hour == 0)
                    return "12 AM";
                else if (hour > 11)
                {
                    if (hour == 12)
                        return "12 PM";
                    else
                        return $"{hour - 12} PM";
                }
                else
                    return $"{hour} AM";
            } else {
                int hours = hora.Hours;
                int minutes = hora.Minutes;
                if (hours == 0)
                    return $"12:{minutes:D2} AM";
                else if (hours > 11)
                {
                    if (hours == 12)
                        return $"12:{minutes:D2} PM";
                    else
                        return $"{hours - 12:D2}:{minutes:D2} PM";
                }
                else
                    return $"{hours:D2}:{minutes:D2} AM";
            }
        }
        static public TimeSpan Time(string horaString)
        {
            var match = TimeRegex.Match(horaString.Trim());
            if (match.Success)
            {
                string meridian = match.Groups[3].Value.ToLower();
                int hours = int.Parse(match.Groups[1].Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                if (hours < 12 && meridian == "pm")
                    hours += 12;
                else if (hours == 12 && meridian == "am")
                    hours = 0;
                return new TimeSpan(hours, int.Parse(match.Groups[2].Value, NumberStyles.Any, CultureInfo.InvariantCulture), 0);
            }
            else
                throw new FormatException();
        }
        static public DateTime Date(string fechaString)
        {
            var match = DateRegex.Match(fechaString.Trim());
            if (match.Success)
                return new DateTime(int.Parse(match.Groups[3].Value, NumberStyles.Any, CultureInfo.InvariantCulture), Months[match.Groups[1].Value], int.Parse(match.Groups[2].Value, NumberStyles.Any, CultureInfo.InvariantCulture));
            else
            {
                match = DateRegex2.Match(fechaString.Trim());
                if (match.Success)
                    return new DateTime(DateTime.Now.Year, int.Parse(match.Groups[1].Value, NumberStyles.Any, CultureInfo.InvariantCulture), int.Parse(match.Groups[2].Value, NumberStyles.Any, CultureInfo.InvariantCulture));
                else throw new FormatException();
            }
        }
        static public string Place(string lugarString)
        {
            var match = PlaceRegex.Match(lugarString);
            if (match.Success)
            {
                if (Places.ContainsKey(match.Groups[1].Value))
                    return Places[match.Groups[1].Value] + " " + match.Groups[2].Value;
                return lugarString;
            }
            return lugarString;
        }
        static public string PeriodTitle(string periodoString)
        {
            string p = periodoString.Substring(4);
            string year = periodoString.Substring(0,4);
            if (p == "10")
                return "Primer semestre " + year;
            else if (p == "15")
                return "Verano " + year;
            else if (p == "20")
                return "Segundo semestre " + year;
            return periodoString;
        }
        static public string PeriodCode(string periodTitle)
        {
            string[] words = periodTitle.Split();
            string year = words.Last();
            string period = words.Length == 2 ? "15" : (words[0][0] == 'P'? "10" : "20");
            return year + period;
        }
        static public string Day(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Monday:
                    return "Lunes";
                case DayOfWeek.Tuesday:
                    return "Martes";
                case DayOfWeek.Wednesday:
                    return "Miércoles";
                case DayOfWeek.Thursday:
                    return "Jueves";
                case DayOfWeek.Friday:
                    return "Viernes";
                case DayOfWeek.Saturday:
                    return "Sábado";
                default:
                    return "";
            }
        }

        static public Dictionary<string, string> Places = new Dictionary<string, string>
        {
            { "MAXIMO AVILES BLONDA"         , "AB" },
            { "CIENCIAS JURIDICAS A"         , "CJA" },
            { "CIENCIAS JURIDICAS B"         , "CJB" },
            { "CIENCIAS MODERNAS"            , "CM" },
            { "EUGENIO MARIA DE HOSTOS"      , "EH" },
            { "ESCUELA DE IDIOMAS"           , "EI" },
            { "ESCUELA DE MEDICINA"          , "EM" },
            { "ESCUELA DE ODONTOLOGIA"       , "EO" },
            { "FACULTAD DE HUMANIDADES"      , "FH" },
            { "FACULTAD DE CS ECONOMICAS"    , "FE" },
            { "FACULTAD DE INGENIERIA"       , "FI" },
            { "JUAN ISIDRO JIMENEZ G"        , "JJ" },
            { "LABORATORIO DE MEDICINA"      , "LM" },
            { "LABORATORIO DE QUIMICA - SEDE", "LQ-SED" },
            { "LABORATORIO DE INFORMATICA"   , "LIF" },
            { "NUEVA UNIVERSIDAD"            , "NU" },
            { "JULIO RAVELO DE LA FUENTE"    , "RN" },
            { "ROGELIO LAMARCHE"             , "RL" },
            { "ZONA ORIENTAL - UASD"         , "ZOR" }
        };
        static public Dictionary<string, int> Months = new Dictionary<string, int>
        {
            {"Ene", 1},
            {"Feb", 2},
            {"Mar", 3},
            {"Abr", 4},
            {"May", 5},
            {"Jun", 6},
            {"Jul", 7},
            {"Ago", 8},
            {"Sep", 9},
            {"Oct", 10},
            {"Nov", 11},
            {"Dic", 12}
        };
        static public Dictionary<char, DayOfWeek> Days = new Dictionary<char, DayOfWeek>
        {
            { 'L', DayOfWeek.Monday    },
            { 'M', DayOfWeek.Tuesday   },
            { 'I', DayOfWeek.Wednesday },
            { 'J', DayOfWeek.Thursday  },
            { 'V', DayOfWeek.Friday    },
            { 'S', DayOfWeek.Saturday  }
        };
        static public Dictionary<GradedCourse.CourseState, string> CourseStates = new Dictionary<GradedCourse.CourseState, string>
        {
            { GradedCourse.CourseState.Absent, "Ausente" },
            { GradedCourse.CourseState.Published, "Publicada" },
            { GradedCourse.CourseState.NotPublished, "No publicada" }
        };

        static public string ShortWords(string phrase)
        {
            string[] words = phrase.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
                words[i] = words[i].PadRight(18).Substring(0, 18 / words.Length).Trim();
            return string.Join(" ", words);
        }
        static public string SimpleEncodeFix(string text)
        {
            text = text.Replace("Ã¡", "á");
            text = text.Replace("Ã©", "é");
            text = text.Replace("Ã­", "í");
            text = text.Replace("Ã³", "ó");
            text = text.Replace("Ã±", "ñ");

            return text.Trim();
        }
    }
}
