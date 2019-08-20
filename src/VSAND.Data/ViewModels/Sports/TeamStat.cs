using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.Sports
{
    public class TeamStat : StatBase
    {
        public int SportTeamStatId { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string DataType { get; set; }
        public int SortOrder { get; set; }
        public bool Enabled { get; set; }
        public bool Calculated { get; set; }
        public int SportTeamStatCategoryId { get; }
        public TeamStat()
        {

        }

        public TeamStat(VsandSportTeamStat stat)
        {
            this.SportTeamStatId = stat.SportTeamStatId;
            this.Name = stat.Name;
            this.SortOrder = stat.SortOrder;
            this.Abbreviation = stat.Abbreviation;
            this.DataType = stat.DataType;
            this.Enabled = stat.Enabled;
            this.Calculated = stat.Calculated ?? false;
            this.SportTeamStatCategoryId = stat.SportTeamStatCategoryId;
        }
    }
}
