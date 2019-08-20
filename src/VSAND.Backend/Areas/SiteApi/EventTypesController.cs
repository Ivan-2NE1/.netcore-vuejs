using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;
using VSAND.Services.Data.Sports;

namespace VSAND.Backend.Areas.SiteApi
{
    [ApiController]
    [Route("SiteApi/[controller]")]
    public class EventTypesController : ControllerBase
    {
        private readonly ISportService _sportService;
        public EventTypesController(ISportService sportService)
        {
            _sportService = sportService;
        }

        // GET: SiteApi/EventTypes/List
        [HttpGet("List", Name = "Get List of Active Event Types for Sport in Schedule Year")]
        public async Task<IEnumerable<ListItem<string>>> GetActiveListAsync([FromQuery] int sportId, [FromQuery] int scheduleYearId)
        {
            return await _sportService.GetActiveEventTypes(sportId, scheduleYearId);
        }

        // GET: SiteApi/EventTypes/ListAll
        [HttpGet("ListAll", Name = "Get List of All Event Types for Sport in Schedule Year")]
        public async Task<IEnumerable<ListItem<string>>> GetListAsync([FromQuery] int sportId, [FromQuery] int scheduleYearId)
        {
            return await _sportService.GetAllEventTypes(sportId, scheduleYearId);
        }

        #region Event Types
        // POST: SiteApi/EventTypes
        [HttpPost]
        public async Task<ApiResult<VsandSportEventType>> AddEventType([FromBody] VsandSportEventType addEventType)
        {
            var result = await _sportService.AddEventTypeAsync(addEventType);
            return new ApiResult<VsandSportEventType>(result);
        }

        // PUT: SiteApi/EventTypes/5
        [HttpPut("{eventTypeId}")]
        public async Task<ApiResult<VsandSportEventType>> UpdateEventType(int eventTypeId, [FromBody] VsandSportEventType chgEventType)
        {
            if (eventTypeId != chgEventType.EventTypeId)
            {
                return null;
            }

            var result = await _sportService.UpdateEventTypeAsync(chgEventType);
            return new ApiResult<VsandSportEventType>(result);
        }

        // DELETE: SiteApi/EventTypes/5
        [HttpDelete("{eventTypeId}")]
        public async Task<ApiResult<VsandSportEventType>> DeleteEventType(int eventTypeId)
        {
            var result = await _sportService.DeleteEventTypeAsync(eventTypeId);
            return new ApiResult<VsandSportEventType>(result);
        }
        #endregion

        #region Event Type Rounds
        // POST: SiteApi/EventTypes/5/Rounds
        [HttpPost("{eventTypeId}/Rounds")]
        public async Task<ApiResult<VsandSportEventTypeRound>> AddEventTypeRound(int eventTypeId, VsandSportEventTypeRound viewModel)
        {
            if (viewModel.EventTypeId != eventTypeId)
            {
                return null;
            }

            var result = await _sportService.AddEventTypeRoundAsync(viewModel);
            return new ApiResult<VsandSportEventTypeRound>(result);
        }

        // PUT: SiteApi/EventTypes/5/Rounds/12
        [HttpPut("{eventTypeId}/Rounds/{roundId}")]
        public async Task<ApiResult<VsandSportEventTypeRound>> UpdateEventTypeRound(int eventTypeId, int roundId, VsandSportEventTypeRound viewModel)
        {
            if (eventTypeId != viewModel.EventTypeId)
            {
                return null;
            }

            if (roundId != viewModel.RoundId)
            {
                return null;
            }

            var result = await _sportService.UpdateEventTypeRoundAsync(viewModel);
            return new ApiResult<VsandSportEventTypeRound>(result);
        }

        // DELETE: SiteApi/EventTypes/5/Rounds/12
        [HttpDelete("{eventTypeId}/Rounds/{roundId}")]
        public async Task<ApiResult<VsandSportEventTypeRound>> DeleteEventTypeRound(int roundId)
        {
            ServiceResult<VsandSportEventTypeRound> result = await _sportService.DeleteEventTypeRoundAsync(roundId);
            return new ApiResult<VsandSportEventTypeRound>(result);
        }

        // PUT: SiteApi/EventTypes/5/Rounds/SortOrder/Update
        [HttpPut("{eventTypeId}/Rounds/SortOrder/Update")]
        public async Task<bool> UpdateEventTypeRoundOrder(int eventTypeId, [FromBody] List<VsandSportEventTypeRound> rounds)
        {
            return await _sportService.UpdateEventTypeRoundOrder(eventTypeId, rounds);
        }
        #endregion

