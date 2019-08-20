using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.Sports
{
    public class TeamStatistics : StatCategoryBase<TeamStat>
    {
        public int SportTeamStatCategoryId { get; }
        public string CategoryName { get; }
        public List<TeamStat> Stats { get; }
        public int SportTeamStatId { get; set; }
        public string TeamStatName { get; set; }
        public string Abbreviation { get; set; }
        public string DataType { get; set; }
        public int SortOrder { get; set; }
        public bool Enabled { get; set; }
        public bool Calculated { get; set; }
        public string TeamStatistic {get; set;}
        public TeamStatistics()
        {

        }

        public TeamStatistics(VsandSportTeamStatCategory cat, VsandSportTeamStat stat)
        {
            this.SportTeamStatCategoryId = cat.SportTeamStatCategoryId;
            this.CategoryName = cat.Name;
            this.Stats = (from s in cat.TeamStats where s.Enabled orderby s.SortOrder ascending select new TeamStat(s)).ToList();

            this.SportTeamStatId = stat.SportTeamStatId;
            this.TeamStatName = stat.Name;
            this.SortOrder = stat.SortOrder;
            this.Abbreviation = stat.Abbreviation;
            this.DataType = stat.DataType;
            this.Enabled = stat.Enabled;
            this.Calculated = stat.Calculated ?? false;

            this.TeamStatistic = this.CategoryName + " - " + this.TeamStatName;
        }
    }
}
