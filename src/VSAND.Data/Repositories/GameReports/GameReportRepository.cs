using NLog;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using VSAND.Common;
using VSAND.Data.Entities;
using VSAND.Data.Identity;
using VSAND.Data.ViewModels;
using System.Threading.Tasks;
using VSAND.Data.ViewModels.GameReport;

namespace VSAND.Data.Repositories
{
    public class GameReportRepository : Repository<VsandGameReport>, IGameReportRepository
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly VsandContext _context;
        public GameReportRepository(VsandContext context) : base(context)
        {
            _context = context ?? throw new ArgumentException("Context is null");
        }
                

        public VsandGameReport GetGameReport(int GameReportId)
        {
            VsandGameReport oGame = null;

            if (GameReportId > 0)
            {
                oGame = (from g in _context.VsandGameReport
                            .Include(gr => gr.Teams).ThenInclude(t => t.Team).ThenInclude(t => t.School)
                         where g.GameReportId == GameReportId
                         select g).FirstOrDefault();
            }

            return oGame;
        }

        public bool IsRegularSeason(int GameReportId)
        {
            bool bRet = false;

            VsandSportEventType oSE = (from gr in _context.VsandGameReport
                                       where gr.GameReportId == GameReportId
                                       select gr.EventType).FirstOrDefault();

            if (oSE != null)
            {
                if (oSE.DefaultSelected.Value)
                {
                    bRet = true;
                }
            }

            return bRet;
        }

        public async Task<VsandGameReport> GetFullGameReport(int GameReportId, int PublicationStoryId = 0)
        {
            VsandGameReport oRet = null;

            // oDb.GameReportSet.MergeOption = MergeOption.NoTracking
            VsandGameReport oGr = await (from gr in _context.VsandGameReport
                                             .Include(gr => gr.County)
                                             .Include(gr => gr.Sport)
                                             .Include(gr => gr.EventType)
                                             .Include(gr => gr.Round)
                                             .Include(gr => gr.Section)
                                             .Include(gr => gr.Group)
                                             .Include(gr => gr.Teams).ThenInclude(t => t.Team)
                                             .Include(gr => gr.Teams).ThenInclude(t => t.TeamStats).ThenInclude(ts => ts.SportTeamStat).ThenInclude(sts => sts.SportTeamStatCategory)
                                             .Include(gr => gr.Teams).ThenInclude(t => t.GameRoster).ThenInclude(gr => gr.Player)
                                             .Include(gr => gr.Teams).ThenInclude(t => t.GameRoster).ThenInclude(gr => gr.Position)
                                             .Include(gr => gr.Meta).ThenInclude(m => m.SportGameMeta)
                                         where gr.GameReportId == GameReportId
                                         select gr).FirstOrDefaultAsync();

            IEnumerable<VsandGameReportPlayByPlay> oScoringPlays = (from pbp in _context.VsandGameReportPlayByPlay
                                                                    where pbp.GameReport.GameReportId == GameReportId
                                                                    orderby pbp.SortOrder ascending
                                                                    select pbp);

            foreach (VsandGameReportTeam OGRT in oGr.Teams)
            {
                if (OGRT.TeamWins == 0 && OGRT.TeamLosses == 0)
                {
                    if (OGRT.Team != null)
                    {
                        // TODO: convert TeamRecord to service?
                        int TeamId = OGRT.Team.TeamId;

                        /*
                        Helper.TeamRecordInfo oRec = new Helper.TeamRecordInfo(TeamId, true);
                        OGRT.TeamWins = oRec.Wins;
                        OGRT.TeamLosses = oRec.Losses;
                        OGRT.TeamTies = oRec.Ties;
                        */
                    }
                }
            }

            if (oScoringPlays != null)
            {
                List<VsandGameReportPlayByPlay> oScoreList = oScoringPlays.ToList();
            }

            // -- Include publication scoring play by plays (if they exist)
            if (PublicationStoryId > 0)
            {
                IEnumerable<VsandPublicationStoryPlayByPlay> oPubPlayByPlays = (from pbp in _context.VsandPublicationStoryPlayByPlay
                                                                                where pbp.PublicationStory.PublicationStoryId == PublicationStoryId
                                                                                && pbp.GameReportPlayByPlay.GameReport.GameReportId == GameReportId
                                                                                select pbp);

                if (oPubPlayByPlays != null)
                {
                    List<VsandPublicationStoryPlayByPlay> oPBPList = oPubPlayByPlays.ToList();
                }
            }

            // -- Include Events
            IEnumerable<VsandGameReportEvent> oEvents = (from e in _context.VsandGameReportEvent.Include(gre => gre.SportEvent)
                                                         where e.GameReport.GameReportId == GameReportId
                                                         orderby e.SortOrder ascending
                                                         select e);
            if (oEvents != null)
            {
                List<VsandGameReportEvent> oEventList = oEvents.ToList();
            }

            // -- Include Event Results
            IEnumerable<VsandGameReportEventResult> oResults = (from r in _context.VsandGameReportEventResult
                                                                        .Include(grer => grer.EventPlayers).ThenInclude(grep => grep.Player)
                                                                        .Include(grer => grer.EventPlayerGroups).ThenInclude(epg => epg.EventPlayerGroupPlayers).ThenInclude(epgp => epgp.Player)
                                                                where r.GameReportEvent.GameReport.GameReportId == GameReportId
                                                                orderby r.SortOrder ascending
                                                                select r);
            if (oResults != null)
            {
                List<VsandGameReportEventResult> oRList = oResults.ToList();
            }

            // -- Event Event Player Stats
            IEnumerable<VsandGameReportEventPlayerStat> oEPStats = (from ep in _context.VsandGameReportEventPlayerStat.Include(greps => greps.SportEventStat)
                                                                    where ep.EventPlayer.EventResult.GameReportEvent.GameReport.GameReportId == GameReportId
                                                                    select ep);
            if (oEPStats != null)
            {
                List<VsandGameReportEventPlayerStat> oEPStatList = oEPStats.ToList();
            }

            IEnumerable<VsandGameReportPlayerStat> oPStats = (from ps in _context.VsandGameReportPlayerStat.Include(grps => grps.SportPlayerStat).ThenInclude(sps => sps.SportPlayerStatCategory)
                                                              where ps.GameReport.GameReportId == GameReportId
                                                              select ps);
            if (oPStats != null)
            {
                List<VsandGameReportPlayerStat> oPStatList = oPStats.ToList();
            }

            // -- Event Player Group Stats
            IEnumerable<VsandGameReportEventPlayerGroupStat> oEPGStats = (from epg in _context.VsandGameReportEventPlayerGroupStat.Include(grepgs => grepgs.SportEventStat)
                                                                          where epg.EventPlayerGroup.EventResult.GameReportEvent.GameReport.GameReportId == GameReportId
                                                                          select epg);

            if (oEPGStats != null)
            {
                List<VsandGameReportEventPlayerGroupStat> oEPGStatList = oEPGStats.ToList();
            }

            // -- Period Scores, sorted
            IEnumerable<VsandGameReportPeriodScore> oPScores = (from ps in _context.VsandGameReportPeriodScore
                                                                where ps.GameReportTeam.GameReport.GameReportId == GameReportId
                                                                orderby ps.IsOtperiod ascending, ps.PeriodNumber ascending
                                                                select ps);

            if (oPScores != null)
            {
                List<VsandGameReportPeriodScore> oPScoreList = oPScores.ToList();
            }

            if (oGr != null)
            {
                oRet = oGr;
            }

            return oRet;
        }

