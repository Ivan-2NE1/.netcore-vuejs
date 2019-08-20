using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;

namespace VSAND.Data.Repositories
{
    public class PlayerRepository : Repository<VsandPlayer>, IPlayerRepository
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly VsandContext _context;
        public PlayerRepository(VsandContext context) : base(context)
        {
            _context = context ?? throw new ArgumentException("Context is null");
        }

        public async Task<PagedResult<PlayerSummary>> Search(string firstName, string lastName, int graduationYear, int schoolId, int pageSize, int pageNumber)
        {
            var oQuery = from p in _context.VsandPlayer select p;
            if (!string.IsNullOrEmpty(firstName))
            {
                oQuery = oQuery.Where(p => p.FirstName.Contains(firstName));
            }
            if (!string.IsNullOrEmpty(lastName))
            {
                oQuery = oQuery.Where(p => p.LastName.Contains(lastName));
            }
            if (graduationYear > 0)
            {
                oQuery = oQuery.Where(p => p.GraduationYear == graduationYear);
            }
            if (schoolId > 0)
            {
                oQuery = oQuery.Where(p => p.SchoolId.HasValue && p.SchoolId.Value == schoolId);
            }

            var iPg = pageNumber - 1;
            if (iPg < 0)
            {
                iPg = 0;
            }

            var iSkip = iPg * pageSize;

            var oRet = new PagedResult<PlayerSummary>(null, 0, pageSize, pageNumber)
            {
                TotalResults = await oQuery.CountAsync(),
                Results = await oQuery.Include(p => p.School)
                                .OrderBy(p => p.LastName).ThenBy(p => p.FirstName).ThenByDescending(p => p.GraduationYear)
                                .Select(p => new PlayerSummary
                                {
                                    PlayerId = p.PlayerId,
                                    FirstName = p.FirstName,
                                    LastName = p.LastName,
                                    GraduationYear = p.GraduationYear,
                                    SchoolId = p.SchoolId,
                                    SchoolName = (p.School != null) ? p.School.Name : ""
                                })
                                .Skip(iSkip).Take(pageSize).ToListAsync()
            };
            return oRet;
        }

        public async Task<VsandPlayer> GetPlayerStatsAsync(int playerId, int teamId)
        {
            var player = await _context.VsandPlayer.FirstOrDefaultAsync(p => p.PlayerId == playerId);

            // Use entity fix-up to get the rest of this shit
            await _context.VsandTeamRoster.FirstOrDefaultAsync(r => r.TeamId == teamId && r.PlayerId == playerId);
            await _context.VsandTeam.Include(t => t.Sport).ThenInclude(s => s.PlayerStatCategories).ThenInclude(c => c.PlayerStats).FirstOrDefaultAsync(t => t.TeamId == teamId);
            await _context.VsandTeam.Include(t => t.Sport).ThenInclude(s => s.SportEvents).ThenInclude(s => s.EventStats).FirstOrDefaultAsync(t => t.TeamId == teamId);
            await _context.VsandGameReport.Include(gr => gr.Teams).Where(gr => gr.Teams.Any(grr => grr.Team.TeamId == teamId && grr.GameRoster.Any(r => r.PlayerId == playerId))).ToListAsync();
            await _context.VsandGameReportEvent.Where(gre => gre.Results.Any(r => r.EventPlayers.Any(ep => ep.PlayerId == playerId)) && gre.GameReport.Teams.Any(grt => grt.TeamId == teamId)).ToListAsync();
            await _context.VsandGameReportEvent.Include(gre => gre.Results).ThenInclude(r => r.EventPlayerGroups).ThenInclude(epg => epg.EventPlayerGroupPlayers).ThenInclude(epgp => epgp.Player).Where(gre => gre.Results.Any(r => r.EventPlayerGroups.Any(epg => epg.EventPlayerGroupPlayers.Any(pgp => pgp.PlayerId == playerId))) && gre.GameReport.Teams.Any(grt => grt.TeamId == teamId)).ToListAsync();
            await _context.VsandGameReportPlayerStat.Where(grps => grps.PlayerId == playerId && grps.GameReport.Teams.Any(grt => grt.TeamId == teamId)).ToListAsync();
            await _context.VsandGameReportEventPlayerStat.Include(greps => greps.EventPlayer).Where(greps => greps.EventPlayer.PlayerId == playerId).ToListAsync();
            await _context.VsandGameReportEventPlayerGroupStat.Include(grepgs => grepgs.EventPlayerGroup).Where(grepgs => grepgs.EventPlayerGroup.EventPlayerGroupPlayers.Any(epgp => epgp.PlayerId == playerId)).ToListAsync();

            var tr = player.TeamRosters.FirstOrDefault();
            tr.Team.Sport.PlayerStatCategories = tr.Team.Sport.PlayerStatCategories.OrderBy(sc => sc.SortOrder).ToList();
            foreach(VsandSportPlayerStatCategory cat in tr.Team.Sport.PlayerStatCategories)
            {
                cat.PlayerStats = cat.PlayerStats.OrderBy(s => s.SortOrder).ToList();
            }

            tr.Team.Sport.SportEvents = tr.Team.Sport.SportEvents.OrderBy(e => e.DefaultSort).ToList();
            foreach(VsandSportEvent e in tr.Team.Sport.SportEvents)
            {
                e.EventStats = e.EventStats.OrderBy(s => s.SortOrder).ToList();
            }

            tr.Team.GameReportEntries = tr.Team.GameReportEntries.OrderBy(grt => grt.GameReport.GameDate).ToList();

            return player;
        }
    }
}
