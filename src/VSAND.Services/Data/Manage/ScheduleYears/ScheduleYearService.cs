using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.Identity;
using VSAND.Data.Repositories;
using VSAND.Data.ViewModels;
using VSAND.Data.ViewModels.ScheduleYears;
using VSAND.Services.Cache;
using VSAND.Services.Data.GameReports;
using VSAND.Services.Files;
using VSAND.Services.Hubs;

namespace VSAND.Services.Data.Manage.ScheduleYears
{
    public class ScheduleYearService : IScheduleYearService
    {
        private readonly IUnitOfWork _uow;
        private readonly ICache _cache;
        private readonly IHubContext<SchedulingHub> _schedulingHub;
        private readonly IGameReportService _gameReportService;

        private readonly IConfiguration Configuration;
        public ScheduleYearService(IUnitOfWork uow, ICache cache, IConfiguration configuration, IGameReportService gameReportService, IHubContext<SchedulingHub> schedulingHub)
        {
            _uow = uow ?? throw new ArgumentException("Unit of Work is null");
            _cache = cache ?? throw new ArgumentException("Cache is null");
            _gameReportService = gameReportService ?? throw new ArgumentException("Game Report Service is null");
            _schedulingHub = schedulingHub ?? throw new ArgumentException("Scheduling Hub is null");
            Configuration = configuration;
        }

        public async Task<ServiceResult<ScheduleYear>> AddScheduleYearAsync(ScheduleYear addScheduleYear)
        {
            var oRet = new ServiceResult<ScheduleYear>();

            // we can do the insert, it won't create a duplicate
            var oSy = new VsandScheduleYear()
            {
                Name = addScheduleYear.Name,
                EndYear = addScheduleYear.EndYear,
                Active = false
            };
            await _uow.ScheduleYears.Insert(oSy);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = addScheduleYear;
                oRet.Success = true;
                oRet.Id = oSy.ScheduleYearId;

                //TODO: This is the layer that the cache engine should be invoked for states (frequently used)
            }
            return oRet;
        }

        public async Task<ServiceResult<ScheduleYear>> SetActiveScheduleYearAsync(int scheduleYearId)
        {
            var oRet = new ServiceResult<ScheduleYear>();

            // Step 1. Remove the Active flag from any active schedule year
            // Step 2. Set the selected schedule year to active
            // Step 3. Clear/Expire relevant caches related to active schedule year

            _uow.BeginTransaction();
            var activeScheduleYears = await _uow.ScheduleYears.List(sy => sy.Active.HasValue && sy.Active.Value == true);
            foreach(var activeSy in activeScheduleYears)
            {
                activeSy.Active = false;
                _uow.ScheduleYears.Update(activeSy);
            }

            var targetScheduleYear = await _uow.ScheduleYears.GetById(scheduleYearId);
            if (targetScheduleYear == null)
            {
                oRet.Message = "The target active schedule year could not be found.";
                return oRet;
            }
            targetScheduleYear.Active = true;
            _uow.ScheduleYears.Update(targetScheduleYear);

            bool saved = await _uow.CommitTransaction();

            if (!saved)
            {
                var lastEx = _uow.GetLastError();
                if (lastEx != null)
                {
                    oRet.Message = $"There was a problem setting the selected schedule year active: {lastEx.Message}";
                } else
                {
                    oRet.Message = "There was a problem setting the selected schdule year active.";
                }
                return oRet;
            }

            oRet.Success = true;
            oRet.Id = targetScheduleYear.ScheduleYearId;
            oRet.obj = new VSAND.Data.ViewModels.ScheduleYear(targetScheduleYear);

            string cacheKey = Cache.Keys.ActiveScheduleYear();
            await _cache.RemoveAsync(cacheKey);
                       
            return oRet;
        }

        public async Task<IEnumerable<ListItem<int>>> GetList()
        {
            //TODO: ScheduleYearService GetList needs some hard caching
            var oResult = await _uow.ScheduleYears.List(null, x => x.OrderByDescending(sy => sy.EndYear));
            var oRet = new List<ListItem<int>>();
            foreach (VSAND.Data.Entities.VsandScheduleYear sy in oResult)
            {
                oRet.Add(new ListItem<int>(sy.ScheduleYearId, sy.Name));
            }
            return oRet;
        }

