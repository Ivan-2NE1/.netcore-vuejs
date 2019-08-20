using System;
using System.Collections.Generic;
using System.Text;

namespace VSAND.Data.ViewModels.FrontEndPreferences
{
    /// <summary>
    /// This object exactly follows the pattern for the existing HSSN preferences objects
    /// </summary>
    public class Preferences
    {
        public string username { get; set; }
        public bool is_reporter { get; set; }
        public List<object> reports_for { get; set; }
        public List<Team> teams { get; set; }
        public List<School> schools { get; set; }
        public bool is_staff_reporter { get; set; }
    }

    public class Team
    {
        public int id { get; set; }
        public string sport_slug { get; set; }
        public string name { get; set; }
        public string school_slug { get; set; }
    }

    public class School
    {
        public int id { get; set; }
        public string name { get; set; }
        public string school_slug { get; set; }
    }

}
