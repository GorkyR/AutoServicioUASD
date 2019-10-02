using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace UASD.Utilities
{
    public static class Extensions
    {
		public static bool IsEither<T>(this T obj, params T[] array) =>
            array.Contains(obj);

        public static bool HasClass(this HtmlNode node, string name) =>
            node.Attributes.Contains("class") &&
            node.GetAttribute("class").ToLower().Split().Contains(name.ToLower());

        public static IList<HtmlNode> GetElementsByTagName(this IEnumerable<HtmlNode> nodeList, string tagName) =>
            (from node in nodeList
             where string.Equals(node.Name, tagName, StringComparison.OrdinalIgnoreCase)
             select node).ToList();

        public static IList<HtmlNode> GetElementsByTagName(this HtmlNode self, string tagName) =>
            GetElementsByTagName(self.Descendants(), tagName);

        public static IList<HtmlNode> GetElementsByTagName(this HtmlDocument doc, string name) =>
            GetElementsByTagName(doc.DocumentNode.Descendants(), name);

        public static IList<HtmlNode> GetElementsByClass(this IEnumerable<HtmlNode> self, string className) =>
            (from node in self
             where node.HasClass(className)
             select node).ToList();

        public static IList<HtmlNode> GetElementsByClass(this HtmlNode self, string className) =>
            GetElementsByClass(self.Descendants(), className);

        public static IList<HtmlNode> GetElementsByClass(this HtmlDocument doc, string className) =>
            GetElementsByClass(doc.DocumentNode.Descendants(), className);

        public static string GetAttribute(this HtmlNode self, string attributeName) =>
            self.GetAttributeValue(attributeName, "");

        public static bool HasAttribute(this HtmlNode self, string attributeName) =>
            self.Attributes.Contains(attributeName);

        public static bool HasAttribute(this HtmlNode self, string name, string value) =>
            self.HasAttribute(name) && self.GetAttribute(name) == value;

        public static IList<HtmlNode> GetElementsWithAttribute(this IEnumerable<HtmlNode> self, string name) =>
            (from node in self
             where node.HasAttribute(name)
             select node).ToList();

        public static IList<HtmlNode> GetElementsWithAttribute(this IEnumerable<HtmlNode> self, string name, string value) =>
            (from node in self
             where node.HasAttribute(name, value)
             select node).ToList();

        public static IList<HtmlNode> GetElementsWithAttribute(this HtmlNode self, string name) =>
            GetElementsWithAttribute(self.Descendants(), name);

        public static IList<HtmlNode> GetElementsWithAttribute(this HtmlDocument doc, string name) =>
            GetElementsWithAttribute(doc.DocumentNode.Descendants(), name);

        public static IList<HtmlNode> GetElementsWithAttribute(this HtmlNode self, string name, string value) =>
            GetElementsWithAttribute(self.Descendants(), name, value);

        public static IList<HtmlNode> GetElementsWithAttribute(this HtmlDocument doc, string name, string value) =>
			GetElementsWithAttribute(doc.DocumentNode.Descendants(), name, value);

        public static string GetTitle(this HtmlDocument doc) =>
            doc.GetElementsByTagName("title")[0].InnerText;

        public static bool /*Case Insensitive*/Matches(this string self, string other) =>
            string.Equals(self, other, StringComparison.OrdinalIgnoreCase);

        public static string[] Split(this string self, string delimiter) =>
            self.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);

        public static List<CourseClassInstance> FilterByDay(this UASD.CourseCollection courseCollection, DayOfWeek dayOfWeek)
        {
            var thatDaysCourses =
                (from course in courseCollection
                 select (from instance in course.Schedule
                         where instance.DayOfWeek == dayOfWeek
                         select new CourseClassInstance { Course = course, Class = instance }))
                .Aggregate((a, b) => a.Concat(b)).ToList();
            thatDaysCourses.Sort((a, b) =>
                (a.Class.StartTime - b.Class.StartTime).Hours
            );
            return thatDaysCourses;
        }
    }
}