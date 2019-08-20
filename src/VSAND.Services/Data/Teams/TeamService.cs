using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.Repositories;
using VSAND.Data.ViewModels;
using VSAND.Data.ViewModels.Teams;
using VSAND.Services.Cache;
using VSAND.Services.Hubs;

namespace VSAND.Services.Data.Teams
{
    public class TeamService : ITeamService
    {
        private IUnitOfWork _uow;
        private IHubContext<ProvisioningHub> _provisioningHub;
        private ICache _cache;

        public TeamService(IUnitOfWork uow, IHubContext<ProvisioningHub> provisioningHub, ICache cache)
        {
            _uow = uow ?? throw new ArgumentException("Unit of Work is null");
            _provisioningHub = provisioningHub ?? throw new ArgumentException("Provisioning Hub is null");
            _cache = cache ?? throw new ArgumentException("Cache is null");
        }

        public async Task<List<ListItem<int>>> AutocompleteAsync(string keyword, int sportId, int scheduleYearId)
        {
            var oRet = new List<ListItem<int>>();

            if (string.IsNullOrEmpty(keyword))
            {
                oRet.Add(new ListItem<int> { id = 0, name = "Missing Search" });
                return oRet;
            }

            if (sportId <= 0)
            {
                oRet.Add(new ListItem<int> { id = 0, name = "Missing Sport" });
                return oRet;
            }

            if (scheduleYearId <= 0)
            {
                oRet.Add(new ListItem<int> { id = 0, name = "Missing Schedule Year" });
                return oRet;
            }

            // TODO: This could be cached to improve performance (store the search keyword + the results);
            var oTeams = await _uow.Teams.PagedList(filter: t => (t.Name.StartsWith(keyword)
            || t.School.Name.StartsWith(keyword) || t.School.AltName.StartsWith(keyword)) && t.SportId == sportId && t.ScheduleYearId == scheduleYearId,
                orderBy: t => t.OrderBy(o => o.Name), null, 1, 10);

            oRet = (from t in oTeams.Results select new ListItem<int> { id = t.TeamId, name = t.Name }).ToList();
            return oRet;
        }

        public async Task<ListItem<int>> AutocompleteRestoreAsync(int teamId)
        {
            var oRet = new ListItem<int>();

            var oTeam = await _uow.Teams.Single(t => t.TeamId == teamId, null, new List<string> { "School" });
            if (oTeam != null)
            {
                string teamName = oTeam.Name;
                if (string.IsNullOrEmpty(teamName) && oTeam.School != null)
                {
                    teamName = oTeam.School.Name;
                }
                oRet = new ListItem<int>(oTeam.TeamId, teamName);
            }
            return oRet;
        }

        public async Task<VsandTeam> GetTeamAsync(int teamId)
        {
            return await _uow.Teams.Single(t => t.TeamId == teamId, null, new List<string> { "School", "Sport", "ScheduleYear" });
        }

        public async Task<VsandTeam> GetTeamAsync(int schoolId, int sportId, int scheduleYearId)
        {
            if (scheduleYearId == 0)
            {
                scheduleYearId = await _uow.ScheduleYears.GetActiveScheduleYearIdAsync();
            }

            return await _uow.Teams.Single(t => t.SchoolId == schoolId && t.SportId == sportId && t.ScheduleYearId == scheduleYearId, null, new List<string> { "School", "Sport", "ScheduleYear" });
        }

        public FullTeam GetFullTeam(int schoolId, int sportId, int scheduleYearId)
        {
            return GetFullTeamAsync(schoolId, sportId, scheduleYearId).Result;
        }

        public async Task<FullTeam> GetFullTeamAsync(int schoolId, int sportId, int scheduleYearId)
        {
            var team = await _uow.Teams.Single(t => t.SchoolId == schoolId && t.SportId == sportId && t.ScheduleYearId == scheduleYearId, null, new List<string> { "School", "Sport", "ScheduleYear" });
            if (team == null)
            {
                return null;
            }

            // Custom Codes
            var customCodes = await _uow.TeamCustomCodes.List(tcc => tcc.TeamId == team.TeamId);

            return new FullTeam(team, customCodes);
        }