        public async Task<ScheduleYear> GetSummary(int scheduleYearId)
        {
            if (scheduleYearId <= 0)
            {
                return null;
            }

            var oSy = await _uow.ScheduleYears.GetById(scheduleYearId);
            var oRet = new ScheduleYear(oSy);

            // now, get our list of summary counts for each sport and attach them
            oRet.Summary = await _uow.ScheduleYearSummaries.GetListAsync(scheduleYearId);

            return oRet;
        }

        public async Task<ScheduleYear> GetProvisioning(int scheduleYearId, int sportId)
        {
            if (scheduleYearId <= 0 || sportId <= 0)
            {
                return null;
            }

            var oSy = await _uow.ScheduleYears.GetById(scheduleYearId);
            var oRet = new ScheduleYear(oSy);
            oRet.SportInfo = await _uow.Sports.SportItem(sportId);

            // now, get our list of summary counts for each sport and attach them
            oRet.ProvisioningSummary = await _uow.ScheduleYearProvisioningSummaries.GetListAsync(scheduleYearId, sportId);

            return oRet;
        }

        /// <summary>
        /// Returns the list of all schedule years in the system, with the Active schedule year first, 
        /// followed by others in reverse order by End Year
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ScheduleYear>> List()
        {
            var oScheduleYears = await _uow.ScheduleYears.List();
            var oRet = new List<ScheduleYear>();
            var oActive = oScheduleYears.FirstOrDefault(sy => sy.Active.HasValue && sy.Active.Value == true);
            if (oActive != null)
            {
                oRet.Add(new ScheduleYear(oActive));
            }
            oRet.AddRange(from sy in oScheduleYears
                          where !sy.Active.HasValue || (sy.Active.HasValue && sy.Active.Value == false)
                          orderby sy.EndYear descending
                          select new ScheduleYear(sy));

            return oRet;
        }

        public async Task<VsandScheduleYear> GetScheduleYear(int scheduleYearId)
        {
            var oSy = await _uow.ScheduleYears.GetById(scheduleYearId);
            return oSy;
        }

        public async Task<ScheduleYear> GetActiveScheduleYear()
        {
            var oSy = await _uow.ScheduleYears.Single(sy => sy.Active.HasValue && sy.Active.Value);
            return new ScheduleYear(oSy);
        }

        public async Task<ScheduleYear> GetActiveScheduleYearCachedAsync()
        {
            string cacheKey = Cache.Keys.ActiveScheduleYear();
            var oSy = await _cache.GetAsync<ScheduleYear>(cacheKey);
            if (oSy == null)
            {
                oSy = await GetActiveScheduleYear();
                if (oSy != null)
                {
                    await _cache.SetAsync(cacheKey, oSy);
                }
            }

            return oSy;
        }

        public int GetLastScheduleYearId(int scheduleYearId)
        {
            return _uow.ScheduleYears.GetLastScheduleYearId(scheduleYearId);
        }

        #region Schedules
        public async Task<List<VsandScheduleLoadFile>> GetScheduleFiles(int scheduleYearId, int sportId)
        {
            return await _uow.ScheduleLoadFiles.List(slf => slf.ScheduleYearId == scheduleYearId && slf.SportId == sportId);
        }

        public async Task<VsandScheduleLoadFile> GetScheduleFileRecord(int fileId)
        {
            return await _uow.ScheduleLoadFiles.Single(slf => slf.FileId == fileId);
        }

        public async Task<ServiceResult<VsandScheduleLoadFile>> UploadScheduleFile(int scheduleYearId, int sportId, string fileName, string fileExt, long fileSize)
        {
            var oRet = new ServiceResult<VsandScheduleLoadFile>();

            _uow.BeginTransaction();

            var file = new VsandScheduleLoadFile()
            {
                FileName = fileName,
                FileType = fileExt,
                FileSize = (int)fileSize,
                FullName = fileName,
                DateLoaded = DateTime.Now,
                ScheduleYearId = scheduleYearId,
                SportId = sportId
            };

            await _uow.ScheduleLoadFiles.Insert(file);

            bool saved = await _uow.CommitTransaction();

            if (!saved)
            {
                var message = "There was a problem saving the uploaded file";
                var lastError = _uow.GetLastError();
                if (lastError != null)
                {
                    message = $"{message}: {lastError.Message}";
                }
                return oRet;
            }

            oRet.Success = true;
            oRet.Id = file.FileId;
            oRet.obj = file;
            return oRet;


        }

