using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.Repositories;
using VSAND.Data.ViewModels.StatAggregation;
using VSAND.Data.ViewModels.Teams;

namespace VSAND.Services.StatAgg
{
    public class TeamStatAggregation : ITeamStatAggregation
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _config;

        public TeamStatAggregation(IUnitOfWork uow, IConfiguration config)
        {
            _uow = uow;
            _config = config;
        }

        public async Task<TeamLeaderboardResult> TeamLeaderBoard(int sportId, int scheduleYearId, int statCategoryId, int gamesPlayed, int orderBy, string orderDir, int pageNumber, int pageSize)
        {
            var oResult = new TeamLeaderboardResult();

            if (sportId <= 0)
            {
                return oResult;
            }

            var sport = await _uow.Sports.Single(s => s.SportId == sportId);
            oResult.Sport = sport;

            VsandScheduleYear scheduleYear = null;
            if (scheduleYearId <= 0)
            {
                scheduleYear = await _uow.ScheduleYears.GetScheduleYearAsync();
                scheduleYearId = scheduleYear.ScheduleYearId;
            }
            else
            {
                scheduleYear = await _uow.ScheduleYears.GetById(scheduleYearId);
            }
            oResult.ScheduleYear = scheduleYear;

            // get the sport slug
            var sportSlug = await _uow.EntitySlugs.Single(es => es.EntityType == "Sport" && es.EntityId == sportId);

            // get the schedule year slug
            var scheduleYearSlug = await _uow.EntitySlugs.Single(es => es.EntityType == "ScheduleYear" && es.EntityId == scheduleYearId);

            if (sportSlug == null || scheduleYearSlug == null)
            {
                return null;
            }

            oResult.Categories = await _uow.SportTeamStatCategories.List(spsc => spsc.SportId == sportId && spsc.TeamStats.Any(ps => ps.Enabled), spsc => spsc.OrderBy(c => c.SortOrder));
            if (!oResult.Categories.Any())
            {
                return oResult;
            }

            VsandSportTeamStatCategory category = null;
            if (statCategoryId <= 0)
            {
                // get the first stat category for the sport
                category = oResult.Categories.FirstOrDefault();
                statCategoryId = category.SportTeamStatCategoryId;
            }
            else
            {
                category = oResult.Categories.FirstOrDefault(c => c.SportTeamStatCategoryId == statCategoryId);
            }
            oResult.Category = category;

            oResult.Stats = await _uow.SportTeamStats.List(sps => sps.SportTeamStatCategoryId == statCategoryId, sps => sps.OrderBy(s => s.SortOrder));
            if (!oResult.Stats.Any())
            {
                return oResult;
            }

            if (orderBy == 0)
            {
                // take the first stat from the stats we got back
                orderBy = oResult.Stats.FirstOrDefault().SportTeamStatId;
            }

            List<int> statIds = oResult.Stats.Select(s => s.SportTeamStatId).ToList();

            if (string.IsNullOrEmpty(orderDir))
            {
                orderDir = "DESC";
            }

            using (SqlConnection oConn = new SqlConnection(_config["StatsConnection"]))
            {
                oConn.Open();

                using (SqlCommand oCmd = new SqlCommand("vsand_LeaderboardTeam", oConn))
                {
                    oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    oCmd.Parameters.AddWithValue("@ScheduleYearId", scheduleYearId);
                    oCmd.Parameters.AddWithValue("@StatCategory", statCategoryId);
                    oCmd.Parameters.AddWithValue("@OrderBy", orderBy);
                    oCmd.Parameters.AddWithValue("@OrderDir", orderDir);
                    oCmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    oCmd.Parameters.AddWithValue("@PageSize", pageSize);
                    oCmd.Parameters.AddWithValue("@MinGamesPlayed", gamesPlayed);
                    var oResultCountParam = new SqlParameter("@RecordCount", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Output, false, 0, 0, "", System.Data.DataRowVersion.Default, 0);
                    oCmd.Parameters.Add(oResultCountParam);

                    using (var oRdr = oCmd.ExecuteReader())
                    {
                        var columns = new List<string>();
                        for (int i = 0; i < oRdr.FieldCount; i++)
                        {
                            columns.Add(oRdr.GetName(i));
                        }

                        while (oRdr.Read())
                        {
                            // Each reader entry is a new player
                            var oStatTeam = new AggStatTeam();
                            oStatTeam.TeamId = (int)oRdr["TeamId"];
                            oStatTeam.Name = oRdr["Name"].ToString();
                            oStatTeam.SchoolId = (int)oRdr["SchoolId"];
                            oStatTeam.SportId = (int)oRdr["SportId"];
                            oStatTeam.ScheduleYearId = (int)oRdr["ScheduleYearId"];
                            oStatTeam.City = oRdr["City"].ToString();
                            oStatTeam.State = oRdr["State"].ToString();
                            oStatTeam.GamesPlayed = (int)oRdr["GamesPlayed"];
                            int teamWins = 0;
                            int teamLosses = 0;
                            int teamTies = 0;
                            int.TryParse(oRdr["Wins"].ToString(), out teamWins);
                            int.TryParse(oRdr["Losses"].ToString(), out teamLosses);
                            int.TryParse(oRdr["Ties"].ToString(), out teamTies);
                            oStatTeam.Record = new TeamRecord(teamWins, teamLosses, teamTies);
                            oStatTeam.Rank = (int)oRdr["Rank"];
                            oStatTeam.SportSlug = sportSlug.Slug;
                            oStatTeam.ScheduleYearSlug = scheduleYearSlug.Slug;
                            // fill in the stat values 
                            foreach (int statId in statIds)
                            {
                                if (columns.Contains(statId.ToString()))
                                {
                                    oStatTeam.StatValues.Add(statId, (double)oRdr[statId.ToString()]);
                                }
                            }
                            oResult.Teams.Add(oStatTeam);
                        }
                    }

                    oResult.TotalResults = (int)oCmd.Parameters["@RecordCount"].Value;
                }
            }

            oResult.OrderById = orderBy;
            oResult.OrderDir = orderDir;
            oResult.PageSize = pageSize;
            if (pageNumber < 1) pageNumber = 1;
            oResult.CurrentPage = pageNumber;
            double totalPages = oResult.TotalResults / oResult.PageSize;
            oResult.TotalPages = (int)Math.Ceiling(totalPages);

            // we need to apply the school slugs that appear on this page so that we can apply their proper data
            var schoolIds = oResult.Teams.Select(t => t.SchoolId);
            var schoolSlugs = await _uow.EntitySlugs.List(es => es.EntityType == "School" && schoolIds.Contains(es.EntityId));
            foreach (var schoolSlug in schoolSlugs)
            {
                var rTeams = oResult.Teams.Where(t => t.SchoolId == schoolSlug.EntityId);
                foreach(var rTeam in rTeams)
                {
                    rTeam.SchoolSlug = schoolSlug.Slug;
                }
            }

            return oResult;
        }
    }
}