        public List<VsandGameReport> GetOpenGames()
        {
            List<VsandGameReport> oRet = null;

            IEnumerable<VsandGameReport> oGr = (from gr in _context.VsandGameReport
                                            .Include(gr => gr.County)
                                            .Include(gr => gr.Sport)
                                            .Include(gr => gr.EventType)
                                            .Include(gr => gr.Round)
                                            .Include(gr => gr.Section)
                                            .Include(gr => gr.Group)
                                            .Include(gr => gr.GamePackages)
                                            .Include(gr => gr.ScoringPlays)
                                            .Include(gr => gr.Teams).ThenInclude(t => t.Team)
                                            .Include(gr => gr.Teams).ThenInclude(t => t.TeamStats).ThenInclude(ts => ts.SportTeamStat).ThenInclude(sts => sts.SportTeamStatCategory)
                                            .Include(gr => gr.Teams).ThenInclude(t => t.GameRoster).ThenInclude(gr => gr.Player)
                                                where gr.Archived == false
                                                select gr);

            // -- Include Events
            IEnumerable<VsandGameReportEvent> oEvents = (from e in _context.VsandGameReportEvent.Include(gre => gre.SportEvent)
                                                         where e.GameReport.Archived == false
                                                         orderby e.SortOrder ascending
                                                         select e);
            if (oEvents != null)
            {
                List<VsandGameReportEvent> oEventList = oEvents.ToList();
            }

            // -- Include Event Results
            IEnumerable<VsandGameReportEventResult> oResults = (from r in _context.VsandGameReportEventResult
                                                                        .Include(grer => grer.EventPlayers).ThenInclude(ep => ep.Player)
                                                                        .Include(grer => grer.EventPlayerGroups).ThenInclude(epg => epg.EventPlayerGroupPlayers).ThenInclude(epgp => epgp.Player)
                                                                where r.GameReportEvent.GameReport.Archived == false
                                                                orderby r.SortOrder ascending
                                                                select r);
            if (oResults != null)
            {
                List<VsandGameReportEventResult> oRList = oResults.ToList();
            }

            // -- Event Event Player Stats
            IEnumerable<VsandGameReportEventPlayerStat> oEPStats = (from ep in _context.VsandGameReportEventPlayerStat.Include(greps => greps.SportEventStat)
                                                                    where ep.EventPlayer.EventResult.GameReportEvent.GameReport.Archived == false
                                                                    select ep);
            if (oEPStats != null)
            {
                List<VsandGameReportEventPlayerStat> oEPStatList = oEPStats.ToList();
            }

            IEnumerable<VsandGameReportPlayerStat> oPStats = (from ps in _context.VsandGameReportPlayerStat.Include(greps => greps.SportPlayerStat).ThenInclude(sps => sps.SportPlayerStatCategory)
                                                              where ps.GameReport.Archived == false
                                                              select ps);
            if (oPStats != null)
            {
                List<VsandGameReportPlayerStat> oPStatList = oPStats.ToList();
            }

            // -- Event Player Group Stats
            IEnumerable<VsandGameReportEventPlayerGroupStat> oEPGStats = (from epg in _context.VsandGameReportEventPlayerGroupStat.Include(grepgs => grepgs.SportEventStat)
                                                                          where epg.EventPlayerGroup.EventResult.GameReportEvent.GameReport.Archived == false
                                                                          select epg);

            if (oEPGStats != null)
            {
                List<VsandGameReportEventPlayerGroupStat> oEPGStatList = oEPGStats.ToList();
            }

            // -- Period Scores, sorted
            IEnumerable<VsandGameReportPeriodScore> oPScores = (from ps in _context.VsandGameReportPeriodScore
                                                                where ps.GameReportTeam.GameReport.Archived == false
                                                                orderby ps.IsOtperiod ascending, ps.PeriodNumber ascending
                                                                select ps);

            if (oPScores != null)
            {
                List<VsandGameReportPeriodScore> oPScoreList = oPScores.ToList();
            }

            if (oGr != null)
            {
                oRet = oGr.ToList();
            }

            return oRet;
        }

        public List<VsandGameReport> GetOpenUnsentGames(int CountyId, int SportId)
        {
            List<VsandGameReport> oRet = null;

            IQueryable<VsandGameReport> oGr = (from gr in _context.VsandGameReport
                                            .Include(gr => gr.County)
                                            .Include(gr => gr.Sport)
                                            .Include(gr => gr.EventType)
                                            .Include(gr => gr.Round)
                                            .Include(gr => gr.Section)
                                            .Include(gr => gr.Group)
                                            .Include(gr => gr.Teams).ThenInclude(t => t.Team)
                                            .Include(gr => gr.PublicationStories)
                                               where gr.Archived == false && gr.GamePackages.Count == 0
                                               select gr);

            if (CountyId > 0)
            {
                oGr = oGr.Where(gr => gr.County.CountyId == CountyId);
            }

            if (SportId > 0)
            {
                oGr = oGr.Where(gr => gr.Sport.SportId == SportId);
            }

            if (oGr != null)
            {
                oRet = oGr.ToList();
            }

            return oRet;
        }

        public List<VsandGameReport> GetOpenSentGames(int CountyId, int SportId)
        {
            List<VsandGameReport> oRet = null;


            IEnumerable<VsandGameReport> oGr = (from gr in _context.VsandGameReport
                                            .Include(gr => gr.County)
                                            .Include(gr => gr.Sport)
                                            .Include(gr => gr.EventType)
                                            .Include(gr => gr.Round)
                                            .Include(gr => gr.Section)
                                            .Include(gr => gr.Group)
                                            .Include(gr => gr.Teams).ThenInclude(t => t.Team)
                                                where gr.Archived == false && gr.GamePackages.Count > 0
                                                select gr);

            if (CountyId > 0)
            {
                oGr = oGr.Where(gr => gr.County.CountyId == CountyId);
            }

            if (SportId > 0)
            {
                oGr = oGr.Where(gr => gr.Sport.SportId == SportId);
            }

            if (oGr != null)
            {
                oRet = oGr.ToList();
            }

            return oRet;
        }

        public List<int> GetOpenGameIds()
        {
            List<int> oRet = null;

            IEnumerable<int> oGr = (from gr in _context.VsandGameReport
                                    where gr.Archived == false
                                    select gr.GameReportId);

            if (oGr != null)
            {
                oRet = oGr.ToList();
            }

            return oRet;
        }