        public async Task ImportScheduleFile(int scheduleYearId, int sportId, int fileId, List<ScheduleLoadFileRow> scheduleData)
        {
            if (scheduleData == null || !scheduleData.Any())
            {
                return;
            }

            var fileItemsCount = await _uow.ScheduleLoadFileRows.Count(r => r.FileId == fileId);
            if (fileItemsCount > 0)
            {
                return;
            }

            var status = new ProcessingStatus
            {
                Current = 0,
                Total = scheduleData.Count
            };
            
            var roomName = $"fileId={fileId}";

            _uow.BeginTransaction();

            var schoolList = new Dictionary<string, int?>();
            var teamList = new Dictionary<string, int?>();

            // process each of the rows that belong to this file
            foreach(ScheduleLoadFileRow schedItem in scheduleData)
            {
                DateTime? eventDate = null;
                if (schedItem.EventDate.HasValue)
                {
                    eventDate = schedItem.EventDate;

                    if (schedItem.EventTime.HasValue)
                    {
                        eventDate = new DateTime(eventDate.Value.Year, eventDate.Value.Month, eventDate.Value.Day, schedItem.EventTime.Value.Hour, schedItem.EventTime.Value.Minute, 0);
                    }
                }

                int? teamSchoolId = null;
                int? teamTeamId = null;
                int? oppSchoolId = null;
                int? oppTeamId = null;

                if (schoolList.ContainsKey(schedItem.TeamName))
                {
                    teamSchoolId = schoolList[schedItem.TeamName];
                } else
                {
                    // look up the school by name
                    var school = await _uow.Schools.Single(s => s.Name == schedItem.TeamName);
                    if (school != null)
                    {
                        schoolList.Add(schedItem.TeamName, school.SchoolId);
                        teamSchoolId = school.SchoolId;
                    }
                }

                if (teamList.ContainsKey(schedItem.TeamName))
                {
                    teamTeamId = teamList[schedItem.TeamName];
                } else
                {
                    if (teamSchoolId.HasValue)
                    {
                        var team = await _uow.Teams.Single(t => t.SchoolId == teamSchoolId.Value && t.ScheduleYearId == scheduleYearId && t.SportId == sportId);
                        if (team != null)
                        {
                            teamList.Add(schedItem.TeamName, team.TeamId);
                            teamTeamId = team.TeamId;
                        }
                    }
                }

                if (schoolList.ContainsKey(schedItem.OpponentName))
                {
                    oppSchoolId = schoolList[schedItem.OpponentName];
                }
                else
                {
                    // look up the school by name
                    var school = await _uow.Schools.Single(s => s.Name == schedItem.OpponentName);
                    if (school != null)
                    {
                        schoolList.Add(schedItem.OpponentName, school.SchoolId);
                        oppSchoolId = school.SchoolId;
                    }
                }

                if (teamList.ContainsKey(schedItem.OpponentName))
                {
                    oppTeamId = teamList[schedItem.OpponentName];
                }
                else
                {
                    if (oppSchoolId.HasValue)
                    {
                        var team = await _uow.Teams.Single(t => t.SchoolId == oppSchoolId.Value && t.ScheduleYearId == scheduleYearId && t.SportId == sportId);
                        if (team != null)
                        {
                            teamList.Add(schedItem.OpponentName, team.TeamId);
                            oppTeamId = team.TeamId;
                        }
                    }
                }

                var fileParse = new VsandScheduleLoadFileParse()
                {
                    FileId = fileId,
                    TeamSchoolName = schedItem.TeamName.Trim(),
                    TeamSchoolId = teamSchoolId,
                    TeamId = teamTeamId,
                    OpponentSchoolName = schedItem.OpponentName.Trim(),
                    OpponentSchoolId = oppSchoolId,
                    OpponentTeamId = oppTeamId,
                    HomeAway = schedItem.HomeAway.Trim(),
                    EventDate = eventDate
                };

                await _uow.ScheduleLoadFileRows.Insert(fileParse);

                status.Current += 1;

                if (status.Current % 10 == 0 || status.Current == status.Total)
                {
                    await _schedulingHub.Clients.Group(roomName).SendAsync("importStatus", status);
                }
            }

            bool saved = await _uow.CommitTransaction();

            //TODO: we could send another message here with any error information (also using signalr)
            if (!saved)
            {
                var message = "There was a problem processing the uploaded file";
                var lastError = _uow.GetLastError();
                if (lastError != null)
                {
                    message = $"{message}: {lastError.Message}";
                }
            }
        }

