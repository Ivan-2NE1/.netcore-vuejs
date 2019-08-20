using System;
using System.Collections.Generic;
using System.Text;

namespace VSAND.Data.ViewModels.StatAggregation
{
    public class AggStatPlayer
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public string JerseyNumber { get; set; }
        public int? GraduationYear { get; set; }
        public string Slug { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string TeamSlug { get; set; }
        public double GamesPlayed { get; set; }
        public int Rank { get; set; }
        public Dictionary<int, double> StatValues { get; set; }

        public AggStatPlayer()
        {
            StatValues = new Dictionary<int, double>();
        }
    }
}
