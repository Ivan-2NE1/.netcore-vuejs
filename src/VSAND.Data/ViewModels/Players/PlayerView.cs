using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels.StatAggregation;

namespace VSAND.Data.ViewModels.Players
{
    public class PlayerView
    {
        public VsandPlayer Player { get; set; }
        public int TeamId { get; set; }
        public List<AggregatedStatItem> Top100Stats { get; set; }
        public Dictionary<string, double> FeaturedStats { get; set; }
        public List<VsandEntitySlug> Slugs { get; set; }

        public PlayerView()
        {
            Top100Stats = new List<AggregatedStatItem>();
            FeaturedStats = new Dictionary<string, double>();
            Slugs = new List<VsandEntitySlug>();
        }
    }
}