        public List<VsandGameReport> GetOpenBySport(int SportId)
        {
            List<VsandGameReport> oRet = null;

            IEnumerable<VsandGameReport> oGr = (from gr in _context.VsandGameReport
                                            .Include(gr => gr.County)
                                            .Include(gr => gr.Sport)
                                            .Include(gr => gr.EventType)
                                            .Include(gr => gr.Round)
                                            .Include(gr => gr.Section)
                                            .Include(gr => gr.Group)
                                            .Include(gr => gr.GamePackages)
                                            .Include(gr => gr.ScoringPlays)
                                            .Include(gr => gr.Teams).ThenInclude(t => t.Team)
                                            .Include(gr => gr.Teams).ThenInclude(t => t.TeamStats).ThenInclude(ts => ts.SportTeamStat).ThenInclude(sts => sts.SportTeamStatCategory)
                                            .Include(gr => gr.Teams).ThenInclude(t => t.GameRoster).ThenInclude(gr => gr.Player)
                                            .Include(gr => gr.ReportedByUser)
                                                where gr.Sport.SportId == SportId && gr.Archived == false
                                                select gr);

            // -- Include Events
            IEnumerable<VsandGameReportEvent> oEvents = (from e in _context.VsandGameReportEvent.Include(gre => gre.SportEvent)
                                                         where e.GameReport.Sport.SportId == SportId
                                                         && e.GameReport.Archived == false
                                                         orderby e.SortOrder ascending
                                                         select e);
            if (oEvents != null)
            {
                List<VsandGameReportEvent> oEventList = oEvents.ToList();
            }

            // -- Include Event Results
            IEnumerable<VsandGameReportEventResult> oResults = (from r in _context.VsandGameReportEventResult.Include(grer => grer.EventPlayers)
                                                                where r.GameReportEvent.GameReport.Sport.SportId == SportId && r.GameReportEvent.GameReport.Archived == false
                                                                orderby r.SortOrder ascending
                                                                select r);
            if (oResults != null)
            {
                List<VsandGameReportEventResult> oRList = oResults.ToList();
            }

            // -- Event Event Player Stats
            IEnumerable<VsandGameReportEventPlayerStat> oEPStats = (from ep in _context.VsandGameReportEventPlayerStat.Include(greps => greps.SportEventStat)
                                                                    where ep.EventPlayer.EventResult.GameReportEvent.GameReport.Sport.SportId == SportId
                                                                    && ep.EventPlayer.EventResult.GameReportEvent.GameReport.Archived == false
                                                                    select ep);
            if (oEPStats != null)
            {
                List<VsandGameReportEventPlayerStat> oEPStatList = oEPStats.ToList();
            }

            IEnumerable<VsandGameReportPlayerStat> oPStats = (from ps in _context.VsandGameReportPlayerStat.Include(grps => grps.SportPlayerStat).ThenInclude(sps => sps.SportPlayerStatCategory)
                                                              where ps.GameReport.Sport.SportId == SportId && ps.GameReport.Archived == false
                                                              select ps);
            if (oPStats != null)
            {
                List<VsandGameReportPlayerStat> oPStatList = oPStats.ToList();
            }

            // -- Period Scores, sorted
            IEnumerable<VsandGameReportPeriodScore> oPScores = (from ps in _context.VsandGameReportPeriodScore
                                                                where ps.GameReportTeam.GameReport.Sport.SportId == SportId && ps.GameReportTeam.GameReport.Archived == false
                                                                orderby ps.IsOtperiod ascending, ps.PeriodNumber ascending
                                                                select ps);

            if (oPScores != null)
            {
                List<VsandGameReportPeriodScore> oPScoreList = oPScores.ToList();
            }

            if (oGr != null)
            {
                oRet = oGr.ToList();
            }

            return oRet;
        }

        public List<VsandGameReport> GetForDateBySport(int SportId, DateTime GameDate)
        {
            List<VsandGameReport> oRet = null;

            IEnumerable<VsandGameReport> oGr = (from gr in _context.VsandGameReport
                                            .Include(gr => gr.County)
                                            .Include(gr => gr.Sport)
                                            .Include(gr => gr.EventType)
                                            .Include(gr => gr.Round)
                                            .Include(gr => gr.Section)
                                            .Include(gr => gr.Group)
                                            .Include(gr => gr.GamePackages)
                                            .Include(gr => gr.ScoringPlays)
                                            .Include(gr => gr.Teams).ThenInclude(t => t.Team)
                                            .Include(gr => gr.Teams).ThenInclude(t => t.TeamStats).ThenInclude(ts => ts.SportTeamStat).ThenInclude(sts => sts.SportTeamStatCategory)
                                            .Include(gr => gr.Teams).ThenInclude(t => t.GameRoster).ThenInclude(gr => gr.Player)
                                            .Include(gr => gr.ReportedByUser)
                                                where gr.Sport.SportId == SportId && gr.GameDate.Year == GameDate.Year && gr.GameDate.Month == GameDate.Month && gr.GameDate.Day == GameDate.Day
                                                select gr);

            // -- Include Events
            IEnumerable<VsandGameReportEvent> oEvents = (from e in _context.VsandGameReportEvent.Include(gre => gre.SportEvent)
                                                         where e.GameReport.Sport.SportId == SportId
                                                         && e.GameReport.GameDate.Year == GameDate.Year && e.GameReport.GameDate.Month == GameDate.Month && e.GameReport.GameDate.Day == GameDate.Day
                                                         orderby e.SortOrder ascending
                                                         select e);
            if (oEvents != null)
            {
                List<VsandGameReportEvent> oEventList = oEvents.ToList();
            }

            // -- Include Event Results
            IEnumerable<VsandGameReportEventResult> oResults = (from r in _context.VsandGameReportEventResult.Include(grer => grer.EventPlayers)
                                                                where r.GameReportEvent.GameReport.Sport.SportId == SportId
                                                                && r.GameReportEvent.GameReport.GameDate.Year == GameDate.Year && r.GameReportEvent.GameReport.GameDate.Month == GameDate.Month && r.GameReportEvent.GameReport.GameDate.Day == GameDate.Day
                                                                orderby r.SortOrder ascending
                                                                select r);
            if (oResults != null)
            {
                List<VsandGameReportEventResult> oRList = oResults.ToList();
            }

            // -- Event Event Player Stats
            IEnumerable<VsandGameReportEventPlayerStat> oEPStats = (from ep in _context.VsandGameReportEventPlayerStat.Include(greps => greps.SportEventStat)
                                                                    where ep.EventPlayer.EventResult.GameReportEvent.GameReport.Sport.SportId == SportId
                                                                    && ep.EventPlayer.EventResult.GameReportEvent.GameReport.GameDate.Year == GameDate.Year
                                                                    && ep.EventPlayer.EventResult.GameReportEvent.GameReport.GameDate.Month == GameDate.Month
                                                                    && ep.EventPlayer.EventResult.GameReportEvent.GameReport.GameDate.Day == GameDate.Day
                                                                    select ep);
            if (oEPStats != null)
            {
                List<VsandGameReportEventPlayerStat> oEPStatList = oEPStats.ToList();
            }

            IEnumerable<VsandGameReportPlayerStat> oPStats = (from ps in _context.VsandGameReportPlayerStat.Include(grps => grps.SportPlayerStat).ThenInclude(sps => sps.SportPlayerStatCategory)
                                                              where ps.GameReport.Sport.SportId == SportId
                                                              && ps.GameReport.GameDate.Year == GameDate.Year
                                                              && ps.GameReport.GameDate.Month == GameDate.Month
                                                              && ps.GameReport.GameDate.Day == GameDate.Day
                                                              select ps);
            if (oPStats != null)
            {
                List<VsandGameReportPlayerStat> oPStatList = oPStats.ToList();
            }

            // -- Period Scores, sorted
            IEnumerable<VsandGameReportPeriodScore> oPScores = (from ps in _context.VsandGameReportPeriodScore
                                                                where ps.GameReportTeam.GameReport.Sport.SportId == SportId
                                                                && ps.GameReportTeam.GameReport.GameDate.Year == GameDate.Year
                                                                && ps.GameReportTeam.GameReport.GameDate.Month == GameDate.Month
                                                                && ps.GameReportTeam.GameReport.GameDate.Day == GameDate.Day
                                                                orderby ps.IsOtperiod ascending, ps.PeriodNumber ascending
                                                                select ps);

            if (oPScores != null)
            {
                List<VsandGameReportPeriodScore> oPScoreList = oPScores.ToList();
            }


            return oRet;
        }