        public async Task CommitScheduleFile(AppxUser user, int scheduleYearId, int sportId, int fileId)
        {
            var roomName = $"fileId={fileId}";

            var file = await _uow.ScheduleLoadFiles.Single(f => f.ScheduleYearId == scheduleYearId && f.SportId == sportId && f.FileId == fileId);
            if (file == null)
            {
                await _schedulingHub.Clients.Group(roomName).SendAsync("abortMessage", "File could not be loaded");
                return;
            }

            var scheduleItems = await _uow.ScheduleLoadFileRows.List(r => r.FileId == fileId && !r.ScheduleId.HasValue, null, new List<string> { "Team" });

            if (scheduleItems == null || !scheduleItems.Any())
            {
                await _schedulingHub.Clients.Group(roomName).SendAsync("abortMessage", "No rows loaded to commit from file");
                return;
            }

            var defaultEventType = await _uow.SportEventTypes.Single(t => t.SportId == sportId && t.ScheduleYearId == scheduleYearId && t.DefaultSelected.HasValue && t.DefaultSelected.Value == true);
            if (defaultEventType == null)
            {
                await _schedulingHub.Clients.Group(roomName).SendAsync("abortMessage", "Default event type not set");
                return;
            }

            bool allowMultipleEvents = false;
            var sport = await _uow.Sports.Single(s => s.SportId == sportId);
            if (sport != null)
            {
                allowMultipleEvents = sport.AllowMultiEventPerDay;
            }

            var status = new ProcessingStatus
            {
                Current = 0,
                Total = scheduleItems.Count
            };
                        
            foreach(var schedItem in scheduleItems)
            {
                // skip items that were already scheduled
                if (schedItem.ScheduleId.HasValue)
                {
                    // update our counts and our signalr processing here
                    status.Current += 1;

                    if (status.Current % 10 == 0 || status.Current == status.Total)
                    {
                        await _schedulingHub.Clients.Group(roomName).SendAsync("commitStatus", status);
                    }
                    continue;
                }

                var refTeamId = 0;
                var participants = CreateParticipantList(schedItem, ref refTeamId);

                if (refTeamId > 0)
                {
                    if (participants.Any() && schedItem.EventDate.HasValue)
                    {
                        // we are checking for an existing scheduled item for the same teams on the same day (or at the same time)
                        var foundCount = 0;
                        var eventYear = schedItem.EventDate.Value.Year;
                        var eventMonth = schedItem.EventDate.Value.Month;
                        var eventDay = schedItem.EventDate.Value.Day;
                        var eventHour = schedItem.EventDate.Value.Hour;
                        var eventMin = schedItem.EventDate.Value.Minute;

                        if (allowMultipleEvents)
                        {
                            foundCount = await _uow.GameReports.Count(gr => gr.Teams.Any(grt => grt.TeamId == refTeamId) && 
                                gr.GameDate.Year == eventYear && 
                                gr.GameDate.Month == eventMonth && 
                                gr.GameDate.Day == eventDay && 
                                gr.GameDate.Hour == eventHour && 
                                gr.GameDate.Minute == eventMin);
                        } else
                        {
                            foundCount = await _uow.GameReports.Count(gr => gr.Teams.Any(grt => grt.TeamId == refTeamId) &&
                                gr.GameDate.Year == eventYear &&
                                gr.GameDate.Month == eventMonth &&
                                gr.GameDate.Day == eventDay);
                        }

                        if (foundCount == 0)
                        {
                            // create the game
                            var oSchedGame = new AddScheduledGame()
                            {
                                SportId = sportId,
                                refTeamId = refTeamId,
                                EventTypeId = defaultEventType.EventTypeId,
                                GameDate = schedItem.EventDate.Value,
                                ScheduleYearId = scheduleYearId,
                                Teams = participants
                            };

                            var gameId = 0;
                            var gameResult = await _gameReportService.AddScheduledGameAsync(user, oSchedGame);
                            if (gameResult.Success)
                            {
                                gameId = gameResult.Id;
                            }

                            if (gameId > 0)
                            {
                                schedItem.ScheduleId = gameId;
                                _uow.ScheduleLoadFileRows.Update(schedItem);
                            }
                        } else
                        {
                            schedItem.ScheduleId = 0;
                            _uow.ScheduleLoadFileRows.Update(schedItem);
                        }
                    }
                } else
                {
                    schedItem.ScheduleId = 0;
                    _uow.ScheduleLoadFileRows.Update(schedItem);
                }

                // update our counts and our signalr processing here
                status.Current += 1;

                if (status.Current % 10 == 0 || status.Current == status.Total)
                {
                    await _schedulingHub.Clients.Group(roomName).SendAsync("commitStatus", status);
                }
            }

            file.ImportDate = DateTime.Now;
            _uow.ScheduleLoadFiles.Update(file);

            await _uow.Save();
        }

