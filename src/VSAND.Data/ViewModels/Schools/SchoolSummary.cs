using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels
{
    public class SchoolSummary
    {
        public int SchoolId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public bool Validated { get; set; }
        public bool CoreCoverage { get; set; }
        public int PlayerCount { get; set; }
        public int TeamCount { get; set; }

        public SchoolSummary(VsandSchool oSchool)
        {
            this.SchoolId = oSchool.SchoolId;
            this.Name = oSchool.Name;
            this.City = oSchool.City;
            this.State = oSchool.State;
            this.Validated = oSchool.Validated;
            this.CoreCoverage = oSchool.CoreCoverage;
        }
    }
}