        public List<VsandGameReport> GetOpenForFeedSubscription(int SportId, int FeedSubscriptionId)
        {
            List<VsandGameReport> oRet = null;


            IEnumerable<VsandGameReport> oGr = (from gr in _context.VsandGameReport
                                            .Include(gr => gr.County)
                                            .Include(gr => gr.Sport)
                                            .Include(gr => gr.EventType)
                                            .Include(gr => gr.Round)
                                            .Include(gr => gr.Section)
                                            .Include(gr => gr.Group)
                                            .Include(gr => gr.GamePackages)
                                            .Include(gr => gr.ScoringPlays)
                                            .Include(gr => gr.Teams).ThenInclude(t => t.Team)
                                            .Include(gr => gr.Notes)
                                            .Include(gr => gr.Teams).ThenInclude(t => t.TeamStats).ThenInclude(ts => ts.SportTeamStat).ThenInclude(sts => sts.SportTeamStatCategory)
                                            .Include(gr => gr.Teams).ThenInclude(t => t.GameRoster).ThenInclude(gr => gr.Player)
                                            .Include(gr => gr.ReportedByUser)
                                                where gr.Sport.SportId == SportId && gr.Archived == false
                                                && gr.Teams.Any(gt => gt.Team.School.FeedSubscriptions.Any(fs => fs.FeedSubscription.FeedSubscriptionId == FeedSubscriptionId))
                                                select gr);

            // -- Include Events
            IEnumerable<VsandGameReportEvent> oEvents = (from e in _context.VsandGameReportEvent.Include(gre => gre.SportEvent)
                                                         where e.GameReport.Sport.SportId == SportId && e.GameReport.Archived == false
                                                         && e.GameReport.Teams.Any(gt => gt.Team.School.FeedSubscriptions.Any(fs => fs.FeedSubscription.FeedSubscriptionId == FeedSubscriptionId))
                                                         orderby e.SortOrder ascending
                                                         select e);
            if (oEvents != null)
            {
                List<VsandGameReportEvent> oEventList = oEvents.ToList();
            }

            // -- Include Event Results
            IEnumerable<VsandGameReportEventResult> oResults = (from r in _context.VsandGameReportEventResult.Include(grer => grer.EventPlayers)
                                                                where r.GameReportEvent.GameReport.Sport.SportId == SportId
                                                                && r.GameReportEvent.GameReport.Archived == false
                                                                && r.GameReportEvent.GameReport.Teams.Any(gt => gt.Team.School.FeedSubscriptions.Any(fs => fs.FeedSubscription.FeedSubscriptionId == FeedSubscriptionId))
                                                                orderby r.SortOrder ascending
                                                                select r);
            if (oResults != null)
            {
                List<VsandGameReportEventResult> oRList = oResults.ToList();
            }

            // -- Event Event Player Stats
            IEnumerable<VsandGameReportEventPlayerStat> oEPStats = (from ep in _context.VsandGameReportEventPlayerStat.Include(greps => greps.SportEventStat)
                                                                    where ep.EventPlayer.EventResult.GameReportEvent.GameReport.Sport.SportId == SportId
                                                                    && ep.EventPlayer.EventResult.GameReportEvent.GameReport.Archived == false
                                                                    && ep.EventPlayer.GameReportTeam.Team.School.FeedSubscriptions.Any(fs => fs.FeedSubscription.FeedSubscriptionId == FeedSubscriptionId)
                                                                    select ep);
            if (oEPStats != null)
            {
                List<VsandGameReportEventPlayerStat> oEPStatList = oEPStats.ToList();
            }

            IEnumerable<VsandGameReportPlayerStat> oPStats = (from ps in _context.VsandGameReportPlayerStat.Include(grps => grps.SportPlayerStat).ThenInclude(sps => sps.SportPlayerStatCategory)
                                                              where ps.GameReport.Sport.SportId == SportId && ps.GameReport.Archived == false
                                                              && ps.GameReport.Teams.Any(gt => gt.Team.School.FeedSubscriptions.Any(fs => fs.FeedSubscription.FeedSubscriptionId == FeedSubscriptionId))
                                                              select ps);
            if (oPStats != null)
            {
                List<VsandGameReportPlayerStat> oPStatList = oPStats.ToList();
            }

            // -- Period Scores, sorted
            IEnumerable<VsandGameReportPeriodScore> oPScores = (from ps in _context.VsandGameReportPeriodScore
                                                                where ps.GameReportTeam.GameReport.Sport.SportId == SportId
                                                                && ps.GameReportTeam.GameReport.Archived == false
                                                                && ps.GameReportTeam.Team.School.FeedSubscriptions.Any(fs => fs.FeedSubscription.FeedSubscriptionId == FeedSubscriptionId)
                                                                orderby ps.IsOtperiod ascending, ps.PeriodNumber ascending
                                                                select ps);

            if (oPScores != null)
            {
                List<VsandGameReportPeriodScore> oPScoreList = oPScores.ToList();
            }

            if (oGr != null)
            {
                oRet = oGr.ToList();
            }

            return oRet;
        }

