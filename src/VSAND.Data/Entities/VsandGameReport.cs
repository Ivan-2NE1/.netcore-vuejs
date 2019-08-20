using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandGameReport
    {
        public VsandGameReport()
        {
            BookNotes = new HashSet<VsandBookNote>();
            GamePackages = new HashSet<VsandGamePackage>();
            EmailLog = new HashSet<VsandGameReportEmailLog>();
            Events = new HashSet<VsandGameReportEvent>();
            Meta = new HashSet<VsandGameReportMeta>();
            Notes = new HashSet<VsandGameReportNote>();
            Pairings = new HashSet<VsandGameReportPairing>();
            ScoringPlays = new HashSet<VsandGameReportPlayByPlay>();
            PlayerStats = new HashSet<VsandGameReportPlayerStat>();
            Teams = new HashSet<VsandGameReportTeam>();
            PublicationStories = new HashSet<VsandPublicationStory>();
        }

        public int GameReportId { get; set; }
        public string Name { get; set; }
        public DateTime GameDate { get; set; }
        public int SportId { get; set; }
        public int ScheduleYearId { get; set; }
        public int GameTypeId { get; set; }
        public int? RoundId { get; set; }
        public int? SectionId { get; set; }
        public int? GroupId { get; set; }
        public string LocationName { get; set; }
        public string LocationCity { get; set; }
        public string LocationState { get; set; }
        public int CountyId { get; set; }
        public int ReportedBy { get; set; }
        public string ReportedByName { get; set; }
        public DateTime ReportedDate { get; set; }
        public string Source { get; set; }
        public bool TriPlus { get; set; }
        public bool Archived { get; set; }
        public bool Locked { get; set; }
        public bool Deleted { get; set; }
        public bool PPEligible { get; set; }
        public bool Exhibition { get; set; }
        public bool Final { get; set; }
        public bool? LeagueGame { get; set; }
        public DateTime? ModifiedDate { get; set; } 

        public VsandCounty County { get; set; }
        public VsandSportEventType EventType { get; set; }
        public VsandSportEventTypeGroup Group { get; set; }
        public AppxUser ReportedByUser { get; set; }
        public VsandSportEventTypeRound Round { get; set; }
        public VsandScheduleYear ScheduleYear { get; set; }
        public VsandSportEventTypeSection Section { get; set; }
        public VsandSport Sport { get; set; }
        public ICollection<VsandBookNote> BookNotes { get; set; }
        public ICollection<VsandGamePackage> GamePackages { get; set; }
        public ICollection<VsandGameReportEmailLog> EmailLog { get; set; }
        public ICollection<VsandGameReportEvent> Events { get; set; }
        public ICollection<VsandGameReportMeta> Meta { get; set; }
        public ICollection<VsandGameReportNote> Notes { get; set; }
        public ICollection<VsandGameReportPairing> Pairings { get; set; }
        public ICollection<VsandGameReportPlayByPlay> ScoringPlays { get; set; }
        public ICollection<VsandGameReportPlayerStat> PlayerStats { get; set; }
        public ICollection<VsandGameReportTeam> Teams { get; set; }
        public ICollection<VsandPublicationStory> PublicationStories { get; set; }
    }
}