        private List<ParticipatingTeam> CreateParticipantList(VsandScheduleLoadFileParse oFileParseRow, ref int refTeamId)
        {
            List<ParticipatingTeam> oParticipants = new List<ParticipatingTeam>();
            ParticipatingTeam oHostTeam = new ParticipatingTeam();
            ParticipatingTeam oOtherteam = new ParticipatingTeam();

            bool bHome = oFileParseRow.HomeAway == "H";
            if (oFileParseRow.Team != null)
            {
                refTeamId = oFileParseRow.Team.TeamId;
                oHostTeam = new ParticipatingTeam(oFileParseRow.Team.TeamId, oFileParseRow.TeamSchoolName, bHome, 0);
                if (oFileParseRow.OpponentTeam != null)
                {
                    oOtherteam = new ParticipatingTeam(oFileParseRow.OpponentTeam.TeamId, oFileParseRow.OpponentSchoolName, !bHome, 0);
                }                    
                else
                {
                    oOtherteam = new ParticipatingTeam(0, oFileParseRow.OpponentSchoolName, !bHome, 0);
                }
            }
            else if (oFileParseRow.OpponentTeam != null)
            {
                oHostTeam = new ParticipatingTeam(oFileParseRow.OpponentTeam.TeamId, oFileParseRow.OpponentSchoolName, bHome, 0);
                oOtherteam = new ParticipatingTeam(0, oFileParseRow.TeamSchoolName, !bHome, 0);
                refTeamId = oFileParseRow.OpponentTeam.TeamId;
            }
            else
            {
                refTeamId = 0;
                return new List<ParticipatingTeam>();
            }
            oParticipants.Add(oHostTeam);
            oParticipants.Add(oOtherteam);
            return oParticipants;
        }

        public async Task<List<ScheduleFileResolveItem>> GetScheduleFile(int scheduleYearId, int sportId, int fileId)
        {
            var file = await _uow.ScheduleLoadFiles.Single(slf => slf.FileId == fileId && slf.ScheduleYearId == scheduleYearId && slf.SportId == sportId, null, new List<string> { "FileRows" });

            var ret = new List<ScheduleFileResolveItem>();

            var fileRows = file.FileRows.ToList();
            foreach(var fileRow in file.FileRows)
            {
                var refName = fileRow.TeamSchoolName;

                if (!ret.Any(ri => ri.Name.Equals(refName, StringComparison.OrdinalIgnoreCase)))
                {
                    var team1 = new ScheduleFileResolveItem(fileRows, fileRow.TeamSchoolName, fileRow.TeamSchoolId, fileRow.TeamId);
                    if (!team1.SchoolId.HasValue)
                    {
                        team1.ResolveMethod = "Search";
                        var resolveToOpts = _uow.Schools.FindPossibleAltSchoolNameList(fileRow.TeamSchoolName);
                        if (resolveToOpts.Any())
                        {
                            team1.ResolveMethod = "Selected";
                            if (resolveToOpts.Count() == 1)
                            {
                                team1.ResolveToSchoolId = resolveToOpts.FirstOrDefault().id;
                            }
                        }
                        team1.ResolveToChoices = resolveToOpts;
                    }
                    ret.Add(team1);
                }

                if (!ret.Any(ri => ri.Name.Equals(fileRow.OpponentSchoolName, StringComparison.OrdinalIgnoreCase)))
                {
                    var team2 = new ScheduleFileResolveItem(fileRows, fileRow.OpponentSchoolName, fileRow.OpponentSchoolId, fileRow.OpponentTeamId);
                    if (!team2.SchoolId.HasValue)
                    {
                        team2.ResolveMethod = "Search";
                        var resolveToOpts = _uow.Schools.FindPossibleAltSchoolNameList(fileRow.OpponentSchoolName);
                        if (resolveToOpts.Any())
                        {
                            team2.ResolveMethod = "Selected";
                            if (resolveToOpts.Count() == 1)
                            {
                                team2.ResolveToSchoolId = resolveToOpts.FirstOrDefault().id;
                            }
                        }
                        team2.ResolveToChoices = resolveToOpts;
                    }

                    ret.Add(team2);
                }
            }

            return ret;
        }