        public List<VsandGameReport> GetForDateForFeedSubscription(int SportId, int FeedSubscriptionId, DateTime GameDate)
        {
            List<VsandGameReport> oRet = null;


            IEnumerable<VsandGameReport> oGr = (from gr in _context.VsandGameReport
                                            .Include(gr => gr.County)
                                            .Include(gr => gr.Sport)
                                            .Include(gr => gr.EventType)
                                            .Include(gr => gr.Round)
                                            .Include(gr => gr.Section)
                                            .Include(gr => gr.Group)
                                            .Include(gr => gr.GamePackages)
                                            .Include(gr => gr.ScoringPlays)
                                            .Include(gr => gr.Teams).ThenInclude(t => t.Team)
                                            .Include(gr => gr.Teams).ThenInclude(t => t.TeamStats).ThenInclude(ts => ts.SportTeamStat).ThenInclude(sts => sts.SportTeamStatCategory)
                                            .Include(gr => gr.Teams).ThenInclude(t => t.GameRoster).ThenInclude(gr => gr.Player)
                                            .Include(gr => gr.ReportedByUser)
                                                where gr.Sport.SportId == SportId
                                                && gr.GameDate.Year == GameDate.Year && gr.GameDate.Month == GameDate.Month && gr.GameDate.Day == GameDate.Day
                                                && gr.Teams.Any(gt => gt.Team.School.FeedSubscriptions.Any(fs => fs.FeedSubscription.FeedSubscriptionId == FeedSubscriptionId))
                                                select gr);

            // -- Include Events
            IEnumerable<VsandGameReportEvent> oEvents = (from e in _context.VsandGameReportEvent.Include(gre => gre.SportEvent)
                                                         where e.GameReport.Sport.SportId == SportId
                                                         && e.GameReport.GameDate.Year == GameDate.Year && e.GameReport.GameDate.Month == GameDate.Month && e.GameReport.GameDate.Day == GameDate.Day
                                                         && e.GameReport.Teams.Any(gt => gt.Team.School.FeedSubscriptions.Any(fs => fs.FeedSubscription.FeedSubscriptionId == FeedSubscriptionId))
                                                         orderby e.SortOrder ascending
                                                         select e);
            if (oEvents != null)
            {
                List<VsandGameReportEvent> oEventList = oEvents.ToList();
            }

            // -- Include Event Results
            IEnumerable<VsandGameReportEventResult> oResults = (from r in _context.VsandGameReportEventResult.Include(grer => grer.EventPlayers)
                                                                where r.GameReportEvent.GameReport.Sport.SportId == SportId
                                                                && r.GameReportEvent.GameReport.GameDate.Year == GameDate.Year && r.GameReportEvent.GameReport.GameDate.Month == GameDate.Month && r.GameReportEvent.GameReport.GameDate.Day == GameDate.Day
                                                                && r.GameReportEvent.GameReport.Teams.Any(gt => gt.Team.School.FeedSubscriptions.Any(fs => fs.FeedSubscription.FeedSubscriptionId == FeedSubscriptionId))
                                                                orderby r.SortOrder ascending
                                                                select r);
            if (oResults != null)
            {
                List<VsandGameReportEventResult> oRList = oResults.ToList();
            }

            // -- Event Event Player Stats
            IEnumerable<VsandGameReportEventPlayerStat> oEPStats = (from ep in _context.VsandGameReportEventPlayerStat.Include(greps => greps.SportEventStat)
                                                                    where ep.EventPlayer.EventResult.GameReportEvent.GameReport.Sport.SportId == SportId
                                                                    && ep.EventPlayer.EventResult.GameReportEvent.GameReport.GameDate.Year == GameDate.Year
                                                                    && ep.EventPlayer.EventResult.GameReportEvent.GameReport.GameDate.Month == GameDate.Month
                                                                    && ep.EventPlayer.EventResult.GameReportEvent.GameReport.GameDate.Day == GameDate.Day
                                                                    && ep.EventPlayer.EventResult.GameReportEvent.GameReport.Teams.Any(gt => gt.Team.School.FeedSubscriptions.Any(fs => fs.FeedSubscription.FeedSubscriptionId == FeedSubscriptionId))
                                                                    select ep);
            if (oEPStats != null)
            {
                List<VsandGameReportEventPlayerStat> oEPStatList = oEPStats.ToList();
            }

            IEnumerable<VsandGameReportPlayerStat> oPStats = (from ps in _context.VsandGameReportPlayerStat.Include(grps => grps.SportPlayerStat).ThenInclude(sps => sps.SportPlayerStatCategory)
                                                              where ps.GameReport.Sport.SportId == SportId
                                                              && ps.GameReport.GameDate.Year == GameDate.Year
                                                              && ps.GameReport.GameDate.Month == GameDate.Month
                                                              && ps.GameReport.GameDate.Day == GameDate.Day
                                                              && ps.GameReport.Teams.Any(gt => gt.Team.School.FeedSubscriptions.Any(fs => fs.FeedSubscription.FeedSubscriptionId == FeedSubscriptionId))
                                                              select ps);
            if (oPStats != null)
            {
                List<VsandGameReportPlayerStat> oPStatList = oPStats.ToList();
            }

            // -- Period Scores, sorted
            IEnumerable<VsandGameReportPeriodScore> oPScores = (from ps in _context.VsandGameReportPeriodScore
                                                                where ps.GameReportTeam.GameReport.Sport.SportId == SportId
                                                                && ps.GameReportTeam.GameReport.GameDate.Year == GameDate.Year
                                                                && ps.GameReportTeam.GameReport.GameDate.Month == GameDate.Month
                                                                && ps.GameReportTeam.GameReport.GameDate.Day == GameDate.Day
                                                                && ps.GameReportTeam.GameReport.Teams.Any(gt => gt.Team.School.FeedSubscriptions.Any(fs => fs.FeedSubscription.FeedSubscriptionId == FeedSubscriptionId))
                                                                orderby ps.IsOtperiod ascending, ps.PeriodNumber ascending
                                                                select ps);

            if (oPScores != null)
            {
                List<VsandGameReportPeriodScore> oPScoreList = oPScores.ToList();
            }

            if (oGr != null)
            {
                oRet = oGr.ToList();
            }

            return oRet;
        }

        public VsandGameReport GetGameReportByParticipatingTeam(int GameReportTeamId)
        {
            VsandGameReport oRet = (from gr in _context.VsandGameReport.Include(gr => gr.Sport)
                                    where gr.Teams.Any(grt => grt.GameReportTeamId == GameReportTeamId)
                                    select gr).FirstOrDefault();

            return oRet;
        }

        public int GetGameReportIdByParticipatingTeams(int TeamId1, int TeamId2)
        {
            int iRet = 0;

            VsandGameReport oData = (from gr in _context.VsandGameReport
                                     where gr.Teams.Any(grt => grt.Team.TeamId == TeamId1) && gr.Teams.Any(grt => grt.Team.TeamId == TeamId2) && gr.Teams.Count == 2
                                     select gr).FirstOrDefault();

            if (oData != null)
            {
                iRet = oData.GameReportId;
            }


            return iRet;
        }

        public bool ExcludeGame(int GameReportId, ref string sMsg, ApplicationUser user)
        {
            bool bRet = false;

            List<int> aTeams = new List<int>();

            VsandGameReport oGame = (from gr in _context.VsandGameReport.Include(gr => gr.Teams)
                                     where gr.GameReportId == GameReportId
                                     select gr).FirstOrDefault();

            if (oGame != null)
            {
                string Username = user.AppxUser.UserId;
                int UserId = user.AppxUser.AdminId;
                AuditRepository.AuditChange(_context, "vsand_GameReport", "GameReportId", GameReportId, "Exclude/Include Game", Username, UserId);

                foreach (VsandGameReportTeam oTeam in oGame.Teams)
                {
                    if (!aTeams.Contains(oTeam.TeamId))
                    {
                        aTeams.Add(oTeam.TeamId);
                    }
                }
                if (oGame.Deleted)
                {
                    oGame.Deleted = false;
                }
                else
                {
                    oGame.Deleted = true;
                }

                try
                {
                    _context.SaveChanges();

                    bRet = true;
                }
                catch (Exception ex)
                {
                    sMsg = ex.Message;
                    Log.Error(ex, sMsg);
                }
            }
            else
            {
                sMsg = "The requested game cannot be found.";
            }

            foreach (int TeamId in aTeams)
            {
                // TODO: event remediation
                // VSAND.Events.Records.RaiseRecalculate(new object(), new VSAND.Events.Records.RecordsEventArgs(TeamId));
            }

            return bRet;
        }

