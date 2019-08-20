using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.Sports
{
    public class EventResultType
    {
        public int sportEventResultId { get; }
        public string name { get; }
        public int sortOrder { get; }
        public bool isTie { get; }

        public EventResultType()
        {

        }

        public EventResultType(VsandSportEventResult oResult)
        {
            this.sportEventResultId = oResult.SportEventResultId;
            this.name = oResult.Name;
            this.sortOrder = oResult.SortOrder;
            this.isTie = oResult.IsTie;
        }
    }
}
