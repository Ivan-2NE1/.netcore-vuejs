using System;
using System.Collections.Generic;
using System.Text;

namespace VSAND.Data.ViewModels.StatAggregation
{
    public class AggregatedStatItem
    {
        public int StatId { get; set; }
        public string StatName { get; set; }
        public string CategoryName { get; set; }
        public string GroupingContext { get; set; }
        public double StatValue { get; set; }
        public int Rank { get; set; }

        public string Described
        {
            get
            {
                string rankWord = Common.NumberHelp.ToOrdinal(Rank);
                string contextDesc = "overall";
                if (!string.IsNullOrEmpty(GroupingContext))
                {
                    contextDesc = $"in {GroupingContext}";
                }
                return $"{rankWord} {contextDesc} in {CategoryName}-{StatName} ({StatValue})";
            }
        }

        public AggregatedStatItem()
        {

        }
    }
}
