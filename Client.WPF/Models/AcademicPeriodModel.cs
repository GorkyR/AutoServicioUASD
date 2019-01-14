﻿using System.Collections.Generic;
using UASD;

namespace Client.WPF.Models
{
    public class AcademicPeriodModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IList<Course> Courses { get; set; }
    }
}
