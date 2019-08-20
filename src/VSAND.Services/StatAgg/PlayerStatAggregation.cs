using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Data.Repositories;
using VSAND.Data.ViewModels.StatAggregation;
using VSAND.Data.Entities;

namespace VSAND.Services.StatAgg
{
    public class PlayerStatAggregation : IPlayerStatAggregation
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _config;

        public PlayerStatAggregation(IUnitOfWork uow, IConfiguration config)
        {
            _uow = uow;
            _config = config;
        }

        public async Task<IndividualLeaderboardResult> IndividualLeaderBoard(int sportId, int scheduleYearId, int statCategoryId, int gamesPlayed, int orderBy, string orderDir, int pageNumber, int pageSize)
        {
            var oResult = new IndividualLeaderboardResult();

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

            oResult.Categories = await _uow.SportPlayerStatCategories.List(spsc => spsc.SportId == sportId && spsc.PlayerStats.Any(ps => ps.Enabled), spsc => spsc.OrderBy(c => c.SortOrder));
            if (!oResult.Categories.Any())
            {
                return oResult;
            }

            VsandSportPlayerStatCategory category = null;
            if (statCategoryId <= 0)
            {
                // get the first stat category for the sport
                category = oResult.Categories.FirstOrDefault();
                statCategoryId = category.SportPlayerStatCategoryId;
            }
            else
            {
                category = oResult.Categories.FirstOrDefault(c => c.SportPlayerStatCategoryId == statCategoryId);
            }   
            oResult.Category = category;

            oResult.Stats = await _uow.SportPlayerStats.List(sps => sps.SportPlayerStatCategoryId == statCategoryId, sps => sps.OrderBy(s => s.SortOrder));
            if (!oResult.Stats.Any())
            {
                return oResult;
            }

            if (orderBy == 0)
            {
                // take the first stat from the stats we got back
                orderBy = oResult.Stats.FirstOrDefault().SportPlayerStatId;
            }

            List<int> statIds = oResult.Stats.Select(s => s.SportPlayerStatId).ToList();

            if (string.IsNullOrEmpty(orderDir))
            {
                orderDir = "DESC";
            }

            using (SqlConnection oConn = new SqlConnection(_config["StatsConnection"]))
            {
                oConn.Open();

                using (SqlCommand oCmd = new SqlCommand("vsand_LeaderboardIndividual", oConn))
                {
                    oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    oCmd.Parameters.AddWithValue("@ScheduleYearId", scheduleYearId);
                    oCmd.Parameters.AddWithValue("@StatCategory", statCategoryId);
                    oCmd.Parameters.AddWithValue("@OrderBy", orderBy);
                    oCmd.Parameters.AddWithValue("@OrderDir", orderDir);
                    oCmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    oCmd.Parameters.AddWithValue("@PageSize", pageSize);
                    oCmd.Parameters.AddWithValue("@GamesPlayed", gamesPlayed);
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
                            var oStatPlayer = new AggStatPlayer();
                            oStatPlayer.PlayerId = (int)oRdr["PlayerId"];
                            oStatPlayer.Name = oRdr["FirstName"] + " " + oRdr["LastName"];
                            oStatPlayer.JerseyNumber = oRdr["JerseyNumber"].ToString();

                            //TODO: Add Graduation Year
                            string gradYr = oRdr["GraduationYear"].ToString();
                            int gradYear = 0;
                            int.TryParse(gradYr, out gradYear);
                            if (gradYear > 0)
                            {
                                oStatPlayer.GraduationYear = gradYear;
                            }

                            oStatPlayer.TeamId = (int)oRdr["TeamId"];
                            oStatPlayer.TeamName = oRdr["Name"].ToString();
                            oStatPlayer.GamesPlayed = (double)oRdr["GamesPlayed"];
                            oStatPlayer.Rank = (int)oRdr["Rank"];
                            // fill in the stat values 
                            foreach (int statId in statIds)
                            {
                                if (columns.Contains(statId.ToString()))
                                {
                                    oStatPlayer.StatValues.Add(statId, (double)oRdr[statId.ToString()]);
                                }
                            }
                            oResult.Players.Add(oStatPlayer);
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

            // we need to load up the players that appear on this page so that we can apply their proper data
            var playerIds = oResult.Players.Select(p => p.PlayerId);
            var playerSlugs = await _uow.EntitySlugs.List(es => es.EntityType == "Player" && playerIds.Contains(es.EntityId));
            foreach (var playerSlug in playerSlugs)
            {
                var rPlayer = oResult.Players.FirstOrDefault(p => p.PlayerId == playerSlug.EntityId);
                if (rPlayer != null)
                {
                    rPlayer.Slug = playerSlug.Slug;
                }
            }

            return oResult;
        }

        public async Task<IndividualLeaderboardResult> IndividualLeaderBoardForStat(int sportId, int scheduleYearId, int orderBy, string orderDir, int pageNumber, int pageSize)
        {
            var oResult = new IndividualLeaderboardResult();

            if (sportId <= 0)
            {
                return oResult;
            }

            if (scheduleYearId <= 0)
            {
                return oResult;
            }
            if (orderBy <= 0)
            {
                return oResult;
            }

            oResult.Stats = await _uow.SportPlayerStats.List(sps => sps.SportPlayerStatId == orderBy);
            if (!oResult.Stats.Any())
            {
                return oResult;
            }

            List<int> statIds = oResult.Stats.Select(s => s.SportPlayerStatId).ToList();

            if (string.IsNullOrEmpty(orderDir))
            {
                orderDir = "DESC";
            }

            using (SqlConnection oConn = new SqlConnection(_config["StatsConnection"]))
            {
                oConn.Open();

                using (SqlCommand oCmd = new SqlCommand("vsand_LeaderboardIndividualStat", oConn))
                {
                    oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    oCmd.Parameters.AddWithValue("@ScheduleYearId", scheduleYearId);
                    oCmd.Parameters.AddWithValue("@OrderBy", orderBy);
                    oCmd.Parameters.AddWithValue("@OrderDir", orderDir);
                    oCmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    oCmd.Parameters.AddWithValue("@PageSize", pageSize);
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
                            var oStatPlayer = new AggStatPlayer();
                            oStatPlayer.PlayerId = (int)oRdr["PlayerId"];
                            oStatPlayer.Name = oRdr["FirstName"] + " " + oRdr["LastName"];
                            oStatPlayer.JerseyNumber = oRdr["JerseyNumber"].ToString();

                            //TODO: Add Graduation Year
                            string gradYr = oRdr["GraduationYear"].ToString();
                            int gradYear = 0;
                            int.TryParse(gradYr, out gradYear);
                            if (gradYear > 0)
                            {
                                oStatPlayer.GraduationYear = gradYear;
                            }

                            oStatPlayer.TeamId = (int)oRdr["TeamId"];
                            oStatPlayer.TeamName = oRdr["Name"].ToString();
                            oStatPlayer.GamesPlayed = (double)oRdr["GamesPlayed"];
                            oStatPlayer.Rank = (int)oRdr["Rank"];
                            // fill in the stat values 
                            foreach (int statId in statIds)
                            {
                                if (columns.Contains(statId.ToString()))
                                {
                                    oStatPlayer.StatValues.Add(statId, (double)oRdr[statId.ToString()]);
                                }
                            }
                            oResult.Players.Add(oStatPlayer);
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

            // we need to load up the players that appear on this page so that we can apply their proper data
            var playerIds = oResult.Players.Select(p => p.PlayerId);
            var playerSlugs = await _uow.EntitySlugs.List(es => es.EntityType == "Player" && playerIds.Contains(es.EntityId));
            foreach (var playerSlug in playerSlugs)
            {
                var rPlayer = oResult.Players.FirstOrDefault(p => p.PlayerId == playerSlug.EntityId);
                if (rPlayer != null)
                {
                    rPlayer.Slug = playerSlug.Slug;
                }
            }

            return oResult;
        }

        public async Task<List<AggregatedStatItem>> TeamRosterEntryTop100(int playerId, int teamId, int sportId)
        {
            var oRet = new List<AggregatedStatItem>();

            // get the list of player stats for the sport related to the team
            var sportStats = await _uow.SportPlayerStats.List(sps => sps.SportId == sportId, 
                sps => sps.OrderBy(s => s.SportPlayerStatCategory).ThenBy(s => s.SortOrder),
                new List<string> { "SportPlayerStatCategory" });

            using (SqlConnection oConn = new SqlConnection(_config["StatsConnection"]))
            {
                oConn.Open();

                using (SqlCommand oCmd = new SqlCommand("vsand_LeaderboardIndividualForTeamRosterEntry", oConn))
                {
                    oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    oCmd.Parameters.AddWithValue("@playerId", playerId);
                    oCmd.Parameters.AddWithValue("@teamId", teamId);

                    using (var oRdr = oCmd.ExecuteReader())
                    {
                        var columns = new List<string>();
                        for (int i = 0; i < oRdr.FieldCount; i++)
                        {
                            columns.Add(oRdr.GetName(i));
                        }

                        while (oRdr.Read())
                        {
                            // There should only be one row, exit after the first item

                            foreach(var sportStat in sportStats)
                            {
                                var statId = sportStat.SportPlayerStatId;
                                var statValColName = statId.ToString();
                                var statRankColName = $"{statValColName}Rank";
                                
                                if (columns.Contains(statValColName) && columns.Contains(statRankColName))
                                {
                                    int statValColIdx = oRdr.GetOrdinal(statValColName);
                                    int statRankColIdx = oRdr.GetOrdinal(statRankColName);

                                    if (!oRdr.IsDBNull(statValColIdx) && !oRdr.IsDBNull(statRankColIdx))
                                    {
                                        var statRank = (int)oRdr[statRankColName];
                                        if (statRank <= 100)
                                        {
                                            var statVal = (double)oRdr[statValColIdx];

                                            if(statVal > 0)
                                            {
                                                // add an entry for this stat
                                                var oStatItem = new AggregatedStatItem()
                                                {
                                                    StatId = statId,
                                                    StatName = sportStat.DisplayName,
                                                    CategoryName = sportStat.SportPlayerStatCategory.Name,
                                                    StatValue = statVal,
                                                    Rank = statRank
                                                };
                                                oRet.Add(oStatItem);
                                            }                                            
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return oRet;
        }

        public async Task<List<AggregatedStatItem>> TeamRosterentrySpecificStatValues(int playerId, int teamId, List<int> statIds)
        {
            var oRet = new List<AggregatedStatItem>();

            // get the list of player stats for the sport related to the team
            var sportStats = await _uow.SportPlayerStats.List(sps => statIds.Contains(sps.SportPlayerStatId),
                sps => sps.OrderBy(s => s.SportPlayerStatCategory).ThenBy(s => s.SortOrder),
                new List<string> { "SportPlayerStatCategory" });

            using (SqlConnection oConn = new SqlConnection(_config["StatsConnection"]))
            {
                oConn.Open();

                using (SqlCommand oCmd = new SqlCommand("vsand_LeaderboardIndividualForTeamRosterEntry", oConn))
                {
                    oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    oCmd.Parameters.AddWithValue("@playerId", playerId);
                    oCmd.Parameters.AddWithValue("@teamId", teamId);

                    using (var oRdr = oCmd.ExecuteReader())
                    {
                        var columns = new List<string>();
                        for (int i = 0; i < oRdr.FieldCount; i++)
                        {
                            columns.Add(oRdr.GetName(i));
                        }

                        while (oRdr.Read())
                        {
                            // There should only be one row, exit after the first item

                            foreach (var sportStat in sportStats)
                            {
                                var statId = sportStat.SportPlayerStatId;
                                var statValColName = statId.ToString();
                                var statRankColName = $"{statValColName}Rank";

                                if (columns.Contains(statValColName) && columns.Contains(statRankColName))
                                {
                                    int statValColIdx = oRdr.GetOrdinal(statValColName);
                                    int statRankColIdx = oRdr.GetOrdinal(statRankColName);

                                    if (!oRdr.IsDBNull(statValColIdx) && !oRdr.IsDBNull(statRankColIdx))
                                    {
                                        var statRank = (int)oRdr[statRankColName];
                                        var statVal = (double)oRdr[statValColIdx];

                                        // add an entry for this stat
                                        var oStatItem = new AggregatedStatItem()
                                        {
                                            StatId = statId,
                                            StatName = sportStat.DisplayName,
                                            CategoryName = sportStat.SportPlayerStatCategory.Name,
                                            StatValue = statVal,
                                            Rank = statRank
                                        };
                                        oRet.Add(oStatItem);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return oRet;
        }
    }
}
