using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.Entities;
using System.Linq;

namespace VSAND.Data.ViewModels.Sports
{
    public class Event
    {
        public int SportEventId { get; }
        public string Name { get; }
        public string Abbreviation { get; }
        public int DefaultSort { get; }
        public string ResultType { get; }
        public string ResultHandler { get; }
        public bool DefaultActivated { get; }
        public int MaxResults { get; }

        public List<EventStat> Stats { get; set; }

        public Event()
        {

        }

        public Event(VsandSportEvent sEvent) {
            this.SportEventId = sEvent.SportEventId;
            this.Name = sEvent.Name;
            this.Abbreviation = sEvent.Abbreviation;
            this.DefaultSort = sEvent.DefaultSort;
            this.ResultType = sEvent.ResultType;
            this.ResultHandler = sEvent.ResultHandler?.Trim();
            this.DefaultActivated = sEvent.DefaultActivated;
            this.MaxResults = sEvent.MaxResults;
            this.Stats = (from s in sEvent.EventStats orderby s.SortOrder select new EventStat(s)).ToList();
        }
    }
}
