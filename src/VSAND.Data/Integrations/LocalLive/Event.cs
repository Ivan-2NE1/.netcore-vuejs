using System;

namespace VSAND.Data.Integrations.LocalLive
{
    public class Event
    {
        public int Id { get; set; }
        public string EventStatus { get; set; }
        public DateTime LastUpdatedUTC { get; set; }
        public DateTime CreatedDateUTC { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime StartTimeUtc { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime EndTimeUtc { get; set; }
        public DateTime? OldStartTime { get; set; }
        public DateTime? OldEndTime { get; set; }
        public string VenueTimeZone { get; set; }
        public string VenueFullName { get; set; }
        public string VenueShortName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int SportID { get; set; }
        public string SportsDisplay { get; set; }
        public string SportsSlug { get; set; }
        public string VarsityLevel { get; set; }
        public string GenderText { get; set; }
        public int GenderID { get; set; }
        public School SchoolHome { get; set; }
        public School SchoolAway { get; set; }
        public string LiveEmbed { get; set; }
        public string LiveMediaID { get; set; }
        public string ArchiveEmbed { get; set; }
        public string ArchiveMediaID { get; set; }
        public string LiveHlsUrl { get; set; }
        public string LiveContentID { get; set; }
        public string LiveSourceID { get; set; }
        public string ArchiveHlsUrl { get; set; }
        public string ArchiveContentID { get; set; }
        public int TrimSeconds { get; set; }
        public string ThumbnailUrl { get; set; }
        public bool Tournament { get; set; }
        public string TournamentName { get; set; }
    }
}
