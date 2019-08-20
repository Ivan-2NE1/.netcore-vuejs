using System;
using System.Collections.Generic;
using System.Text;

namespace VSAND.Data.ViewModels.ScheduleYears
{
    public class ScheduleLoadFileRow
    {
        public string TeamName { get; set; }
        public string OpponentName { get; set; }
        public string HomeAway { get; set; }
        public DateTime? EventDate { get; set; }
        public DateTime? EventTime { get; set; }

        public ScheduleLoadFileRow()
        {

        }

        public static ScheduleLoadFileRow ImportRow(IList<string> rowData, IList<string> columnNames)
        {
            try
            {
                DateTime? eDate = null;
                DateTime? eTime = null;
                DateTime tmpDate = new DateTime();

                if (DateTime.TryParse(rowData[columnNames.IndexOf("eventdate")], out tmpDate))
                {
                    eDate = tmpDate;
                }

                if (DateTime.TryParse(rowData[columnNames.IndexOf("eventtime")], out tmpDate))
                {
                    eTime = tmpDate;
                }

                var fileData = new ScheduleLoadFileRow()
                {
                    TeamName = rowData[columnNames.IndexOf("hometeamname")].Trim(),
                    OpponentName = rowData[columnNames.IndexOf("opponentnameawayteam")].Trim(),
                    HomeAway = rowData[columnNames.IndexOf("homeaway")].Trim(),
                    EventDate = eDate,
                    EventTime = eTime
                };
                return fileData;
            } catch(Exception ex)
            {
                // whatever
            }
            return null;
            
        }
    }
}
