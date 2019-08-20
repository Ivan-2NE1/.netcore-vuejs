namespace VSAND.Data.Entities
{
    public class VsandSchoolsWithoutAccounts
    {
        public int SchoolId { get; set; }
        public string Name { get; set; }
        public int MasterAccount { get; set; }
        public int UserAccounts { get; set; }
    }
}
