using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.Sports
{
    public class PlayerStat : StatBase
    {
        public int SportPlayerStatId { get; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string DisplayName {
            get {
                string name = Name;
                if (Abbreviation != null && !string.IsNullOrWhiteSpace(Abbreviation))
                {
                    name = Abbreviation;
                }
                return name;
            }
        }
        public string DataType { get; set; }
        public int SortOrder { get; set; }
        public bool Enabled { get; set; }
        public bool Calculated { get; set; }
        public int SportPlayerStatCategoryId { get; }

        public PlayerStat()
        {

        }

        public PlayerStat(VsandSportPlayerStat stat)
        {
            this.SportPlayerStatId = stat.SportPlayerStatId;
            this.Name = stat.Name;
            this.SortOrder = stat.SortOrder;
            this.Abbreviation = stat.Abbreviation;
            this.DataType = stat.DataType;
            this.Enabled = stat.Enabled;
            this.Calculated = stat.Calculated ?? false;
            this.SportPlayerStatCategoryId = stat.SportPlayerStatCategoryId;
        }
    }
}
