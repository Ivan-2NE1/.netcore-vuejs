using System;

namespace VSAND.Data.ViewModels
{
    public class TeamsWithDuplicateGameReports
    {
        public int RowId { get; set; }
        public int TeamId { get; set; }
        public DateTime? GameDate { get; set; }
        public int DupeCount { get; set; }
    }
}