        public async Task<FullTeam> GetFullTeamCachedAsync(int schoolId, int sportId, int scheduleYearId)
        {
            if (scheduleYearId == 0)
            {
                scheduleYearId = await _uow.ScheduleYears.GetActiveScheduleYearIdAsync();
            }

            string cacheKey = Cache.Keys.FullTeam(schoolId, sportId, scheduleYearId);

            FullTeam team = await _cache.GetAsync<FullTeam>(cacheKey);
            if (team == null)
            {
                team = await GetFullTeamAsync(schoolId, sportId, scheduleYearId);
                if (team != null)
                {
                    await _cache.SetAsync(cacheKey, team);
                }
            }

            return team;
        }

        public async Task<VsandTeam> GetTeamDistribution(int teamId)
        {
            var team = await _uow.Teams.Single(t => t.TeamId == teamId, null, new List<string> { "School", "Sport", "ScheduleYear" });

            if (team != null)
            {
                // for the team sport id, return the list of subscribers that match the school
                var oNotifyList = await _uow.TeamNotifications.List(n => n.SchoolId == team.SchoolId && n.SportId == team.SportId);
            }

            return team;
        }

        public async Task<VsandTeam> GetTeamRoster(int teamId)
        {
            var oTeam = await _uow.Teams.Single(t => t.TeamId == teamId, null, new List<string> { "RosterEntries.Player", "Sport", "ScheduleYear", "School" });
            return oTeam;
        }

        public async Task<List<VsandSportPosition>> GetPositionsAsync(int sportId)
        {
            var oPosition = new List<VsandSportPosition>();
            oPosition = await _uow.SportPositions.List(p => p.SportId == sportId);

            return oPosition;
        }

        public async Task<PagedResult<TeamSummary>> GetTeamsAsync(SearchRequest searchRequest)
        {
            var oTeams = await _uow.Teams.GetLatestTeamsPagedAsync(searchRequest);
            return oTeams;
        }

        public async Task<int> GetTeamIdAsync(int schoolId, int sportId, int scheduleYearId)
        {
            var iRet = 0;
            var oTeam = await _uow.Teams.Single(t => t.SchoolId == schoolId && t.SportId == sportId && t.ScheduleYearId == scheduleYearId);
            if (oTeam != null)
            {
                iRet = oTeam.TeamId;
            }
            return iRet;
        }

        public async Task<List<ListItem<string>>> GetUniqueCustomCodeValues(string codeName)
        {
            var oResult = await _uow.TeamCustomCodes.GetUniqueValues(codeName);

            List<ListItem<string>> oRet = (from r in oResult select new ListItem<string> { id = r, name = r }).ToList();
            return oRet;

            // TODO: TeamService -> GetUniqueCustomcodevalues can be cached using the codename as part of the cache key
        }

        public async Task ProvisionTeams(AppxUser user, int scheduleYearId, int sportId, List<int> schoolIds)
        {
            var status = new ProcessingStatus
            {
                Current = 0,
                Total = schoolIds.Count
            };

            var roomName = $"scheduleYearId={scheduleYearId}&sportId={sportId}";

            foreach (int schoolId in schoolIds)
            {
                var school = await _uow.Schools.GetById(schoolId);
                if (school != null)
                {
                    string teamName = "";
                    if (school.Name != null && !string.IsNullOrEmpty(school.Name))
                    {
                        teamName = school.Name.Trim();
                    }
                    await _uow.Teams.AddSchoolTeamAsync(schoolId, sportId, teamName, scheduleYearId, true, user);
                }

                status.Current += 1;

                if (status.Current % 10 == 0 || status.Current == status.Total)
                {
                    await _provisioningHub.Clients.Group(roomName).SendAsync("status", status);
                }
            }
        }

        public async Task<int> AddTeamAsync(AppxUser user, int scheduleYearId, int sportId, int schoolId)
        {
            // returns the team Id if successful and 0 if unsuccessful
            int teamId = await _uow.Teams.AddSchoolTeamAsync(schoolId, sportId, "", scheduleYearId, true, user);
            return teamId;
        }

        public async Task<List<TeamRoster>> GetRoster(int teamid)
        {
            var oRet = new List<TeamRoster>();

            var oTeamRoster = await _uow.TeamRoster.List(p => p.TeamId == teamid, null, new List<string> { "PositionNavigation", "Position2Navigation", "Player", "Team.Sport.Positions" });

            oRet = (from gr in oTeamRoster select new TeamRoster(teamid, gr)).ToList();
            return oRet;
        }

        public async Task<ServiceResult<VsandTeamRoster>> SaveRoster(VsandTeamRoster teamRoster)
        {
            var oRet = new ServiceResult<VsandTeamRoster>();

            _uow.TeamRoster.Update(teamRoster);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = teamRoster;
                oRet.Success = true;
                oRet.Id = teamRoster.TeamId;
            }
            return oRet;
        }

