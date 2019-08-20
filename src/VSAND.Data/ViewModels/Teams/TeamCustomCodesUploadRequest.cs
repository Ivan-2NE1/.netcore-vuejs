using Microsoft.AspNetCore.Http;

namespace VSAND.Data.ViewModels.Teams
{
    public class TeamCustomCodesUploadRequest
    {
        public string SportName { get; set; }
        public string ScheduleYearName { get; set; }
        public IFormFile File { get; set; }

        public TeamCustomCodesUploadRequest()
        {

        }
    }
}
