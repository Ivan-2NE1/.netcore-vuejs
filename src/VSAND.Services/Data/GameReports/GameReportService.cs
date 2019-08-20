using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.Repositories;
using VSAND.Data.ViewModels;
using VSAND.Data.ViewModels.GameReport;
using VSAND.Services.Cache;

namespace VSAND.Services.Data.GameReports
{
    public class GameReportService : IGameReportService
    {
        private readonly ILogger Log = LogManager.GetCurrentClassLogger();

        private readonly IUnitOfWork _uow;
        private readonly ICache _cache;

        public GameReportService(IUnitOfWork uow, ICache cache)
        {
            _uow = uow ?? throw new ArgumentException("Unit of Work");
            _cache = cache ?? throw new ArgumentException("Cache is null");
        }

        public ServiceResult<AddGameReport> AddGameReport(AppxUser user, AddGameReport addGame)
        {
            return AddGameReportAsync(user, addGame).Result;
        }

        public async Task<ServiceResult<AddGameReport>> AddGameReportAsync(AppxUser user, AddGameReport addGame)
        {
            int iGameReportId = 0;
            string sReportedByName = user.UserId;
            int UserId = user.AdminId;

            int iCounty = 0;
            int iScheduleYearId = 0;
            int iDefaultPeriods = 0;
            List<VsandSportTeamStat> oTeamStats = null;
            ParticipatingTeam oHome = addGame.Teams.FirstOrDefault(t => t.HomeTeam == true);
            if (oHome == null)
            {
                oHome = addGame.Teams.FirstOrDefault(t => t.TeamId > 0);
            }
            List<VsandSportEvent> oSportEvents = null;
            List<int> aGameReportEvent = new List<int>();
            int iHomeTeamId = oHome.TeamId;
            bool bPpEligible = true;

            var gameTeamIds = (from t in addGame.Teams where t.TeamId > 0 select t.TeamId).ToList();
            var teams = await _uow.Teams.List(t => gameTeamIds.Contains(t.TeamId), null, new List<string> { "School.County" });

            if (oHome.TeamId > 0)
            {
                var homeTeam = teams.FirstOrDefault(t => t.TeamId == oHome.TeamId);
                VsandCounty oCounty = homeTeam?.School?.County;
                if (oCounty != null)
                {
                    iCounty = oCounty.CountyId;
                }
            }

            if (iCounty == 0)
            {
                // -- Pick the first non-home, team with a valid id
                ParticipatingTeam oRefTeam = addGame.Teams.FirstOrDefault(pvp => pvp.HomeTeam == false && pvp.TeamId > 0);
                if (oRefTeam != null)
                {
                    int iRefTeamId = oRefTeam.TeamId;
                    var refTeam = teams.FirstOrDefault(t => t.TeamId == iRefTeamId);
                    VsandCounty oCounty = refTeam?.School?.County;
                    if (oCounty != null)
                    {
                        iCounty = oCounty.CountyId;
                    }
                }
            }

            if (iCounty == 0)
            {
                // -- The game must have a county
                VsandCounty oCounty = await _uow.Counties.Single(null, c => c.OrderByDescending(cty => cty.Schools.Count));
                if (oCounty != null)
                {
                    iCounty = oCounty.CountyId;
                }
            }

            VsandTeam oHomeTeam = teams.FirstOrDefault(t => t.TeamId == oHome.TeamId);
            if (oHomeTeam != null)
            {
                iScheduleYearId = oHomeTeam.ScheduleYearId;
            }

            if (iScheduleYearId == 0)
            {
                iScheduleYearId = await _uow.ScheduleYears.GetActiveScheduleYearIdAsync();
            }

            VsandSport oInitSport = await _uow.Sports.Single(s => s.SportId == addGame.SportId);
            if (oInitSport != null)
            {
                if (oInitSport.DefaultPeriods.HasValue)
                    iDefaultPeriods = oInitSport.DefaultPeriods.Value;
            }

            //TODO: Need SportTeamStat list from repository
            //IEnumerable<VsandSportTeamStat> oTs = (from ts in _context.VsandSportTeamStat
            //                                       where ts.Sport.SportId == sportId && ts.Enabled
            //                                       select ts);
            //if (oTs != null)
            //{
            //    if (oTs.Any())
            //    {
            //        oTeamStats = oTs.ToList();
            //    }
            //}

            var oSe = await _uow.SportEvents.List(se => se.SportId == addGame.SportId && se.DefaultActivated && se.Enabled.HasValue && se.Enabled.Value,
                se => se.OrderBy(e => e.DefaultSort));
            oSportEvents = oSe.ToList();

            // We can deprecate this! girls wrestling will be a separate sport
            //if (sportId == 17 && exhibition)
            //{
            //    // -- Reset the list to include only the Girls Wrestling Weight Classes
            //    oSe = (from se in _context.VsandSportEvent
            //           where se.Sport.SportId == sportId && se.Enabled == true && se.DefaultActivated == false
            //           orderby se.DefaultSort ascending
            //           select se);
            //    if (oSe != null && oSe.Any())
            //    {
            //        oSportEvents = oSe.ToList();
            //    }
            //}

            // -- Pre-create any non-existent, out-of-state schools and teams before entering the transaction
            foreach (ParticipatingTeam oPTeam in addGame.Teams.Where(pt => pt.TeamId == 0 && !string.IsNullOrWhiteSpace(pt.TeamName)))
            {
                int iTeamId = 0;
                // -- New school / team
                oPTeam.TeamName = oPTeam.TeamName.Trim();
                string sSchoolName = oPTeam.TeamName;
                string sState = oPTeam.State;
                // -- Try to get the a school
                int iTeamSchool = _uow.Schools.GetSchoolIdByName(sSchoolName, sState);
                if (iTeamSchool == 0)
                {
                    // -- Try it with the AP/Pub abbreviation added in
                    string pubState = "";
                    var state = await _uow.States.Single(s => s.Abbreviation == sState);
                    if (state != null)
                    {
                        pubState = state.PubAbbreviation;
                    }
                    if (!string.IsNullOrEmpty(pubState))
                    {
                        iTeamSchool = _uow.Schools.GetSchoolIdByName(sSchoolName + " (" + pubState + ")", sState);
                    }

                    if (iTeamSchool == 0)
                    {
                        // -- Create the school
                        iTeamSchool = _uow.Schools.SchoolQuickAdd(sSchoolName, sState);
                    }
                    else
                    {
                        // -- Fix-up school/team name based on found value
                        sSchoolName = _uow.Schools.GetSchoolName(iTeamSchool);
                        oPTeam.TeamName = sSchoolName;
                    }
                }
                if (iTeamSchool > 0)
                {
                    // -- Create the team if need be
                    iTeamId = _uow.Teams.GetSchoolTeamId(addGame.SportId, iTeamSchool, iScheduleYearId);
                    if (iTeamId == 0)
                    {
                        iTeamId = _uow.Teams.QuickAddTeam(sSchoolName, iTeamSchool, addGame.SportId, iScheduleYearId);
                    }

                    oPTeam.TeamId = iTeamId;
                }
            }

            //TODO: Need PowerPointsConfig on Repository level
            // -- Is this game power points eligible
            var powerpointsConfig = await _uow.PowerPointsConfig.Single(ppc => ppc.SportId == addGame.SportId && ppc.ScheduleYearId == iScheduleYearId);

            if (powerpointsConfig != null)
            {
                bPpEligible = powerpointsConfig.IsPPEligible;
            }

            // -- Create the game name
            string sGameName = CreateGameName(addGame.EventTypeId, addGame.RoundId, addGame.SectionId, addGame.GroupId, addGame.Teams, 0);

            string sHomeTeam = "";
            bool bTriPlus = (addGame.Teams.Count > 2);

            // Begin the transactional part of adding the game
            _uow.BeginTransaction();

            VsandGameReport oGame = null;

            oGame = new VsandGameReport
            {
                Name = sGameName,
                GameDate = addGame.GameDate,
                ReportedByName = sReportedByName,
                ReportedBy = UserId,
                Source = addGame.Source,
                TriPlus = bTriPlus,
                ReportedDate = DateTime.Now,
                PPEligible = bPpEligible,
                Exhibition = false,
                SportId = addGame.SportId,
                ScheduleYearId = iScheduleYearId,
                GameTypeId = addGame.EventTypeId,
                CountyId = iCounty,
                LocationName = addGame.LocationName,
                LocationCity = addGame.LocationCity,
                LocationState = addGame.LocationState,
                Final = true, // When the game is being reported, always set it to final = true
                ModifiedDate = DateTime.Now
            };

            if (addGame.RoundId > 0)
            {
                oGame.RoundId = addGame.RoundId;
            }
            if (addGame.SectionId > 0)
            {
                oGame.SectionId = addGame.SectionId;
            }
            if (addGame.GroupId > 0)
            {
                oGame.GroupId = addGame.GroupId;
            }

            if (!string.IsNullOrEmpty(addGame.Notes.Trim()))
            {
                VsandGameReportNote oNote = new VsandGameReportNote
                {
                    NoteBy = sReportedByName,
                    Note = addGame.Notes.Trim(),
                    NoteById = UserId,
                    NoteDate = DateTime.Now
                };
                oGame.Notes.Add(oNote);
            }

            foreach (ParticipatingTeam oPt in addGame.Teams)
            {
                int iTeamId = oPt.TeamId;
                string sTeamName = oPt.TeamName.Trim();

                if (iTeamId > 0 | !string.IsNullOrEmpty(sTeamName))
                {
                    VsandGameReportTeam oGT = new VsandGameReportTeam
                    {
                        TeamId = iTeamId,
                        TeamName = oPt.TeamName,
                        GameReport = oGame,
                        HomeTeam = oPt.HomeTeam,
                        FinalScore = oPt.Score,
                        Abbreviation = oPt.Abbreviation
                    };

                    if (oPt.HomeTeam)
                    {
                        sHomeTeam = oPt.TeamName;
                    }

                    if (iDefaultPeriods > 0)
                    {
                        for (int iPeriod = 1; iPeriod <= iDefaultPeriods; iPeriod++)
                        {
                            VsandGameReportPeriodScore oScore = new VsandGameReportPeriodScore
                            {
                                PeriodNumber = iPeriod
                            };
                            oGT.PeriodScores.Add(oScore);
                        }
                    }

                    if (oTeamStats != null)
                    {
                        foreach (VsandSportTeamStat oTeamStat in oTeamStats)
                        {
                            int iSportTeamStatId = oTeamStat.SportTeamStatId;

                            VsandGameReportTeamStat oGRTStat = new VsandGameReportTeamStat
                            {
                                TeamStatId = iSportTeamStatId,
                                StatValue = 0
                            };

                            oGT.TeamStats.Add(oGRTStat);
                        }
                    }

                    oGame.Teams.Add(oGT);
                }
            }

            // -- Add the meta information, if any exists
            if (addGame.Meta != null && addGame.Meta.Any())
            {
                foreach (GameReportMeta gm in addGame.Meta)
                {
                    VsandGameReportMeta oGameMeta = new VsandGameReportMeta
                    {
                        SportGameMetaId = gm.SportGameMetaId,
                        MetaValue = gm.MetaValue
                    };
                    oGame.Meta.Add(oGameMeta);
                }
            }

            // -- Initialize any events for this sport that are default activated
            if (oSportEvents != null)
            {
                int iSESort = 1;
                foreach (VsandSportEvent oSE in oSportEvents)
                {
                    int iSportEventId = oSE.SportEventId;
                    VsandGameReportEvent oAddEvent = new VsandGameReportEvent
                    {
                        SportEventId = iSportEventId,
                        SortOrder = iSESort
                    };

                    oGame.Events.Add(oAddEvent);
                    iSESort += 1;
                }
            }

            await _uow.GameReports.Insert(oGame);

            // Here is where we try to commit the transaction
            var bCommitted = await _uow.CommitTransaction();
            if (bCommitted)
            {
                iGameReportId = oGame.GameReportId;
                foreach (VsandGameReportEvent oEvent in oGame.Events)
                {
                    aGameReportEvent.Add(oEvent.EventId);
                }
            }
            else
            {
                iGameReportId = 0;
            }

            if (iGameReportId > 0)
            {
                _uow.Audit.AuditChange("vsand_GameReport", "GameReportId", iGameReportId, "Insert", sReportedByName, UserId);

                // TODO: event remediation
                // VSAND.Events.GameReport.RaiseGameUpdated(new object(), new VSAND.Events.GameReport.GameReportEventArgs(iGameReportId, userId));

                try
                {
                    switch (addGame.SportId)
                    {
                        case 17: // -- Wrestling
                            {
                                // TODO: implement game report events repository and move this out of here
                                // VSAND.Helper.GameReportEvents.SortEventsByStartingWeightClass(iGameReportId, exhibition);
                                break;
                            }

                        case 47:
                        case 48: // -- B/G Golf
                            {
                                // TODO: implement game report events repository and move this out of here
                                // VSAND.Helper.GameReportEvents.EnableEventsByPlayFormat(iGameReportId, sportId);
                                break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex.Message);
                }

                for (int iEvent = 0; iEvent <= aGameReportEvent.Count - 1; iEvent++)
                {
                    // TODO: event remediation
                    // VSAND.Events.GameReport.RaiseEventUpdated(new object(), new VSAND.Events.GameReport.GameReportEventEventArgs(aGameReportEvent(iEvent)));
                }

                // -- For Each Team, Recaculate Records
                foreach (ParticipatingTeam oTeam in addGame.Teams)
                {
                    if (oTeam.TeamId > 0)
                    {
                        // TODO: event remediation
                        // VSAND.Events.Records.RaiseRecalculate(new object(), new VSAND.Events.Records.RecordsEventArgs(oTeam.TeamId));
                    }
                }
            }

            var oRet = new ServiceResult<AddGameReport>()
            {
                Success = (iGameReportId > 0),
                Id = iGameReportId,
                obj = addGame
            };

            return oRet;
        }

        public async Task<ServiceResult<AddScheduledGame>> AddScheduledGameAsync(AppxUser user, AddScheduledGame scheduleGame)
        {
            var oRet = new ServiceResult<AddScheduledGame>();

            int iGameReportId = 0;
            string sReportedByName = user.UserId;
            int UserId = user.AdminId;
            int iScheduleYearId = 0;
            int iCounty = 0;

            ParticipatingTeam oHome = scheduleGame.Teams.FirstOrDefault(t => t.HomeTeam == true);
            if (oHome == null)
            {
                oHome = scheduleGame.Teams.FirstOrDefault(t => t.TeamId > 0);
            }

            var gameTeamIds = (from t in scheduleGame.Teams where t.TeamId > 0 select t.TeamId).ToList();
            var teams = await _uow.Teams.List(t => gameTeamIds.Contains(t.TeamId), null, new List<string> { "School.County" });

            if (oHome.TeamId > 0)
            {
                var homeTeam = teams.FirstOrDefault(t => t.TeamId == oHome.TeamId);
                VsandCounty oCounty = homeTeam?.School?.County;
                if (oCounty != null)
                {
                    iCounty = oCounty.CountyId;
                }
            }

            if (iCounty == 0)
            {
                ParticipatingTeam oRefTeam = scheduleGame.Teams.FirstOrDefault(pvp => pvp.HomeTeam == false && pvp.TeamId > 0);
                if (oRefTeam != null)
                {
                    int iRefTeamId = oRefTeam.TeamId;
                    var refTeam = teams.FirstOrDefault(t => t.TeamId == iRefTeamId);
                    VsandCounty oCounty = refTeam?.School?.County;
                    if (oCounty != null)
                    {
                        iCounty = oCounty.CountyId;
                    }
                }
            }

            if (iCounty == 0)
            {
                VsandCounty oCounty = await _uow.Counties.Single(null, c => c.OrderByDescending(cty => cty.Schools.Count));
                if (oCounty != null)
                {
                    iCounty = oCounty.CountyId;
                }
            }

            VsandTeam oHomeTeam = teams.FirstOrDefault(t => t.TeamId == oHome.TeamId);
            if (oHomeTeam != null)
            {
                iScheduleYearId = oHomeTeam.ScheduleYearId;
            }

            if (iScheduleYearId == 0)
            {
                iScheduleYearId = await _uow.ScheduleYears.GetActiveScheduleYearIdAsync();
            }

            bool bTriPlus = (scheduleGame.Teams.Count > 2);

            VsandGameReport oGame = null;

            oGame = new VsandGameReport
            {
                GameDate = scheduleGame.GameDate,
                ReportedByName = sReportedByName,
                ReportedBy = UserId,
                Source = "",
                TriPlus = bTriPlus,
                ReportedDate = DateTime.Now,
                PPEligible = false,
                Exhibition = false,
                SportId = scheduleGame.SportId,
                ScheduleYearId = iScheduleYearId,
                GameTypeId = scheduleGame.EventTypeId,
                CountyId = iCounty,
                LocationName = scheduleGame.LocationName,
                LocationCity = scheduleGame.LocationCity,
                LocationState = scheduleGame.LocationState,
                Final = false
            };

            if (scheduleGame.RoundId > 0)
            {
                oGame.RoundId = scheduleGame.RoundId;
            }
            if (scheduleGame.SectionId > 0)
            {
                oGame.SectionId = scheduleGame.SectionId;
            }
            if (scheduleGame.GroupId > 0)
            {
                oGame.GroupId = scheduleGame.GroupId;
            }

            // need to add the teams to the game!
            foreach (ParticipatingTeam oPt in scheduleGame.Teams)
            {
                int iTeamId = oPt.TeamId;
                string sTeamName = oPt.TeamName.Trim();

                if (iTeamId > 0 | !string.IsNullOrEmpty(sTeamName))
                {
                    VsandGameReportTeam oGT = new VsandGameReportTeam
                    {
                        TeamId = iTeamId,
                        TeamName = oPt.TeamName,
                        GameReport = oGame,
                        HomeTeam = oPt.HomeTeam,
                        FinalScore = oPt.Score,
                        Abbreviation = oPt.Abbreviation
                    };

                    oGame.Teams.Add(oGT);
                }
            }
            // Very important! Set the game name :)
            oGame.Name = CreateGameName(scheduleGame.EventTypeId, scheduleGame.RoundId ?? 0, scheduleGame.SectionId ?? 0, scheduleGame.GroupId ?? 0, scheduleGame.Teams, 0);

            await _uow.GameReports.Insert(oGame);

            var bRet = await _uow.Save();
            if (bRet)
            {
                iGameReportId = oGame.GameReportId;
                if (iGameReportId > 0)
                {
                    oRet.obj = scheduleGame;
                    oRet.Success = true;
                    oRet.Id = iGameReportId;
                }
            }
            else
            {
                var lastEx = _uow.GetLastError();
                if (lastEx != null)
                {
                    oRet.Message = lastEx.Message;
                }
            }

            return oRet;
        }

        public string CreateGameName(IEnumerable<VsandGameReportTeam> oGameTeams, int schoolId)
        {
            return CreateGameName(oGameTeams.ToList(), schoolId);
        }

        public string CreateGameName(List<VsandGameReportTeam> oGameTeams, int schoolId)
        {
            List<ParticipatingTeam> oTeamList = new List<ParticipatingTeam>();
            foreach (VsandGameReportTeam oGT in oGameTeams)
            {
                oTeamList.Add(new ParticipatingTeam(oGT));
            }
            return CreateGameName(0, 0, 0, 0, oTeamList, schoolId);
        }

        public string CreateGameName(int EventTypeId, int RoundId, int SectionId, int GroupId, List<VsandGameReportTeam> oGameTeams, int schoolId)
        {
            List<ParticipatingTeam> oTeamList = new List<ParticipatingTeam>();
            foreach (VsandGameReportTeam oGT in oGameTeams)
            {
                oTeamList.Add(new ParticipatingTeam(oGT));
            }
            return CreateGameName(EventTypeId, RoundId, SectionId, GroupId, oTeamList, schoolId);
        }

        public string CreateGameName(int EventTypeId, int RoundId, int SectionId, int GroupId, List<ParticipatingTeam> oTeams, int schoolId)
        {
            return CreateGameNameAsync(EventTypeId, RoundId, SectionId, GroupId, oTeams, schoolId).Result;
        }

        public async Task<string> CreateGameNameAsync(int EventTypeId, int RoundId, int SectionId, int GroupId, List<ParticipatingTeam> oTeams, int schoolId)
        {
            string sRet = "";

            bool bReg = true;
            int SportId = 0;
            int scheduleYearId = 0;
            string sFilterType = "";
            string sFilter = "";
            if (EventTypeId > 0)
            {
                VsandSportEventType oEventType = await _uow.SportEventTypes.Single(et => et.EventTypeId == EventTypeId);
                if (oEventType != null)
                {
                    if (!oEventType.DefaultSelected.Value)
                    {
                        bReg = false;
                        SportId = oEventType.SportId;
                        scheduleYearId = oEventType.ScheduleYearId.Value;
                        if (oEventType.ParticpatingTeamsType != null)
                        {
                            sFilterType = oEventType.ParticpatingTeamsType;
                        }
                        if (oEventType.ParticipatingTeamsFilter != null)
                        {
                            sFilter = oEventType.ParticipatingTeamsFilter;
                        }
                    }
                }
            }

            if (bReg)
            {
                // -- Regular Season
                string sHome = "";
                List<string> aTeams = new List<string>();
                foreach (ParticipatingTeam oTeam in oTeams)
                {
                    if (oTeam.HomeTeam)
                    {
                        sHome = " at " + oTeam.TeamName + " (" + oTeam.Score + ")";
                    }
                    else
                    {
                        aTeams.Add(oTeam.TeamName + " (" + oTeam.Score + ")");
                    }
                }
                sRet = string.Join(", ", aTeams) + sHome;

                if (sRet.Length > 150)
                {
                    sRet = oTeams.Count + "-Teams" + sHome;
                }
            }
            else
            {
                List<EventTypeListItem> oEDataList = await _uow.ScheduleYearEventTypeListItems.GetEventTypeObjectsAsync(SportId, scheduleYearId);
                EventTypeListItem oEData = null;
                if (oEDataList != null)
                {
                    oEData = oEDataList.FirstOrDefault(etl => etl.EventTypeId == EventTypeId && etl.RoundId == RoundId && etl.SectionId == SectionId && etl.GroupId == GroupId);
                }
                if (oEData != null)
                {
                    if (oTeams.Count == 2)
                    {
                        string sHome = "";
                        List<string> aTeams = new List<string>();
                        foreach (ParticipatingTeam oTeam in oTeams)
                        {
                            if (oTeam.HomeTeam)
                            {
                                sHome = " at " + oTeam.TeamName + " (" + oTeam.Score + ")";
                            }
                            else
                            {
                                aTeams.Add(oTeam.TeamName + " (" + oTeam.Score + ")");
                            }
                        }
                        sRet = string.Join(", ", aTeams) + sHome;
                        if (!string.IsNullOrEmpty(oEData.EventTypeName))
                        {
                            sRet = sRet + ", " + oEData.EventTypeName;
                        }
                        if (!string.IsNullOrEmpty(oEData.RoundName))
                        {
                            sRet = sRet + ", " + oEData.RoundName;
                        }
                        if (!string.IsNullOrEmpty(oEData.SectionName))
                        {
                            sRet = sRet + ", " + oEData.SectionName;
                        }
                        if (!string.IsNullOrEmpty(oEData.GroupName))
                        {
                            sRet = sRet + ", " + oEData.GroupName;
                        }
                    }
                    else
                    {
                        string sJoin = "";
                        string sFilterTo = "";
                        if (!string.IsNullOrEmpty(sFilterType))
                        {
                            if (!string.IsNullOrEmpty(sFilterType))
                            {
                                for (int i = 0; i <= oTeams.Count - 1; i++)
                                {
                                    int RefTeamId = oTeams[i].TeamId;
                                    if (RefTeamId > 0)
                                    {
                                        switch (sFilterType.ToLower())
                                        {
                                            case "team":
                                                {
                                                    // TODO: implement team custom code repository and move this out of here
                                                    sFilterTo = ""; // VSAND.Helper.TeamCustomCode.GetCustomCode(RefTeamId, sFilter);
                                                    break;
                                                }

                                            case "school":
                                                {
                                                    // int SchoolId = _schoolRepository.GetSchoolIdByTeam(RefTeamId);
                                                    if (schoolId > 0)
                                                    {
                                                        // TODO: implement school custom code repository and move this out of here
                                                        sFilterTo = ""; // VSAND.Helper.SchoolCustomCode.GetCustomCode(SchoolId, sFilter);
                                                    }
                                                    break;
                                                }
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(sFilterTo))
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(oEData.EventTypeName))
                        {
                            sRet = sRet + sJoin + oEData.EventTypeName.Trim();
                            sJoin = ", ";
                        }
                        if (!string.IsNullOrEmpty(sFilterTo))
                        {
                            sRet = sRet + sJoin + sFilter + " " + sFilterTo.Trim();
                            sJoin = " - ";
                        }
                        if (!string.IsNullOrEmpty(oEData.RoundName))
                        {
                            sRet = sRet + sJoin + oEData.RoundName.Trim();
                            sJoin = ", ";
                        }
                        if (!string.IsNullOrEmpty(oEData.SectionName))
                        {
                            sRet = sRet + sJoin + oEData.SectionName.Trim();
                            sJoin = ", ";
                        }
                        if (!string.IsNullOrEmpty(oEData.GroupName))
                        {
                            sRet = sRet + sJoin + oEData.GroupName;
                            sJoin = ", ";
                        }
                    }
                }
                else
                {
                    string sHome = "";
                    List<string> aTeams = new List<string>();
                    foreach (ParticipatingTeam oTeam in oTeams)
                    {
                        if (oTeam.HomeTeam)
                        {
                            sHome = " at " + oTeam.TeamName + " (" + oTeam.Score + ")";
                        }
                        else
                        {
                            aTeams.Add(oTeam.TeamName + " (" + oTeam.Score + ")");
                        }
                    }
                    sRet = string.Join(", ", aTeams) + sHome;

                    if (sRet.Length > 150)
                    {
                        sRet = oTeams.Count + "-Teams" + sHome;
                    }
                }
            }

            return sRet;
        }

        public string CreateResultName(int TeamId, bool bLowScoreWins, ICollection<VsandGameReportTeam> oGameTeams)
        {
            return CreateResultNameAsync(TeamId, bLowScoreWins, oGameTeams).Result;
        }

        public async Task<string> CreateResultNameAsync(int TeamId, bool bLowScoreWins, ICollection<VsandGameReportTeam> oGameTeams)
        {
            string sResult = "";
            string sInfo = "";

            if (oGameTeams.Count == 2)
            {
                VsandGameReportTeam oTeam = oGameTeams.FirstOrDefault(gt => gt.TeamId == TeamId);
                double dTeamScore = oTeam.FinalScore;

                double dOppScore = 0;
                VsandGameReportTeam oOpp = oGameTeams.FirstOrDefault(gt => gt.TeamId != TeamId);
                if (oOpp != null)
                {
                    dOppScore = oOpp.FinalScore;
                }
                else
                {
                    sResult = "Invalid Opponent";
                    return sResult;
                }

                bool bGolf = false;
                if (oTeam.Team.Sport.Name.ToLower().Contains("golf"))
                    bGolf = true;

                if (bGolf)
                {
                    sInfo = "<div class=\"tinytext\">Stroke Play</div>";
                    int GameReportId = oTeam.GameReportId;

                    // -- Check to see if this was match play, if so, high score wins         
                    var oMeta = await _uow.GameReportMeta.Single(gm => gm.GameReportId == GameReportId && gm.SportGameMeta.ValueType == "VSAND.GolfPlayFormat");

                    if (oMeta != null)
                    {
                        if (oMeta.MetaValue == "1")
                        {
                            bLowScoreWins = false;
                            sInfo = "<div class=\"tinytext\">Match Play</div>";
                        }
                    }
                }

                bool bForfeit = oGameTeams.Any(grt => grt.Forfeit);
                string sForfeit = (bForfeit ? "<span style=\"color:red;\">*</span>" : "").ToString();
                string sScoreLine = dTeamScore + "-" + dOppScore;
                if (bLowScoreWins)
                {
                    if (dTeamScore < dOppScore)
                    {
                        sResult = "(W" + sForfeit + ") " + sScoreLine;
                    }
                    else if (dTeamScore > dOppScore)
                    {
                        sResult = "(L" + sForfeit + ") " + sScoreLine;
                    }
                    else
                    {
                        sResult = "(T" + sForfeit + ") " + sScoreLine;
                    }
                }
                else if (dTeamScore > dOppScore)
                {
                    sResult = "(W" + sForfeit + ") " + sScoreLine;
                }
                else if (dTeamScore < dOppScore)
                {
                    sResult = "(L" + sForfeit + ") " + sScoreLine;
                }
                else
                {
                    sResult = "(T" + sForfeit + ") " + sScoreLine;
                }
            }

            return sResult + sInfo;
        }

        public Task<ServiceResult<AddScheduledGame>> AddScheduledGameAsync(AddScheduledGame oGame)
        {
            throw new NotImplementedException();
            // var oRet = new ServiceResult<AddScheduledGame>();

            // What validation do we need to place on the game object that was passed in?

            // return oRet;
        }

        public async Task<AddGameReport> GetAddGameReport(int? sportId, int? scheduleYearId, int? teamId)
        {
            var oRet = new AddGameReport();

            if (teamId.HasValue && teamId.Value > 0)
            {
                // when teamid is provided, the sport and scheduleyearid don't need to be accepted from the querystring
                var oTeam = await _uow.Teams.GetById(teamId.Value);
                if (oTeam != null)
                {
                    sportId = oTeam.SportId;
                    scheduleYearId = oTeam.ScheduleYearId;
                    oRet.refTeamId = oTeam.TeamId;
                    oRet.Teams.Add(new ParticipatingTeam(oTeam.TeamId, oTeam.Name, true, 0));
                }
            }

            if (sportId.HasValue && sportId.Value > 0)
            {
                oRet.SportId = sportId.Value;
            }
            if (scheduleYearId.HasValue && scheduleYearId.Value > 0)
            {
                oRet.ScheduleYearId = scheduleYearId.Value;
            }
            else
            {
                // Init this to the active schedule year id
                oRet.ScheduleYearId = await _uow.ScheduleYears.GetActiveScheduleYearIdAsync();
            }

            oRet.Initialize();

            return oRet;
        }

        public async Task<AddScheduledGame> GetAddScheduledGame(int? sportId, int? scheduleYearId, int? teamId)
        {
            var oRet = new AddScheduledGame();
            if (teamId.HasValue && teamId.Value > 0)
            {
                var oTeam = await _uow.Teams.GetById(teamId.Value);
                if (oTeam != null)
                {
                    sportId = oTeam.SportId;
                    scheduleYearId = oTeam.ScheduleYearId;
                    oRet.refTeamId = oTeam.TeamId;
                    oRet.Teams.Add(new ParticipatingTeam(oTeam.TeamId, oTeam.Name, true, 0));
                }
            }
            if (sportId.HasValue && sportId.Value > 0)
            {
                oRet.SportId = sportId.Value;
            }
            if (scheduleYearId.HasValue && scheduleYearId.Value > 0)
            {
                oRet.ScheduleYearId = scheduleYearId.Value;
            }
            else
            {
                oRet.ScheduleYearId = await _uow.ScheduleYears.GetActiveScheduleYearIdAsync();
            }

            oRet.Initialize();

            return oRet;
        }

        public async Task<ServiceResult<GameReport>> UpdateGameReportOverview(AppxUser user, GameReport gameReport)
        {
            var result = new ServiceResult<GameReport>();

            if (gameReport == null)
            {
                result.Message = "Invalid report data";
                return result;
            }

            var game = await _uow.GameReports.Single(gr => gr.GameReportId == gameReport.GameReportId,
                null,
                new List<string> { "Meta", "Teams.Team.School", "Sport.GameMeta" });

            if (game == null)
            {
                result.Message = "Unable to load game with report id";
                return result;
            }

            bool changes = false;
            bool eventTypeChanged = false;

            // has the final flag changed
            if (game.Final != gameReport.Final)
            {
                game.Final = gameReport.Final;
                changes = true;
            }

            // check for changes on gamedate
            if (game.GameDate.Year != gameReport.GameDate.Year ||
                game.GameDate.Month != gameReport.GameDate.Month ||
                game.GameDate.Day != gameReport.GameDate.Day ||
                game.GameDate.Hour != gameReport.GameDate.Hour ||
                game.GameDate.Minute != gameReport.GameDate.Minute)
            {
                game.GameDate = gameReport.GameDate;
                changes = true;
            }

            if (game.GameTypeId != gameReport.EventTypeId)
            {
                game.GameTypeId = gameReport.EventTypeId;
                eventTypeChanged = true;
                changes = true;
            }

            if (game.RoundId != gameReport.RoundId)
            {
                game.RoundId = gameReport.RoundId;
                eventTypeChanged = true;
                changes = true;
            }

            if (game.SectionId != gameReport.SectionId)
            {
                game.SectionId = gameReport.SectionId;
                eventTypeChanged = true;
                changes = true;
            }

            if (game.GroupId != gameReport.GroupId)
            {
                game.GroupId = gameReport.GroupId;
                eventTypeChanged = true;
                changes = true;
            }

            if (game.LocationName != gameReport.LocationName)
            {
                game.LocationName = gameReport.LocationName.Trim();
                changes = true;
            }

            if (game.LocationCity != gameReport.LocationCity)
            {
                game.LocationCity = gameReport.LocationCity.Trim();
                changes = true;
            }

            if (game.LocationState != gameReport.LocationState)
            {
                game.LocationState = gameReport.LocationState.Trim();
                changes = true;
            }

            // Check to see if anything changes in the participating teams list
            bool teamsChanged = false;
            bool teamValuesChanged = false;
            bool lowScoreWins = game.Sport.EnableLowScoreWins ?? false;
            bool enablePlayerOfRecord = game.Sport.EnablePlayerOfRecord;
            double bestScore = lowScoreWins ? 99999 : 0;

            // find any teams that exist in the database but aren't in the participating teams list
            var ptIds = gameReport.ParticipatingTeams.Select(pt => pt.TeamId).ToList();

            var removedGameTeams = game.Teams.Where(grt => !ptIds.Contains(grt.TeamId)).ToList();
            for (var gameTeamIdx = removedGameTeams.Count - 1; gameTeamIdx >= 0; gameTeamIdx--)
            {
                var gameTeam = removedGameTeams[gameTeamIdx];

                var pTeam = gameReport.ParticipatingTeams.FirstOrDefault(pt => pt.TeamId == gameTeam.TeamId);
                if (pTeam == null)
                {
                    // we are gonna remove this guy, no longer in the game
                    _uow.Audit.AuditChange("vsand_GameReportTeam", "GameReportTeamId", gameTeam.GameReportTeamId, "Delete", user);

                    // remove their event player records
                    var eventPlayers = await _uow.GameReportEventPlayers.List(grep => grep.GameReportTeamId == gameTeam.GameReportTeamId);
                    if (eventPlayers != null && eventPlayers.Any())
                    {
                        _uow.GameReportEventPlayers.DeleteRange(eventPlayers);
                    }

                    // remove their event player group records
                    var eventPlayerGroups = await _uow.GameReportEventPlayerGroups.List(grepg => grepg.GameReportTeamId == gameTeam.GameReportTeamId);
                    if (eventPlayerGroups != null && eventPlayerGroups.Any())
                    {
                        _uow.GameReportEventPlayerGroups.DeleteRange(eventPlayerGroups);
                    }

                    _uow.GameReportTeams.Delete(gameTeam);
                    teamsChanged = true;
                }
            }

            // add any new teams into the list
            var gtIds = game.Teams.Select(grt => grt.TeamId).ToList();
            var addGameTeams = gameReport.ParticipatingTeams.Where(pt => !gtIds.Contains(pt.TeamId)).ToList();
            foreach (var addTeam in addGameTeams)
            {
                if (addTeam.SchoolId == 0 && addTeam.TeamId == 0)
                {
                    // we need to quick add the school for this
                    addTeam.SchoolId = _uow.Schools.SchoolQuickAdd(addTeam.TeamName, addTeam.State);
                    if (addTeam.SchoolId == 0)
                    {
                        // there was a problem adding a school
                        result.Message = $"There was a problem creating the new school for {addTeam.TeamName}";
                        return result;
                    }
                }

                if (addTeam.TeamId == 0 && addTeam.SchoolId > 0)
                {
                    // check to see if a team exists already
                    addTeam.TeamId = await _uow.Teams.GetSchoolTeamIdAsync(game.SportId, addTeam.SchoolId, game.ScheduleYearId);
                    if (addTeam.TeamId == 0)
                    {
                        addTeam.TeamId = await _uow.Teams.AddSchoolTeamAsync(addTeam.SchoolId, game.SportId, "", game.ScheduleYearId, true, user);
                    }
                }

                var addGameTeam = new VsandGameReportTeam()
                {
                    TeamId = addTeam.TeamId,
                    TeamName = addTeam.TeamName,
                    HomeTeam = addTeam.HomeTeam,
                    FinalScore = addTeam.Score
                };

                game.Teams.Add(addGameTeam);
                teamsChanged = true;
            }

            // now, cycle through everything and perform upates where necessary for final score, home team flag
            foreach(var pt in gameReport.ParticipatingTeams)
            {
                var gameTeam = game.Teams.FirstOrDefault(grt => grt.TeamId == pt.TeamId);
                if (gameTeam != null)
                {
                    if (gameTeam.FinalScore != pt.Score)
                    {
                        gameTeam.FinalScore = pt.Score;
                        teamValuesChanged = true;
                    }

                    if (gameTeam.HomeTeam != pt.HomeTeam)
                    {
                        gameTeam.HomeTeam = pt.HomeTeam;
                        teamValuesChanged = true;
                    }
                }
            }

            if (teamsChanged)
            {
                // we need to change the name on the game record
                game.Name = CreateGameName(game.GameTypeId, game.RoundId ?? 0, game.SectionId ?? 0, game.GroupId ?? 0, gameReport.ParticipatingTeams, 0);                
            }

            bool metaChanges = false;
            if (gameReport.Meta != null && gameReport.Meta.Any())
            {
                for (var metaIdx = 0; metaIdx < gameReport.Meta.Count - 1; metaIdx++)
                {
                    var gameMeta = game.Meta.FirstOrDefault(m => m.SportGameMetaId == gameReport.Meta[metaIdx].SportGameMetaId);
                    if (gameMeta == null)
                    {
                        gameMeta = new VsandGameReportMeta()
                        {
                            SportGameMetaId = gameReport.Meta[metaIdx].SportGameMetaId,
                            MetaValue = gameReport.Meta[metaIdx].MetaValue
                        };
                        metaChanges = true;
                        game.Meta.Add(gameMeta);
                    }
                    if (gameMeta.MetaValue != gameReport.Meta[metaIdx].MetaValue)
                    {
                        gameMeta.MetaValue = gameReport.Meta[metaIdx].MetaValue;
                        metaChanges = true;
                    }
                }
            }
           

            bool addNote = false;
            if (!string.IsNullOrEmpty(gameReport.AddNote.Trim()))
            {
                var note = new VsandGameReportNote()
                {
                    Note = gameReport.AddNote.Trim(),
                    NoteById = user.AdminId,
                    NoteBy = user.UserId,
                    NoteDate = DateTime.Now
                };
                game.Notes.Add(note);
                addNote = true;
            }

            bool trackedChanges = changes || eventTypeChanged || teamsChanged || metaChanges;

            // what are we gonna put on our audit record?
            if (trackedChanges || teamsChanged || addNote)
            {
                if (teamsChanged)
                {
                    _uow.Audit.AuditChange("vsand_GameReport", "GameReportId", game.GameReportId, "Update(TeamsChanged)", user.UserId, user.AdminId);
                } else if(trackedChanges)
                {
                    _uow.Audit.AuditChange("vsand_GameReport", "GameReportId", game.GameReportId, "Update", user.UserId, user.AdminId);
                }

                if (teamsChanged || trackedChanges)
                {
                    game.ModifiedDate = DateTime.Now;
                }
                
                _uow.GameReports.Update(game);

                bool gameSaved = await _uow.Save();
                if (!gameSaved)
                {
                    result.Message = "There was a problem saving your changes to this game report.";
                    return result;
                } else
                {
                    // purge our cache related to full game if teamsChanged || trackedChanges
                    string cacheKey = Cache.Keys.FullGameReport(game.GameReportId);
                    await _cache.RemoveAsync(cacheKey);
                }
            }

            result.Success = true;
            return result;
        }

        public async Task<GameReport> GetFullGameReport(int gameReportId)
        {
            var oGame = await _uow.GameReports.Single(gr => gr.GameReportId == gameReportId,
                null,
                new List<string> { "Teams.Team.School",
                    "Teams.PeriodScores",
                    "Sport",
                    "Teams.TeamStats",
                    "Sport.TeamStatCategories.TeamStats",
                    "PlayerStats",
                    "Sport.PlayerStatCategories.PlayerStats" });
            var oReport = new GameReport(oGame);

            // TODO: GameReportService I'd really love to not query the database here when I need to get all of the stuff for the sport related to this game.

            return oReport;
        }

        public async Task<GameReport> GetFullGameReportCachedAsync(int gameReportId)
        {
            string cacheKey = Cache.Keys.FullGameReport(gameReportId);

            var oGame = await _cache.GetAsync<GameReport>(cacheKey);
            if (oGame == null)
            {
                oGame = await GetFullGameReport(gameReportId);
                if (oGame != null)
                {
                    // Since this is the cached version, we need to get the sport slug and assign it
                    var sportSlug = await _uow.EntitySlugs.Single(es => es.EntityType == "Sport" && es.EntityId == oGame.Sport.SportId);
                    if (sportSlug != null)
                    {
                        oGame.Sport.Slug = sportSlug.Slug;
                    }

                    await _cache.SetAsync(cacheKey, oGame);
                }
            }
            return oGame;
        }

        public async Task<GameReport> GetGameReport(int gameReportId)
        {
            var oGame = await _uow.GameReports.Single(gr => gr.GameReportId == gameReportId,
                null,
                new List<string> { "Meta", "Teams.Team.School", "Sport.GameMeta", "Notes" });
            var oReport = new GameReport(oGame);

            return oReport;
        }

        public async Task<GameReport> GetGameReportScoring(int gameReportId)
        {
            var oGame = await _uow.GameReports.Single(gr => gr.GameReportId == gameReportId,
                null,
                new List<string> { "Teams.Team.School", "Teams.PeriodScores", "Sport" });
            var oReport = new GameReport(oGame);

            // TODO: GameReportService I'd really love to not query the database here when I need to get all of the stuff for the sport related to this game.

            return oReport;
        }

        public async Task<GameReport> GetGameReportEvents(int gameReportId)
        {
            var oGame = await _uow.GameReports.Single(gr => gr.GameReportId == gameReportId,
                null,
                new List<string> {
                    "Teams.Team.School",
                    "Teams.Team.RosterEntries.Player",
                    "Events.Results.EventPlayers.GameReportEventPlayerStats",
                    "Events.Results.EventPlayerGroups.EventPlayerGroupPlayers",
                    "Events.Results.EventPlayerGroups.GameReportEventPlayerGroupStats",
                    "Sport.SportEvents.EventStats",
                    "Sport.EventResults" });

            var oReport = new GameReport(oGame);

            return oReport;
        }

        public async Task<GameReport> GetGameReportTeamStats(int gameReportId)
        {
            var oGame = await _uow.GameReports.Single(gr => gr.GameReportId == gameReportId,
                null,
                new List<string> { "Teams.Team.School", "Teams.TeamStats", "Sport.TeamStatCategories.TeamStats" });
            var oReport = new GameReport(oGame);

            return oReport;
        }

        public async Task<GameReport> GetGameReportPlayerStats(int gameReportId, int gameReportTeamId)
        {
            var oGame = await _uow.GameReports.Single(gr => gr.GameReportId == gameReportId,
                null,
                new List<string> {
                    "PlayerStats",
                    "Teams.Team.School",
                    "Sport.PlayerStatCategories.PlayerStats",
                    "Sport.Positions"
                });

            // Get the reference teamid from our game teams list
            var oRefTeam = oGame.Teams.FirstOrDefault(grt => grt.GameReportTeamId == gameReportTeamId);
            if (oRefTeam != null)
            {
                var oTeamRoster = await _uow.TeamRoster.List(tr => tr.TeamId == oRefTeam.TeamId, null, new List<string> { "Player" });
            }

            var oGameRoster = await _uow.GameReportRoster.List(grr => grr.GameReportTeamId == gameReportTeamId);

            var oReport = new GameReport(oGame);

            return oReport;
        }

        public async Task<PagedResult<GameReportSummary>> ReverseChronologicalList(SearchRequest oRequest)
        {
            var oRet = new PagedResult<GameReportSummary>(null, 0, oRequest.PageSize, oRequest.PageNumber);

            var oGames = await _uow.GameReports.GetLatestGamesPagedAsync(oRequest);

            oRet.TotalResults = oGames.TotalResults;
            oRet.PageSize = oGames.PageSize;
            var oSummaries = new List<GameReportSummary>();
            foreach (var oGame in oGames.Results)
            {
                oSummaries.Add(new GameReportSummary(oGame));
            }

            oRet.Results = oSummaries;
            return oRet;
        }

        public async Task<List<GameReportSummary>> ScheduleScoreboard(DateTime? viewDate, int? sportId, int? schoolId, int? scheduleYearId)
        {
            var oGames = await _uow.GameReports.ScheduleScoreboard(viewDate, sportId, schoolId, scheduleYearId);

            var oRet = (from gr in oGames where gr.Teams.Any() orderby gr.GameDate ascending select new GameReportSummary(gr)).ToList();
            return oRet;
        }

        public async Task<IEnumerable<TeamGameSummary>> TeamGames(int teamId)
        {
            var oRet = new List<TeamGameSummary>();
            if (teamId <= 0) return oRet;

            var oGames = await _uow.GameReports.GetTeamGamesAsync(teamId);
            VsandLeagueRule leagueRule = null;
            oRet = (from gr in oGames select new TeamGameSummary(teamId, gr)).ToList();
            if (oRet != null && oRet.Any())
            {
                var refGame = oRet.FirstOrDefault();
                if (refGame != null)
                {
                    var oGame = oGames.FirstOrDefault(gr => gr.GameReportId == refGame.GameReportId);
                    leagueRule = await _uow.LeagueRules.Single(lr => lr.SportId == oGame.SportId
                        && lr.ScheduleYearId == oGame.ScheduleYearId
                        && lr.Conference == refGame.TeamConference
                        && lr.Division == refGame.TeamDivision, null, new List<string>() { "RuleItems" });
                }
            }

            // now that we have all of these in here, go through and calculate their running record
            int recordWins = 0;
            int recordLosses = 0;
            int recordTies = 0;
            int confWins = 0;
            int confLosses = 0;
            int confTies = 0;

            foreach (var gr in oRet.Where(g => !g.Deleted && g.Final))
            {
                bool league = false;
                if (gr.Opponent != null)
                {
                    if (leagueRule != null)
                    {
                        switch (leagueRule.RuleType.ToLower())
                        {
                            case "conference":
                                league = gr.TeamConference.Equals(gr.OpponentConference, StringComparison.OrdinalIgnoreCase);
                                break;
                            case "division":
                                league = gr.TeamConference.Equals(gr.OpponentConference, StringComparison.OrdinalIgnoreCase)
                                    && gr.TeamDivision.Equals(gr.OpponentDivision, StringComparison.OrdinalIgnoreCase);
                                break;
                            case "rule":
                                league = leagueRule.RuleItems.Any(i => i.Conference.Equals(gr.OpponentConference, StringComparison.OrdinalIgnoreCase)
                                    && i.Division.Equals(gr.OpponentDivision, StringComparison.OrdinalIgnoreCase));
                                break;

                        }
                    }
                    else
                    {
                        // When there is no league rule, treat the match of conference and division
                        league = gr.TeamConference.Equals(gr.OpponentConference, StringComparison.OrdinalIgnoreCase) && gr.TeamDivision.Equals(gr.OpponentDivision, StringComparison.OrdinalIgnoreCase);
                    }

                    switch (gr.ResultName)
                    {
                        case "W":
                            recordWins += 1;
                            if (league)
                            {
                                confWins += 1;
                            }
                            break;
                        case "L":
                            recordLosses += 1;
                            if (league)
                            {
                                confLosses += 1;
                            }
                            break;
                        case "T":
                            recordTies += 1;
                            if (league)
                            {
                                confTies += 1;
                            }
                            break;
                    }

                    gr.RecordWins = recordWins;
                    gr.RecordLosses = recordLosses;
                    gr.RecordTies = recordTies;
                    gr.ConferenceWins = confWins;
                    gr.ConferenceLosses = confLosses;
                    gr.ConferenceTies = confTies;
                }
            }

            return oRet;
        }

        public async Task<IEnumerable<TeamRecordInfo>> TeamRecordInfo(int teamId, bool UseCached)
        {
            var oRet = new List<TeamRecordInfo>();
            if (teamId <= 0) return oRet;

            var oTeamInfo = await _uow.GameReports.GetTeamRecordInfo(teamId);
            oRet = (from gr in oTeamInfo select new TeamRecordInfo(gr, UseCached)).ToList();

            return oRet;
        }

        public async Task<ServiceResult<GameReportPlayerStat>> SavePlayerStatAsync(AppxUser user, GameReportPlayerStat playerStat)
        {
            bool inserted = false;
            var oStat = await _uow.GameReportPlayerStats.Single(grps => grps.GameReportId == playerStat.GameReportId && grps.PlayerId == playerStat.PlayerId && grps.StatId == playerStat.StatId);
            if (oStat == null)
            {
                // we need to add the stat
                oStat = new VsandGameReportPlayerStat()
                {
                    GameReportId = playerStat.GameReportId,
                    PlayerId = playerStat.PlayerId,
                    StatId = playerStat.StatId,
                    StatValue = playerStat.StatValue
                };
                await _uow.GameReportPlayerStats.Insert(oStat);
                inserted = true;
            }
            else
            {
                _uow.Audit.AuditChange("vsand_GameReportPlayerStat", "PlayerStatId", oStat.PlayerStatId, "Update", user.UserId, user.AdminId);
                // we just save the new stat value
                oStat.StatValue = playerStat.StatValue;
                _uow.GameReportPlayerStats.Update(oStat);
            }

            var oResult = new ServiceResult<GameReportPlayerStat>
            {
                Success = await _uow.Save(),
                Id = oStat.PlayerStatId
            };

            // update game report modified date
            var gameReport = await _uow.GameReports.Single(gr => gr.GameReportId == playerStat.GameReportId);
            gameReport.ModifiedDate = DateTime.Now;
            _uow.GameReports.Update(gameReport);

            var bModifiedDateUpdated = await _uow.Save();
            if (bModifiedDateUpdated)
            {
                oResult.Success = false;
                oResult.Message = "An error occurred while updating the game report modified date.";
            }

            if (inserted)
            {
                _uow.Audit.AuditChange("vsand_GameReportPlayerStat", "PlayerStatId", oStat.PlayerStatId, "Insert", user.UserId, user.AdminId);
            }

            oResult.obj = new GameReportPlayerStat(oStat);

            return oResult;
        }

        public async Task<ServiceResult<GameReportTeamStat>> SaveTeamStatAsync(AppxUser user, GameReportTeamStat teamStat)
        {
            bool inserted = false;
            var oStat = await _uow.GameReportTeamStats.Single(grts => grts.GameReportTeamId == teamStat.GameReportTeamId && grts.StatId == teamStat.StatId);
            if (oStat == null)
            {
                // we need to add the stat
                oStat = new VsandGameReportTeamStat()
                {
                    GameReportTeamId = teamStat.GameReportTeamId,
                    StatId = teamStat.StatId,
                    StatValue = teamStat.StatValue
                };
                await _uow.GameReportTeamStats.Insert(oStat);
                inserted = true;
            }
            else
            {
                _uow.Audit.AuditChange("vsand_GameReportTeamStat", "TeamStatId", oStat.TeamStatId, "Update", user.UserId, user.AdminId);
                // we just save the new stat value
                oStat.StatValue = teamStat.StatValue;
                _uow.GameReportTeamStats.Update(oStat);
            }

            var oResult = new ServiceResult<GameReportTeamStat>
            {
                Success = await _uow.Save(),
                Id = oStat.TeamStatId
            };

            // update game report modified date
            var gameReportTeam = await _uow.GameReportTeams.Single(t => t.GameReportTeamId == teamStat.GameReportTeamId);
            var gameReport = await _uow.GameReports.Single(gr => gr.GameReportId == gameReportTeam.GameReportId);
            gameReport.ModifiedDate = DateTime.Now;
            _uow.GameReports.Update(gameReport);

            var bModifiedDateUpdated = await _uow.Save();
            if (bModifiedDateUpdated)
            {
                oResult.Success = false;
                oResult.Message = "An error occurred while updating the game report modified date.";
            }

            if (inserted)
            {
                _uow.Audit.AuditChange("vsand_GameReportTeamStat", "TeamStatId", oStat.TeamStatId, "Insert", user.UserId, user.AdminId);
            }
            oResult.obj = new GameReportTeamStat(oStat);

            return oResult;
        }

        public async Task<ServiceResult<List<GameReportPlayerStat>>> InitPlayer(AppxUser user, int gameReportId, int gameReportTeamId, int playerId)
        {
            var result = new ServiceResult<List<GameReportPlayerStat>>();
            if (gameReportId <= 0 | playerId <= 0)
            {
                // invalid request
                result.Success = false;
                result.Message = "Invalid request";
                return result;
            }

            var allowed = await AllowedToEdit(gameReportId, user.SchoolId.HasValue ? user.SchoolId.Value : 0);
            if (!allowed)
            {
                result.Success = false;
                result.Message = "Not allowed";
                return result;
            }

            var gameRosterEntry = await _uow.GameReportRoster.Single(grr => grr.GameReportTeam.GameReportId == gameReportId && grr.GameReportTeamId == gameReportTeamId && grr.PlayerId == playerId);
            if (gameRosterEntry == null)
            {
                gameRosterEntry = new VsandGameReportRoster()
                {
                    GameReportTeamId = gameReportTeamId,
                    PlayerId = playerId,
                };
                await _uow.GameReportRoster.Insert(gameRosterEntry);
            }

            var oStatList = new List<GameReportPlayerStat>();

            var playerStats = await _uow.GameReportPlayerStats.List(grps => grps.GameReportId == gameReportId && grps.PlayerId == playerId);

            oStatList.AddRange(from grps in playerStats select new GameReportPlayerStat(grps));
            var hasStatIds = (from grps in playerStats select grps.StatId).ToList();

            var sportPlayerStats = await _uow.SportPlayerStats.List(sps => sps.Sport.VsandGameReport.Any(gr => gr.GameReportId == gameReportId) && sps.Enabled && !hasStatIds.Contains(sps.SportPlayerStatId));
            foreach (VsandSportPlayerStat sps in sportPlayerStats)
            {
                var addStat = new VsandGameReportPlayerStat()
                {
                    GameReportId = gameReportId,
                    PlayerId = playerId,
                    StatId = sps.SportPlayerStatId,
                    StatValue = 0
                };
                await _uow.GameReportPlayerStats.Insert(addStat);
            }

            result.Success = await _uow.Save();
            result.obj = oStatList;

            return result;
        }

        #region Bulk Moved from Repository to Service
        public async Task<bool> AllowedToEdit(int gameReportId, int schoolId)
        {
            bool bRet = false;

            if (schoolId == 0)
            {
                return true;
            }

            var checkObj = await _uow.GameReportTeams.Single(grt => grt.GameReportId == gameReportId && grt.Team.SchoolId == schoolId);
            if (checkObj != null)
            {
                bRet = true;
            }

            return bRet;
        }

        //public bool AllowedToCompose(int gameReportId, int adminId)
        //{
        //    bool bRet = false;

        //    VsandGameReport oData = (from gr in _context.VsandGameReport
        //                             where gr.GameReportId == gameReportId && gr.Archived == false && gr.PublicationStories.Any(ps => ps.AssignedToUser.AdminId == adminId)
        //                             select gr).FirstOrDefault();

        //    if (oData != null)
        //    {
        //        bRet = true;
        //    }

        //    return bRet;
        //}

        //public bool ArchiveGameReport(int gameReportId, ref string sMsg)
        //{
        //    bool bRet = false;

        //    VsandGameReport oGameReport = (from gr in _context.VsandGameReport
        //                                   where gr.GameReportId == gameReportId
        //                                   select gr).FirstOrDefault();

        //    if (oGameReport != null)
        //    {
        //        oGameReport.Archived = true;

        //        try
        //        {
        //            _context.SaveChanges();

        //            bRet = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            sMsg = ex.Message;
        //            Log.Error(ex, sMsg);
        //        }
        //    }
        //    else
        //    {
        //        sMsg = "Game report was not found.";
        //    }

        //    return bRet;
        //}

        public async Task<bool> UpdatedGameReportLocation(int GameReportId, string LocationName, string LocationCity, string LocationState, AppxUser user)
        {
            bool bRet = false;

            _uow.Audit.AuditChange("vsand_GameReport", "GameReportId", GameReportId, "Update Game Location", user.UserId, user.AdminId);

            var oGame = await _uow.GameReports.GetById(GameReportId);

            if (oGame != null)
            {
                oGame.LocationName = LocationName;
                oGame.LocationCity = LocationCity;
                oGame.LocationState = LocationState;

                bRet = await _uow.Save();
            }

            return bRet;
        }

        //public async Task<bool> UpdateGameReportExhibition(int GameReportId, bool exhibition, ref string sMsg, AppxUser user)
        //{
        //    bool bRet = false;
        //    _uow.Audit.AuditChange("vsand_GameReport", "GameReportId", GameReportId, "Update Game Exhibition", user.UserId, user.AdminId);

        //    VsandGameReport oGame = (from gr in _context.VsandGameReport
        //                             where gr.GameReportId == GameReportId
        //                             select gr).FirstOrDefault();

        //    if (oGame != null)
        //    {
        //        oGame.Exhibition = exhibition;
        //        try
        //        {
        //            _context.SaveChanges();

        //            bRet = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            sMsg = ex.Message;
        //            Log.Error(ex, sMsg);
        //        }
        //    }

        //    return bRet;
        //}

        public async Task<bool> AddGameNote(int gameReportId, string note, AppxUser user)
        {
            VsandGameReportNote oNote = new VsandGameReportNote
            {
                NoteBy = user.UserId,
                GameReportId = gameReportId,
                NoteById = user.AdminId,
                Note = note,
                NoteDate = DateTime.Now,
            };

            await _uow.GameReportNotes.Insert(oNote);
            bool bRet = await _uow.Save();
            return bRet;
        }

        public async Task<List<VsandGameReportNote>> GetNotes(int GameReportId)
        {
            List<VsandGameReportNote> oRet = await _uow.GameReportNotes.List(grn => grn.GameReportId == GameReportId, grn => grn.OrderByDescending(n => n.NoteDate));
            return oRet;
        }

        public async Task<bool> UpdateGameReportEventType(int GameReportId, int EventTypeId, int RoundId, int SectionId, int GroupId, AppxUser user)
        {
            bool bRet = false;

            _uow.Audit.AuditChange("vsand_GameReport", "GameReportId", GameReportId, "Change Game EventType", user.UserId, user.AdminId);

            var oGame = await _uow.GameReports.GetById(GameReportId);

            if (oGame != null)
            {
                oGame.GameTypeId = EventTypeId;
                oGame.RoundId = RoundId;
                oGame.SectionId = SectionId;
                oGame.GroupId = GroupId;
                oGame.ModifiedDate = DateTime.Now;

                bRet = await _uow.Save();
            }

            return bRet;
        }

        //public bool UpdateGameReportName(int GameReportId, int schoolId)
        //{
        //    bool bRet = false;

        //    VsandGameReport oGame = _context.VsandGameReport
        //                                .Include(gr => gr.Teams).ThenInclude(t => t.Team)
        //                                .FirstOrDefault(gr => gr.GameReportId == GameReportId);

        //    if (oGame != null)
        //    {
        //        // Dim sGameName As String = ""
        //        // If oGame.Teams.Count > 2 Then
        //        // Dim sHomeTeam As String = ""
        //        // Dim oHome As VsandGameReportTeam = oGame.Teams.FirstOrDefault(Function(oT As VsandGameReportTeam) oT.HomeTeam = True)
        //        // If oHome IsNot Nothing Then
        //        // sHomeTeam = oHome.TeamName
        //        // End If
        //        // sGameName = oGame.Teams.Count && "-Team Meet at " && sHomeTeam
        //        // Else
        //        // sGameName = CreateGameName(oGame.Teams)
        //        // End If
        //        // oGame.Name = sGameName

        //        // Dim sName As String = CreateGameName(oGame.Teams)
        //        // oGame.Name = sName

        //        string sGameName = await CreateGameNameAsync(oGame.GameTypeId, oGame.RoundId.Value, oGame.SectionId.Value, oGame.GroupId.Value, oGame.Teams.ToList(), schoolId);
        //        oGame.Name = sGameName;

        //        try
        //        {
        //            _context.SaveChanges();
        //            bRet = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, ex.Message);
        //        }
        //    }
        //    return bRet;
        //}

        public async Task<bool> UpdateGameReportDate(int GameReportId, DateTime GameDate, AppxUser user)
        {
            bool bRet = false;

            _uow.Audit.AuditChange("vsand_GameReport", "GameReportId", GameReportId, "Change Game Date", user.UserId, user.AdminId);

            var oGame = await _uow.GameReports.GetById(GameReportId);
            if (oGame != null)
            {
                oGame.GameDate = GameDate;
                bRet = await _uow.Save();
            }

            if (bRet)
            {
                // TODO: event remediation
                // VSAND.Events.GameReport.RaiseGameUpdated(new object(), new VSAND.Events.GameReport.GameReportEventArgs(GameReportId, UserId));
            }

            return bRet;
        }

        //TODO: Remediate UpdateGameReportTeams
        //public async Task<bool> UpdateGameReportTeams(int GameReportId, List<ParticipatingTeam> oTeams, int schoolId, AppxUser user)
        //{
        //    bool bRet = false;

        //    bool bTeamsChanged = false;

        //    bool bScoreChanged = false;
        //    bool bEnablePlayerOfRecord = false;

        //    VsandGameReport oGameReport = (from gr in _context.VsandGameReport
        //                                        .Include(t => t.Teams).ThenInclude(t => t.Team)
        //                                        .Include(t => t.Sport)
        //                                   where gr.GameReportId == GameReportId
        //                                   select gr).FirstOrDefault();

        //    if (oGameReport != null)
        //    {
        //        bool bLowScoreWins = oGameReport.Sport.EnableLowScoreWins.Value;
        //        bEnablePlayerOfRecord = oGameReport.Sport.EnablePlayerOfRecord;
        //        double dBestScore = bLowScoreWins ? 999999 : 0;

        //        // -- Remove teams that do not exist anymore, update teams that do, add teams that are brand new
        //        for (int iTeam = oGameReport.Teams.Count - 1; iTeam >= 0; iTeam += -1)
        //        {
        //            VsandGameReportTeam oTeam = oGameReport.Teams.ElementAt(iTeam);
        //            int TeamId = oTeam.TeamId;

        //            ParticipatingTeam oPTeam = oTeams.FirstOrDefault(pt => pt.TeamId == TeamId);
        //            if (oPTeam == null)
        //            {
        //                AuditRepository.AuditChange(_context, "vsand_GameReportTeam", "GameReportTeamId", oTeam.GameReportTeamId, "Delete", Username, UserId);
        //                // -- Remove any related objects as well
        //                // oTeam.EventPlayers.Load();

        //                // TODO: make sure this doesn't crash
        //                _context.VsandGameReportEventPlayer.RemoveRange(oTeam.EventPlayers);

        //                /*
        //                for (int iEPlayer = oTeam.EventPlayers.Count - 1; iEPlayer >= 0; iEPlayer += -1)
        //                {
        //                    VsandGameReportEventPlayer oEPlayer = oTeam.EventPlayers.ElementAt(iEPlayer);
        //                    _context.VsandGameReportEventPlayer.Remove(oEPlayer);
        //                }
        //                */

        //                // oTeam.EventPlayerGroups.Load();

        //                // TODO: make sure this doesn't crash
        //                _context.VsandGameReportEventPlayerGroup.RemoveRange(oTeam.EventPlayerGroups);

        //                /*
        //                for (int iEGroup = oTeam.EventPlayerGroups.Count - 1; iEGroup >= 0; iEGroup += -1)
        //                {
        //                    VsandGameReportEventPlayerGroup oEGroup = oTeam.EventPlayerGroups.ElementAt(iEGroup);
        //                    _context.VsandGameReportEventPlayerGroup.Remove(oEGroup);
        //                }
        //                */

        //                _context.VsandGameReportTeam.Remove(oTeam);

        //                bTeamsChanged = true;
        //            }
        //            else
        //            {
        //                AuditRepository.AuditChange(_context, "vsand_GameReportTeam", "GameReportTeamId", oTeam.GameReportTeamId, "Update", Username, UserId);
        //                // -- Update the team with the new info
        //                if (oTeam.FinalScore != oPTeam.Score)
        //                {
        //                    bScoreChanged = true;
        //                }
        //                oTeam.FinalScore = oPTeam.Score;
        //                oTeam.Abbreviation = oPTeam.Abbreviation;
        //                oTeam.HomeTeam = oPTeam.HomeTeam;
        //                if (bLowScoreWins)
        //                {
        //                    if (oPTeam.Score < dBestScore)
        //                    {
        //                        dBestScore = oPTeam.Score;
        //                    }
        //                }
        //                else if (oPTeam.Score > dBestScore)
        //                    dBestScore = oPTeam.Score;
        //            }
        //        }

        //        if (oTeams.Count != oGameReport.Teams.Count)
        //        {
        //            int ScheduleYearId = oGameReport.ScheduleYearId;
        //            int SportId = oGameReport.SportId;

        //            // -- There are adds
        //            foreach (ParticipatingTeam oPTeam in oTeams)
        //            {
        //                int iPTeamId = oPTeam.TeamId;

        //                if (oPTeam.TeamId == 0)
        //                {
        //                    // -- Need to create school (maybe) and team (for sure)
        //                    // int SchoolId = _schoolRepository.SchoolQuickAdd(oPTeam.TeamName, oPTeam.State);
        //                    if (schoolId > 0)
        //                    {
        //                        // TODO: implement team repository and move this out of here
        //                        iPTeamId = 0; // VSAND.Helper.Team.QuickAddTeam(oPTeam.TeamName, SchoolId, SportId, ScheduleYearId);
        //                    }
        //                }

        //                if (iPTeamId > 0)
        //                {
        //                    bool bWinningTeam = false;
        //                    if (bLowScoreWins)
        //                    {
        //                        if (oPTeam.Score < dBestScore)
        //                        {
        //                            bWinningTeam = true;
        //                        }
        //                    }
        //                    else if (oPTeam.Score > dBestScore)
        //                    {
        //                        bWinningTeam = true;
        //                    }

        //                    ICollection<VsandGameReportTeam> oGameTeams = oGameReport.Teams;
        //                    VsandGameReportTeam oGRT = oGameTeams.FirstOrDefault(grt => grt.TeamId == iPTeamId);
        //                    if (oGRT == null)
        //                    {
        //                        oGRT = new VsandGameReportTeam
        //                        {
        //                            TeamName = oPTeam.TeamName,
        //                            HomeTeam = oPTeam.HomeTeam,
        //                            FinalScore = oPTeam.Score,
        //                            WinningTeam = bWinningTeam,
        //                            Abbreviation = oPTeam.Abbreviation,
        //                            TeamId = iPTeamId,
        //                        };
        //                        oGRT.HomeTeam = oPTeam.HomeTeam;
        //                        oGameReport.Teams.Add(oGRT);

        //                        bTeamsChanged = true;
        //                        bScoreChanged = true;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    try
        //    {
        //        _context.SaveChanges();

        //        // -- Update the game name
        //        AuditRepository.AuditChange(_context, "vsand_GameReport", "GameReportId", GameReportId, "Update", Username, UserId);
        //        oGameReport.Name = CreateGameName(oGameReport.GameTypeId, oGameReport.RoundId.Value, oGameReport.SectionId.Value, oGameReport.GroupId.Value, oTeams, schoolId);
        //        _context.SaveChanges();

        //        bRet = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, ex.Message);
        //    }

        //    if (bRet)
        //    {
        //        if (bTeamsChanged)
        //        {
        //            // TODO: event remediation
        //            // VSAND.Events.GameReport.RaiseGameTeamsChanged(new object(), new VSAND.Events.GameReport.GameReportEventArgs(GameReportId, UserId));
        //        }
        //        else
        //        {
        //            // TODO: event remediation
        //            // VSAND.Events.GameReport.RaiseGameUpdated(new object(), new VSAND.Events.GameReport.GameReportEventArgs(GameReportId, UserId));
        //            // VSAND.Events.GameReport.RaiseScoringUpdated(new object(), new VSAND.Events.GameReport.PeriodScoreEventArgs(GameReportId, UserId));
        //            if (bEnablePlayerOfRecord && bScoreChanged)
        //            {
        //                // TODO: implement game report player stat repository and move this out of here
        //                // VSAND.Helper.GameReportPlayerStat.UpdatePlayerOfRecordStats(GameReportId);
        //            }
        //        }

        //        // -- Update the team records
        //        foreach (ParticipatingTeam oTeam in oTeams)
        //        {
        //            if (oTeam.TeamId > 0)
        //            {
        //                // TODO: event remediation
        //                // VSAND.Events.Records.RaiseRecalculate(new object(), new VSAND.Events.Records.RecordsEventArgs(oTeam.TeamId));
        //            }
        //        }
        //    }

        //    return bRet;
        //}

        public async Task<bool> UpdateGameReportMeta(int gameReportId, List<GameReportMeta> oMetaList, AppxUser user)
        {
            bool bRet = false;

            int SportId = 0;

            var oGameMeta = await _uow.GameReportMeta.List(grm => grm.GameReportId == gameReportId);

            foreach (GameReportMeta meta in oMetaList)
            {
                var oGm = oGameMeta.FirstOrDefault(grm => grm.SportGameMetaId == meta.SportGameMetaId);
                if (oGm == null)
                {
                    oGm = new VsandGameReportMeta()
                    {
                        GameReportId = gameReportId,
                        SportGameMetaId = meta.SportGameMetaId
                    };
                    await _uow.GameReportMeta.Insert(oGm);
                }

                oGm.MetaValue = meta.MetaValue;
            }

            // update game report modified date
            var gameReport = await _uow.GameReports.Single(gr => gr.GameReportId == gameReportId);
            gameReport.ModifiedDate = DateTime.Now;

            bRet = await _uow.Save();

            if (bRet && SportId == 17)
            {
                // TODO implement game report events repository and move this out of here
                // VSAND.Helper.GameReportEvents.SortEventsByStartingWeightClass(GameReportId, bExhibition);
            }

            return bRet;
        }

        public async Task<VsandGameReport> GetEntityGameRport(int gameReportId)
        {
            return await _uow.GameReports.GetById(gameReportId);

        }

        public async Task<ServiceResult<VsandGameReport>> DeleteGameReportAsync(int gameReportId)
        {
            var oRet = new ServiceResult<VsandGameReport>();

            VsandGameReport remGameReport = await GetEntityGameRport(gameReportId);
            if (remGameReport.Deleted == false)
            {
                remGameReport.Deleted = true;
            }
            else
            {
                remGameReport.Deleted = false;
            }
            _uow.GameReports.Update(remGameReport);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = remGameReport;
                oRet.Success = true;
                oRet.Id = remGameReport.GameReportId;
                oRet.Message = remGameReport.Deleted.ToString();
            }

            return oRet;
        }
        #endregion
    }
}