        public List<VsandGameReport> SearchGameReports(int SearchSchoolId, int SearchSportId, int SearchScheduleYearId, DateTime SearchGameDate, int SearchTouchedBy, int maximumRows, int startRowIndex)
        {
            List<VsandGameReport> oRet = new List<VsandGameReport>();

            int iSkip = startRowIndex * maximumRows;

            throw new NotImplementedException("Need to translate to an EF query");

            /*
            // TODO: translate this into an EF query
            IEnumerable<VsandGameReport> oResult = from gr in oDb.SearchGameReports(Interaction.IIf(SearchSchoolId > 0, SearchSchoolId, null), Interaction.IIf(SearchSportId > 0, SearchSportId, null), Interaction.IIf(SearchScheduleYearId > 0, SearchScheduleYearId, null), IIf(SearchGameDate > DateHelp.SqlMinDate, SearchGameDate, null), Interaction.IIf(SearchTouchedBy > 0, SearchTouchedBy, null), startRowIndex, maximumRows).OrderByDescending(gr => gr.ReportedDate)
                                                   select gr;

            foreach (VsandGameReport oG in oResult)
            {
                oG.SportReference.Load();
                oG.EventTypeReference.Load();
                oG.EventType.ScheduleYearReference.Load();
                oG.RoundReference.Load();
                oG.SectionReference.Load();
                oG.GroupReference.Load();
                oG.Teams.Load();
                foreach (VsandGameReportTeam oGT in oG.Teams)
                {
                    try
                    {
                        oGT.TeamReference.Load();
                    }
                    catch (Exception ex)
                    {
                    }
                }
                oRet.Add(oG);
            }

            return oRet;
            */
        }

        public int SearchGameReportsCount(int SearchSchoolId, int SearchSportId, int SearchScheduleYearId, DateTime SearchGameDate, int SearchTouchedBy)
        {
            // TODO: remediate this and translate to an entity query
            int? schoolId = SearchSchoolId > 0 ? SearchSchoolId : (int?)null;
            int? sportId = SearchSportId > 0 ? SearchSportId : (int?)null;
            int? scheduleYearId = SearchScheduleYearId > 0 ? SearchScheduleYearId : (int?)null;
            DateTime? gameDate = SearchGameDate > DateHelp.SqlMinDate ? SearchGameDate : (DateTime?)null;
            int? touchedBy = SearchTouchedBy > 0 ? SearchTouchedBy : (int?)null;

            // int ResultCount = _context.Database
            //     .ExecuteSqlCommand("vsandEntities.SearchGameReportsCount @SchoolId = {schoolId}, @SportId = {sportId}, @ScheduleYearId = {scheduleYearId}, @GameDate = {gameData}, @TouchedBy = {touchedBy}", "Ada");

            // return ResultCount;

            throw new NotImplementedException("Need to convert this to an EF query");
        }

        public List<VsandGameReport> GetTeamReportHistory(int TeamId, DateTime? StartDate, DateTime? EndDate)
        {
            StartDate = StartDate ?? DateHelp.SqlMinDate;
            EndDate = EndDate ?? DateHelp.SqlMaxDate;

            List<VsandGameReport> oRet = null;
            if (TeamId > 0)
            {

                IQueryable<VsandGameReport> oData = null;
                oData = (from gr in _context.VsandGameReport
                          .Include(gr => gr.Sport)
                          .Include(gr => gr.ScheduleYear)
                          .Include(gr => gr.EventType)
                          .Include(gr => gr.Round)
                          .Include(gr => gr.Section)
                          .Include(gr => gr.Group)
                          .Include(gr => gr.Teams).ThenInclude(t => t.Team).ThenInclude(t => t.CustomCodes)
                          .Include(gr => gr.Teams).ThenInclude(t => t.Team).ThenInclude(t => t.School)
                          .Include(gr => gr.Pairings)
                         where gr.Teams.Any(t => t.Team.TeamId == TeamId)
                         select gr);

                if (StartDate > DateHelp.SqlMinDate)
                {
                    DateTime dStart = new DateTime(StartDate.Value.Year, StartDate.Value.Month, StartDate.Value.Day, 0, 0, 0);
                    oData = oData.Where(gr => gr.GameDate >= dStart);
                }
                if (EndDate < DateHelp.SqlMaxDate)
                {
                    DateTime dEnd = new DateTime(EndDate.Value.Year, EndDate.Value.Month, EndDate.Value.Day, 23, 59, 59);
                    oData = oData.Where(gr => gr.GameDate <= dEnd);
                }
                oData = oData.OrderByDescending(gr => gr.GameDate);
                oRet = oData.ToList();
            }
            return oRet;
        }
               

        public string UpdateRecord(int TeamId, bool bLowScoreWins, ICollection<VsandGameReportTeam> oGameTeams, bool bLeague, ref int iWins, ref int iLosses, ref int iTies, ref int iLeagueWins, ref int iLeagueLosses, ref int iLeagueTies)
        {
            return UpdateRecord(TeamId, bLowScoreWins, oGameTeams, bLeague, ref iWins, ref iLosses, ref iTies, ref iLeagueWins, ref iLeagueLosses, ref iLeagueTies, "");
        }

        public string UpdateRecord(int TeamId, bool bLowScoreWins, ICollection<VsandGameReportTeam> oGameTeams, bool bLeague, ref int iWins, ref int iLosses, ref int iTies, ref int iLeagueWins, ref int iLeagueLosses, ref int iLeagueTies, string sConf)
        {
            string sResult = "";

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
                    int GameReportId = oTeam.GameReportId;

                    // -- Check to see if this was match play, if so, high score wins

                    VsandGameReportMeta oMeta = (from m in _context.VsandGameReportMeta
                                                 where m.GameReport.GameReportId == GameReportId && m.SportGameMeta.ValueType == "VSAND.GolfPlayFormat"
                                                 select m).FirstOrDefault();

                    if (oMeta != null)
                    {
                        if (oMeta.MetaValue == "1")
                        {
                            bLowScoreWins = false;
                        }
                    }
                }

