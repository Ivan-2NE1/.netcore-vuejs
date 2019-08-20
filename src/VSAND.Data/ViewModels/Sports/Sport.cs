using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.Sports
{
    public class Sport
    {
        public int SportId { get; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(5)]
        public string Abbreviation { get; set; }
        [MaxLength(20)]
        public string Season { get; set; }
        public bool Enabled { get; set; }
        [MaxLength(20)]
        [DisplayName("Meet Name")]
        public string MeetName { get; set; }
        [MaxLength(20)]
        [DisplayName("Meet Type")]
        public string MeetType { get; set; }
        [MaxLength(20)]
        [DisplayName("Period Name")]
        public string PeriodName { get; set; }
        [MaxLength(25)]
        [DisplayName("Period Name Plural")]
        public string PeriodNamePlural { get; set; }
        public bool AllowOvertime { get; set; }
        public bool AllowTie { get; set; }
        public bool AllowMultipleEventsPerDay { get; set; }
        public bool EnableTriPlus { get; set; }
        public bool EnableJerseyNumber { get; set; }
        public bool EnablePosition { get; set; }
        [MaxLength(50)]
        [DisplayName("Overtime Name")]
        public string OvertimeName { get; set; }
        [MaxLength(50)]
        [DisplayName("Overtime Name Plural")]
        public string OvertimeNamePlural { get; set; }
        public int DefaultPeriods { get; set; }
        public bool EnablePeriodScoring { get; set; }
        public string PlayerName { get; set; }
        public string PlayerNamePlural { get; set; }
        public bool EnableLowScoreWins { get; set; }
        public bool EnableScoringPlayByPlay { get; set; }
        public string ScoringPlayByPlayType { get; set; }
        public bool CountZeroValuStats { get; set; }
        public bool EnablePowerPoints { get; set; }
        public string PowerPointsDataType { get; }
        public string PowerPointsLabel { get; set; }
        public bool EnableDifferential { get; set; }
        public string DifferentialDataType { get; set; }
        public string DifferentialLabel { get; set; }
        public bool EnableGamePosition { get; set; }
        public bool EnableStarter { get; set; }
        public bool EnableGameRosterOrder { get; set; }
        public string GameRosterOrderLabel { get; set; }
        public bool EnablePlayerOfRecord { get; set; }
        public int PlayerOfRecordPosition { get; set; }
        public int GameRosterOrderStatCategory { get; set; }
        public int PlayerOfRecordStatCategory { get; set; }
        public bool EnableShootout { get; set; }
        public string PlayerOfRecordLabel { get; set; }
        public string Slug { get; set; }
        public List<GameMeta> GameMetas { get; set; }
        public List<Event> Events { get; set; }
        public List<EventResultType> EventResultTypes { get; set; }
        public List<TeamStatCategory> TeamStatCategories { get; set; }
        public List<PlayerStatCategory> PlayerStatCategories { get; set; }
        public List<Position> Positions { get; set; }

        public Sport(VsandSport oSport)
        {
            this.SportId = oSport.SportId;
            this.Name = oSport.Name;
            this.Abbreviation = oSport.Abbreviation;
            this.Season = oSport.Season;
            this.Enabled = oSport.Enabled;
            this.MeetName = oSport.MeetName;
            this.MeetType = oSport.MeetType;
            this.PeriodName = oSport.PeriodName;
            this.PeriodNamePlural = oSport.PeriodNamePlural;
            this.AllowOvertime = oSport.AllowOt;
            this.AllowMultipleEventsPerDay = oSport.AllowMultiEventPerDay;
            this.AllowTie = oSport.AllowTie;
            this.EnableTriPlus = oSport.EnableTriPlusScheduling;
            this.EnableJerseyNumber = oSport.EnableJerseyNumber;
            this.EnablePosition = oSport.EnablePosition;
            this.OvertimeName = oSport.OTName;
            this.OvertimeNamePlural = oSport.OTNamePlural;
            this.DefaultPeriods = oSport.DefaultPeriods ?? 0;
            this.EnablePeriodScoring = oSport.EnablePeriodScoring;
            this.PlayerName = oSport.PlayerName;
            this.PlayerNamePlural = oSport.PlayerNamePlural;
            this.EnableLowScoreWins = oSport.EnableLowScoreWins ?? false;
            this.EnableScoringPlayByPlay = oSport.EnableScoringPlayByPlay ?? false;
            this.ScoringPlayByPlayType = oSport.ScoringPlayByPlayHandler;
            this.CountZeroValuStats = oSport.CountZeroValueStats;
            this.EnablePowerPoints = oSport.EnablePowerPoints ?? false;
            this.PowerPointsDataType = oSport.PowerPointsDataType;
            this.PowerPointsLabel = oSport.PowerPointsLabel;
            this.EnableDifferential = oSport.EnableDifferential ?? false;
            this.DifferentialDataType = oSport.DifferentialDataType;
            this.DifferentialLabel = oSport.DifferentialLabel;
            this.EnableGamePosition = oSport.EnableGamePosition;
            this.EnableStarter = oSport.EnableStarter;
            this.EnableGameRosterOrder = oSport.EnableGameRosterOrder;
            this.GameRosterOrderLabel = oSport.GameRosterOrderLabel;
            this.EnablePlayerOfRecord = oSport.EnablePlayerOfRecord;
            this.PlayerOfRecordPosition = oSport.PlayerOfRecordPosition ?? 0;
            this.GameRosterOrderStatCategory = oSport.GameRosterOrderStatCategory ?? 0;
            this.PlayerOfRecordStatCategory = oSport.PlayerOfRecordStatCategory ?? 0;
            this.EnableShootout = oSport.EnableShootOut;
            this.PlayerOfRecordLabel = oSport.PlayerOfRecordLabel;

            this.Positions = (from p in oSport.Positions select new Position(p)).ToList();
            this.GameMetas = (from m in oSport.GameMeta select new GameMeta(m)).ToList();            
            this.EventResultTypes = (from t in oSport.EventResults select new EventResultType(t)).ToList();
            this.TeamStatCategories = (from c in oSport.TeamStatCategories where c.TeamStats.Any(ts => ts.Enabled) orderby c.SortOrder ascending select new TeamStatCategory(c)).ToList();
            this.PlayerStatCategories = (from c in oSport.PlayerStatCategories where c.PlayerStats.Any(s => s.Enabled) orderby c.SortOrder ascending select new PlayerStatCategory(c)).ToList();
            this.Events = (from e in oSport.SportEvents select new Event(e)).ToList();
        }

        public VsandSport ToEntity()
        {
            var oRet = new VsandSport()
            {
                SportId = this.SportId,
                Name = this.Name,
                Abbreviation = this.Abbreviation,
                Season = this.Season,
                MeetName = this.MeetName,
                MeetType = this.MeetType,
                PeriodName = this.PeriodName,
                PeriodNamePlural = this.PeriodNamePlural,
                AllowOt = this.AllowOvertime,
                AllowTie = this.AllowTie,
                AllowMultiEventPerDay = this.AllowMultipleEventsPerDay,
                EnableTriPlusScheduling = this.EnableTriPlus,
                EnableJerseyNumber = this.EnableJerseyNumber,
                EnablePosition = this.EnablePosition,
                OTName = this.OvertimeName,
                OTNamePlural = this.OvertimeNamePlural,
                DefaultPeriods = this.DefaultPeriods,
                EnablePeriodScoring = this.EnablePeriodScoring,
                PlayerName = this.PlayerName,
                PlayerNamePlural = this.PlayerNamePlural,
                EnableLowScoreWins = this.EnableLowScoreWins,
                EnableScoringPlayByPlay = this.EnableScoringPlayByPlay,
                ScoringPlayByPlayHandler = this.ScoringPlayByPlayType,
                CountZeroValueStats = this.CountZeroValuStats,
                EnablePowerPoints = this.EnablePowerPoints,
                PowerPointsDataType = this.PowerPointsDataType,
                PowerPointsLabel = this.PowerPointsLabel,
                EnableDifferential = this.EnableDifferential,
                DifferentialDataType = this.DifferentialDataType,
                DifferentialLabel = this.DifferentialLabel,
                Enabled = this.Enabled,
                EnableGamePosition = this.EnableGamePosition,
                EnableStarter = this.EnableStarter,
                EnableGameRosterOrder = this.EnableGameRosterOrder,
                GameRosterOrderLabel = this.GameRosterOrderLabel,
                EnablePlayerOfRecord = this.EnablePlayerOfRecord,
                PlayerOfRecordPosition = this.PlayerOfRecordPosition,
                GameRosterOrderStatCategory = this.GameRosterOrderStatCategory,
                PlayerOfRecordStatCategory = this.PlayerOfRecordStatCategory,
                EnableShootOut = this.EnableShootout,
                PlayerOfRecordLabel = this.PlayerOfRecordLabel
            };
            return oRet;
        }
    }
}
