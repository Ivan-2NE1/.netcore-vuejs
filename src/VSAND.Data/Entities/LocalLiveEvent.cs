using System;

namespace VSAND.Data.Entities
{
    public class LocalLiveEvent
    {
        public int EventId { get; set; }
        public string EventStatus { get; set; }
        public DateTime LastUpdatedUTC { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string VenueFullName { get; set; }
        public string VenueShortName { get; set; }
        public string Title { get; set; }
        public int SportID { get; set; }
        public string VarsityLevel { get; set; }
        public string GenderText { get; set; }
        public int SchoolHomeId { get; set; }
        public string SchoolHomeName { get; set; }
        public int SchoolAwayId { get; set; }
        public string SchoolAwayName { get; set; }
        public string LiveMediaID { get; set; }
        public string ArchiveMediaID { get; set; }
        public string ThumbnailUrl { get; set; }
        public bool Tournament { get; set; }
        public string TournamentName { get; set; }

        public LocalLiveEvent()
        {

        }

        public LocalLiveEvent(Integrations.LocalLive.Event @event)
        {
            EventId = @event.Id;
            EventStatus = @event.EventStatus;
            LastUpdatedUTC = @event.LastUpdatedUTC;
            StartTime = @event.StartTime;
            EndTime = @event.EndTime;
            VenueFullName = @event.VenueFullName;
            VenueShortName = @event.VenueShortName;
            Title = @event.Title;
            SportID = @event.SportID;
            VarsityLevel = @event.VarsityLevel;
            GenderText = @event.GenderText;
            SchoolHomeId = @event.SchoolHome.Id;
            SchoolHomeName = @event.SchoolHome.Name;
            SchoolAwayId = @event.SchoolAway.Id;
            SchoolAwayName = @event.SchoolAway.Name;
            LiveMediaID = @event.LiveMediaID;
            ArchiveMediaID = @event.ArchiveMediaID;
            ThumbnailUrl = @event.ThumbnailUrl;
            Tournament = @event.Tournament;
            TournamentName = @event.TournamentName;
        }
    }
}
