using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.Sports
{
    public class EventStat
    {
        public int SportEventStatId { get; }
        public string Name { get; }
        public string Abbreviation { get; }
        public string DataType { get; }
        public int SortOrder { get; }
        public bool Enabled { get; }
        public bool Calculated { get; }

        public EventStat()
        {

        }

        public EventStat(VsandSportEventStat stat)
        {
            this.SportEventStatId = stat.SportEventStatId;
            this.Name = stat.Name;
            this.Abbreviation = stat.Abbreviation;
            this.DataType = stat.DataType;
            this.SortOrder = stat.SortOrder;
            this.Enabled = stat.Enabled;
            this.Calculated = stat.Calculated ?? false;
        }
    }
}
