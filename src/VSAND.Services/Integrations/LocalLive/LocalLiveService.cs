using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.Integrations.LocalLive;
using VSAND.Data.Repositories;
using VSAND.Data.ViewModels;

namespace VSAND.Services.Integrations.LocalLive
{
    public class LocalLiveService : ILocalLiveService
    {
        public IUnitOfWork _uow;
        public LocalLiveService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<List<Event>> GetAllEventsAsync(string include)
        {
            var oRet = new List<Event>();

            var referenceSchedule = await GetScheduleAsync(include, 1);
            if (referenceSchedule == null)
            {
                return null;
            }

            oRet.AddRange(referenceSchedule.Events);

            // loop here
            for (int i = referenceSchedule.CurrentPage + 1; i < referenceSchedule.TotalPages; i++)
            {
                var schedule = await GetScheduleAsync(include, i);
                if (schedule == null)
                {
                    return oRet;
                }

                oRet.AddRange(schedule.Events);
            }

            return oRet.OrderBy(e => e.StartTime).ToList();
        }

        public async Task<ServiceResult<List<LocalLiveEvent>>> AddEventsAsync(List<Event> events)
        {
            var eventEntities = events.Select(e => new LocalLiveEvent(e)).ToList();
            return await AddEventsAsync(eventEntities);
        }

        public async Task<ServiceResult<List<LocalLiveEvent>>> AddEventsAsync(List<LocalLiveEvent> events)
        {
            var oRet = new ServiceResult<List<LocalLiveEvent>>();

            if (events.Count == 0)
            {
                oRet.Success = false;
                oRet.Message = "Event list is empty. No events were inserted.";

                return oRet;
            }

            int failedInserts = 0;
            int successfulInserts = 0;

            foreach (var @event in events)
            {
                await _uow.LocalLiveEvents.Insert(@event);

                var bInserted = await _uow.Save();
                if (bInserted == true)
                {
                    successfulInserts++;
                }
                else
                {
                    failedInserts++;
                }
            }

            if (successfulInserts > 0)
            {
                oRet.Success = true;

                if (failedInserts == 0)
                {
                    oRet.Message = $"{successfulInserts} successful insert(s).";
                }
                else
                {
                    oRet.Message = $"{successfulInserts} successful insert(s) and {failedInserts} failed insert(s).";
                }
            }
            else
            {
                oRet.Success = false;
                oRet.Message = $"{failedInserts} failed insert(s).";
            }

            return oRet;
        }

        private async Task<Schedule> GetScheduleAsync(string include, int pageNumber)
        {
            var client = new RestClient("http://nj-api.locallive.tv");
            var request = new RestRequest("/Api/NJ_Schedule");

            request.AddQueryParameter("Include", include);
            request.AddQueryParameter("PageNumber", pageNumber.ToString());

            var response = await client.ExecuteTaskAsync(request);

            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<List<Schedule>>(response.Content)[0];
            }

            return null;
        }
    }
}
