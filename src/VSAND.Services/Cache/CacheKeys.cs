namespace VSAND.Services.Cache
{
    public static class Keys
    {
        #region CMS
        public static string CMSConfig(string configCat, string configName)
        {
            return $"CMSConfig:{configCat}:{configName}";
        }

        public static string SlugRoute(string path)
        {
            return $"CMSSlugRoute:{path}";
        }

        public static string FrontEndFeaturedSports(string season)
        {
            return $"FrontEndFeatureSports:{season}";
        }

        public static string FrontEndSportsBySeason()
        {
            return "FrontEndSportsBySeason";
        }

        public static string FrontEndSportsByGender()
        {
            return "FrontEndSportsByGender";
        }

        public static string FrontEndSportStandingView(int sportId, int scheduleYearId, string conference)
        {
            return $"FrontEndStandingView:{sportId}:{scheduleYearId}:{conference}";
        }

        public static string FrontEndPowerpointsView(int sportId, int scheduleYearId, string section, string group)
        {
            return $"FrontEndPowerpointsView:{sportId}:{scheduleYearId}:{section}:{group}";
        }

        #endregion

        #region Sports
        public static string FullSport(int sportId)
        {
            return $"FullSport:{sportId}";
        }

        public static string ActiveSportList()
        {
            return "ActiveSportList";
        }

        #endregion

        #region ScheduleYears
        public static string ActiveScheduleYear()
        {
            return "ScheduleYears:Active";
        }
        #endregion

        #region Teams
        public static string FullTeam(int schoolId, int sportId, int scheduleYearId)
        {
            return $"Team:Full:{schoolId}:{sportId}:{scheduleYearId}";
        }
        #endregion

        #region Game Reports
        public static string FullGameReport(int gameReportId)
        {
            return $"GameReport:Full:{gameReportId}";
        }
        #endregion

        #region Players
        public static string FullPlayer(int playerId)
        {
            return $"Player:Full:{playerId}";
        }

        public static string FullPlayerView(int playerId, int sportId, int scheduleYearId)
        {
            return $"PlayerView:Full:{playerId}:{sportId}:{scheduleYearId}";
        }
        #endregion

        #region Schools
        public static string FrontEndDisplaySchoolList()
        {
            return "FrontEndDisplaySchoolList";
        }
        #endregion

        #region FrontEnd Display
        public static string FrontEndDisplaySportStatsHomeView(int sportId)
        {
            return $"FrontEnd:Display:SportStatsHomeView:{sportId}";
        }
        #endregion

    }
}
