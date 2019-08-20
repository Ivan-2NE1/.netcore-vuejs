namespace VSAND.Data.ViewModels
{
    public class EventTypeListItem
    {
        public int EventTypeId { get; set; } = 0;

        public int RoundId { get; set; } = 0;
        public int SectionId { get; set; } = 0;
        public int GroupId { get; set; } = 0;
        public string EventTypeName { get; set; } = "";
        public string RoundName { get; set; } = "";
        public string SectionName { get; set; } = "";
        public string GroupName { get; set; } = "";

        public string EventTypeListName
        {
            get {
                string sRet = EventTypeName;
                if (!string.IsNullOrEmpty(this.RoundName))
                {
                    sRet = sRet + " / " + this.RoundName;
                }
                if (!string.IsNullOrEmpty(this.SectionName))
                {
                    sRet = sRet + " / " + this.SectionName;
                }
                if (!string.IsNullOrEmpty(this.GroupName))
                {
                    sRet = sRet + " / " + this.GroupName;
                }

                return sRet;
            }
        }

        public string EventTypeListValue
        {
            get
            {
                return this.EventTypeId + "|" + this.RoundId + "|" + this.SectionId + "|" + this.GroupId;
            }
        }

        public EventTypeListItem(int iEventTypeId, string sEventTypeName, int iRoundId, string sRoundName, int iSectionId, string sSectionName, int iGroupId, string sGroupName)
        {
            this.EventTypeId = iEventTypeId;
            this.EventTypeName = sEventTypeName;
            this.RoundId = iRoundId;
            this.RoundName = sRoundName;
            this.SectionId = iSectionId;
            this.SectionName = sSectionName;
            this.GroupId = iGroupId;
            this.GroupName = sGroupName;
        }
    }

}
