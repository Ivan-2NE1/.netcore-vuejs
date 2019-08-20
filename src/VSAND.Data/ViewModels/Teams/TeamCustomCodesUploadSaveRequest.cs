using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.Teams
{
    public class TeamCustomCodesUploadSaveRequest
    {
        public int SportId { get; set; }
        public int ScheduleYearId { get; set; }
        public List<VsandTeamCustomCode> Codes { get; set; }

        public TeamCustomCodesUploadSaveRequest()
        {

        }
    }
}
