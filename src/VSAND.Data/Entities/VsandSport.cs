using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VSAND.Data.Entities
{
    public partial class VsandSport
    {
        public VsandSport()
        {
            VsandBook = new HashSet<VsandBook>();
            VsandBookFav = new HashSet<VsandBookFav>();
            VsandBookNote = new HashSet<VsandBookNote>();
            VsandGameReport = new HashSet<VsandGameReport>();
            LeagueRules = new HashSet<VsandLeagueRule>();
            VsandNews = new HashSet<VsandNews>();
            VsandPlannerNote = new HashSet<VsandPlannerNote>();
            VsandPlayerRecruiting = new HashSet<VsandPlayerRecruiting>();
            PowerPointsConfigs = new HashSet<VsandPowerPointsConfig>();
            VsandPublicationSportSubscription = new HashSet<VsandPublicationSportSubscription>();
            VsandRoundup = new HashSet<VsandRoundup>();
            VsandRoundupType = new HashSet<VsandRoundupType>();
            VsandScheduleLoadFile = new HashSet<VsandScheduleLoadFile>();
            VsandSchoolCustomCode = new HashSet<VsandSchoolCustomCode>();
            SportEvents = new HashSet<VsandSportEvent>();
            EventResults = new HashSet<VsandSportEventResult>();
            VsandSportEventStat = new HashSet<VsandSportEventStat>();
            EventTypes = new HashSet<VsandSportEventType>();
            GameMeta = new HashSet<VsandSportGameMeta>();
            VsandSportPlayerStat = new HashSet<VsandSportPlayerStat>();
            PlayerStatCategories = new HashSet<VsandSportPlayerStatCategory>();
            Positions = new HashSet<VsandSportPosition>();
            VsandSportSeason = new HashSet<VsandSportSeason>();
            VsandSportStatFormula = new HashSet<VsandSportStatFormula>();
            VsandSportTeamStat = new HashSet<VsandSportTeamStat>();
            TeamStatCategories = new HashSet<VsandSportTeamStatCategory>();
            VsandSystemFormat = new HashSet<VsandSystemFormat>();
            VsandSystemMessageSport = new HashSet<VsandSystemMessageSport>();
            Teams = new HashSet<VsandTeam>();
            VsandTeamNotifyList = new HashSet<VsandTeamNotifyList>();
            Users = new HashSet<VsandUserSport>();
        }

        public int SportId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string NeutralName { get; set; }

        [Required]
        [MaxLength(5)]
        public string Abbreviation { get; set; }
        [Required]
        [MaxLength(5)]
        public string Gender { get; set; }

        [MaxLength(20)]
        public string Season { get; set; }

        [Required]
        [MaxLength(20)]
        [DisplayName("Meet Name")]
        public string MeetName { get; set; }

        [Required]
        [MaxLength(20)]
        [DisplayName("Meet Type")]
        public string MeetType { get; set; }

        public string PeriodName { get; set; }

        public string PeriodNamePlural { get; set; }

        public bool AllowOt { get; set; }

        public bool AllowTie { get; set; }

        public bool AllowMultiEventPerDay { get; set; }

        public bool EnableTriPlusScheduling { get; set; }

        public bool EnableJerseyNumber { get; set; }

        public bool EnablePosition { get; set; }

        public string OTName { get; set; }

        public string OTNamePlural { get; set; }

        public int? DefaultPeriods { get; set; }

        public bool EnablePeriodScoring { get; set; }

        [MaxLength(20)]
        [DisplayName("Singular Participant Name")]
        public string PlayerName { get; set; }

        [MaxLength(20)]
        [DisplayName("Plural Participant Name")]
        public string PlayerNamePlural { get; set; }

        public bool? EnableLowScoreWins { get; set; }

        public bool? EnableScoringPlayByPlay { get; set; }

        public string ScoringPlayByPlayHandler { get; set; }

        public bool CountZeroValueStats { get; set; }

        public string LegacyFormatter { get; set; }

        public string SportsMlformatter { get; set; }

        public string ReadOnlyFormatter { get; set; }

        public string AtomFormatter { get; set; }

        public bool? EnablePowerPoints { get; set; }

        public string PowerPointsDataType { get; set; }

        public string PowerPointsLabel { get; set; }

        public bool? EnableDifferential { get; set; }

        public string DifferentialDataType { get; set; }

        public string DifferentialLabel { get; set; }

        [Required]
        public bool Enabled { get; set; }

        public bool EnableGamePosition { get; set; }

        public bool EnableStarter { get; set; }

        public bool EnableGameRosterOrder { get; set; }

        [MaxLength(50)]
        [DisplayName("Roster Order Label")]
        public string GameRosterOrderLabel { get; set; }

        public bool EnablePlayerOfRecord { get; set; }

        public int? PlayerOfRecordPosition { get; set; }

        public int? GameRosterOrderStatCategory { get; set; }

        public int? PlayerOfRecordStatCategory { get; set; }

        public bool EnableShootOut { get; set; }

        public string PlayerOfRecordLabel { get; set; }

        public ICollection<VsandBook> VsandBook { get; set; }

        public ICollection<VsandBookFav> VsandBookFav { get; set; }

        public ICollection<VsandBookNote> VsandBookNote { get; set; }

        public ICollection<VsandGameReport> VsandGameReport { get; set; }

        public ICollection<VsandNews> VsandNews { get; set; }

        public ICollection<VsandPlannerNote> VsandPlannerNote { get; set; }

        public ICollection<VsandPlayerRecruiting> VsandPlayerRecruiting { get; set; }

        public ICollection<VsandPublicationSportSubscription> VsandPublicationSportSubscription { get; set; }

        public ICollection<VsandRoundup> VsandRoundup { get; set; }

        public ICollection<VsandRoundupType> VsandRoundupType { get; set; }

        public ICollection<VsandScheduleLoadFile> VsandScheduleLoadFile { get; set; }

        public ICollection<VsandSchoolCustomCode> VsandSchoolCustomCode { get; set; }

        public ICollection<VsandSportEvent> SportEvents { get; set; }

        public ICollection<VsandSportEventResult> EventResults { get; set; }

        public ICollection<VsandSportEventStat> VsandSportEventStat { get; set; }

        public ICollection<VsandSportEventType> EventTypes { get; set; }

        public ICollection<VsandSportGameMeta> GameMeta { get; set; }

        public ICollection<VsandSportPlayerStat> VsandSportPlayerStat { get; set; }

        public ICollection<VsandSportPlayerStatCategory> PlayerStatCategories { get; set; }

        public ICollection<VsandSportPosition> Positions { get; set; }

        public ICollection<VsandSportSeason> VsandSportSeason { get; set; }

        public ICollection<VsandSportStatFormula> VsandSportStatFormula { get; set; }

        public ICollection<VsandSportTeamStat> VsandSportTeamStat { get; set; }

        public ICollection<VsandSportTeamStatCategory> TeamStatCategories { get; set; }

        public ICollection<VsandSystemFormat> VsandSystemFormat { get; set; }

        public ICollection<VsandSystemMessageSport> VsandSystemMessageSport { get; set; }

        public ICollection<VsandTeam> Teams { get; set; }

        public ICollection<VsandTeamNotifyList> VsandTeamNotifyList { get; set; }

        public ICollection<VsandUserSport> Users { get; set; }

        public ICollection<VsandPowerPointsConfig> PowerPointsConfigs { get; set; }

        public ICollection<VsandLeagueRule> LeagueRules { get; set; }
    }
}