        #region Event Type Sections
        // POST: SiteApi/EventTypes/5/Sections
        [HttpPost("{eventTypeId}/Sections")]
        public async Task<ApiResult<VsandSportEventTypeSection>> AddEventTypeSection(int eventTypeId, VsandSportEventTypeSection viewModel)
        {
            if (viewModel.EventTypeId != eventTypeId)
            {
                return null;
            }

            var result = await _sportService.AddEventTypeSectionAsync(viewModel);
            return new ApiResult<VsandSportEventTypeSection>(result);
        }

        // PUT: SiteApi/EventTypes/5/Sections/12
        [HttpPut("{eventTypeId}/Sections/{sectionId}")]
        public async Task<ApiResult<VsandSportEventTypeSection>> UpdateEventTypeSection(int eventTypeId, int sectionId, VsandSportEventTypeSection viewModel)
        {
            if (eventTypeId != viewModel.EventTypeId)
            {
                return null;
            }

            if (sectionId != viewModel.SectionId)
            {
                return null;
            }

            var result = await _sportService.UpdateEventTypeSectionAsync(viewModel);
            return new ApiResult<VsandSportEventTypeSection>(result);
        }

        // DELETE: SiteApi/EventTypes/5/Sections/12
        [HttpDelete("{eventTypeId}/Sections/{sectionId}")]
        public async Task<ApiResult<VsandSportEventTypeSection>> DeleteEventTypeSection(int sectionId)
        {
            ServiceResult<VsandSportEventTypeSection> result = await _sportService.DeleteEventTypeSectionAsync(sectionId);
            return new ApiResult<VsandSportEventTypeSection>(result);
        }

        // PUT: SiteApi/EventTypes/5/Sections/SortOrder/Update
        [HttpPut("{eventTypeId}/Sections/SortOrder/Update")]
        public async Task<bool> UpdateEventTypeSectionOrder(int eventTypeId, [FromBody] List<VsandSportEventTypeSection> sections)
        {
            return await _sportService.UpdateEventTypeSectionOrder(eventTypeId, sections);
        }
        #endregion

        #region Event Type Groups
        // POST: SiteApi/EventTypes/Sections/5/Groups
        [HttpPost("Sections/{sectionId}/Groups")]
        public async Task<ApiResult<VsandSportEventTypeGroup>> AddEventTypeGroup(int sectionId, VsandSportEventTypeGroup viewModel)
        {
            if (viewModel.SectionId != sectionId)
            {
                return null;
            }

            var result = await _sportService.AddEventTypeGroupAsync(viewModel);
            return new ApiResult<VsandSportEventTypeGroup>(result);
        }

        // PUT: SiteApi/ScheduleYears/EventType/Sections/5/Groups/12
        [HttpPut("Sections/{sectionId}/Groups/{groupId}")]
        public async Task<ApiResult<VsandSportEventTypeGroup>> UpdateEventTypeGroup(int sectionId, int groupId, VsandSportEventTypeGroup viewModel)
        {
            if (sectionId != viewModel.SectionId)
            {
                return null;
            }

            if (groupId != viewModel.GroupId)
            {
                return null;
            }

            var result = await _sportService.UpdateEventTypeGroupAsync(viewModel);
            return new ApiResult<VsandSportEventTypeGroup>(result);
        }

        // DELETE: SiteApi/EventTypes/Sections/5/Groups/12
        [HttpDelete("Sections/{sectionId}/Groups/{groupId}")]
        public async Task<ApiResult<VsandSportEventTypeGroup>> DeleteEventTypeGroup(int groupId)
        {
            ServiceResult<VsandSportEventTypeGroup> result = await _sportService.DeleteEventTypeGroupAsync(groupId);
            return new ApiResult<VsandSportEventTypeGroup>(result);
        }

        // PUT: SiteApi/EventTypes/Sections/5/Groups/SortOrder/Update
        [HttpPut("Sections/{sectionId}/Groups/SortOrder/Update")]
        public async Task<bool> UpdateEventTypeGroupOrder(int sectionId, [FromBody] List<VsandSportEventTypeGroup> groups)
        {
            return await _sportService.UpdateEventTypeGroupOrder(sectionId, groups);
        }
        #endregion
    }
}