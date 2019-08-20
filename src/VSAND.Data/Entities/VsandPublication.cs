using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class VsandPublication
    {
        public VsandPublication()
        {
            VsandGamePackage = new HashSet<VsandGamePackage>();
            VsandNews = new HashSet<VsandNews>();
            VsandPlannerCategory = new HashSet<VsandPlannerCategory>();
            VsandPlannerDayBudget = new HashSet<VsandPlannerDayBudget>();
            VsandPlannerNote = new HashSet<VsandPlannerNote>();
            VsandPlanningCalendar = new HashSet<VsandPlanningCalendar>();
            VsandPublicationEditionSubscription = new HashSet<VsandPublicationEditionSubscription>();
            VsandPublicationFormatVariable = new HashSet<VsandPublicationFormatVariable>();
            VsandPublicationFtp = new HashSet<VsandPublicationFtp>();
            VsandPublicationRouteCode = new HashSet<VsandPublicationRouteCode>();
            VsandPublicationSchool = new HashSet<VsandPublicationSchool>();
            VsandPublicationSportSubscription = new HashSet<VsandPublicationSportSubscription>();
            VsandPublicationStory = new HashSet<VsandPublicationStory>();
        }

        public int PublicationId { get; set; }
        public string Name { get; set; }
        public string PublicationCredit { get; set; }
        public string FileRoutePrefix { get; set; }
        public string Ftpusername { get; set; }
        public string Ftppassword { get; set; }
        public string Ftpurl { get; set; }
        public string ArticleFormat { get; set; }
        public string ScoreboardFormat { get; set; }
        public string ScoreboardProviderPage { get; set; }
        public string ScheduleFormat { get; set; }
        public string ScheduleProviderPage { get; set; }
        public string CustomRoutingFlag { get; set; }
        public string Ftpformat { get; set; }

        public ICollection<VsandGamePackage> VsandGamePackage { get; set; }
        public ICollection<VsandNews> VsandNews { get; set; }
        public ICollection<VsandPlannerCategory> VsandPlannerCategory { get; set; }
        public ICollection<VsandPlannerDayBudget> VsandPlannerDayBudget { get; set; }
        public ICollection<VsandPlannerNote> VsandPlannerNote { get; set; }
        public ICollection<VsandPlanningCalendar> VsandPlanningCalendar { get; set; }
        public ICollection<VsandPublicationEditionSubscription> VsandPublicationEditionSubscription { get; set; }
        public ICollection<VsandPublicationFormatVariable> VsandPublicationFormatVariable { get; set; }
        public ICollection<VsandPublicationFtp> VsandPublicationFtp { get; set; }
        public ICollection<VsandPublicationRouteCode> VsandPublicationRouteCode { get; set; }
        public ICollection<VsandPublicationSchool> VsandPublicationSchool { get; set; }
        public ICollection<VsandPublicationSportSubscription> VsandPublicationSportSubscription { get; set; }
        public ICollection<VsandPublicationStory> VsandPublicationStory { get; set; }
    }
}
