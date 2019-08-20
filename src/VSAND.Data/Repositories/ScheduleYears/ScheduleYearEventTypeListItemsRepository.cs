using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;

namespace VSAND.Data.Repositories
{
    public class ScheduleYearEventTypeListItemsRepository : IScheduleYearEventTypeListItemsRepository
    {
        private readonly VsandContext _context;
        public ScheduleYearEventTypeListItemsRepository(VsandContext context)
        {
            _context = context;
        }

        public List<EventTypeListItem> GetActiveEventTypeObjects(int sportId, int scheduleYearId)
        {
            return GetActiveEventTypeObjectsAsync(sportId, scheduleYearId).Result;
        }

        public async Task<List<EventTypeListItem>> GetActiveEventTypeObjectsAsync(int sportId, int scheduleYearId)
        {
            var etList = await (from et in _context.VsandSportEventType
                         .Include(t => t.Rounds)
                         .Include(t => t.Sections)
                         .ThenInclude(s => s.Groups)
                         where et.SportId == sportId && et.ScheduleYearId == scheduleYearId && ((!et.StartDate.HasValue || et.StartDate.Value <= DateTime.Now) 
                         && (!et.EndDate.HasValue || et.EndDate.Value >= DateTime.Now) || (et.DefaultSelected.HasValue && et.DefaultSelected.Value))
                         select et).ToListAsync();

            return FormatEventTypeObjects(etList);
        }

        public List<EventTypeListItem> GetEventTypeObjects(int sportId, int scheduleYearId)
        {
            return GetEventTypeObjectsAsync(sportId, scheduleYearId).Result;
        }

        public async Task<List<EventTypeListItem>> GetEventTypeObjectsAsync(int sportId, int scheduleYearId)
        {
            var etList = await (from et in _context.VsandSportEventType
                        .Include(t => t.Rounds)
                        .Include(t => t.Sections)
                        .ThenInclude(s => s.Groups)
                        where et.SportId == sportId && et.ScheduleYearId == scheduleYearId
                               select et).ToListAsync();

            return FormatEventTypeObjects(etList);
        }

        private static List<EventTypeListItem> FormatEventTypeObjects(List<VsandSportEventType> etList)
        {
            var etItems = new List<EventTypeListItem>();
            
            foreach (VsandSportEventType et in etList)
            {
                if (et.Rounds.Any())
                {
                    foreach (VsandSportEventTypeRound round in et.Rounds)
                    {
                        string roundName = round.Name;
                        DateTime? roundDate = round.StartDate;
                        if (roundDate.HasValue)
                        {
                            if (roundDate.Value.Hour > 0)
                            {
                                roundName += ", " + roundDate.Value.ToString("%h");
                                if (roundDate.Value.Minute > 0)
                                {
                                    roundName += ":" + roundDate.Value.ToString("mm");
                                }

                                if (roundDate.Value.Hour >= 12)
                                {
                                    roundName += "pm";
                                }
                                else
                                {
                                    roundName += "am";
                                }
                            }
                        }

                        if (et.Sections.Any())
                        {
                            foreach (VsandSportEventTypeSection section in et.Sections)
                            {
                                if (section.Groups.Any())
                                {
                                    foreach (VsandSportEventTypeGroup group in section.Groups)
                                    {
                                        etItems.Add(new EventTypeListItem(et.EventTypeId, et.Name, round.RoundId, roundName, section.SectionId, section.Name, group.GroupId, group.Name));
                                    }
                                }
                                else
                                {
                                    etItems.Add(new EventTypeListItem(et.EventTypeId, et.Name, round.RoundId, roundName, section.SectionId, section.Name, 0, ""));
                                }
                            }
                        }
                        else
                        {
                            etItems.Add(new EventTypeListItem(et.EventTypeId, et.Name, round.RoundId, roundName, 0, "", 0, ""));
                        }
                    }
                }
                else
                {
                    if (et.Sections.Any())
                    {
                        foreach (VsandSportEventTypeSection section in et.Sections)
                        {
                            if (section.Groups.Any())
                            {
                                foreach (VsandSportEventTypeGroup group in section.Groups)
                                {
                                    etItems.Add(new EventTypeListItem(et.EventTypeId, et.Name, 0, "", section.SectionId, section.Name, group.GroupId, group.Name));
                                }
                            }
                            else
                            {
                                etItems.Add(new EventTypeListItem(et.EventTypeId, et.Name, 0, "", section.SectionId, section.Name, 0, ""));
                            }
                        }
                    }
                    else
                    {
                        etItems.Add(new EventTypeListItem(et.EventTypeId, et.Name, 0, "", 0, "", 0, ""));
                    }
                }
            }

            return etItems;
        }
    }
}