        public async Task<ServiceResult<bool>> ResolveScheduleItem(ApplicationUser user, int fileId, ScheduleFileResolvedItem resolved)
        {
            var oResult = new ServiceResult<bool>();

            var file = await _uow.ScheduleLoadFiles.Single(f => f.FileId == fileId);
            if (file == null)
            {
                oResult.Message = "Could not find the file!";
                return oResult;
            }

            int sportId = file.SportId;
            int scheduleYearId = file.ScheduleYearId;
            
            _uow.BeginTransaction();

            int teamId = 0;
            string schoolName = resolved.RenameTo;
            // try to find a team for this school
            if (resolved.SchoolId > 0)
            {
                var school = await _uow.Schools.Single(s => s.SchoolId == resolved.SchoolId);
                if (school == null)
                {
                    oResult.Message = "Could not find the school?";
                    return oResult;
                }

                schoolName = school.Name.Trim();
            } else
            {
                if (resolved.SchoolId == 0) // They want to create the school
                {
                    var createMsg = "";
                    resolved.SchoolId = _uow.Schools.AddSchool(schoolName, "", "", "", resolved.City.Trim(), resolved.State, "", "", resolved.PrivateSchool, resolved.CountyId, "", "", "", "", "", "", "", false, ref createMsg, user);
                    if (resolved.SchoolId == 0)
                    {
                        oResult.Message = $"There was a problem creating the school: {createMsg}";
                        return oResult;
                    }
                }

                if (resolved.SchoolId == 0)
                {
                    oResult.Message = "There was a problem creating the school";
                    return oResult;
                }
            }

            if (resolved.SchoolId > 0)
            {
                teamId = await _uow.Teams.GetSchoolTeamIdAsync(sportId, resolved.SchoolId, scheduleYearId);
                if (teamId == 0)
                {
                    // need to create the team for this school
                    teamId = await _uow.Teams.QuickAddTeamAsync(schoolName, resolved.SchoolId, sportId, scheduleYearId);
                }
            }
            
            var affected = await _uow.ScheduleLoadFileRows.List(r => r.FileId == fileId && (r.TeamSchoolName == resolved.Name || r.OpponentSchoolName == resolved.Name));
            foreach(var row in affected)
            {
                if (row.TeamSchoolName.Equals(resolved.Name, StringComparison.OrdinalIgnoreCase))
                {
                    row.TeamSchoolId = resolved.SchoolId;
                    row.TeamId = teamId;
                    row.TeamSchoolName = schoolName;
                    _uow.ScheduleLoadFileRows.Update(row);

                } else if (row.OpponentSchoolName.Equals(resolved.Name, StringComparison.OrdinalIgnoreCase))
                {
                    row.OpponentSchoolId = resolved.SchoolId;
                    row.OpponentTeamId = teamId;
                    row.OpponentSchoolName = schoolName;
                    _uow.ScheduleLoadFileRows.Update(row);
                }
            }

            bool saved = await _uow.CommitTransaction();

            if (!saved)
            {
                oResult.Message = "There was a problem saving your resolution for this import row";
                var lastEx = _uow.GetLastError();
                if (lastEx != null)
                {
                    oResult.Message = $"{oResult.Message}: {lastEx.Message}";
                }
                return oResult;
            }

            oResult.Success = true;
            return oResult;
        }

        #endregion

