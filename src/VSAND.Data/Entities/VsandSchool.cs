using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VSAND.Data.Entities
{
    public partial class VsandSchool
    {
        public VsandSchool()
        {
            VsandBookFav = new HashSet<VsandBookFav>();
            VsandBookMember = new HashSet<VsandBookMember>();
            FeedSubscriptions = new HashSet<VsandFeedSubscriptionScopeDefinition>();
            VsandPublicationSchool = new HashSet<VsandPublicationSchool>();
            VsandScheduleLoadFileParseOpponentSchool = new HashSet<VsandScheduleLoadFileParse>();
            VsandScheduleLoadFileParseTeamSchool = new HashSet<VsandScheduleLoadFileParse>();
            Contacts = new HashSet<VsandSchoolContact>();
            CustomCodes = new HashSet<VsandSchoolCustomCode>();
            Editions = new HashSet<VsandSchoolEdition>();
            Teams = new HashSet<VsandTeam>();
            NotifyList = new HashSet<VsandTeamNotifyList>();
        }

        public int SchoolId { get; set; }

        [Required]
        public string Name { get; set; }

        public string AltName { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        [DisplayName("Zip Code")]
        public string ZipCode { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        public bool? PrivateSchool { get; set; }

        public bool CoreCoverage { get; set; }

        public bool FrontEndDisplay { get; set; }

        public int? CountyId { get; set; }

        public string Nickname { get; set; }

        public string Mascot { get; set; }

        public string Color1 { get; set; }

        public string Color2 { get; set; }

        public string Color3 { get; set; }

        public string Url { get; set; }

        public string Graphic { get; set; }

        public bool? GraphicApproved { get; set; }

        public bool Validated { get; set; }

        public string ValidatedBy { get; set; }

        public int? ValidatedById { get; set; }

        [NotMapped]
        public string Slug
        {
            get
            {
                //TODO: School slug should probably be initialized once, and then manually editable and stored in the db as a field
                // Clean-up name replacing any spaces or special chars with "-"
                // Then replace any double hyphens with single hyphen
                // Then append town name with same replacement rules
                System.Text.RegularExpressions.Regex oRe = new System.Text.RegularExpressions.Regex("[^A-Z0-9a-z]");
                string cleanName = oRe.Replace(Name.Trim(), "-");
                string cleanCity = City;
                if (string.IsNullOrWhiteSpace(cleanCity))
                {
                    cleanCity = "";
                }
                cleanCity = oRe.Replace(cleanCity.Trim(), "-");
                string slug = cleanName + "-" + cleanCity;
                slug = slug.Replace("--", "-");
                return slug.ToLowerInvariant();
            }
        }

        public VsandCounty County { get; set; }

        public ICollection<VsandBookFav> VsandBookFav { get; set; }

        public ICollection<VsandBookMember> VsandBookMember { get; set; }

        public ICollection<VsandFeedSubscriptionScopeDefinition> FeedSubscriptions { get; set; }

        public ICollection<VsandPublicationSchool> VsandPublicationSchool { get; set; }

        public ICollection<VsandScheduleLoadFileParse> VsandScheduleLoadFileParseOpponentSchool { get; set; }

        public ICollection<VsandScheduleLoadFileParse> VsandScheduleLoadFileParseTeamSchool { get; set; }

        public ICollection<VsandSchoolContact> Contacts { get; set; }

        public ICollection<VsandSchoolCustomCode> CustomCodes { get; set; }

        public ICollection<VsandSchoolEdition> Editions { get; set; }

        public ICollection<VsandTeam> Teams { get; set; }

        public ICollection<VsandTeamNotifyList> NotifyList { get; set; }
    }
}