        public async Task<ServiceResult<VsandTeamRoster>> DeleteRoster(int rosterId)
        {
            var oRet = new ServiceResult<VsandTeamRoster>();

            VsandTeamRoster remTeamRoster = await _uow.TeamRoster.GetById(rosterId);
            await _uow.TeamRoster.Delete(remTeamRoster.RosterId);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = remTeamRoster;
                oRet.Success = true;
                oRet.Id = remTeamRoster.RosterId;
            }

            return oRet;
        }

        public async Task<TeamOosRecord> GetOosRecord(int teamId)
        {
            var oRet = new TeamOosRecord();

            var codeNames = new List<string> { "GroupExchange", "OOSFinalLosses", "OOSFinalTies", "OOSFinalWins", "OOSUpdate" };
            var team = await _uow.Teams.Single(t => t.TeamId == teamId, null, new List<string> { "School" });
            if (team != null)
            {
                var codes = await _uow.TeamCustomCodes.List(tcc => tcc.TeamId == teamId && codeNames.Contains(tcc.CodeName));
                oRet = new TeamOosRecord(team, codes);
            }

            return oRet;
        }

        public async Task<ServiceResult<TeamOosRecord>> SaveOosRecord(TeamOosRecord teamRecord)
        {
            var oRet = new ServiceResult<TeamOosRecord>();

            var codeNames = new List<string> { "GroupExchange", "OOSFinalLosses", "OOSFinalTies", "OOSFinalWins", "OOSUpdate" };
            var team = await _uow.Teams.Single(t => t.TeamId == teamRecord.TeamId);
            if (team == null)
            {
                oRet.Message = "Could not load team record for OOS info.";
                return oRet;
            }

            var codes = await _uow.TeamCustomCodes.List(tcc => tcc.TeamId == teamRecord.TeamId && codeNames.Contains(tcc.CodeName));

            _uow.BeginTransaction();
            var groupExchange = codes.FirstOrDefault(tcc => tcc.CodeName.Equals("GroupExchange", StringComparison.OrdinalIgnoreCase));
            if (groupExchange != null)
            {
                groupExchange.CodeValue = teamRecord.GroupExchange.ToString();
                _uow.TeamCustomCodes.Update(groupExchange);
            }
            else
            {
                groupExchange = new VsandTeamCustomCode()
                {
                    TeamId = teamRecord.TeamId,
                    CodeName = "GroupExchange",
                    CodeValue = teamRecord.GroupExchange.ToString()
                };
                await _uow.TeamCustomCodes.Insert(groupExchange);
            }

            var recordWins = codes.FirstOrDefault(tcc => tcc.CodeName.Equals("OOSFinalWins", StringComparison.OrdinalIgnoreCase));
            if (recordWins != null)
            {
                recordWins.CodeValue = teamRecord.Wins.ToString();
                _uow.TeamCustomCodes.Update(recordWins);
            }
            else
            {
                recordWins = new VsandTeamCustomCode()
                {
                    TeamId = teamRecord.TeamId,
                    CodeName = "OOSFinalWins",
                    CodeValue = teamRecord.Wins.ToString()
                };
                await _uow.TeamCustomCodes.Insert(recordWins);
            }

            var recordLosses = codes.FirstOrDefault(tcc => tcc.CodeName.Equals("OOSFinalLosses", StringComparison.OrdinalIgnoreCase));
            if (recordLosses != null)
            {
                recordLosses.CodeValue = teamRecord.Losses.ToString();
                _uow.TeamCustomCodes.Update(recordLosses);
            } else
            {
                recordLosses = new VsandTeamCustomCode()
                {
                    TeamId = teamRecord.TeamId,
                    CodeName = "OOSFinalLosses",
                    CodeValue = teamRecord.Losses.ToString()
                };
                await _uow.TeamCustomCodes.Insert(recordLosses);
            }

            var recordTies = codes.FirstOrDefault(tcc => tcc.CodeName.Equals("OOSFinalTies", StringComparison.OrdinalIgnoreCase));
            if (recordTies != null)
            {
                recordTies.CodeValue = teamRecord.Ties.ToString();
                _uow.TeamCustomCodes.Update(recordTies);
            } else
            {
                recordTies = new VsandTeamCustomCode()
                {
                    TeamId = teamRecord.TeamId,
                    CodeName = "OOSFinalTies",
                    CodeValue = teamRecord.Ties.ToString()
                };
                await _uow.TeamCustomCodes.Insert(recordTies);
            }

            var recordUpdated = codes.FirstOrDefault(tcc => tcc.CodeName.Equals("OOSUpdate", StringComparison.OrdinalIgnoreCase));
            if (recordUpdated != null)
            {
                recordUpdated.CodeValue = DateTime.Now.ToString();
                _uow.TeamCustomCodes.Update(recordUpdated);
            } else
            {
                recordUpdated = new VsandTeamCustomCode()
                {
                    TeamId = teamRecord.TeamId,
                    CodeName = "OOSUpdate",
                    CodeValue = DateTime.Now.ToString()
                };
                await _uow.TeamCustomCodes.Insert(recordUpdated);
            }

            bool saved = await _uow.CommitTransaction();
            if (!saved)
            {
                Exception lastEx = _uow.GetLastError();
                string message = "There was a problem updating the OOS record info.";
                if (lastEx != null)
                {
                    message = $"There was a problem updating the OOS record info: {lastEx.Message}";
                }
                oRet.Message = message;
                return oRet;
            }

            oRet.Success = true;
            oRet.Id = teamRecord.TeamId;
            oRet.obj = teamRecord;
            return oRet;
        }

        #region Custom Codes
        public async Task<List<TeamCustomCode>> GetTeamCustomCodes(int teamId)
        {
            var customCodes = await _uow.TeamCustomCodes.List(cc => cc.TeamId == teamId);
            return customCodes.Select(cc => new TeamCustomCode(cc)).ToList();
        }

        public async Task<ServiceResult<VsandTeamCustomCode>> AddCustomCodeAsync(VsandTeamCustomCode addCustomCode)
        {
            var oRet = new ServiceResult<VsandTeamCustomCode>();

            await _uow.TeamCustomCodes.Insert(addCustomCode);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = addCustomCode;
                oRet.Success = true;
                oRet.Id = addCustomCode.CustomCodeId;
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandTeamCustomCode>> UpdateCustomCodeAsync(VsandTeamCustomCode chgCustomCode)
        {
            var oRet = new ServiceResult<VsandTeamCustomCode>();

            _uow.TeamCustomCodes.Update(chgCustomCode);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = chgCustomCode;
                oRet.Success = true;
                oRet.Id = chgCustomCode.CustomCodeId;
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandTeamCustomCode>> DeleteCustomCodeAsync(int customCodeId)
        {
            var oRet = new ServiceResult<VsandTeamCustomCode>();

            VsandTeamCustomCode remCustomCode = await _uow.TeamCustomCodes.GetById(customCodeId);
            await _uow.TeamCustomCodes.Delete(remCustomCode.CustomCodeId);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = remCustomCode;
                oRet.Success = true;
                oRet.Id = remCustomCode.CustomCodeId;
            }

            return oRet;
        }

        public async Task<List<ListItem<int>>> CoreCoverageListAsync(int sportId, int scheduleYearId)
        {
            var teams = await _uow.Teams.List(t => t.School.CoreCoverage && t.SportId == sportId && t.ScheduleYearId == scheduleYearId, t => t.OrderBy(tm => tm.School.Name), new List<string> { "School" });
            return (from t in teams select new ListItem<int>(t.TeamId, t.School.Name)).ToList();
        }

        public async Task<ServiceResult<bool>> BulkSaveTeamCustomCodes(int sportId, int scheduleYearId, List<VsandTeamCustomCode> codes)
        {
            _uow.BeginTransaction();
            foreach (var code in codes)
            {
                var saveCode = await _uow.TeamCustomCodes.Single(tcc => tcc.TeamId == code.TeamId && tcc.CodeName == code.CodeName);
                if (saveCode == null)
                {
                    saveCode = new VsandTeamCustomCode()
                    {
                        TeamId = code.TeamId,
                        CodeName = code.CodeName,
                        CodeValue = code.CodeValue
                    };

                    await _uow.TeamCustomCodes.Insert(saveCode);
                }
                else
                {
                    saveCode.CodeValue = code.CodeValue;
                    _uow.TeamCustomCodes.Update(saveCode);
                }
            }
            bool success = await _uow.CommitTransaction();

            var result = new ServiceResult<bool>();
            result.Success = success;
            return result;
        }
        #endregion
    }
}