        #region Power Points
        public async Task<VsandPowerPointsConfig> GetPowerPointsConfig(int scheduleYearId, int sportId)
        {
            var powerPoints = await _uow.PowerPointsConfig.Single(ppc => ppc.ScheduleYearId == scheduleYearId && ppc.SportId == sportId);
            return powerPoints;
        }

        public async Task<ServiceResult<VsandPowerPointsConfig>> InsertOrUpdatePowerPoints(VsandPowerPointsConfig ppConfig)
        {
            var oRet = new ServiceResult<VsandPowerPointsConfig>();

            if (ppConfig.PPConfigId == 0)
            {
                await _uow.PowerPointsConfig.Insert(ppConfig);
            }
            else
            {
                _uow.PowerPointsConfig.Update(ppConfig);
            }

            var bSaved = await _uow.Save();
            if (bSaved == false)
            {
                oRet.Success = false;
                oRet.Message = "An error occurred while saving the power points config.";
                return oRet;
            }

            oRet.Success = true;
            oRet.Id = ppConfig.PPConfigId;
            oRet.obj = ppConfig;

            return oRet;
        }
        #endregion

        #region League Rules
        public async Task<List<VsandLeagueRule>> GetLeagueRulesAsync(int scheduleYearId, int sportId)
        {
            return await _uow.LeagueRules.List(r => r.ScheduleYearId == scheduleYearId && r.SportId == sportId, null, new List<string> { "RuleItems" });
        }

        public async Task<ServiceResult<LeagueRule>> UpdateLeagueRule(LeagueRule leagueRuleVm)
        {
            var oRet = new ServiceResult<LeagueRule>();

            var leagueRule = leagueRuleVm.ToEntityModel();

            // these are in the format ConferenceName|DivisionName
            List<string> receivedRuleItems = leagueRuleVm.RuleItems.Select(ri => ri.id).ToList();

            List<VsandLeagueRuleItem> dbRuleItems = await _uow.LeagueRuleItems.List(lri => lri.LeagueRuleId == leagueRule.LeagueRuleId);

            _uow.BeginTransaction();

            foreach (var dbRuleItem in dbRuleItems)
            {
                if (!receivedRuleItems.Contains($"{dbRuleItem.Conference}|{dbRuleItem.Division}"))
                {
                    _uow.LeagueRuleItems.Delete(dbRuleItem);
                }
            }

            foreach (var recievedRuleItem in receivedRuleItems)
            {
                var parts = recievedRuleItem.Split('|');
                if (parts.Length == 2)
                {
                    var conferenceName = parts[0].Trim();
                    var divisonName = parts[1].Trim();

                    if (!dbRuleItems.Any(lri => lri.Conference == conferenceName && lri.Division == divisonName))
                    {
                        var leagueRuleItem = new VsandLeagueRuleItem
                        {
                            Conference = conferenceName,
                            Division = divisonName,
                            LeagueRuleId = leagueRule.LeagueRuleId
                        };

                        await _uow.LeagueRuleItems.Insert(leagueRuleItem);
                    }
                }
                else
                {
                    oRet.Success = false;
                    oRet.Message = "The league rule is in an invalid format. Please refresh and try again.";

                    return oRet;
                }
            }

            _uow.LeagueRules.Update(leagueRule);

            var bSaved = await _uow.Save();
            if (bSaved == false)
            {
                oRet.Success = false;
                oRet.Message = "An error occurred while trying to save the league rule. No changes have been made.";

                return oRet;
            }

            var bCommitted = await _uow.CommitTransaction();
            if (bCommitted == false)
            {
                oRet.Success = false;
                oRet.Message = "An error occurred while trying to commit the changes to the league rule. No changes have been made.";

                return oRet;
            }

            oRet.Success = true;
            oRet.Message = "League rule has been successfully updated.";

            return oRet;
        }

