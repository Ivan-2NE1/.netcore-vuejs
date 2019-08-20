using System.Collections.Generic;

namespace VSAND.Data.ViewModels
{
    public class RankResult
    {
        public int RankId { get; set; }
        public IEnumerable<TeamRecordInfo> Records { get; set; }
    }
}
