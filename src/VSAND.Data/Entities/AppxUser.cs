using System;
using System.Collections.Generic;

namespace VSAND.Data.Entities
{
    public partial class AppxUser
    {
        public AppxUser()
        {
            AppxUserRoles = new HashSet<AppxUserRoleMember>();
            VsandBook = new HashSet<VsandBook>();
            VsandBookFav = new HashSet<VsandBookFav>();
            VsandBookSubscription = new HashSet<VsandBookSubscription>();
            VsandGamePackage = new HashSet<VsandGamePackage>();
            VsandGameReport = new HashSet<VsandGameReport>();
            VsandGameReportEmailLog = new HashSet<VsandGameReportEmailLog>();
            VsandGameReportNote = new HashSet<VsandGameReportNote>();
            VsandNewsAssignTo = new HashSet<VsandNews>();
            VsandNewsCreatedBy = new HashSet<VsandNews>();
            VsandNewsPackage = new HashSet<VsandNewsPackage>();
            VsandNewsStatusBy = new HashSet<VsandNews>();
            VsandNewsStoryAssignTo = new HashSet<VsandNewsStory>();
            VsandNewsStoryStatusBy = new HashSet<VsandNewsStory>();
            VsandPublicationStoryAssignTo = new HashSet<VsandPublicationStory>();
            VsandPublicationStoryCreatedBy = new HashSet<VsandPublicationStory>();
            VsandPublicationStoryNote = new HashSet<VsandPublicationStoryNote>();
            VsandPublicationStoryStatusByNavigation = new HashSet<VsandPublicationStory>();
            VsandRoundupLeadStory = new HashSet<VsandRoundupLeadStory>();
            VsandStoryAssignTo = new HashSet<VsandStory>();
            VsandStoryStatusByNavigation = new HashSet<VsandStory>();
            VsandUserSport = new HashSet<VsandUserSport>();
        }

        public int AdminId { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public string PasswordReminder { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string RegistrationKey { get; set; }
        public string ConfirmationKey { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public string ConfirmationIp { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool Approved { get; set; }
        public bool Suspended { get; set; }
        public bool IsAdmin { get; set; }
        public int? SchoolId { get; set; }
        public int? PublicationId { get; set; }
        public int? TmpMapId { get; set; }
        public string TmpMapType { get; set; }

        public ICollection<AppxUserRoleMember> AppxUserRoles { get; set; }
        public ICollection<VsandBook> VsandBook { get; set; }
        public ICollection<VsandBookFav> VsandBookFav { get; set; }
        public ICollection<VsandBookSubscription> VsandBookSubscription { get; set; }
        public ICollection<VsandGamePackage> VsandGamePackage { get; set; }
        public ICollection<VsandGameReport> VsandGameReport { get; set; }
        public ICollection<VsandGameReportEmailLog> VsandGameReportEmailLog { get; set; }
        public ICollection<VsandGameReportNote> VsandGameReportNote { get; set; }
        public ICollection<VsandNews> VsandNewsAssignTo { get; set; }
        public ICollection<VsandNews> VsandNewsCreatedBy { get; set; }
        public ICollection<VsandNewsPackage> VsandNewsPackage { get; set; }
        public ICollection<VsandNews> VsandNewsStatusBy { get; set; }
        public ICollection<VsandNewsStory> VsandNewsStoryAssignTo { get; set; }
        public ICollection<VsandNewsStory> VsandNewsStoryStatusBy { get; set; }
        public ICollection<VsandPublicationStory> VsandPublicationStoryAssignTo { get; set; }
        public ICollection<VsandPublicationStory> VsandPublicationStoryCreatedBy { get; set; }
        public ICollection<VsandPublicationStoryNote> VsandPublicationStoryNote { get; set; }
        public ICollection<VsandPublicationStory> VsandPublicationStoryStatusByNavigation { get; set; }
        public ICollection<VsandRoundupLeadStory> VsandRoundupLeadStory { get; set; }
        public ICollection<VsandStory> VsandStoryAssignTo { get; set; }
        public ICollection<VsandStory> VsandStoryStatusByNavigation { get; set; }
        public ICollection<VsandUserSport> VsandUserSport { get; set; }
    }
}