        public async Task<ServiceResult<VsandLeagueRule>> PopulateLeagueRulesFromPreviousYear(int scheduleYearId, int sportId)
        {
            var oRet = new ServiceResult<VsandLeagueRule>();

            int lastScheduleYearId = GetLastScheduleYearId(scheduleYearId);
            var oLastRules = await _uow.LeagueRules.List(lr => lr.ScheduleYearId == lastScheduleYearId && lr.SportId == sportId, null, new List<string> { "RuleItems" });

            var bHasPrev = oLastRules.Count > 0;
            if (bHasPrev)
            {
                bool bAnyRuleSaved = false;
                var oMsgs = new StringBuilder();

                foreach (var oLastRule in oLastRules)
                {
                    var oItems = new List<VsandLeagueRuleItem>();
                    foreach (var oLastItem in oLastRule.RuleItems)
                    {
                        var oItem = new VsandLeagueRuleItem
                        {
                            Conference = oLastItem.Conference,
                            Division = oLastItem.Division
                        };

                        oItems.Add(oItem);
                    }

                    var leagueRule = new VsandLeagueRule
                    {
                        SportId = sportId,
                        ScheduleYearId = scheduleYearId,
                        Conference = oLastRule.Conference,
                        Division = oLastRule.Division,
                        RuleType = oLastRule.RuleType,
                        RuleItems = oItems
                    };
                    await _uow.LeagueRules.Insert(leagueRule);

                    var bSaved = await _uow.Save();
                    if (bSaved == true)
                    {
                        bAnyRuleSaved = true;
                    }
                    else
                    {
                        oMsgs.AppendLine($"There was a problem creating the rule for {oLastRule.Conference} - {oLastRule.Division}");
                    }
                }

                if (oMsgs.Length > 0)
                {
                    // success is contingent on any rule being saved successfully
                    oRet.Success = bAnyRuleSaved;
                    oRet.Message = oMsgs.ToString();
                }
                else
                {
                    oRet.Success = true;
                    oRet.Message = "The rules were all successfully imported.";
                }
            }
            else
            {
                oRet.Success = false;
                oRet.Message = "There are no rules in the previous schedule year.";
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandLeagueRule>> PopulateLeagueRulesFromExistingData(int scheduleYearId, int sportId)
        {
            var oRet = new ServiceResult<VsandLeagueRule>();
            var state = Configuration["HomeState"];

            var teams = await _uow.Teams.List(t => t.SportId == sportId && t.ScheduleYearId == scheduleYearId && t.School.CoreCoverage, null, new List<string> { "CustomCodes" });

            bool bAnySaved = false;
            var oRuleList = new List<VsandLeagueRuleItem>();

            foreach (var team in teams)
            {
                var conferenceName = "";
                var divisionName = "";

                var conference = team.CustomCodes.FirstOrDefault(cc => cc.CodeName.Equals("Conference", StringComparison.OrdinalIgnoreCase));
                if (conference != null)
                {
                    conferenceName = conference.CodeValue.Trim();
                }

                var division = team.CustomCodes.FirstOrDefault(cc => cc.CodeName.Equals("Division", StringComparison.OrdinalIgnoreCase));
                if (division != null)
                {
                    divisionName = division.CodeValue.Trim();
                }

                if (!string.IsNullOrEmpty(conferenceName.Trim()))
                {
                    var oFound = oRuleList.FirstOrDefault(rli => rli.Conference.Equals(conferenceName, StringComparison.OrdinalIgnoreCase) && rli.Division.Equals(divisionName, StringComparison.OrdinalIgnoreCase));
                    if (oFound == null)
                    {
                        var oItem = new VsandLeagueRuleItem
                        {
                            Conference = conferenceName,
                            Division = divisionName
                        };

                        oRuleList.Add(oItem);
                    }
                }
            }

            var oMsgs = new StringBuilder();
            foreach (var oRule in oRuleList.OrderBy(rl => rl.Conference).ThenBy(rl => rl.Division))
            {
                var leagueRule = new VsandLeagueRule
                {
                    SportId = sportId,
                    ScheduleYearId = scheduleYearId,
                    Conference = oRule.Conference,
                    Division = oRule.Division,
                    RuleType = "division",
                };

                await _uow.LeagueRules.Insert(leagueRule);

                var bSaved = await _uow.Save();
                if (bSaved == true)
                {
                    bAnySaved = true;
                }
                else
                {
                    oMsgs.AppendLine($"There was a problem creating the rule for {oRule.Conference} - {oRule.Division}");
                }
            }

            if (oMsgs.Length > 0)
            {
                // success is contingent on any rule being saved successfully
                oRet.Success = bAnySaved;
                oRet.Message = oMsgs.ToString();
            }
            else
            {
                oRet.Success = true;
                oRet.Message = "The rules were all successfully created.";
            }

            return oRet;
        }
        #endregion
    }
}
