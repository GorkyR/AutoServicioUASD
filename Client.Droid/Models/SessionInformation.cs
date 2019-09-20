using System.Collections.Generic;
using UASD;

namespace Client.Droid.Models
{
    public class SessionInformation
    {
        public int Id { get; set; }
        public string Username { get; set;  }
        public string ID { get; set; }
        public string NIP { get; set;  }

        public CourseCollection  Schedule { get; set; }
        public IList<AcademicPeriodModel> Report { get; set; }
        public CourseCollection  Projection { get; set; }
        public CareerInformation Information { get; set; }

        public SessionInformation() { }
        public SessionInformation(string username, string id, string nip)
        {
            Username = username;
            ID = id;
            NIP = nip;
        }
    }
}