                if (bLowScoreWins)
                {
                    if (dTeamScore < dOppScore)
                    {
                        iWins = iWins + 1;
                        if (bLeague)
                        {
                            iLeagueWins = iLeagueWins + 1;
                        }
                    }
                    else if (dTeamScore > dOppScore)
                    {
                        iLosses = iLosses + 1;
                        if (bLeague)
                        {
                            iLeagueLosses = iLeagueLosses + 1;
                        }
                    }
                    else
                    {
                        iTies = iTies + 1;
                        if (bLeague)
                        {
                            iLeagueTies = iLeagueTies + 1;
                        }
                    }
                }
                else if (dTeamScore > dOppScore)
                {
                    iWins = iWins + 1;
                    if (bLeague)
                    {
                        iLeagueWins = iLeagueWins + 1;
                    }
                }
                else if (dTeamScore < dOppScore)
                {
                    iLosses = iLosses + 1;
                    if (bLeague)
                    {
                        iLeagueLosses = iLeagueLosses + 1;
                    }
                }
                else
                {
                    iTies = iTies + 1;
                    if (bLeague)
                    {
                        iLeagueTies = iLeagueTies + 1;
                    }
                }
            }

            sResult = FormatRecord(iWins, iLosses, iTies, iLeagueWins, iLeagueLosses, iLeagueTies, sConf);
            return sResult;
        }

        public string FormatRecord(int iWins, int iLosses, int iTies, int iLeagueWins, int iLeagueLosses, int iLeagueTies, string sConf)
        {
            // -- Overall Record
            string sRet = iWins + "-" + iLosses;
            if (iTies > 0)
            {
                sRet += "-" + iTies;
            }
            if (!string.IsNullOrEmpty(sConf))
            {
                // -- We'll show a league record for someone that has a conference assigned
                sRet += " (" + iLeagueWins + "-" + iLeagueLosses;
                if (iLeagueTies > 0)
                {
                    sRet += "-" + iLeagueTies;
                }
                sRet += ")";
            }

            return sRet;
        }

        // TODO: remove NVelocity and refactor formatting (do formatting in Javascript?)
        /*
        public string GetFormattedGameReport(int GameReportId, SortedList<string, object> dContext, string Formatter)
        {
            VsandGameReport oGameReport = GetFullGameReport(GameReportId);
            return GetFormattedGameReport(oGameReport, dContext, Formatter);
        }

        public string GetFormattedGameReport(VsandGameReport oGameReport, int FormatId)
        {
            string sFormatter = "";
            SortedList<string, object> dContext = new SortedList<string, object>();

            // -- Get the formatter
            VsandSystemFormat oFormat = VSAND.Helper.SystemFormat.GetFormat(FormatId);
            if (oFormat != null)
            {
                sFormatter = HttpContext.Current.Server.MapPath("/app_data/Formatters/GameReport/" + oFormat.FileName);
            }

            // -- Load the publication format variables into the list
            List<VsandSystemFormatVariable> oVars = VSAND.Helper.SystemFormat.GetFormatVariables();
            foreach (VsandSystemFormatVariable oVar in oVars)
            {
                string sName = oVar.VariableName;
                string sValue = oVar.VariableValue;
                string sType = oVar.ValueType;

                switch (sType.ToLower())
                {
                    case "asciicode":
                        {
                            try
                            {
                                int iChar = int.Parse(sValue);
                                dContext.Add(sName, Convert.ToChar(iChar));
                            }
                            catch (Exception ex)
                            {
                                Log.Error("Error converting ASCIICode format variable: " + sName + " / " + sValue, ex);
                            }

                            break;
                        }

                    case "string":
                        {
                            dContext.Add(sName, sValue);
                            break;
                        }
                }
            }

            return GetFormattedGameReport(oGameReport, dContext, sFormatter);
        }

        public string GetFormattedGameReport(VsandGameReport oGameReport, SortedList<string, object> dContext, string Formatter)
        {
            string sRet = "";

            NVelocity.App.VelocityEngine oEngine = new NVelocity.App.VelocityEngine();
            Commons.Collections.ExtendedProperties oProps = new Commons.Collections.ExtendedProperties();
            string sBasePath = Path.GetDirectoryName(Formatter);
            string sFormatterName = Path.GetFileName(Formatter);
            oProps.SetProperty("file.resource.loader.path", sBasePath);
            // oProps.AddProperty("directive.manager", "NVelocity.Runtime.Directive.DirectiveManager")
            // oProps.AddProperty("resource.manager.class", "NVelocity.Runtime.Resource.ResourceManagerImpl")
            // oProps.AddProperty("runtime.introspector.uberspect", "NVelocity.Util.Introspection.UberspectImpl")
            oEngine.Init(oProps);

            NVelocity.Template oTemplate = null;
            bool bLoaded = false;
            try
            {
                oTemplate = oEngine.GetTemplate(sFormatterName);
                bLoaded = true;
            }
            catch (Exception ex)
            {
                Log.Warn("Trying with template: " + sFormatterName);
                Log.Error(ex.Message, ex);
            }

            if (bLoaded)
            {
                NVelocity.VelocityContext oContext = new NVelocity.VelocityContext();
                // -- Put the additional context values that were provided
                if (dContext != null)
                {
                    IEnumerator<KeyValuePair<string, object>> oEnum = dContext.GetEnumerator();
                    while (oEnum.MoveNext())
                    {
                        oContext.Put(oEnum.Current.Key, oEnum.Current.Value);
                    }
                }
                // -- Put the global context values
                oContext.Put("Formatting", new Helper.Formatting());
                oContext.Put("ShortTimeFormatter", new VSAND.SmallTime());
                oContext.Put("LongTimeFormatter", new VSAND.LongTime());
                oContext.Put("SprintTimeFormatter", new VSAND.SprintTime());
                oContext.Put("TimeFormatter", new VSAND.Time());
                oContext.Put("HeightFormatter", new VSAND.Height());
                oContext.Put("SmallHeightFormatter", new VSAND.SmallHeight());
                oContext.Put("DistanceFormatter", new VSAND.Distance());
                oContext.Put("ShortDistanceFormatter", new VSAND.ShortDistance());
                oContext.Put("SmallDistanceFormatter", new VSAND.SmallDistance());
                oContext.Put("PlayByPlayHandler", new VSAND.ScoringPlay());
                oContext.Put("EventDistanceMeasurement", new VSAND.DistanceMeasure());
                oContext.Put("WrestlingWeightClass", new VSAND.WrestlingWeightClass());
                oContext.Put("IPFormatter", new VSAND.InningsPitched());
                // -- Put the game report object
                oContext.Put("GameReport", oGameReport);

                using (StringWriter oSw = new StringWriter())
                {
                    oTemplate.Merge(oContext, oSw);
                    sRet = oSw.GetStringBuilder().ToString();
                }
            }

            return sRet;
        }
        */

        public List<VsandTeam> GetDuplicateGameReports(DateTime ViewDate)
        {
            List<VsandTeam> oRet = null;

            IEnumerable<VsandTeam> oTeams = (from td in _context.TeamsWithDuplicateGameReports
                                             join t in _context.VsandTeam on td.TeamId equals t.TeamId
                                             where td.GameDate.Value.Year == ViewDate.Year && td.GameDate.Value.Month == ViewDate.Month && td.GameDate.Value.Day == ViewDate.Day
                                             orderby t.Name ascending
                                             select t);

            foreach (VsandTeam oTeam in oTeams)
            {
                int TeamId = oTeam.TeamId;
                int SportId = oTeam.SportId;
                VsandSport oSport = (from s in _context.VsandSport
                                     where s.SportId == SportId
                                     select s).FirstOrDefault();

                IEnumerable<VsandGameReport> oGames = (from gr in _context.VsandGameReport.Include(gr => gr.Teams)
                                                       where gr.GameDate.Year == ViewDate.Year
                                                       && gr.GameDate.Month == ViewDate.Month
                                                       && gr.GameDate.Day == ViewDate.Day
                                                       && gr.Teams.Any(grt => grt.Team.TeamId == TeamId)
                                                       orderby gr.GameReportId ascending
                                                       select gr);


                if (oGames != null)
                {
                    List<VsandGameReport> oGamesList = oGames.ToList();
                }
            }

            if (oTeams != null)
            {
                oRet = oTeams.ToList();
            }


            return oRet;
        }

        public VsandGameReport FindDuplicateGameReport(DateTime GameDate, List<ParticipatingTeam> Teams)
        {
            VsandGameReport oRet = null;

            if (Teams.Count < 10)
            {
                try
                {
                    IQueryable<VsandGameReport> oData = (from gr in _context.VsandGameReport
                                                            .Include(gr => gr.ReportedByUser)
                                                            .Include(gr => gr.Teams).ThenInclude(t => t.Team).ThenInclude(t => t.Sport)
                                                         where gr.GameDate.Year == GameDate.Year && gr.GameDate.Month == GameDate.Month && gr.GameDate.Day == GameDate.Day
                                                         select gr);

                    foreach (ParticipatingTeam oTeam in Teams)
                    {
                        int TeamId = oTeam.TeamId;
                        oData = oData.Where(gr => gr.Teams.Any(grt => grt.Team.TeamId == TeamId));
                    }
                    oRet = oData.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex.Message);
                }
            }

            return oRet;
        }

        public async Task<PagedResult<VsandGameReport>> GetLatestGamesPagedAsync(SearchRequest oSearch)
        {
            var oRet = new PagedResult<VsandGameReport>(null, 0, oSearch.PageSize, oSearch.PageNumber);

            var oQuery = from gr in _context.VsandGameReport select gr;
            if (oSearch.SchoolId.HasValue && oSearch.SchoolId.Value > 0)
            {
                oQuery = oQuery.Where(x => x.Teams.Any(t => t.Team.SchoolId == oSearch.SchoolId.Value));
            }
            if (oSearch.Sports != null && oSearch.Sports.Any())
            {
                oQuery = oQuery.Where(x => oSearch.Sports.Contains(x.SportId));
            }
            if (oSearch.Counties != null && oSearch.Counties.Any())
            {
                oQuery = oQuery.Where(x => oSearch.Counties.Contains(x.CountyId));
            }
            if (oSearch.Conferences != null && oSearch.Conferences.Any())
            {
                oQuery = oQuery.Where(x => x.Teams.Any(grt => oSearch.Conferences.Contains(grt.Team.CustomCodes.FirstOrDefault(cc => cc.CodeName == "Conference").CodeValue)));
            }
            if (oSearch.ScheduleYearId.HasValue && oSearch.ScheduleYearId.Value > 0)
            {
                oQuery = oQuery.Where(x => x.ScheduleYearId == oSearch.ScheduleYearId.Value);
            }
            if (oSearch.GameDate.HasValue && oSearch.GameDate.Value >= VSAND.Common.DateHelp.SqlMinDate && oSearch.GameDate.Value <= VSAND.Common.DateHelp.SqlMaxDate)
            {
                oQuery = oQuery.Where(x => x.GameDate.Year == oSearch.GameDate.Value.Year
                && x.GameDate.Month == oSearch.GameDate.Value.Month
                && x.GameDate.Day == oSearch.GameDate.Value.Day);
            }

            var totalResults = await oQuery.CountAsync();

            var iPg = oSearch.PageNumber - 1;
            if (iPg < 0) iPg = 0;
            var iSkip = iPg * oSearch.PageSize;

            var oResults = await (from x in oQuery
                                  .Include(x => x.Teams)
                                  .ThenInclude(g => g.Team)
                                  .ThenInclude(t => t.School)
                                  .ThenInclude(s => s.VsandPublicationSchool)
                                  .Include(x => x.Teams)
                                  .ThenInclude(g => g.Team)
                                  .ThenInclude(m => m.CustomCodes)
                                  .Include(x => x.Sport)
                                  orderby x.GameDate descending
                                  select x).Skip(iSkip).Take(oSearch.PageSize).ToListAsync();

            oRet.Results = oResults;
            oRet.TotalResults = totalResults;

            return oRet;
        }

        public async Task<List<VsandGameReport>> ScheduleScoreboard(DateTime? viewDate, int? sportId, int? schoolId, int? scheduleYearId)
        {
            if (!viewDate.HasValue)
            {
                viewDate = DateTime.Now;
            }
            DateTime startDate = new DateTime(viewDate.Value.Year, viewDate.Value.Month, viewDate.Value.Day);
            DateTime endDate = startDate.AddDays(2);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            int filterSportId = 0;
            if (sportId.HasValue)
            {
                filterSportId = sportId.Value;
            }
            int filterSchoolId = 0;
            if (schoolId.HasValue)
            {
                filterSchoolId = schoolId.Value;
            }
            int filterScheduleYearId = 0;
            if (scheduleYearId.HasValue)
            {
                filterScheduleYearId = scheduleYearId.Value;
            }

            bool teamView = filterSportId > 0 && filterSchoolId > 0 && filterScheduleYearId > 0;

            var oQuery = from gr in _context.VsandGameReport select gr;
            if (!teamView)
            {
                oQuery = oQuery.Where(gr => gr.GameDate >= startDate && gr.GameDate <= endDate);
            }
            if (sportId.HasValue && sportId.Value > 0)
            {
                oQuery = oQuery.Where(x => x.SportId == sportId.Value);
            }
            if (schoolId.HasValue && schoolId.Value > 0)
            {
                oQuery = oQuery.Where(x => x.Teams.Any(grt => grt.Team.SchoolId == schoolId.Value));
            }
            if (scheduleYearId.HasValue && scheduleYearId.Value > 0)
            {
                oQuery = oQuery.Where(x => x.ScheduleYearId == scheduleYearId.Value);
            }
            
            var oResults = await (from x in oQuery
                                  .Include(x => x.Teams)
                                  .ThenInclude(g => g.Team)
                                  .ThenInclude(t => t.School)
                                  .Include(x => x.Teams)
                                  .ThenInclude(g => g.Team)
                                  .ThenInclude(m => m.CustomCodes)
                                  .Include(x => x.Sport)
                                  orderby x.GameDate ascending
                                  select x).ToListAsync();

            return oResults;
        }

        public async Task<IEnumerable<VsandGameReport>> GetTeamGamesAsync(int teamId)
        {
            var oRet = await (from gr in _context.VsandGameReport
                              .Include(x => x.Teams).ThenInclude(grt => grt.Team).ThenInclude(t => t.School)
                              .Include(x => x.Sport)
                              .Include(x => x.EventType)
                              .Include(x => x.Round)
                              .Include(x => x.Section)
                              orderby gr.GameDate ascending
                              where gr.Teams.Any(t => t.TeamId == teamId)
                              select gr).ToListAsync();

            // get the list of team ids that are referenced in this list
            var teamIds = (from gr in oRet from grt in gr.Teams select grt.TeamId).Distinct().ToList();

            // Use entity fix-up to pull in the Conference and Division custom codes
            var codeNames = new List<string> { "Conference", "Division" };
            var oCodes = await (from tcc in _context.VsandTeamCustomCode
                                where codeNames.Contains(tcc.CodeName) && teamIds.Contains(tcc.TeamId) select tcc).ToListAsync();
            
            return oRet;
        }

        public async Task<IEnumerable<VsandTeam>> GetTeamRecordInfo(int teamId)
        {
            var oRet = await (from gr in _context.VsandTeam
                              .Include(gr => gr.CustomCodes)
                              .Include(gr => gr.GameReportEntries).ThenInclude(x => x.GameReport)
                              .Include(gr => gr.School)
                              .Include(gr => gr.Sport)
                              where gr.TeamId == teamId
                              select gr).ToListAsync();

            return oRet;
        }
    }
}
