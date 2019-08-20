using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VSAND.Data.Entities;
using VSAND.Data.Identity;
using VSAND.Data.ViewModels;

namespace VSAND.Data
{
    public partial class VsandContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
    {
        public VsandContext(DbContextOptions<VsandContext> options) : base(options)
        {
        }

        public virtual DbSet<AppxAudit> AppxAudit { get; set; }
        public virtual DbSet<AppxCmsContent> AppxCmsContent { get; set; }
        public virtual DbSet<AppxCmsContentArea> AppxCmsContentArea { get; set; }
        public virtual DbSet<AppxCmsContentMessage> AppxCmsContentMessage { get; set; }
        public virtual DbSet<AppxCmsContentPage> AppxCmsContentPage { get; set; }
        public virtual DbSet<AppxCmsContentVirtual> AppxCmsContentVirtual { get; set; }
        public virtual DbSet<AppxCmsEasyMenu> AppxCmsEasyMenu { get; set; }
        public virtual DbSet<AppxCmsEvent> AppxcmsEvent { get; set; }
        public virtual DbSet<AppxCmsEventType> AppxcmsEventType { get; set; }
        public virtual DbSet<AppxCmsNews> AppxcmsNews { get; set; }
        public virtual DbSet<AppxCmsNewsComment> AppxcmsNewsComment { get; set; }
        public virtual DbSet<AppxCmsNewsType> AppxcmsNewsType { get; set; }
        public virtual DbSet<AppxConfig> AppxConfig { get; set; }
        public virtual DbSet<AppxEmailTemplate> AppxEmailTemplate { get; set; }
        public virtual DbSet<AppxErrorLog> AppxErrorLog { get; set; }
        public virtual DbSet<AppxMailingList> AppxMailingList { get; set; }
        public virtual DbSet<AppxMailingListSubscription> AppxMailingListSubscription { get; set; }
        public virtual DbSet<AppxMemberPermission> AppxMemberPermission { get; set; }
        public virtual DbSet<AppxMemberUser> AppxMemberUser { get; set; }
        public virtual DbSet<AppxMessageBlastCampaign> AppxMessageBlastCampaign { get; set; }
        public virtual DbSet<AppxMessageBlastCampaignAttachment> AppxMessageBlastCampaignAttachment { get; set; }
        public virtual DbSet<AppxMessageBlastCampaignDistribution> AppxMessageBlastCampaignDistribution { get; set; }
        public virtual DbSet<AppxMessageBlastCampaignSend> AppxMessageBlastCampaignSend { get; set; }
        public virtual DbSet<AppxMessageBlastCampaignSendRecipient> AppxMessageBlastCampaignSendRecipient { get; set; }
        public virtual DbSet<AppxMessageBlastCampaignSendStatus> AppxMessageBlastCampaignSendStatus { get; set; }
        public virtual DbSet<AppxMessageBlastDistributionList> AppxMessageBlastDistributionList { get; set; }
        public virtual DbSet<AppxMessageBlastListSource> AppxMessageBlastListSource { get; set; }
        public virtual DbSet<AppxReferrerConversion> AppxReferrerConversion { get; set; }
        public virtual DbSet<AppxReferrerKeyword> AppxReferrerKeyword { get; set; }
        public virtual DbSet<AppxReferrerReferral> AppxReferrerReferral { get; set; }
        public virtual DbSet<AppxReferrerUrl> AppxReferrerUrl { get; set; }
        public virtual DbSet<AppxSurveyHeader> AppxSurveyHeader { get; set; }
        public virtual DbSet<AppxSurveyQuestion> AppxSurveyQuestion { get; set; }
        public virtual DbSet<AppxSurveyResponse> AppxSurveyResponse { get; set; }
        public virtual DbSet<AppxSurveyResponseHeader> AppxSurveyResponseHeader { get; set; }
        public virtual DbSet<AppxUser> AppxUser { get; set; }
        public virtual DbSet<AppxUserInfo> AppxUserInfo { get; set; }
        public virtual DbSet<AppxUserInfoCategory> AppxUserInfoCategory { get; set; }
        public virtual DbSet<AppxUserInfoColumn> AppxUserInfoColumn { get; set; }
        public virtual DbSet<AppxUserRole> AppxUserRole { get; set; }
        public virtual DbSet<AppxUserRoleMember> AppxUserRoleMember { get; set; }
        public virtual DbSet<AspnetApplications> AspnetApplications { get; set; }
        public virtual DbSet<AspnetMembership> AspnetMembership { get; set; }
        public virtual DbSet<AspnetPaths> AspnetPaths { get; set; }
        public virtual DbSet<AspnetPersonalizationAllUsers> AspnetPersonalizationAllUsers { get; set; }
        public virtual DbSet<AspnetPersonalizationPerUser> AspnetPersonalizationPerUser { get; set; }
        public virtual DbSet<AspnetProfile> AspnetProfile { get; set; }
        public virtual DbSet<AspnetRoles> AspnetRoles { get; set; }
        public virtual DbSet<AspnetSchemaVersions> AspnetSchemaVersions { get; set; }
        public virtual DbSet<AspnetUsers> AspnetUsers { get; set; }
        public virtual DbSet<AspnetUsersInRoles> AspnetUsersInRoles { get; set; }
        public virtual DbSet<AspnetWebEventEvents> AspnetWebEventEvents { get; set; }
        public virtual DbSet<LocalLiveEvent> LocalLiveEvents { get; set; }
        public virtual DbSet<NjspBridgeServiceLog> NjspBridgeServiceLog { get; set; }
        public virtual DbSet<VsandBook> VsandBook { get; set; }
        public virtual DbSet<VsandBookFav> VsandBookFav { get; set; }
        public virtual DbSet<VsandBookMember> VsandBookMember { get; set; }
        public virtual DbSet<VsandBookNote> VsandBookNote { get; set; }
        public virtual DbSet<VsandBookSubscription> VsandBookSubscription { get; set; }
        public virtual DbSet<VsandConference> VsandConference { get; set; }
        public virtual DbSet<VsandCounty> VsandCounty { get; set; }
        public virtual DbSet<VsandEdition> VsandEdition { get; set; }
        public virtual DbSet<VsandEntitySlug> VsandEntitySlugs { get; set; }
        public virtual DbSet<VsandFeedSubscription> VsandFeedSubscription { get; set; }
        public virtual DbSet<VsandFeedSubscriptionScope> VsandFeedSubscriptionScope { get; set; }
        public virtual DbSet<VsandFeedSubscriptionScopeDefinition> VsandFeedSubscriptionScopeDefinition { get; set; }
        public virtual DbSet<VsandFeedSubscriptionScopeType> VsandFeedSubscriptionScopeType { get; set; }
        public virtual DbSet<VsandGamePackage> VsandGamePackage { get; set; }
        public virtual DbSet<VsandGameReport> VsandGameReport { get; set; }
        public virtual DbSet<VsandGameReportEmailLog> VsandGameReportEmailLog { get; set; }
        public virtual DbSet<VsandGameReportEvent> VsandGameReportEvent { get; set; }
        public virtual DbSet<VsandGameReportEventPlayer> VsandGameReportEventPlayer { get; set; }
        public virtual DbSet<VsandGameReportEventPlayerGroup> VsandGameReportEventPlayerGroup { get; set; }
        public virtual DbSet<VsandGameReportEventPlayerGroupPlayer> VsandGameReportEventPlayerGroupPlayer { get; set; }
        public virtual DbSet<VsandGameReportEventPlayerGroupStat> VsandGameReportEventPlayerGroupStat { get; set; }
        public virtual DbSet<VsandGameReportEventPlayerStat> VsandGameReportEventPlayerStat { get; set; }
        public virtual DbSet<VsandGameReportEventResult> VsandGameReportEventResult { get; set; }
        public virtual DbSet<VsandGameReportMeta> VsandGameReportMeta { get; set; }
        public virtual DbSet<VsandGameReportNote> VsandGameReportNote { get; set; }
        public virtual DbSet<VsandGameReportPairing> VsandGameReportPairing { get; set; }
        public virtual DbSet<VsandGameReportPairingTeam> VsandGameReportPairingTeam { get; set; }
        public virtual DbSet<VsandGameReportPeriodScore> VsandGameReportPeriodScore { get; set; }
        public virtual DbSet<VsandGameReportPlayByPlay> VsandGameReportPlayByPlay { get; set; }
        public virtual DbSet<VsandGameReportPlayerStat> VsandGameReportPlayerStat { get; set; }
        public virtual DbSet<VsandGameReportRoster> VsandGameReportRoster { get; set; }
        public virtual DbSet<VsandGameReportTeam> VsandGameReportTeam { get; set; }
        public virtual DbSet<VsandGameReportTeamStat> VsandGameReportTeamStat { get; set; }
        public virtual DbSet<VsandLeagueRule> VsandLeagueRule { get; set; }
        public virtual DbSet<VsandLeagueRuleItem> VsandLeagueRuleItem { get; set; }
        public virtual DbSet<VsandLog> VsandLog { get; set; }
        public virtual DbSet<VsandNews> VsandNews { get; set; }
        public virtual DbSet<VsandNewsPackage> VsandNewsPackage { get; set; }
        public virtual DbSet<VsandNewsStory> VsandNewsStory { get; set; }
        public virtual DbSet<VsandNewsType> VsandNewsType { get; set; }
        public virtual DbSet<VsandOptOut> VsandOptOut { get; set; }
        public virtual DbSet<VsandPitchCountTracking> VsandPitchCountTracking { get; set; }
        public virtual DbSet<VsandPlannerCategory> VsandPlannerCategory { get; set; }
        public virtual DbSet<VsandPlannerDayBudget> VsandPlannerDayBudget { get; set; }
        public virtual DbSet<VsandPlannerLayout> VsandPlannerLayout { get; set; }
        public virtual DbSet<VsandPlannerNote> VsandPlannerNote { get; set; }
        public virtual DbSet<VsandPlanningCalendar> VsandPlanningCalendar { get; set; }
        public virtual DbSet<VsandPlayer> VsandPlayer { get; set; }
        public virtual DbSet<VsandPlayerRecruiting> VsandPlayerRecruiting { get; set; }
        public virtual DbSet<VsandPlayerStatSummaryShim> VsandPlayerStatSummaryShim { get; set; }
        public virtual DbSet<VsandPublication> VsandPublication { get; set; }
        public virtual DbSet<VsandPublicationEditionSubscription> VsandPublicationEditionSubscription { get; set; }
        public virtual DbSet<VsandPublicationFormat> VsandPublicationFormat { get; set; }
        public virtual DbSet<VsandPublicationFormatVariable> VsandPublicationFormatVariable { get; set; }
        public virtual DbSet<VsandPublicationFtp> VsandPublicationFtp { get; set; }
        public virtual DbSet<VsandPublicationRouteCode> VsandPublicationRouteCode { get; set; }
        public virtual DbSet<VsandPublicationSchool> VsandPublicationSchool { get; set; }
        public virtual DbSet<VsandPublicationSportSubscription> VsandPublicationSportSubscription { get; set; }
        public virtual DbSet<VsandPublicationStory> VsandPublicationStory { get; set; }
        public virtual DbSet<VsandPublicationStoryNote> VsandPublicationStoryNote { get; set; }
        public virtual DbSet<VsandPublicationStoryPlayByPlay> VsandPublicationStoryPlayByPlay { get; set; }
        public virtual DbSet<VsandRoundup> VsandRoundup { get; set; }
        public virtual DbSet<VsandRoundupLeadStory> VsandRoundupLeadStory { get; set; }
        public virtual DbSet<VsandRoundupMember> VsandRoundupMember { get; set; }
        public virtual DbSet<VsandRoundupType> VsandRoundupType { get; set; }
        public virtual DbSet<VsandScheduleLoadFile> VsandScheduleLoadFile { get; set; }
        public virtual DbSet<VsandScheduleLoadFileParse> VsandScheduleLoadFileParse { get; set; }
        public virtual DbSet<VsandScheduleLoadImportSource> VsandScheduleLoadImportSource { get; set; }
        public virtual DbSet<VsandScheduleYear> VsandScheduleYear { get; set; }
        public virtual DbSet<VsandPowerPointsConfig> VsandPowerPointsConfig { get; set; }
        public virtual DbSet<VsandSchool> VsandSchool { get; set; }
        public virtual DbSet<VsandSchoolContact> VsandSchoolContact { get; set; }
        public virtual DbSet<VsandSchoolCustomCode> VsandSchoolCustomCode { get; set; }
        public virtual DbSet<VsandSchoolEdition> VsandSchoolEdition { get; set; }
        public virtual DbSet<VsandSendHistory> VsandSendHistory { get; set; }
        public virtual DbSet<VsandshimProblemSchool> VsandshimProblemSchool { get; set; }
        public virtual DbSet<VsandshimSummaryCountByDay> VsandshimSummaryCountByDay { get; set; }
        public virtual DbSet<VsandshimUserActivity> VsandshimUserActivity { get; set; }
        public virtual DbSet<VsandSport> VsandSport { get; set; }
        public virtual DbSet<VsandSportEvent> VsandSportEvent { get; set; }
        public virtual DbSet<VsandSportEventResult> VsandSportEventResult { get; set; }
        public virtual DbSet<VsandSportEventStat> VsandSportEventStat { get; set; }
        public virtual DbSet<VsandSportEventType> VsandSportEventType { get; set; }
        public virtual DbSet<VsandSportEventTypeAlias> VsandSportEventTypeAlias { get; set; }
        public virtual DbSet<VsandSportEventTypeGroup> VsandSportEventTypeGroup { get; set; }
        public virtual DbSet<VsandSportEventTypeRound> VsandSportEventTypeRound { get; set; }
        public virtual DbSet<VsandSportEventTypeSection> VsandSportEventTypeSection { get; set; }
        public virtual DbSet<VsandSportGameMeta> VsandSportGameMeta { get; set; }
        public virtual DbSet<VsandSportPlayerStat> VsandSportPlayerStat { get; set; }
        public virtual DbSet<VsandSportPlayerStatCategory> VsandSportPlayerStatCategory { get; set; }
        public virtual DbSet<VsandSportPosition> VsandSportPosition { get; set; }
        public virtual DbSet<VsandSportSeason> VsandSportSeason { get; set; }
        public virtual DbSet<VsandSportStatFormula> VsandSportStatFormula { get; set; }
        public virtual DbSet<VsandSportTeamStat> VsandSportTeamStat { get; set; }
        public virtual DbSet<VsandSportTeamStatCategory> VsandSportTeamStatCategory { get; set; }
        public virtual DbSet<VsandState> VsandState { get; set; }
        public virtual DbSet<VsandStatQuery> VsandStatQuery { get; set; }
        public virtual DbSet<VsandStory> VsandStory { get; set; }
        public virtual DbSet<VsandSystemFormat> VsandSystemFormat { get; set; }
        public virtual DbSet<VsandSystemFormatVariable> VsandSystemFormatVariable { get; set; }
        public virtual DbSet<VsandSystemMessage> VsandSystemMessage { get; set; }
        public virtual DbSet<VsandSystemMessageSport> VsandSystemMessageSport { get; set; }
        public virtual DbSet<VsandTeam> VsandTeam { get; set; }
        public virtual DbSet<VsandTeamContact> VsandTeamContact { get; set; }
        public virtual DbSet<VsandTeamCustomCode> VsandTeamCustomCode { get; set; }
        public virtual DbSet<VsandTeamNotifyList> VsandTeamNotifyList { get; set; }
        public virtual DbSet<VsandTeamRoster> VsandTeamRoster { get; set; }
        public virtual DbSet<VsandTeamRosterCustomCode> VsandTeamRosterCustomCode { get; set; }
        public virtual DbSet<VsandTeamSchedule> VsandTeamSchedule { get; set; }
        public virtual DbSet<VsandTeamScheduleTeam> VsandTeamScheduleTeam { get; set; }
        public virtual DbSet<VsandTeamStatSummaryShim> VsandTeamStatSummaryShim { get; set; }
        public virtual DbSet<VsandUserSport> VsandUserSport { get; set; }

        public virtual DbQuery<VsandSchoolsWithoutAccounts> VsandSchoolsWithoutAccounts { get; set; }
        public virtual DbQuery<TeamsWithDuplicateGameReports> TeamsWithDuplicateGameReports { get; set; }

        // TODO: Unable to generate entity type for table 'dbo.appxUsers'. Please see the warning messages.
        // TODO: Unable to generate entity type for table 'dbo.vsand_College'. Please see the warning messages.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationRole>(entity =>
            {
                entity.ToTable("vsand_IdentityRole");
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<ApplicationRoleClaim>(entity =>
            {
                entity.ToTable("vsand_IdentityRoleClaim");
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("vsand_IdentityUser");
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<ApplicationUserClaim>(entity =>
            {
                entity.ToTable("vsand_IdentityUserClaim");
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<ApplicationUserLogin>(entity =>
            {
                entity.ToTable("vsand_IdentityUserLogin");
                entity.HasKey(e => e.UserId);
            });

            modelBuilder.Entity<ApplicationUserRole>(entity =>
            {
                entity.ToTable("vsand_IdentityUserRole");
                entity.HasKey(e => new { e.UserId, e.RoleId });
            });

            modelBuilder.Entity<ApplicationUserToken>(entity =>
            {
                entity.ToTable("vsand_IdentityUserToken");
                entity.HasKey(e => e.UserId);
            });

            modelBuilder.Entity<AppxAudit>(entity =>
            {
                entity.HasKey(e => e.AuditId);

                entity.ToTable("appxAudit");

                entity.Property(e => e.AuditId).HasColumnName("AuditID");

                entity.Property(e => e.AuditAction)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AuditData).HasColumnType("ntext");

                entity.Property(e => e.AuditTable)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AuditUser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AuditUserId).HasColumnName("AuditUserID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<AppxCmsContent>(entity =>
            {
                entity.HasKey(e => e.ContentId);

                entity.ToTable("appxCMS_Content");

                entity.Property(e => e.ContentId).HasColumnName("ContentID");

                entity.Property(e => e.ContentArea)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ContentData).HasColumnType("ntext");

                entity.Property(e => e.PageRef)
                    .IsRequired()
                    .HasMaxLength(260)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxCmsContentArea>(entity =>
            {
                entity.HasKey(e => e.ContentAreaId);

                entity.ToTable("appxCMS_ContentArea");

                entity.Property(e => e.ContentAreaId).HasColumnName("ContentAreaID");

                entity.Property(e => e.ContentArea)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxCmsContentMessage>(entity =>
            {
                entity.HasKey(e => e.ContentMessageId);

                entity.ToTable("appxCMS_ContentMessage");

                entity.Property(e => e.ContentMessageId).HasColumnName("ContentMessageID");

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.ContentClass)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PageRef)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Placeholder)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxCmsContentPage>(entity =>
            {
                entity.HasKey(e => e.ContentPageId);

                entity.ToTable("appxCMS_ContentPage");

                entity.Property(e => e.ContentPageId).HasColumnName("ContentPageID");

                entity.Property(e => e.MetaAbstract)
                    .HasColumnName("Meta_Abstract")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.MetaAuthor)
                    .HasColumnName("Meta_Author")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MetaCopyright)
                    .HasColumnName("Meta_Copyright")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MetaDescription)
                    .HasColumnName("Meta_Description")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.MetaKeyword)
                    .HasColumnName("Meta_Keyword")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.PageRef)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PageTitle)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.PageType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ScriptResource).HasColumnType("ntext");
            });

            modelBuilder.Entity<AppxCmsContentVirtual>(entity =>
            {
                entity.HasKey(e => e.VcontentId);

                entity.ToTable("appxCMS_ContentVirtual");

                entity.Property(e => e.VcontentId).HasColumnName("VContentID");

                entity.Property(e => e.ContentData).HasColumnType("ntext");

                entity.Property(e => e.ExpirationAction)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExpirationContent).HasColumnType("ntext");

                entity.Property(e => e.ExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.PageRef)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PageTemplate)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxCmsEasyMenu>(entity =>
            {
                entity.ToTable("appxCMS_EasyMenu");

                entity.Property(e => e.AllowedRoles)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Icon)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.InnerHtml)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxCmsEvent>(entity =>
            {
                entity.HasKey(e => e.EventId);

                entity.ToTable("appxcms_Event");

                entity.Property(e => e.EventEndDate).HasColumnType("datetime");

                entity.Property(e => e.EventStartDate).HasColumnType("datetime");

                entity.Property(e => e.EventSubType)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EventSummary)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EventTitle)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.EventType)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxCmsEventType>(entity =>
            {
                entity.HasKey(e => e.EventTypeId);

                entity.ToTable("appxcms_EventType");

                entity.Property(e => e.EventType)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxCmsNews>(entity =>
            {
                entity.HasKey(e => e.NewsId);

                entity.ToTable("appxcms_News");

                entity.Property(e => e.NewsId).HasColumnName("NewsID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Creator)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExpireDate).HasColumnType("datetime");

                entity.Property(e => e.Headline)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PublishDate).HasColumnType("datetime");

                entity.Property(e => e.Story).IsUnicode(false);

                entity.Property(e => e.Summary)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxCmsNewsComment>(entity =>
            {
                entity.HasKey(e => e.CommentId);

                entity.ToTable("appxcms_NewsComment");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.Property(e => e.CommentDate).HasColumnType("smalldatetime");

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Ipaddress)
                    .IsRequired()
                    .HasColumnName("IPAddress")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxCmsNewsType>(entity =>
            {
                entity.HasKey(e => e.NewsTypeId);

                entity.ToTable("appxcms_NewsType");

                entity.Property(e => e.Copyright)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Language)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ManagingEditor)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NewsType)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ttl).HasColumnName("TTL");

                entity.Property(e => e.Webmaster)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxConfig>(entity =>
            {
                entity.HasKey(e => e.ConfigId);

                entity.ToTable("appxConfig");

                entity.Property(e => e.ConfigId).HasColumnName("ConfigID");

                entity.Property(e => e.ConfigCat)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ConfigName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ConfigVal)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxEmailTemplate>(entity =>
            {
                entity.HasKey(e => e.EmailId);

                entity.ToTable("appxEmailTemplate");

                entity.Property(e => e.EmailId).HasColumnName("EmailID");

                entity.Property(e => e.Bcclist)
                    .HasColumnName("BCCList")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Body).HasColumnType("text");

                entity.Property(e => e.Cclist)
                    .HasColumnName("CCList")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmailType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FromAddress)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsHtml).HasColumnName("IsHTML");

                entity.Property(e => e.ReplyToAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ToAddress)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxErrorLog>(entity =>
            {
                entity.HasKey(e => e.ErrorId);

                entity.ToTable("appxErrorLog");

                entity.Property(e => e.ErrorId).HasColumnName("ErrorID");

                entity.Property(e => e.AcknowledgedUser)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ErrorClass)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ErrorDate).HasColumnType("datetime");

                entity.Property(e => e.ErrorMessage).HasColumnType("ntext");
            });

            modelBuilder.Entity<AppxMailingList>(entity =>
            {
                entity.HasKey(e => e.MailingListId);

                entity.ToTable("appxMailingList");

                entity.Property(e => e.Description)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.Frequency)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxMailingListSubscription>(entity =>
            {
                entity.HasKey(e => e.SubscriptionId);

                entity.ToTable("appxMailingList_Subscription");

                entity.Property(e => e.ConfirmationDate).HasColumnType("datetime");

                entity.Property(e => e.ConfirmationIp)
                    .HasColumnName("ConfirmationIP")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ConfirmationKey)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.SubscribeDate).HasColumnType("datetime");

                entity.Property(e => e.SubscribeIp)
                    .IsRequired()
                    .HasColumnName("SubscribeIP")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxMemberPermission>(entity =>
            {
                entity.HasKey(e => e.MemberPermissionId);

                entity.ToTable("appxMember_Permission");

                entity.Property(e => e.PermissionName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PermissionPath)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PermissionType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxMemberUser>(entity =>
            {
                entity.HasKey(e => e.MemberId);

                entity.ToTable("appxMember_User");

                entity.Property(e => e.ConfirmationDate).HasColumnType("datetime");

                entity.Property(e => e.ConfirmationIp)
                    .HasColumnName("ConfirmationIP")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ConfirmationKey)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastLoginDate).HasColumnType("datetime");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordReminder)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");

                entity.Property(e => e.RegistrationKey)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.UserAlias)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ZipCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxMessageBlastCampaign>(entity =>
            {
                entity.HasKey(e => e.CampaignId);

                entity.ToTable("appxMessageBlast_Campaign");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Htmlbody)
                    .HasColumnName("HTMLBody")
                    .HasColumnType("ntext");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RecipientTemplateField)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ReplyTo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SenderEmail)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SenderName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Smtpserver)
                    .HasColumnName("SMTPServer")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TextBody).HasColumnType("ntext");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxMessageBlastCampaignAttachment>(entity =>
            {
                entity.HasKey(e => e.AttachmentId);

                entity.ToTable("appxMessageBlast_CampaignAttachment");

                entity.Property(e => e.AttachmentId).HasColumnName("AttachmentID");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxMessageBlastCampaignDistribution>(entity =>
            {
                entity.HasKey(e => e.CampaignDistributionId);

                entity.ToTable("appxMessageBlast_CampaignDistribution");

                entity.Property(e => e.CampaignDistributionId).HasColumnName("CampaignDistributionID");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.DistributionListId).HasColumnName("DistributionListID");
            });

            modelBuilder.Entity<AppxMessageBlastCampaignSend>(entity =>
            {
                entity.HasKey(e => e.SendId);

                entity.ToTable("appxMessageBlast_CampaignSend");

                entity.Property(e => e.SendId).HasColumnName("SendID");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.SentBy)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SentById).HasColumnName("SentByID");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.TrackingNumber)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxMessageBlastCampaignSendRecipient>(entity =>
            {
                entity.HasKey(e => e.RecipientId);

                entity.ToTable("appxMessageBlast_CampaignSendRecipient");

                entity.Property(e => e.RecipientId).HasColumnName("RecipientID");

                entity.Property(e => e.BounceData)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MessageSentDate).HasColumnType("datetime");

                entity.Property(e => e.RecipientData).HasColumnType("ntext");

                entity.Property(e => e.RecipientEmail)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SendId).HasColumnName("SendID");
            });

            modelBuilder.Entity<AppxMessageBlastCampaignSendStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId);

                entity.ToTable("appxMessageBlast_CampaignSendStatus");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.SendId).HasColumnName("SendID");

                entity.Property(e => e.StatusDate).HasColumnType("datetime");

                entity.Property(e => e.StatusMsg)
                    .IsRequired()
                    .HasMaxLength(4000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxMessageBlastDistributionList>(entity =>
            {
                entity.HasKey(e => e.DistributionListId);

                entity.ToTable("appxMessageBlast_DistributionList");

                entity.Property(e => e.DistributionListId).HasColumnName("DistributionListID");

                entity.Property(e => e.Filter).HasColumnType("ntext");

                entity.Property(e => e.ListSourceId).HasColumnName("ListSourceID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxMessageBlastListSource>(entity =>
            {
                entity.HasKey(e => e.ListSourceId);

                entity.ToTable("appxMessageBlast_ListSource");

                entity.Property(e => e.ListSourceId).HasColumnName("ListSourceID");

                entity.Property(e => e.ConnectionString)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DataSourceType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ListName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ListQuery)
                    .IsRequired()
                    .HasColumnType("ntext");
            });

            modelBuilder.Entity<AppxReferrerConversion>(entity =>
            {
                entity.HasKey(e => e.Roid);

                entity.ToTable("appxReferrer_Conversion");

                entity.Property(e => e.Roid).HasColumnName("ROID");

                entity.Property(e => e.ConversionId).HasColumnName("ConversionID");

                entity.Property(e => e.ConversionType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Rid).HasColumnName("RID");
            });

            modelBuilder.Entity<AppxReferrerKeyword>(entity =>
            {
                entity.HasKey(e => e.Rkid);

                entity.ToTable("appxReferrer_Keyword");

                entity.Property(e => e.Rkid).HasColumnName("RKID");

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Keyword)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ruid).HasColumnName("RUID");
            });

            modelBuilder.Entity<AppxReferrerReferral>(entity =>
            {
                entity.HasKey(e => e.Rid);

                entity.ToTable("appxReferrer_Referral");

                entity.Property(e => e.Rid).HasColumnName("RID");

                entity.Property(e => e.Browser)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Ipaddress)
                    .HasColumnName("IPAddress")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ReferralDate).HasColumnType("datetime");

                entity.Property(e => e.Rkid)
                    .IsRequired()
                    .HasColumnName("RKID")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxReferrerUrl>(entity =>
            {
                entity.HasKey(e => e.Ruid);

                entity.ToTable("appxReferrer_URL");

                entity.Property(e => e.Ruid).HasColumnName("RUID");

                entity.Property(e => e.SiteName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("URL")
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxSurveyHeader>(entity =>
            {
                entity.HasKey(e => e.SurveyId);

                entity.ToTable("appxSurvey_Header");

                entity.Property(e => e.SurveyId).HasColumnName("SurveyID");

                entity.Property(e => e.ConfirmationText).HasColumnType("ntext");

                entity.Property(e => e.PostText).HasColumnType("ntext");

                entity.Property(e => e.PreText).HasColumnType("ntext");

                entity.Property(e => e.Redirect)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ResponseAction)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ResponseActionResource)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SubmitButtonText)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SurveyName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxSurveyQuestion>(entity =>
            {
                entity.HasKey(e => e.SurveyQuestionId);

                entity.ToTable("appxSurvey_Question");

                entity.Property(e => e.SurveyQuestionId).HasColumnName("SurveyQuestionID");

                entity.Property(e => e.FieldName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FieldType)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Question)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ResponseOptions).HasColumnType("ntext");

                entity.Property(e => e.SurveyId).HasColumnName("SurveyID");

                entity.Property(e => e.ValidationMessage)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxSurveyResponse>(entity =>
            {
                entity.HasKey(e => e.SurveyResponseId);

                entity.ToTable("appxSurvey_Response");

                entity.Property(e => e.SurveyResponseId).HasColumnName("SurveyResponseID");

                entity.Property(e => e.Response)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.SurveyQuestionId).HasColumnName("SurveyQuestionID");

                entity.Property(e => e.SurveyResponseHeaderId).HasColumnName("SurveyResponseHeaderID");
            });

            modelBuilder.Entity<AppxSurveyResponseHeader>(entity =>
            {
                entity.HasKey(e => e.SurveyResponseHeaderId);

                entity.ToTable("appxSurvey_ResponseHeader");

                entity.Property(e => e.SurveyResponseHeaderId).HasColumnName("SurveyResponseHeaderID");

                entity.Property(e => e.AcknowledgedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AcknowledgedDate).HasColumnType("datetime");

                entity.Property(e => e.Browser)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Ipaddress)
                    .HasColumnName("IPAddress")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Ipgeocode)
                    .HasColumnName("IPGeocode")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.RespondantId).HasColumnName("RespondantID");

                entity.Property(e => e.ResponseDate).HasColumnType("datetime");

                entity.Property(e => e.ResponseUrl)
                    .HasColumnName("ResponseURL")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.SurveyId).HasColumnName("SurveyID");
            });

            modelBuilder.Entity<AppxUser>(entity =>
            {
                entity.HasKey(e => e.AdminId);

                entity.ToTable("appxUser");

                entity.HasIndex(e => e.EmailAddress)
                    .HasName("missing_index_834_833_appxUser");

                entity.HasIndex(e => e.IsAdmin)
                    .HasName("missing_index_1852_1851_appxUser");

                entity.HasIndex(e => e.SchoolId)
                    .HasName("idx_appx_User1");

                entity.HasIndex(e => new { e.RegistrationKey, e.ConfirmationKey })
                    .HasName("missing_index_259546_259545_appxUser");

                entity.Property(e => e.AdminId).HasColumnName("AdminID");

                entity.Property(e => e.ConfirmationDate).HasColumnType("datetime");

                entity.Property(e => e.ConfirmationIp)
                    .HasColumnName("ConfirmationIP")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ConfirmationKey)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastLoginDate).HasColumnType("datetime");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordReminder)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");

                entity.Property(e => e.RegistrationKey)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.TmpMapType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("UserID")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxUserInfo>(entity =>
            {
                entity.HasKey(e => e.UserInfoId);

                entity.ToTable("appxUser_Info");

                entity.Property(e => e.Value)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxUserInfoCategory>(entity =>
            {
                entity.HasKey(e => e.UserInfoCategoryId);

                entity.ToTable("appxUser_InfoCategory");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxUserInfoColumn>(entity =>
            {
                entity.HasKey(e => e.UserInfoColumnId);

                entity.ToTable("appxUser_InfoColumn");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ValidationMessage)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ValueList)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxUserRole>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.ToTable("appxUser_Role");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.Description)
                    .HasMaxLength(350)
                    .IsUnicode(false);

                entity.Property(e => e.RoleCat)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AppxUserRoleMember>(entity =>
            {
                entity.HasKey(e => e.AdminRoleId);

                entity.ToTable("appxUser_RoleMember");

                entity.Property(e => e.AdminRoleId).HasColumnName("AdminRoleID");

                entity.Property(e => e.AdminId).HasColumnName("AdminID");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.AppxUserRoles)
                    .HasForeignKey(d => d.AdminId)
                    .HasConstraintName("FK_appxUser_RoleMember_appxUser");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AppxUserRoleMember)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_appxUser_RoleMember_appxUser_Role");
            });

            modelBuilder.Entity<AspnetApplications>(entity =>
            {
                entity.HasKey(e => e.ApplicationId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("aspnet_Applications");

                entity.HasIndex(e => e.ApplicationName)
                    .HasName("UQ__aspnet_Applicati__5EDF0F2E")
                    .IsUnique();

                entity.HasIndex(e => e.LoweredApplicationName)
                    .HasName("aspnet_Applications_Index")
                    .ForSqlServerIsClustered();

                entity.Property(e => e.ApplicationId).ValueGeneratedNever();

                entity.Property(e => e.ApplicationName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Description).HasMaxLength(256);

                entity.Property(e => e.LoweredApplicationName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<AspnetMembership>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("aspnet_Membership");

                entity.HasIndex(e => new { e.ApplicationId, e.LoweredEmail })
                    .HasName("aspnet_Membership_index")
                    .ForSqlServerIsClustered();

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Comment).HasColumnType("ntext");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FailedPasswordAnswerAttemptWindowStart).HasColumnType("datetime");

                entity.Property(e => e.FailedPasswordAttemptWindowStart).HasColumnType("datetime");

                entity.Property(e => e.LastLockoutDate).HasColumnType("datetime");

                entity.Property(e => e.LastLoginDate).HasColumnType("datetime");

                entity.Property(e => e.LastPasswordChangedDate).HasColumnType("datetime");

                entity.Property(e => e.LoweredEmail).HasMaxLength(256);

                entity.Property(e => e.MobilePin)
                    .HasColumnName("MobilePIN")
                    .HasMaxLength(16);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.PasswordAnswer).HasMaxLength(128);

                entity.Property(e => e.PasswordQuestion).HasMaxLength(256);

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.AspnetMembership)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__aspnet_Me__Appli__72E607DB");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.AspnetMembership)
                    .HasForeignKey<AspnetMembership>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__aspnet_Me__UserI__73DA2C14");
            });

            modelBuilder.Entity<AspnetPaths>(entity =>
            {
                entity.HasKey(e => e.PathId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("aspnet_Paths");

                entity.HasIndex(e => new { e.ApplicationId, e.LoweredPath })
                    .HasName("aspnet_Paths_index")
                    .IsUnique()
                    .ForSqlServerIsClustered();

                entity.Property(e => e.PathId).ValueGeneratedNever();

                entity.Property(e => e.LoweredPath)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.AspnetPaths)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__aspnet_Pa__Appli__247D636F");
            });

            modelBuilder.Entity<AspnetPersonalizationAllUsers>(entity =>
            {
                entity.HasKey(e => e.PathId);

                entity.ToTable("aspnet_PersonalizationAllUsers");

                entity.Property(e => e.PathId).ValueGeneratedNever();

                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.PageSettings)
                    .IsRequired()
                    .HasColumnType("image");

                entity.HasOne(d => d.Path)
                    .WithOne(p => p.AspnetPersonalizationAllUsers)
                    .HasForeignKey<AspnetPersonalizationAllUsers>(d => d.PathId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__aspnet_Pe__PathI__2A363CC5");
            });

            modelBuilder.Entity<AspnetPersonalizationPerUser>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("aspnet_PersonalizationPerUser");

                entity.HasIndex(e => new { e.PathId, e.UserId })
                    .HasName("aspnet_PersonalizationPerUser_index1")
                    .IsUnique()
                    .ForSqlServerIsClustered();

                entity.HasIndex(e => new { e.UserId, e.PathId })
                    .HasName("aspnet_PersonalizationPerUser_ncindex2")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.PageSettings)
                    .IsRequired()
                    .HasColumnType("image");

                entity.HasOne(d => d.Path)
                    .WithMany(p => p.AspnetPersonalizationPerUser)
                    .HasForeignKey(d => d.PathId)
                    .HasConstraintName("FK__aspnet_Pe__PathI__2E06CDA9");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspnetPersonalizationPerUser)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__aspnet_Pe__UserI__2EFAF1E2");
            });

            modelBuilder.Entity<AspnetProfile>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("aspnet_Profile");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.PropertyNames)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.Property(e => e.PropertyValuesBinary)
                    .IsRequired()
                    .HasColumnType("image");

                entity.Property(e => e.PropertyValuesString)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.AspnetProfile)
                    .HasForeignKey<AspnetProfile>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__aspnet_Pr__UserI__07E124C1");
            });

            modelBuilder.Entity<AspnetRoles>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("aspnet_Roles");

                entity.HasIndex(e => new { e.ApplicationId, e.LoweredRoleName })
                    .HasName("aspnet_Roles_index1")
                    .IsUnique()
                    .ForSqlServerIsClustered();

                entity.Property(e => e.RoleId).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(256);

                entity.Property(e => e.LoweredRoleName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.AspnetRoles)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__aspnet_Ro__Appli__116A8EFB");
            });

            modelBuilder.Entity<AspnetSchemaVersions>(entity =>
            {
                entity.HasKey(e => new { e.Feature, e.CompatibleSchemaVersion });

                entity.ToTable("aspnet_SchemaVersions");

                entity.Property(e => e.Feature).HasMaxLength(128);

                entity.Property(e => e.CompatibleSchemaVersion).HasMaxLength(128);
            });

            modelBuilder.Entity<AspnetUsers>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("aspnet_Users");

                entity.HasIndex(e => new { e.ApplicationId, e.LastActivityDate })
                    .HasName("aspnet_Users_Index2");

                entity.HasIndex(e => new { e.ApplicationId, e.LoweredUserName })
                    .HasName("aspnet_Users_Index")
                    .IsUnique()
                    .ForSqlServerIsClustered();

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.LastActivityDate).HasColumnType("datetime");

                entity.Property(e => e.LoweredUserName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.MobileAlias).HasMaxLength(16);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.AspnetUsers)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__aspnet_Us__Appli__62AFA012");
            });

            modelBuilder.Entity<AspnetUsersInRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.ToTable("aspnet_UsersInRoles");

                entity.HasIndex(e => e.RoleId)
                    .HasName("aspnet_UsersInRoles_index");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspnetUsersInRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__aspnet_Us__RoleI__162F4418");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspnetUsersInRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__aspnet_Us__UserI__153B1FDF");
            });

            modelBuilder.Entity<AspnetWebEventEvents>(entity =>
            {
                entity.HasKey(e => e.EventId);

                entity.ToTable("aspnet_WebEvent_Events");

                entity.Property(e => e.EventId)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.ApplicationPath).HasMaxLength(256);

                entity.Property(e => e.ApplicationVirtualPath).HasMaxLength(256);

                entity.Property(e => e.Details).HasColumnType("ntext");

                entity.Property(e => e.EventOccurrence).HasColumnType("decimal(19, 0)");

                entity.Property(e => e.EventSequence).HasColumnType("decimal(19, 0)");

                entity.Property(e => e.EventTime).HasColumnType("datetime");

                entity.Property(e => e.EventTimeUtc).HasColumnType("datetime");

                entity.Property(e => e.EventType)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.ExceptionType).HasMaxLength(256);

                entity.Property(e => e.MachineName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Message).HasMaxLength(1024);

                entity.Property(e => e.RequestUrl).HasMaxLength(1024);
            });

            modelBuilder.Entity<LocalLiveEvent>(entity =>
            {
                entity.HasKey(e => e.EventId);

                entity.ToTable("locallive_Event");

                entity.Property(e => e.LastUpdatedUTC).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.EventStatus)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.VenueFullName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.VenueShortName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.VarsityLevel)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.GenderText)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SchoolHomeName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SchoolAwayName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LiveMediaID)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ArchiveMediaID)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ThumbnailUrl)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TournamentName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<NjspBridgeServiceLog>(entity =>
            {
                entity.HasKey(e => e.BridgeMsgId);

                entity.ToTable("njsp_BridgeServiceLog");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.MessageTypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MsmqmsgId)
                    .IsRequired()
                    .HasColumnName("MSMQMsgId")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Uid).HasColumnName("UID");
            });

            modelBuilder.Entity<VsandBook>(entity =>
            {
                entity.HasKey(e => e.BookId);

                entity.ToTable("vsand_Book");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.VsandBook)
                    .HasForeignKey(d => d.CreatedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_Book_appxUser");

                entity.HasOne(d => d.ScheduleYear)
                    .WithMany(p => p.VsandBook)
                    .HasForeignKey(d => d.ScheduleYearId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_Book_vsand_ScheduleYear");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.VsandBook)
                    .HasForeignKey(d => d.SportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_Book_vsand_Sport");
            });

            modelBuilder.Entity<VsandBookFav>(entity =>
            {
                entity.HasKey(e => e.BookFavId);

                entity.ToTable("vsand_BookFav");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.VsandBookFav)
                    .HasForeignKey(d => d.AdminId)
                    .HasConstraintName("FK_vsand_BookFav_appxUser");

                entity.HasOne(d => d.School)
                    .WithMany(p => p.VsandBookFav)
                    .HasForeignKey(d => d.SchoolId)
                    .HasConstraintName("FK_vsand_BookFav_vsand_School");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.VsandBookFav)
                    .HasForeignKey(d => d.SportId)
                    .HasConstraintName("FK_vsand_BookFav_vsand_Sport");
            });

            modelBuilder.Entity<VsandBookMember>(entity =>
            {
                entity.HasKey(e => e.BookMemberId);

                entity.ToTable("vsand_BookMember");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.VsandBookMember)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK_vsand_BookMember_vsand_Book");

                entity.HasOne(d => d.School)
                    .WithMany(p => p.VsandBookMember)
                    .HasForeignKey(d => d.SchoolId)
                    .HasConstraintName("FK_vsand_BookMember_vsand_School");
            });

            modelBuilder.Entity<VsandBookNote>(entity =>
            {
                entity.HasKey(e => e.NoteId);

                entity.ToTable("vsand_BookNote");

                entity.HasIndex(e => new { e.GameReportId, e.TeamId })
                    .HasName("missing_index_70082_70081_vsand_BookNote");

                entity.HasIndex(e => new { e.ScheduleYearId, e.SportId, e.GameReportId, e.TeamId, e.PlayerId, e.BookId })
                    .HasName("idx_vsand_BookNote_Speed1");

                entity.Property(e => e.Note).IsUnicode(false);

                entity.Property(e => e.NoteBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NoteDate).HasColumnType("datetime");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.VsandBookNote)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_BookNote_vsand_Book");

                entity.HasOne(d => d.GameReport)
                    .WithMany(p => p.BookNotes)
                    .HasForeignKey(d => d.GameReportId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_vsand_BookNote_vsand_GameReport");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.VsandBookNote)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_vsand_BookNote_vsand_Player");

                entity.HasOne(d => d.ScheduleYear)
                    .WithMany(p => p.VsandBookNote)
                    .HasForeignKey(d => d.ScheduleYearId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_BookNote_vsand_ScheduleYear");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.VsandBookNote)
                    .HasForeignKey(d => d.SportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_BookNote_vsand_Sport");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.VsandBookNote)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_vsand_BookNote_vsand_Team");
            });

            modelBuilder.Entity<VsandBookSubscription>(entity =>
            {
                entity.HasKey(e => e.SubscriptionId);

                entity.ToTable("vsand_BookSubscription");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.VsandBookSubscription)
                    .HasForeignKey(d => d.AdminId)
                    .HasConstraintName("FK_vsand_BookSubscription_appxUser");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.VsandBookSubscription)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK_vsand_BookSubscription_vsand_Book");
            });

            modelBuilder.Entity<VsandConference>(entity =>
            {
                entity.HasKey(e => e.ConferenceId);

                entity.ToTable("vsand_Conference");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VsandCounty>(entity =>
            {
                entity.HasKey(e => e.CountyId);

                entity.ToTable("vsand_County");

                entity.Property(e => e.CountyId).HasColumnName("CountyID");

                entity.Property(e => e.CountyAbbr)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VsandEdition>(entity =>
            {
                entity.HasKey(e => e.EditionId);

                entity.ToTable("vsand_Edition");

                entity.Property(e => e.EditionId).HasColumnName("EditionID");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VsandEntitySlug>(entity =>
            {
                entity.HasKey(e => e.Slug);
                entity.ToTable("vsand_EntitySlug");
                entity.Property(e => e.EntityType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EntityId)
                    .IsRequired();
            });

            modelBuilder.Entity<VsandFeedSubscription>(entity =>
            {
                entity.HasKey(e => e.FeedSubscriptionId);

                entity.ToTable("vsand_FeedSubscription");

                entity.Property(e => e.FeedSubscriptionId).HasColumnName("FeedSubscriptionID");

                entity.Property(e => e.FeedType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SubscriptionKey)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SubscriptionScope)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.ScopeType)
                    .WithMany(p => p.VsandFeedSubscription)
                    .HasForeignKey(d => d.ScopeTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_FeedSubscription_vsand_FeedSubscriptionScopeType");
            });

            modelBuilder.Entity<VsandFeedSubscriptionScope>(entity =>
            {
                entity.HasKey(e => e.ScopeId);

                entity.ToTable("vsand_FeedSubscriptionScope");

                entity.HasOne(d => d.FeedSubscription)
                    .WithMany(p => p.VsandFeedSubscriptionScope)
                    .HasForeignKey(d => d.FeedSubscriptionId)
                    .HasConstraintName("FK_vsand_FeedSubscriptionScope_vsand_FeedSubscription");

                entity.HasOne(d => d.ScopeType)
                    .WithMany(p => p.VsandFeedSubscriptionScope)
                    .HasForeignKey(d => d.ScopeTypeId)
                    .HasConstraintName("FK_vsand_FeedSubscriptionScope_vsand_FeedSubscriptionScopeType");
            });

            modelBuilder.Entity<VsandFeedSubscriptionScopeDefinition>(entity =>
            {
                entity.HasKey(e => e.SubscriptionSchoolId);

                entity.ToTable("vsand_FeedSubscriptionScopeDefinition");

                entity.HasOne(d => d.FeedSubscription)
                    .WithMany(p => p.VsandFeedSubscriptionScopeDefinition)
                    .HasForeignKey(d => d.FeedSubscriptionId)
                    .HasConstraintName("FK_vsand_FeedSubscriptionScopeDefinition_vsand_FeedSubscription");

                entity.HasOne(d => d.School)
                    .WithMany(p => p.FeedSubscriptions)
                    .HasForeignKey(d => d.SchoolId)
                    .HasConstraintName("FK_vsand_FeedSubscriptionScopeDefinition_vsand_School");
            });

            modelBuilder.Entity<VsandFeedSubscriptionScopeType>(entity =>
            {
                entity.HasKey(e => e.ScopeTypeId);

                entity.ToTable("vsand_FeedSubscriptionScopeType");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VsandGamePackage>(entity =>
            {
                entity.HasKey(e => e.GamePackageId);

                entity.ToTable("vsand_GamePackage");

                entity.HasIndex(e => e.GameReportId)
                    .HasName("idx_vsand_GamePackage1");

                entity.HasIndex(e => e.StoryId)
                    .HasName("idx_vsand_GamePackage3");

                entity.HasIndex(e => new { e.CreatedById, e.CreatedDate })
                    .HasName("missing_index_172658_172657_vsand_GamePackage");

                entity.HasIndex(e => new { e.PublicationStoryId, e.StoryId, e.PublicationId })
                    .HasName("idx_vsand_GamePackage2");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FileName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FormattedStory)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.GameReportData)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.VsandGamePackage)
                    .HasForeignKey(d => d.CreatedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_GamePackage_appxUser");

                entity.HasOne(d => d.GameReport)
                    .WithMany(p => p.GamePackages)
                    .HasForeignKey(d => d.GameReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_GamePackage_vsand_GameReport");

                entity.HasOne(d => d.Publication)
                    .WithMany(p => p.VsandGamePackage)
                    .HasForeignKey(d => d.PublicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_GamePackage_vsand_Publication");

                entity.HasOne(d => d.Story)
                    .WithMany(p => p.VsandGamePackage)
                    .HasForeignKey(d => d.StoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_GamePackage_vsand_GamePackage");
            });

            modelBuilder.Entity<VsandGameReport>(entity =>
            {
                entity.HasKey(e => e.GameReportId);

                entity.ToTable("vsand_GameReport");

                entity.HasIndex(e => e.GameTypeId)
                    .HasName("idx_vsand_GameReport4");

                entity.HasIndex(e => e.RoundId)
                    .HasName("idx_vsand_GameReport1");

                entity.HasIndex(e => new { e.GameReportId, e.Archived })
                    .HasName("idx_vsand_GameReport_WithArchived1");

                entity.HasIndex(e => new { e.GameReportId, e.GameDate })
                    .HasName("<Name of Missing Index, sysname,>");

                entity.HasIndex(e => new { e.GameReportId, e.ScheduleYearId })
                    .HasName("idx_vsand_GameReport_ScheduleYearId");

                entity.HasIndex(e => new { e.GameReportId, e.SportId })
                    .HasName("idx_vsand_GameReport_WithSportId1");

                entity.HasIndex(e => new { e.ReportedDate, e.Source })
                    .HasName("idx_vsand_GameReport2");

                entity.HasIndex(e => new { e.GameDate, e.ReportedDate, e.SportId })
                    .HasName("missing_index_164593_164592_vsand_GameReport");

                entity.HasIndex(e => new { e.GameDate, e.SportId, e.Deleted })
                    .HasName("idx_vsand_GameReport3");

                entity.HasIndex(e => new { e.GameReportId, e.GameDate, e.SportId })
                    .HasName("missing_index_1012_1011_vsand_GameReport");

                entity.HasIndex(e => new { e.SportId, e.Source, e.ReportedDate })
                    .HasName("missing_index_709_708_vsand_GameReport");

                entity.HasIndex(e => new { e.GameDate, e.GameTypeId, e.SportId, e.Deleted })
                    .HasName("missing_index_2183_2182_vsand_GameReport");

                entity.HasIndex(e => new { e.GameReportId, e.ScheduleYearId, e.Deleted, e.GameDate })
                    .HasName("idx_vsand_GameReport_StatFilterSpeed");

                entity.HasIndex(e => new { e.GameReportId, e.SportId, e.ScheduleYearId, e.Deleted })
                    .HasName("missing_index_119650_119649_vsand_GameReport");

                entity.HasIndex(e => new { e.Name, e.GameDate, e.RoundId, e.SectionId, e.GroupId, e.LocationName, e.LocationCity, e.LocationState, e.ReportedBy, e.ReportedByName, e.ReportedDate, e.Source, e.TriPlus, e.Archived, e.Locked, e.CountyId, e.SportId, e.Deleted, e.GameReportId, e.GameTypeId, e.ScheduleYearId })
                    .HasName("_dta_index_vsand_GameReport_17_309576141__K13_K4_K21_K1_K6_K5_2_3_7_8_9_10_11_12_14_15_16_17_18_19_20");

                entity.Property(e => e.GameReportId).HasColumnName("GameReportID");

                entity.Property(e => e.GameDate).HasColumnType("datetime");

                entity.Property(e => e.LocationCity)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LocationName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LocationState)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.PPEligible).HasColumnName("PPEligible");

                entity.Property(e => e.ReportedByName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReportedDate).HasColumnType("datetime");

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.County)
                    .WithMany(p => p.GameReports)
                    .HasForeignKey(d => d.CountyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_GameReport_vsand_County");

                entity.HasOne(d => d.EventType)
                    .WithMany(p => p.GameReports)
                    .HasForeignKey(d => d.GameTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_GameReport_vsand_SportEventType");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.VsandGameReport)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_vsand_GameReport_vsand_SportEventTypeGroup");

                entity.HasOne(d => d.ReportedByUser)
                    .WithMany(p => p.VsandGameReport)
                    .HasForeignKey(d => d.ReportedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_GameReport_appxUser");

                entity.HasOne(d => d.Round)
                    .WithMany(p => p.VsandGameReport)
                    .HasForeignKey(d => d.RoundId)
                    .HasConstraintName("FK_vsand_GameReport_vsand_SportEventTypeRound");

                entity.HasOne(d => d.ScheduleYear)
                    .WithMany(p => p.VsandGameReport)
                    .HasForeignKey(d => d.ScheduleYearId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_GameReport_vsand_ScheduleYear");

                entity.HasOne(d => d.Section)
                    .WithMany(p => p.GameReports)
                    .HasForeignKey(d => d.SectionId)
                    .HasConstraintName("FK_vsand_GameReport_vsand_SportEventTypeSection");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.VsandGameReport)
                    .HasForeignKey(d => d.SportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_GameReport_vsand_Sport");
            });

            modelBuilder.Entity<VsandGameReportEmailLog>(entity =>
            {
                entity.HasKey(e => e.GameReportEmailId);

                entity.ToTable("vsand_GameReportEmailLog");

                entity.HasIndex(e => new { e.GameReportId, e.UserId })
                    .HasName("idx_vsand_GameReportEmailLog1");

                entity.Property(e => e.FromIp)
                    .IsRequired()
                    .HasColumnName("FromIP")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SendTo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SentAt).HasColumnType("datetime");

                entity.HasOne(d => d.GameReport)
                    .WithMany(p => p.EmailLog)
                    .HasForeignKey(d => d.GameReportId)
                    .HasConstraintName("FK_vsand_GameReportEmailLog_vsand_GameReport");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.VsandGameReportEmailLog)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_GameReportEmailLog_appxUser");
            });

            modelBuilder.Entity<VsandGameReportEvent>(entity =>
            {
                entity.HasKey(e => e.EventId);

                entity.ToTable("vsand_GameReportEvent");

                entity.HasIndex(e => new { e.EventId, e.GameReportId })
                    .HasName("idx_vsand_GameReportEvent_AggQuery1");

                entity.Property(e => e.RoundName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.GameReport)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.GameReportId)
                    .HasConstraintName("FK_vsand_GameReportEvent_vsand_GameReport");

                entity.HasOne(d => d.SportEvent)
                    .WithMany(p => p.GameReportEvents)
                    .HasForeignKey(d => d.SportEventId)
                    .HasConstraintName("FK_vsand_GameReportEvent_vsand_SportEvent");
            });

            modelBuilder.Entity<VsandGameReportEventPlayer>(entity =>
            {
                entity.HasKey(e => e.EventPlayerId);

                entity.ToTable("vsand_GameReportEventPlayer");

                entity.HasIndex(e => e.PlayerId)
                    .HasName("idx_vsand_GameReportEventPlayer2");

                entity.HasIndex(e => new { e.GameReportTeamId, e.PlayerId })
                    .HasName("idx_vsand_GameReportEventPlayer1");

                entity.HasIndex(e => new { e.EventPlayerId, e.PlayerId, e.EventResultId })
                    .HasName("idx_vsand_GameReportEventPlayer_AggQuery1");

                entity.HasOne(d => d.EventResult)
                    .WithMany(p => p.EventPlayers)
                    .HasForeignKey(d => d.EventResultId)
                    .HasConstraintName("FK_vsand_GameReportEventPlayer_vsand_GameReportEventResult");

                entity.HasOne(d => d.GameReportTeam)
                    .WithMany(p => p.EventPlayers)
                    .HasForeignKey(d => d.GameReportTeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_GameReportEventPlayer_vsand_GameReportTeam");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.GameReportEventPlayer)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_vsand_GameReportEventPlayer_vsand_Player");
            });

            modelBuilder.Entity<VsandGameReportEventPlayerGroup>(entity =>
            {
                entity.HasKey(e => e.PlayerGroupId);

                entity.ToTable("vsand_GameReportEventPlayerGroup");

                entity.HasIndex(e => new { e.PlayerGroupId, e.EventResultId })
                    .HasName("vsand_GameReportEventPlayerGroup1");

                entity.HasIndex(e => new { e.PlayerGroupId, e.GameReportTeamId })
                    .HasName("idx_vsand_GameReportEventPlayerGroup2");

                entity.HasOne(d => d.EventResult)
                    .WithMany(p => p.EventPlayerGroups)
                    .HasForeignKey(d => d.EventResultId)
                    .HasConstraintName("FK_vsand_GameReportEventPlayerGroup_vsand_GameReportEventResult");

                entity.HasOne(d => d.GameReportTeam)
                    .WithMany(p => p.EventPlayerGroups)
                    .HasForeignKey(d => d.GameReportTeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_GameReportEventPlayerGroup_vsand_GameReportTeam");
            });

            modelBuilder.Entity<VsandGameReportEventPlayerGroupPlayer>(entity =>
            {
                entity.HasKey(e => e.PlayerGroupPlayerId);

                entity.ToTable("vsand_GameReportEventPlayerGroupPlayer");

                entity.HasIndex(e => e.PlayerGroupId)
                    .HasName("idx_vsand_GameReportEventPlayerGroupPlayer1");

                entity.HasIndex(e => e.PlayerId)
                    .HasName("idx_vsand_GameReportEventPlayerGroup1");

                entity.HasOne(d => d.PlayerGroup)
                    .WithMany(p => p.EventPlayerGroupPlayers)
                    .HasForeignKey(d => d.PlayerGroupId)
                    .HasConstraintName("FK_vsand_GameReportEventPlayerGroupPlayer_vsand_GameReportEventPlayerGroup");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.GameReportEventPlayerGroupPlayer)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_vsand_GameReportEventPlayerGroupPlayer_vsand_Player");
            });

            modelBuilder.Entity<VsandGameReportEventPlayerGroupStat>(entity =>
            {
                entity.HasKey(e => e.PlayerGroupStatId);

                entity.ToTable("vsand_GameReportEventPlayerGroupStat");

                entity.HasIndex(e => new { e.PlayerGroupStatId, e.StatId, e.StatValue, e.PlayerGroupId })
                    .HasName("idx_vsand_GameReportEventPlayerGroupStat1");

                entity.HasOne(d => d.EventPlayerGroup)
                    .WithMany(p => p.GameReportEventPlayerGroupStats)
                    .HasForeignKey(d => d.PlayerGroupId)
                    .HasConstraintName("FK_vsand_GameReportEventPlayerGroupStat_vsand_GameReportEventPlayerGroup");

                entity.HasOne(d => d.SportEventStat)
                    .WithMany(p => p.EventPlayerGroupStats)
                    .HasForeignKey(d => d.StatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_GameReportEventPlayerGroupStat_vsand_SportEventStat");
            });

            modelBuilder.Entity<VsandGameReportEventPlayerStat>(entity =>
            {
                entity.HasKey(e => e.EventPlayerStatId);

                entity.ToTable("vsand_GameReportEventPlayerStat");

                entity.HasIndex(e => e.EventPlayerId)
                    .HasName("idx_vsand_GameReportEventPlayerStat1");

                entity.HasIndex(e => new { e.EventPlayerId, e.StatValue, e.StatId })
                    .HasName("idx_vsand_GameReportEventPlayerStat_AggQuerySpeed1");

                entity.HasOne(d => d.EventPlayer)
                    .WithMany(p => p.GameReportEventPlayerStats)
                    .HasForeignKey(d => d.EventPlayerId)
                    .HasConstraintName("FK_vsand_GameReportEventPlayerStat_vsand_GameReportEventPlayer");

                entity.HasOne(d => d.SportEventStat)
                    .WithMany(p => p.EventPlayerStats)
                    .HasForeignKey(d => d.StatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_GameReportEventPlayerStat_vsand_SportEventStat");
            });

            modelBuilder.Entity<VsandGameReportEventResult>(entity =>
            {
                entity.HasKey(e => e.EventResultId);

                entity.ToTable("vsand_GameReportEventResult");

                entity.HasIndex(e => new { e.EventResultId, e.EventId })
                    .HasName("idx_vsand_GameReportEventResult_AggQuery1");

                entity.Property(e => e.Duration)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Overtime)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ResultType)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.GameReportEvent)
                    .WithMany(p => p.Results)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK_vsand_GameReportEventResult_vsand_GameReportEvent");
            });

            modelBuilder.Entity<VsandGameReportMeta>(entity =>
            {
                entity.HasKey(e => e.GameReportMetaId);

                entity.ToTable("vsand_GameReportMeta");

                entity.HasIndex(e => e.GameReportId)
                    .HasName("idx_vsand_GameReportMeta1");

                entity.Property(e => e.MetaValue)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.GameReport)
                    .WithMany(p => p.Meta)
                    .HasForeignKey(d => d.GameReportId)
                    .HasConstraintName("FK_vsand_GameReportMeta_vsand_GameReport");

                entity.HasOne(d => d.SportGameMeta)
                    .WithMany(p => p.VsandGameReportMeta)
                    .HasForeignKey(d => d.SportGameMetaId)
                    .HasConstraintName("FK_vsand_GameReportMeta_vsand_SportGameMeta");
            });

            modelBuilder.Entity<VsandGameReportNote>(entity =>
            {
                entity.HasKey(e => e.NoteId);

                entity.ToTable("vsand_GameReportNote");

                entity.HasIndex(e => e.GameReportId)
                    .HasName("idx_vsand_GameReportNote1");

                entity.HasIndex(e => e.NoteById)
                    .HasName("missing_index_1856_1855_vsand_GameReportNote");

                entity.Property(e => e.Note).IsUnicode(false);

                entity.Property(e => e.NoteBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NoteDate).HasColumnType("datetime");

                entity.HasOne(d => d.GameReport)
                    .WithMany(p => p.Notes)
                    .HasForeignKey(d => d.GameReportId)
                    .HasConstraintName("FK_vsand_GameReportNote_vsand_GameReport");

                entity.HasOne(d => d.NoteByNavigation)
                    .WithMany(p => p.VsandGameReportNote)
                    .HasForeignKey(d => d.NoteById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_GameReportNote_appxUser");
            });

            modelBuilder.Entity<VsandGameReportPairing>(entity =>
            {
                entity.HasKey(e => e.PairingId);

                entity.ToTable("vsand_GameReportPairing");

                entity.HasOne(d => d.GameReport)
                    .WithMany(p => p.Pairings)
                    .HasForeignKey(d => d.GameReportId)
                    .HasConstraintName("FK_vsand_GameReportPairing_vsand_GameReport");
            });

            modelBuilder.Entity<VsandGameReportPairingTeam>(entity =>
            {
                entity.HasKey(e => e.PairingTeamId);

                entity.ToTable("vsand_GameReportPairingTeam");

                entity.HasOne(d => d.GameReportTeam)
                    .WithMany(p => p.VsandGameReportPairingTeam)
                    .HasForeignKey(d => d.GameReportTeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_GameReportPairingTeam_vsand_GameReportTeam");

                entity.HasOne(d => d.Pairing)
                    .WithMany(p => p.VsandGameReportPairingTeam)
                    .HasForeignKey(d => d.PairingId)
                    .HasConstraintName("FK_vsand_GameReportPairingTeam_vsand_GameReportPairing");
            });

            modelBuilder.Entity<VsandGameReportPeriodScore>(entity =>
            {
                entity.HasKey(e => e.PeriodScoreId);

                entity.ToTable("vsand_GameReportPeriodScore");

                entity.HasIndex(e => new { e.PeriodScoreId, e.PeriodNumber, e.IsOtperiod, e.Score, e.ScoreSpecial, e.IsSoperiod, e.GameReportTeamId })
                    .HasName("idx_vsand_GameReportPeriodScore1");

                entity.Property(e => e.IsOtperiod).HasColumnName("IsOTPeriod");

                entity.Property(e => e.IsSoperiod).HasColumnName("IsSOPeriod");

                entity.Property(e => e.ScoreSpecial)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.GameReportTeam)
                    .WithMany(p => p.PeriodScores)
                    .HasForeignKey(d => d.GameReportTeamId)
                    .HasConstraintName("FK_vsand_GameReportPeriodScore_vsand_GameReportTeam");
            });

            modelBuilder.Entity<VsandGameReportPlayByPlay>(entity =>
            {
                entity.HasKey(e => e.PlayByPlayId);

                entity.ToTable("vsand_GameReportPlayByPlay");

                entity.HasIndex(e => new { e.PlayByPlayId, e.PeriodNumber, e.SortOrder, e.ObjectData, e.FormattedText, e.GameReportId })
                    .HasName("idx_vsand_GameReportPlayByPlay_speed1");

                entity.Property(e => e.FormattedText)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectData)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.HasOne(d => d.GameReport)
                    .WithMany(p => p.ScoringPlays)
                    .HasForeignKey(d => d.GameReportId)
                    .HasConstraintName("FK_vsand_GameReportPlayByPlay_vsand_GameReport");
            });

            modelBuilder.Entity<VsandGameReportPlayerStat>(entity =>
            {
                entity.HasKey(e => e.PlayerStatId);

                entity.ToTable("vsand_GameReportPlayerStat");

                entity.HasIndex(e => new { e.GameReportId, e.PlayerId, e.StatValue })
                    .HasName("idx_vsand_GameReportPlayerStat_PlayerStatValueExists");

                entity.HasIndex(e => new { e.GameReportId, e.StatId, e.PlayerId })
                    .HasName("idx_vsand_GameReportPlayerStat1");

                entity.HasIndex(e => new { e.StatId, e.PlayerId, e.StatValue })
                    .HasName("missing_index_3813_3812_vsand_GameReportPlayerStat");

                entity.HasIndex(e => new { e.GameReportId, e.PlayerId, e.StatValue, e.StatId })
                    .HasName("idx_vsand_GameReportPlayerStatSummaryData");

                entity.HasIndex(e => new { e.StatId, e.GameReportId, e.PlayerId, e.StatValue })
                    .HasName("vsand_GameReportPlayerStatValueSpeed");

                entity.HasOne(d => d.GameReport)
                    .WithMany(p => p.PlayerStats)
                    .HasForeignKey(d => d.GameReportId)
                    .HasConstraintName("FK_vsand_GameReportPlayerStat_vsand_GameReport");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.GameReportPlayerStats)
                    .HasForeignKey(d => d.PlayerId)
                    .HasConstraintName("FK_vsand_GameReportPlayerStat_vsand_Player");

                entity.HasOne(d => d.SportPlayerStat)
                    .WithMany(p => p.GameReportPlayerStats)
                    .HasForeignKey(d => d.StatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_GameReportPlayerStat_vsand_SportPlayerStat");
            });

            modelBuilder.Entity<VsandGameReportRoster>(entity =>
            {
                entity.HasKey(e => e.GameReportRosterId);

                entity.ToTable("vsand_GameReportRoster");

                entity.HasIndex(e => e.PlayerId)
                    .HasName("idx_vsand_GameReportRoster_PlayerSearch");

                entity.HasIndex(e => new { e.GameReportRosterId, e.PlayerId, e.PositionId, e.Starter, e.RosterOrder, e.PlayerOfRecord, e.RecordWins, e.RecordLosses, e.RecordTies, e.GameReportTeamId })
                    .HasName("vsand_GameReportRosterSpeed1");

                entity.HasOne(d => d.GameReportTeam)
                    .WithMany(p => p.GameRoster)
                    .HasForeignKey(d => d.GameReportTeamId)
                    .HasConstraintName("FK_vsand_GameReportRoster_vsand_GameReportTeam");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.GameReportRosters)
                    .HasForeignKey(d => d.PlayerId)
                    .HasConstraintName("FK_vsand_GameReportRoster_vsand_Player");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.VsandGameReportRoster)
                    .HasForeignKey(d => d.PositionId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_vsand_GameReportRoster_vsand_SportPosition");
            });

            modelBuilder.Entity<VsandGameReportTeam>(entity =>
            {
                entity.HasKey(e => e.GameReportTeamId);

                entity.ToTable("vsand_GameReportTeam");

                entity.HasIndex(e => e.TeamId)
                    .HasName("idx_vsand_GameReportTeam_ByTeamId1");

                entity.HasIndex(e => new { e.GameReportId, e.TeamId })
                    .HasName("_dta_index_vsand_GameReportTeam_17_1509580416__K2_K3");

                entity.Property(e => e.Abbreviation)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TeamName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.GameReport)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.GameReportId)
                    .HasConstraintName("FK_vsand_GameReportTeam_vsand_GameReport");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.GameReportEntries)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_GameReportTeam_vsand_Team");
            });

            modelBuilder.Entity<VsandGameReportTeamStat>(entity =>
            {
                entity.HasKey(e => e.TeamStatId);

                entity.ToTable("vsand_GameReportTeamStat");

                entity.HasIndex(e => e.GameReportTeamId)
                    .HasName("vsand_GameReportTeamStatSpeed1");

                entity.HasOne(d => d.GameReportTeam)
                    .WithMany(p => p.TeamStats)
                    .HasForeignKey(d => d.GameReportTeamId)
                    .HasConstraintName("FK_vsand_GameReportTeamStat_vsand_GameReportTeam");

                entity.HasOne(d => d.SportTeamStat)
                    .WithMany(p => p.VsandGameReportTeamStat)
                    .HasForeignKey(d => d.StatId)
                    .HasConstraintName("FK_vsand_GameReportTeamStat_vsand_SportTeamStat");
            });

            modelBuilder.Entity<VsandLeagueRule>(entity =>
            {
                entity.HasKey(e => e.LeagueRuleId);

                entity.ToTable("vsand_LeagueRule");

                entity.Property(e => e.Conference)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Division)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RuleType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.LeagueRules)
                    .HasForeignKey(d => d.SportId);

                entity.HasOne(d => d.ScheduleYear)
                    .WithMany(p => p.LeagueRules)
                    .HasForeignKey(d => d.ScheduleYearId);
            });

            modelBuilder.Entity<VsandLeagueRuleItem>(entity =>
            {
                entity.HasKey(e => new { e.LeagueRuleId, e.Conference, e.Division });

                entity.ToTable("vsand_LeagueRuleItem");

                entity.Property(e => e.Conference)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Division)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.LeagueRule)
                    .WithMany(p => p.RuleItems)
                    .HasForeignKey(d => d.LeagueRuleId)
                    .HasConstraintName("FK_vsand_LeagueRuleItem_vsand_LeagueRule");
            });

            modelBuilder.Entity<VsandLog>(entity =>
            {
                entity.ToTable("vsand_Log");

                entity.HasIndex(e => e.Acknowledged)
                    .HasName("missing_index_14470_14469_vsand_Log");

                entity.HasIndex(e => new { e.Id, e.ErrorLevel, e.Acknowledged })
                    .HasName("idx_vsand_Log1");

                entity.Property(e => e.AcknowledgedDate).HasColumnType("datetime");

                entity.Property(e => e.AcknowledgedUser)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.ErrorLevel)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Exception).IsUnicode(false);

                entity.Property(e => e.Logger)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.RequestUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Thread)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserIdentity)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VsandNews>(entity =>
            {
                entity.HasKey(e => e.NewsId);

                entity.ToTable("vsand_News");

                entity.HasIndex(e => new { e.Slug, e.Title, e.SportId, e.NewsTypeId, e.CreatedDate })
                    .HasName("idx_vsand_News1");

                entity.Property(e => e.AssignToDate).HasColumnType("datetime");

                entity.Property(e => e.AssignToName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ByLine)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedByName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.OnlineDate).HasColumnType("datetime");

                entity.Property(e => e.PubDate).HasColumnType("datetime");

                entity.Property(e => e.Slug)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StatusByName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StatusDate).HasColumnType("datetime");

                entity.Property(e => e.SubStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.AssignTo)
                    .WithMany(p => p.VsandNewsAssignTo)
                    .HasForeignKey(d => d.AssignToId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_News_appxUser1");

                entity.HasOne(d => d.CreatedBy)
                    .WithMany(p => p.VsandNewsCreatedBy)
                    .HasForeignKey(d => d.CreatedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_News_appxUser");

                entity.HasOne(d => d.NewsType)
                    .WithMany(p => p.VsandNews)
                    .HasForeignKey(d => d.NewsTypeId)
                    .HasConstraintName("FK_vsand_News_vsand_NewsType");

                entity.HasOne(d => d.Publication)
                    .WithMany(p => p.VsandNews)
                    .HasForeignKey(d => d.PublicationId)
                    .HasConstraintName("FK_vsand_News_vsand_Publication");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.VsandNews)
                    .HasForeignKey(d => d.SportId)
                    .HasConstraintName("FK_vsand_News_vsand_Sport");

                entity.HasOne(d => d.StatusBy)
                    .WithMany(p => p.VsandNewsStatusBy)
                    .HasForeignKey(d => d.StatusById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_News_appxUser2");
            });

            modelBuilder.Entity<VsandNewsPackage>(entity =>
            {
                entity.HasKey(e => e.NewsPackageId);

                entity.ToTable("vsand_NewsPackage");

                entity.HasIndex(e => e.NewsId)
                    .HasName("idx_vsand_NewsPackage_speed1");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FileName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FormattedStory)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.VsandNewsPackage)
                    .HasForeignKey(d => d.CreatedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_NewsPackage_appxUser");

                entity.HasOne(d => d.News)
                    .WithMany(p => p.VsandNewsPackage)
                    .HasForeignKey(d => d.NewsId)
                    .HasConstraintName("FK_vsand_NewsPackage_vsand_News");

                entity.HasOne(d => d.NewsStory)
                    .WithMany(p => p.VsandNewsPackage)
                    .HasForeignKey(d => d.NewsStoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_NewsPackage_vsand_NewsStory");
            });

            modelBuilder.Entity<VsandNewsStory>(entity =>
            {
                entity.HasKey(e => e.StoryId);

                entity.ToTable("vsand_NewsStory");

                entity.HasIndex(e => e.NewsId)
                    .HasName("idx_vsand_NewsStory1");

                entity.Property(e => e.AssignToName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectData).IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StatusByName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StatusDate).HasColumnType("datetime");

                entity.Property(e => e.Story).IsUnicode(false);

                entity.Property(e => e.SubStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.AssignTo)
                    .WithMany(p => p.VsandNewsStoryAssignTo)
                    .HasForeignKey(d => d.AssignToId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_NewsStory_appxUser");

                entity.HasOne(d => d.News)
                    .WithMany(p => p.VsandNewsStory)
                    .HasForeignKey(d => d.NewsId)
                    .HasConstraintName("FK_vsand_NewsStory_vsand_News");

                entity.HasOne(d => d.StatusBy)
                    .WithMany(p => p.VsandNewsStoryStatusBy)
                    .HasForeignKey(d => d.StatusById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_NewsStory_appxUser1");
            });

            modelBuilder.Entity<VsandNewsType>(entity =>
            {
                entity.HasKey(e => e.NewsTypeId);

                entity.ToTable("vsand_NewsType");

                entity.Property(e => e.AtomFormatter)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ControlPath)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.LegacyFormatter)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TypeName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VsandOptOut>(entity =>
            {
                entity.HasKey(e => e.EmailAddress);

                entity.ToTable("vsand_OptOut");

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Ipaddress)
                    .IsRequired()
                    .HasColumnName("IPAddress")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.OptOutDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<VsandPitchCountTracking>(entity =>
            {
                entity.HasKey(e => new { e.PlayerId, e.TeamId, e.GameDate, e.GameType });

                entity.ToTable("vsand_PitchCountTracking");

                entity.HasIndex(e => e.TrackingId)
                    .HasName("idx_pctTrackingId")
                    .IsUnique();

                entity.Property(e => e.GameDate).HasColumnType("datetime");

                entity.Property(e => e.Pit).HasColumnName("PIT");

                entity.Property(e => e.TrackingId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<VsandPlannerCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.ToTable("vsand_PlannerCategory");

                entity.Property(e => e.BodyBgColor)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.BodyColor)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TitleBgColor)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TitleColor)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Publication)
                    .WithMany(p => p.VsandPlannerCategory)
                    .HasForeignKey(d => d.PublicationId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_vsand_PlannerCategory_vsand_Publication");
            });

            modelBuilder.Entity<VsandPlannerDayBudget>(entity =>
            {
                entity.HasKey(e => e.BudgetId);

                entity.ToTable("vsand_PlannerDayBudget");

                entity.Property(e => e.PlannerDate).HasColumnType("datetime");

                entity.HasOne(d => d.Publication)
                    .WithMany(p => p.VsandPlannerDayBudget)
                    .HasForeignKey(d => d.PublicationId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_vsand_PlannerDayBudget_vsand_Publication");
            });

            modelBuilder.Entity<VsandPlannerLayout>(entity =>
            {
                entity.HasKey(e => e.LayoutId);

                entity.ToTable("vsand_PlannerLayout");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.VsandPlannerLayout)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_PlannerLayout_vsand_PlannerCategory");
            });

            modelBuilder.Entity<VsandPlannerNote>(entity =>
            {
                entity.HasKey(e => e.NoteId);

                entity.ToTable("vsand_PlannerNote");

                entity.HasIndex(e => new { e.Note, e.PlannerDate })
                    .HasName("idx_vsand_PlannerNote1");

                entity.Property(e => e.AddedDate).HasColumnType("datetime");

                entity.Property(e => e.Note).HasColumnType("varchar(max)");

                entity.Property(e => e.PlannerDate).HasColumnType("datetime");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.VsandPlannerNote)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_vsand_PlannerNote_vsand_PlannerCategory");

                entity.HasOne(d => d.Publication)
                    .WithMany(p => p.VsandPlannerNote)
                    .HasForeignKey(d => d.PublicationId)
                    .HasConstraintName("FK_vsand_PlannerNote_vsand_Publication");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.VsandPlannerNote)
                    .HasForeignKey(d => d.SportId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_vsand_PlannerNote_vsand_Sport");
            });

            modelBuilder.Entity<VsandPlanningCalendar>(entity =>
            {
                entity.HasKey(e => e.CalendarId);

                entity.ToTable("vsand_PlanningCalendar");

                entity.Property(e => e.DateQualifier)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.Entry)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.Publication)
                    .WithMany(p => p.VsandPlanningCalendar)
                    .HasForeignKey(d => d.PublicationId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_vsand_PlanningCalendar_vsand_Publication");
            });

            modelBuilder.Entity<VsandPlayer>(entity =>
            {
                entity.HasKey(e => e.PlayerId);

                entity.ToTable("vsand_Player");

                entity.HasIndex(e => e.Validated)
                    .HasName("missing_index_61517_61516_vsand_Player");

                entity.HasIndex(e => e.ValidatedBy)
                    .HasName("idx_vsand_Player1");

                entity.HasIndex(e => new { e.PlayerId, e.FirstName, e.LastName, e.SchoolId })
                    .HasName("idx_vsand_Player_InSchoolWithPlayerIdFNameAndLName1");

                entity.HasIndex(e => new { e.PlayerId, e.GraduationYear, e.Height, e.Weight, e.BirthDate, e.Active, e.SchoolId, e.Validated, e.ValidatedBy, e.ValidatedById, e.FirstName, e.LastName })
                    .HasName("idx_vsand_Player_ByFirstLastName");

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Height)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ValidatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VsandPlayerRecruiting>(entity =>
            {
                entity.HasKey(e => e.RecruitingId);

                entity.ToTable("vsand_PlayerRecruiting");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Notes).IsUnicode(false);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.UniversityName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.VsandPlayerRecruiting)
                    .HasForeignKey(d => d.PlayerId)
                    .HasConstraintName("FK_vsand_PlayerRecruiting_vsand_Player");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.VsandPlayerRecruiting)
                    .HasForeignKey(d => d.SportId)
                    .HasConstraintName("FK_vsand_PlayerRecruiting_vsand_Sport");
            });

            modelBuilder.Entity<VsandPlayerStatSummaryShim>(entity =>
            {
                entity.HasKey(e => e.PlayerId);

                entity.ToTable("vsand_PlayerStatSummaryShim");

                entity.Property(e => e.PlayerId).ValueGeneratedNever();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Team)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VsandPublication>(entity =>
            {
                entity.HasKey(e => e.PublicationId);

                entity.ToTable("vsand_Publication");

                entity.Property(e => e.PublicationId).HasColumnName("PublicationID");

                entity.Property(e => e.ArticleFormat)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CustomRoutingFlag)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FileRoutePrefix)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Ftpformat)
                    .HasColumnName("FTPFormat")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Ftppassword)
                    .HasColumnName("FTPPassword")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Ftpurl)
                    .HasColumnName("FTPURL")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Ftpusername)
                    .HasColumnName("FTPUsername")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PublicationCredit)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ScheduleFormat)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ScheduleProviderPage)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ScoreboardFormat)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ScoreboardProviderPage)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VsandPublicationEditionSubscription>(entity =>
            {
                entity.HasKey(e => e.PublicationEditionId);

                entity.ToTable("vsand_PublicationEditionSubscription");

                entity.Property(e => e.PublicationEditionId).HasColumnName("PublicationEditionID");

                entity.Property(e => e.EditionId).HasColumnName("EditionID");

                entity.Property(e => e.PublicationId).HasColumnName("PublicationID");

                entity.HasOne(d => d.Edition)
                    .WithMany(p => p.VsandPublicationEditionSubscription)
                    .HasForeignKey(d => d.EditionId)
                    .HasConstraintName("FK_vsand_PublicationEditionSubscription_vsand_Edition");

                entity.HasOne(d => d.Publication)
                    .WithMany(p => p.VsandPublicationEditionSubscription)
                    .HasForeignKey(d => d.PublicationId)
                    .HasConstraintName("FK_vsand_PublicationEditionSubscription_vsand_Publication");
            });

            modelBuilder.Entity<VsandPublicationFormat>(entity =>
            {
                entity.HasKey(e => e.FormatId);

                entity.ToTable("vsand_PublicationFormat");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FormatType)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VsandPublicationFormatVariable>(entity =>
            {
                entity.HasKey(e => e.FormatVariableId);

                entity.ToTable("vsand_PublicationFormatVariable");

                entity.Property(e => e.ValueType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VariableName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VariableValue)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Publication)
                    .WithMany(p => p.VsandPublicationFormatVariable)
                    .HasForeignKey(d => d.PublicationId)
                    .HasConstraintName("FK_vsand_PublicationFormatVariable_vsand_Publication");
            });

            modelBuilder.Entity<VsandPublicationFtp>(entity =>
            {
                entity.HasKey(e => e.PublicationFtpid);

                entity.ToTable("vsand_PublicationFTP");

                entity.Property(e => e.PublicationFtpid).HasColumnName("PublicationFTPId");

                entity.Property(e => e.Ftpformat)
                    .IsRequired()
                    .HasColumnName("FTPFormat")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Ftppassword)
                    .IsRequired()
                    .HasColumnName("FTPPassword")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Ftpurl)
                    .IsRequired()
                    .HasColumnName("FTPUrl")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Ftpusername)
                    .IsRequired()
                    .HasColumnName("FTPUsername")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Publication)
                    .WithMany(p => p.VsandPublicationFtp)
                    .HasForeignKey(d => d.PublicationId)
                    .HasConstraintName("FK_vsand_PublicationFTP_vsand_Publication");
            });

            modelBuilder.Entity<VsandPublicationRouteCode>(entity =>
            {
                entity.HasKey(e => e.PublicationRouteCodeId);

                entity.ToTable("vsand_PublicationRouteCode");

                entity.Property(e => e.PublicationRouteCodeId).HasColumnName("PublicationRouteCodeID");

                entity.Property(e => e.PublicationId).HasColumnName("PublicationID");

                entity.Property(e => e.RoutingCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Publication)
                    .WithMany(p => p.VsandPublicationRouteCode)
                    .HasForeignKey(d => d.PublicationId)
                    .HasConstraintName("FK_vsand_PublicationRouteCode_vsand_Publication");
            });

            modelBuilder.Entity<VsandPublicationSchool>(entity =>
            {
                entity.HasKey(e => e.PublicationSchoolId);

                entity.ToTable("vsand_PublicationSchool");

                entity.Property(e => e.PublicationSchoolId).HasColumnName("PublicationSchoolID");

                entity.Property(e => e.PublicationId).HasColumnName("PublicationID");

                entity.Property(e => e.SchoolId).HasColumnName("SchoolID");

                entity.HasOne(d => d.Publication)
                    .WithMany(p => p.VsandPublicationSchool)
                    .HasForeignKey(d => d.PublicationId)
                    .HasConstraintName("FK_vsand_PublicationSchool_vsand_Publication");

                entity.HasOne(d => d.School)
                    .WithMany(p => p.VsandPublicationSchool)
                    .HasForeignKey(d => d.SchoolId)
                    .HasConstraintName("FK_vsand_PublicationSchool_vsand_School");
            });

            modelBuilder.Entity<VsandPublicationSportSubscription>(entity =>
            {
                entity.HasKey(e => e.PubSportSubscriptionId);

                entity.ToTable("vsand_PublicationSportSubscription");

                entity.Property(e => e.PubSportSubscriptionId).HasColumnName("PubSportSubscriptionID");

                entity.Property(e => e.PublicationId).HasColumnName("PublicationID");

                entity.Property(e => e.SportAbbr)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SportId).HasColumnName("SportID");

                entity.Property(e => e.SubscriptionType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Format)
                    .WithMany(p => p.PublicationSportSubscriptions)
                    .HasForeignKey(d => d.FormatId)
                    .HasConstraintName("FK_vsand_PublicationSportSubscription_vsand_SystemFormat");

                entity.HasOne(d => d.Publication)
                    .WithMany(p => p.VsandPublicationSportSubscription)
                    .HasForeignKey(d => d.PublicationId)
                    .HasConstraintName("FK_vsand_PublicationSportSubscription_vsand_Publication");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.VsandPublicationSportSubscription)
                    .HasForeignKey(d => d.SportId)
                    .HasConstraintName("FK_vsand_PublicationSportSubscription_vsand_Sport");
            });

            modelBuilder.Entity<VsandPublicationStory>(entity =>
            {
                entity.HasKey(e => e.PublicationStoryId);

                entity.ToTable("vsand_PublicationStory");

                entity.HasIndex(e => new { e.GameReportId, e.AssignToId })
                    .HasName("idx_vsand_PublicationStory1");

                entity.HasIndex(e => new { e.PublicationId, e.GameReportId })
                    .HasName("idx_PubStoryWorksheetSpeed1");

                entity.Property(e => e.AssignToDate).HasColumnType("datetime");

                entity.Property(e => e.AssignToName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedByName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StatusBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StatusDate).HasColumnType("datetime");

                entity.Property(e => e.SubStatus)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.AssignedToUser)
                    .WithMany(p => p.VsandPublicationStoryAssignTo)
                    .HasForeignKey(d => d.AssignToId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_vsand_PublicationStory_appxUser2");

                entity.HasOne(d => d.CreatedBy)
                    .WithMany(p => p.VsandPublicationStoryCreatedBy)
                    .HasForeignKey(d => d.CreatedById)
                    .HasConstraintName("FK_vsand_PublicationStory_appxUser");

                entity.HasOne(d => d.GameReport)
                    .WithMany(p => p.PublicationStories)
                    .HasForeignKey(d => d.GameReportId)
                    .HasConstraintName("FK_vsand_PublicationStory_vsand_GameReport");

                entity.HasOne(d => d.Publication)
                    .WithMany(p => p.VsandPublicationStory)
                    .HasForeignKey(d => d.PublicationId)
                    .HasConstraintName("FK_vsand_PublicationStory_vsand_Publication");

                entity.HasOne(d => d.StatusByNavigation)
                    .WithMany(p => p.VsandPublicationStoryStatusByNavigation)
                    .HasForeignKey(d => d.StatusById)
                    .HasConstraintName("FK_vsand_PublicationStory_appxUser1");
            });

            modelBuilder.Entity<VsandPublicationStoryNote>(entity =>
            {
                entity.HasKey(e => e.NoteId);

                entity.ToTable("vsand_PublicationStoryNote");

                entity.Property(e => e.Note)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.NoteBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NoteDate).HasColumnType("datetime");

                entity.HasOne(d => d.NoteByNavigation)
                    .WithMany(p => p.VsandPublicationStoryNote)
                    .HasForeignKey(d => d.NoteById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_PublicationStoryNote_appxUser");

                entity.HasOne(d => d.PublicationStory)
                    .WithMany(p => p.VsandPublicationStoryNote)
                    .HasForeignKey(d => d.PublicationStoryId)
                    .HasConstraintName("FK_vsand_PublicationStoryNote_vsand_PublicationStory");
            });

            modelBuilder.Entity<VsandPublicationStoryPlayByPlay>(entity =>
            {
                entity.HasKey(e => e.PubStoryPlayByPlayId);

                entity.ToTable("vsand_PublicationStoryPlayByPlay");

                entity.HasIndex(e => e.PlayByPlayId)
                    .HasName("missing_index_850_849_vsand_PublicationStoryPlayByPlay");

                entity.HasIndex(e => e.PublicationStoryId)
                    .HasName("idx_vsand_PublicationStoryPlayByPlay1");

                entity.Property(e => e.FormattedText)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.GameReportPlayByPlay)
                    .WithMany(p => p.VsandPublicationStoryPlayByPlay)
                    .HasForeignKey(d => d.PlayByPlayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_PublicationStoryPlayByPlay_vsand_GameReportPlayByPlay");

                entity.HasOne(d => d.PublicationStory)
                    .WithMany(p => p.VsandPublicationStoryPlayByPlay)
                    .HasForeignKey(d => d.PublicationStoryId)
                    .HasConstraintName("FK_vsand_PublicationStoryPlayByPlay_vsand_PublicationStory");
            });

            modelBuilder.Entity<VsandRoundup>(entity =>
            {
                entity.HasKey(e => e.RoundupId);

                entity.ToTable("vsand_Roundup");

                entity.HasIndex(e => new { e.RoundupId, e.Archived })
                    .HasName("idx_vsand_Roundup_WithArchived1");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LeadStory).IsUnicode(false);

                entity.Property(e => e.Title1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title3)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Edition)
                    .WithMany(p => p.VsandRoundup)
                    .HasForeignKey(d => d.EditionId)
                    .HasConstraintName("FK_vsand_Roundup_vsand_Edition");

                entity.HasOne(d => d.RoundupFormatNavigation)
                    .WithMany(p => p.VsandRoundup)
                    .HasForeignKey(d => d.RoundupFormat)
                    .HasConstraintName("FK_vsand_Roundup_vsand_RoundupType");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.VsandRoundup)
                    .HasForeignKey(d => d.SportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_Roundup_vsand_Sport");
            });

            modelBuilder.Entity<VsandRoundupLeadStory>(entity =>
            {
                entity.HasKey(e => e.LeadStoryId);

                entity.ToTable("vsand_RoundupLeadStory");

                entity.HasIndex(e => e.RoundupId)
                    .HasName("idx_vsand_RoundupLeadStory1");

                entity.Property(e => e.Article).IsUnicode(false);

                entity.Property(e => e.ByLine)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.SourceLine)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.VsandRoundupLeadStory)
                    .HasForeignKey(d => d.CreatedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_RoundupLeadStory_appxUser");

                entity.HasOne(d => d.Roundup)
                    .WithMany(p => p.VsandRoundupLeadStory)
                    .HasForeignKey(d => d.RoundupId)
                    .HasConstraintName("FK_vsand_RoundupLeadStory_vsand_Roundup");
            });

            modelBuilder.Entity<VsandRoundupMember>(entity =>
            {
                entity.HasKey(e => e.RoundupMemberId);

                entity.ToTable("vsand_RoundupMember");

                entity.HasIndex(e => e.GamePackageId)
                    .HasName("idx_vsand_RoundupMember_Speed1");

                entity.HasIndex(e => e.RoundupId)
                    .HasName("idx_vsand_RoundupMember1");

                entity.HasOne(d => d.GamePackage)
                    .WithMany(p => p.VsandRoundupMember)
                    .HasForeignKey(d => d.GamePackageId)
                    .HasConstraintName("FK_vsand_RoundupMember_vsand_GamePackage");

                entity.HasOne(d => d.Roundup)
                    .WithMany(p => p.VsandRoundupMember)
                    .HasForeignKey(d => d.RoundupId)
                    .HasConstraintName("FK_vsand_RoundupMember_vsand_Roundup");
            });

            modelBuilder.Entity<VsandRoundupType>(entity =>
            {
                entity.HasKey(e => e.RoundupTypeId);

                entity.ToTable("vsand_RoundupType");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.VsandRoundupType)
                    .HasForeignKey(d => d.SportId)
                    .HasConstraintName("FK_vsand_RoundupType_vsand_Sport");
            });

            modelBuilder.Entity<VsandScheduleLoadFile>(entity =>
            {
                entity.HasKey(e => e.FileId);

                entity.ToTable("vsand_ScheduleLoadFile");

                entity.Property(e => e.FileId).HasColumnName("FileID");

                entity.Property(e => e.DateLoaded).HasColumnType("datetime");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(260)
                    .IsUnicode(false);

                entity.Property(e => e.FileType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ImportDate).HasColumnType("datetime");

                entity.Property(e => e.ScheduleYearId).HasColumnName("ScheduleYearID");

                entity.Property(e => e.SportId).HasColumnName("SportID");

                entity.HasOne(d => d.ScheduleYear)
                    .WithMany(p => p.VsandScheduleLoadFile)
                    .HasForeignKey(d => d.ScheduleYearId)
                    .HasConstraintName("FK_vsand_ScheduleLoadFile_vsand_ScheduleYear");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.VsandScheduleLoadFile)
                    .HasForeignKey(d => d.SportId)
                    .HasConstraintName("FK_vsand_ScheduleLoadFile_vsand_Sport");
            });

            modelBuilder.Entity<VsandScheduleLoadFileParse>(entity =>
            {
                entity.HasKey(e => e.FileParseId);

                entity.ToTable("vsand_ScheduleLoadFileParse");

                entity.HasIndex(e => new { e.FileParseId, e.OpponentSchoolId, e.OpponentSchoolName })
                    .HasName("missing_index_19579_19578_vsand_ScheduleLoadFileParse");

                entity.HasIndex(e => new { e.OpponentSchoolName, e.OpponentSchoolId, e.FileId })
                    .HasName("missing_index_94322_94321_vsand_ScheduleLoadFileParse");

                entity.HasIndex(e => new { e.TeamSchoolName, e.TeamSchoolId, e.FileId })
                    .HasName("missing_index_94320_94319_vsand_ScheduleLoadFileParse");

                entity.Property(e => e.FileParseId).HasColumnName("FileParseID");

                entity.Property(e => e.EventDate).HasColumnType("datetime");

                entity.Property(e => e.FileId).HasColumnName("FileID");

                entity.Property(e => e.HomeAway)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.OpponentSchoolId).HasColumnName("OpponentSchoolID");

                entity.Property(e => e.OpponentSchoolName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OpponentTeamId).HasColumnName("OpponentTeamID");

                entity.Property(e => e.SourceType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TeamId).HasColumnName("TeamID");

                entity.Property(e => e.TeamSchoolId).HasColumnName("TeamSchoolID");

                entity.Property(e => e.TeamSchoolName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Venue)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.File)
                    .WithMany(p => p.FileRows)
                    .HasForeignKey(d => d.FileId)
                    .HasConstraintName("FK_vsand_ScheduleLoadFileParse_vsand_ScheduleLoadFile");

                entity.HasOne(d => d.OpponentSchool)
                    .WithMany(p => p.VsandScheduleLoadFileParseOpponentSchool)
                    .HasForeignKey(d => d.OpponentSchoolId)
                    .HasConstraintName("FK_vsand_ScheduleLoadFileParse_vsand_School1");

                entity.HasOne(d => d.OpponentTeam)
                    .WithMany(p => p.VsandScheduleLoadFileParseOpponentTeam)
                    .HasForeignKey(d => d.OpponentTeamId)
                    .HasConstraintName("FK_vsand_ScheduleLoadFileParse_vsand_Team1");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.VsandScheduleLoadFileParseTeam)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("FK_vsand_ScheduleLoadFileParse_vsand_Team");

                entity.HasOne(d => d.TeamSchool)
                    .WithMany(p => p.VsandScheduleLoadFileParseTeamSchool)
                    .HasForeignKey(d => d.TeamSchoolId)
                    .HasConstraintName("FK_vsand_ScheduleLoadFileParse_vsand_School");
            });

            modelBuilder.Entity<VsandScheduleLoadImportSource>(entity =>
            {
                entity.HasKey(e => e.ImportSourceId);

                entity.ToTable("vsand_ScheduleLoadImportSource");

                entity.Property(e => e.ImportSourceId).HasColumnName("ImportSourceID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RetrieveUrl)
                    .IsRequired()
                    .HasColumnName("RetrieveURL")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.RetrieveUrltype)
                    .HasColumnName("RetrieveURLType")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SchoolListSource)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SchoolListSourceType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SportListSource)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SportListSourceType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VsandScheduleYear>(entity =>
            {
                entity.HasKey(e => e.ScheduleYearId);

                entity.ToTable("vsand_ScheduleYear");

                entity.Property(e => e.ScheduleYearId).HasColumnName("ScheduleYearID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VsandPowerPointsConfig>(entity =>
            {
                entity.HasKey(e => e.PPConfigId);

                entity.ToTable("vsand_ScheduleYearPowerPointsConfig");

                entity.Property(e => e.PPConfigId).HasColumnName("PPConfigId");

                entity.Property(e => e.BestNGames).HasColumnName("BestNGames");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.GracePeriodEnd).HasColumnType("datetime");

                entity.Property(e => e.SeedingPeriodEnd).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.ScheduleYear)
                    .WithMany(p => p.PowerPointsConfigs)
                    .HasForeignKey(d => d.ScheduleYearId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
                //.HasConstraintName("FK_vsand_Team_vsand_ScheduleYear");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.PowerPointsConfigs)
                    .HasForeignKey(d => d.SportId);
                //.HasConstraintName("FK_vsand_Team_vsand_Sport");
            });

            modelBuilder.Entity<VsandSchool>(entity =>
            {
                entity.HasKey(e => e.SchoolId);

                entity.ToTable("vsand_School");

                entity.Property(e => e.Address1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Address2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AltName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Color1)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Color2)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Color3)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Graphic)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Mascot)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nickname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.State)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ValidatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.County)
                    .WithMany(p => p.Schools)
                    .HasForeignKey(d => d.CountyId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_vsand_School_vsand_County");
            });

            modelBuilder.Entity<VsandSchoolContact>(entity =>
            {
                entity.HasKey(e => e.SchoolContactId);

                entity.ToTable("vsand_SchoolContact");

                entity.Property(e => e.SchoolContactId).HasColumnName("SchoolContactID");

                entity.Property(e => e.Address1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Address2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CellPhone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FaxNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HomePhone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SchoolId).HasColumnName("SchoolID");

                entity.Property(e => e.SchoolPhone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.State)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.School)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.SchoolId)
                    .HasConstraintName("FK_vsand_SchoolContact_vsand_School");
            });

            modelBuilder.Entity<VsandSchoolCustomCode>(entity =>
            {
                entity.HasKey(e => e.CustomCodeId);

                entity.ToTable("vsand_SchoolCustomCode");

                entity.Property(e => e.CustomCodeId).HasColumnName("CustomCodeID");

                entity.Property(e => e.CodeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodeValue)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SchoolId).HasColumnName("SchoolID");

                entity.Property(e => e.SportId).HasColumnName("SportID");

                entity.HasOne(d => d.School)
                    .WithMany(p => p.CustomCodes)
                    .HasForeignKey(d => d.SchoolId)
                    .HasConstraintName("FK_vsand_SchoolCustomCode_vsand_School");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.VsandSchoolCustomCode)
                    .HasForeignKey(d => d.SportId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_vsand_SchoolCustomCode_vsand_Sport");
            });

            modelBuilder.Entity<VsandSchoolEdition>(entity =>
            {
                entity.HasKey(e => e.SchoolEditionId);

                entity.ToTable("vsand_SchoolEdition");

                entity.Property(e => e.SchoolEditionId).HasColumnName("SchoolEditionID");

                entity.Property(e => e.EditionId).HasColumnName("EditionID");

                entity.Property(e => e.SchoolId).HasColumnName("SchoolID");

                entity.HasOne(d => d.Edition)
                    .WithMany(p => p.VsandSchoolEdition)
                    .HasForeignKey(d => d.EditionId)
                    .HasConstraintName("FK_vsand_SchoolEdition_vsand_Edition");

                entity.HasOne(d => d.School)
                    .WithMany(p => p.Editions)
                    .HasForeignKey(d => d.SchoolId)
                    .HasConstraintName("FK_vsand_SchoolEdition_vsand_School");
            });

            modelBuilder.Entity<VsandSendHistory>(entity =>
            {
                entity.HasKey(e => e.HistoryId);

                entity.ToTable("vsand_SendHistory");

                entity.HasIndex(e => e.ReferenceId)
                    .HasName("idx_vsand_SendHistory2");

                entity.HasIndex(e => new { e.ReferenceId, e.SportId, e.PublicationId })
                    .HasName("idx_vsand_SendHistory1");

                entity.Property(e => e.SentBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SentDate).HasColumnType("datetime");

                entity.Property(e => e.ViewDate).HasColumnType("datetime");

                entity.Property(e => e.ViewType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VsandshimProblemSchool>(entity =>
            {
                entity.HasKey(e => e.Name);

                entity.ToTable("vsandshim_ProblemSchool");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<VsandshimSummaryCountByDay>(entity =>
            {
                entity.HasKey(e => e.SummaryDate);

                entity.ToTable("vsandshim_SummaryCountByDay");

                entity.Property(e => e.SummaryDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<VsandshimUserActivity>(entity =>
            {
                entity.HasKey(e => new { e.ActivityType, e.ActivityId, e.ActivityById });

                entity.ToTable("vsandshim_UserActivity");

                entity.Property(e => e.ActivityType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ActivityByFirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ActivityByLastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ActivityDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VsandSport>(entity =>
            {
                entity.HasKey(e => e.SportId);

                entity.ToTable("vsand_Sport");

                entity.Property(e => e.Abbreviation)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.AllowOt).HasColumnName("AllowOT");

                entity.Property(e => e.AtomFormatter)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DifferentialDataType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DifferentialLabel)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.GameRosterOrderLabel)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LegacyFormatter)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MeetName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MeetType)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OTName)
                    .HasColumnName("OTName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OTNamePlural)
                    .HasColumnName("OTNamePlural")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PeriodName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PeriodNamePlural)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.PlayerName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PlayerNamePlural)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PlayerOfRecordLabel)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PowerPointsDataType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PowerPointsLabel)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReadOnlyFormatter)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ScoringPlayByPlayHandler)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Season)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SportsMlformatter)
                    .HasColumnName("SportsMLFormatter")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VsandSportEvent>(entity =>
            {
                entity.HasKey(e => e.SportEventId);

                entity.ToTable("vsand_SportEvent");

                entity.Property(e => e.Abbreviation)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ResultHandler)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ResultType)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.SportEvents)
                    .HasForeignKey(d => d.SportId)
                    .HasConstraintName("FK_vsand_SportEvent_vsand_Sport");
            });

            modelBuilder.Entity<VsandSportEventResult>(entity =>
            {
                entity.HasKey(e => e.SportEventResultId);

                entity.ToTable("vsand_SportEventResult");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.EventResults)
                    .HasForeignKey(d => d.SportId)
                    .HasConstraintName("FK_vsand_SportEventResult_vsand_Sport");
            });

            modelBuilder.Entity<VsandSportEventStat>(entity =>
            {
                entity.HasKey(e => e.SportEventStatId);

                entity.ToTable("vsand_SportEventStat");

                entity.Property(e => e.Abbreviation)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DataType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.SportEvent)
                    .WithMany(p => p.EventStats)
                    .HasForeignKey(d => d.SportEventId)
                    .HasConstraintName("FK_vsand_SportEventStat_vsand_SportEventStat");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.VsandSportEventStat)
                    .HasForeignKey(d => d.SportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_SportEventStat_vsand_Sport");
            });

            modelBuilder.Entity<VsandSportEventType>(entity =>
            {
                entity.HasKey(e => e.EventTypeId);

                entity.ToTable("vsand_SportEventType");

                entity.Property(e => e.EventTypeId).HasColumnName("EventTypeID");

                entity.Property(e => e.CustomCodeName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomFormId).HasColumnName("CustomFormID");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.GroupLabel)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.ParticipatingTeamsFilter)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ParticpatingTeamsType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ScheduleYearId).HasColumnName("ScheduleYearID");

                entity.Property(e => e.ScoreboardTypeId).HasColumnName("ScoreboardTypeID");

                entity.Property(e => e.SectionLabel)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SportId).HasColumnName("SportID");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Venue)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.ScheduleYear)
                    .WithMany(p => p.VsandSportEventType)
                    .HasForeignKey(d => d.ScheduleYearId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_vsand_SportEventType_vsand_ScheduleYear");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.EventTypes)
                    .HasForeignKey(d => d.SportId)
                    .HasConstraintName("FK_vsand_SportEventType_vsand_Sport");
            });

            modelBuilder.Entity<VsandSportEventTypeAlias>(entity =>
            {
                entity.HasKey(e => e.EventTypeAliasId);

                entity.ToTable("vsand_SportEventTypeAlias");

                entity.Property(e => e.EventTypeAliasId).HasColumnName("EventTypeAliasID");

                entity.Property(e => e.EventTypeId).HasColumnName("EventTypeID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.EventType)
                    .WithMany(p => p.VsandSportEventTypeAlias)
                    .HasForeignKey(d => d.EventTypeId)
                    .HasConstraintName("FK_vsand_SportEventTypeAlias_vsand_SportEventType");
            });

            modelBuilder.Entity<VsandSportEventTypeGroup>(entity =>
            {
                entity.HasKey(e => e.GroupId);

                entity.ToTable("vsand_SportEventTypeGroup");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Section)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.SectionId)
                    .HasConstraintName("FK_vsand_SportEventTypeGroup_vsand_SportEventTypeSection");
            });

            modelBuilder.Entity<VsandSportEventTypeRound>(entity =>
            {
                entity.HasKey(e => e.RoundId);

                entity.ToTable("vsand_SportEventTypeRound");

                entity.Property(e => e.RoundId).HasColumnName("RoundID");

                entity.Property(e => e.CustomFormId).HasColumnName("CustomFormID");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.EventTypeId).HasColumnName("EventTypeID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ParticipatingTeamsFilter)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.SportEventType)
                    .WithMany(p => p.Rounds)
                    .HasForeignKey(d => d.EventTypeId)
                    .HasConstraintName("FK_vsand_SportEventTypeRound_vsand_SportEventType");
            });

            modelBuilder.Entity<VsandSportEventTypeSection>(entity =>
            {
                entity.HasKey(e => e.SectionId);

                entity.ToTable("vsand_SportEventTypeSection");

                entity.Property(e => e.EventTypeId).HasColumnName("EventTypeID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.EventType)
                    .WithMany(p => p.Sections)
                    .HasForeignKey(d => d.EventTypeId)
                    .HasConstraintName("FK_vsand_SportEventTypeSection_vsand_SportEventType");
            });

            modelBuilder.Entity<VsandSportGameMeta>(entity =>
            {
                entity.HasKey(e => e.SportGameMetaId);

                entity.ToTable("vsand_SportGameMeta");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PromptHelp)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ValueType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.GameMeta)
                    .HasForeignKey(d => d.SportId)
                    .HasConstraintName("FK_vsand_SportGameMeta_vsand_Sport");
            });

            modelBuilder.Entity<VsandSportPlayerStat>(entity =>
            {
                entity.HasKey(e => e.SportPlayerStatId);

                entity.ToTable("vsand_SportPlayerStat");

                entity.Property(e => e.Abbreviation)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DataType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.VsandSportPlayerStat)
                    .HasForeignKey(d => d.SportId)
                    .HasConstraintName("FK_vsand_SportPlayerStat_vsand_SportPlayerStatCategory");

                entity.HasOne(d => d.SportPlayerStatCategory)
                    .WithMany(p => p.PlayerStats)
                    .HasForeignKey(d => d.SportPlayerStatCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_SportPlayerStat_vsand_SportPlayerStatCategory1");
            });

            modelBuilder.Entity<VsandSportPlayerStatCategory>(entity =>
            {
                entity.HasKey(e => e.SportPlayerStatCategoryId);

                entity.ToTable("vsand_SportPlayerStatCategory");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.PlayerStatCategories)
                    .HasForeignKey(d => d.SportId)
                    .HasConstraintName("FK_vsand_SportPlayerStatCategory_vsand_Sport");
            });

            modelBuilder.Entity<VsandSportPosition>(entity =>
            {
                entity.HasKey(e => e.SportPositionId);

                entity.ToTable("vsand_SportPosition");

                entity.Property(e => e.Abbreviation)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.Positions)
                    .HasForeignKey(d => d.SportId)
                    .HasConstraintName("FK_vsand_SportPosition_vsand_Sport");
            });

            modelBuilder.Entity<VsandSportSeason>(entity =>
            {
                entity.HasKey(e => e.SeasonId);

                entity.ToTable("vsand_SportSeason");

                entity.Property(e => e.SeasonId).HasColumnName("SeasonID");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ScheduleYearId).HasColumnName("ScheduleYearID");

                entity.Property(e => e.SportId).HasColumnName("SportID");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.ScheduleYear)
                    .WithMany(p => p.VsandSportSeason)
                    .HasForeignKey(d => d.ScheduleYearId)
                    .HasConstraintName("FK_vsand_SportSeason_vsand_ScheduleYear");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.VsandSportSeason)
                    .HasForeignKey(d => d.SportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_SportSeason_vsand_Sport");
            });

            modelBuilder.Entity<VsandSportStatFormula>(entity =>
            {
                entity.HasKey(e => e.FormulaId);

                entity.ToTable("vsand_SportStatFormula");

                entity.Property(e => e.Formula)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PreviewFormula)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Scope)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.VsandSportStatFormula)
                    .HasForeignKey(d => d.SportId)
                    .HasConstraintName("FK_vsand_SportStatFormula_vsand_Sport");
            });

            modelBuilder.Entity<VsandSportTeamStat>(entity =>
            {
                entity.HasKey(e => e.SportTeamStatId);

                entity.ToTable("vsand_SportTeamStat");

                entity.Property(e => e.Abbreviation)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DataType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.VsandSportTeamStat)
                    .HasForeignKey(d => d.SportId)
                    .HasConstraintName("FK_vsand_SportTeamStat_vsand_Sport");

                entity.HasOne(d => d.SportTeamStatCategory)
                    .WithMany(p => p.TeamStats)
                    .HasForeignKey(d => d.SportTeamStatCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_SportTeamStat_vsand_SportTeamStat");
            });

            modelBuilder.Entity<VsandSportTeamStatCategory>(entity =>
            {
                entity.HasKey(e => e.SportTeamStatCategoryId);

                entity.ToTable("vsand_SportTeamStatCategory");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.TeamStatCategories)
                    .HasForeignKey(d => d.SportId)
                    .HasConstraintName("FK_vsand_SportTeamStatCategory_vsand_Sport");
            });

            modelBuilder.Entity<VsandState>(entity =>
            {
                entity.HasKey(e => e.StateId);

                entity.ToTable("vsand_State");

                entity.Property(e => e.Abbreviation)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PubAbbreviation)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VsandStatQuery>(entity =>
            {
                entity.HasKey(e => e.StatQueryId);

                entity.ToTable("vsand_StatQuery");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Handler)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OutputTitle)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.QueryData)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VsandStory>(entity =>
            {
                entity.HasKey(e => e.StoryId);

                entity.ToTable("vsand_Story");

                entity.HasIndex(e => e.PublicationStoryId)
                    .HasName("idx_vsand_Story_ByPublicationId1");

                entity.HasIndex(e => new { e.StatusById, e.StatusDate })
                    .HasName("idx_vsand_Story1");

                entity.Property(e => e.AssignToName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ByLine)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SourceLine)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StatusBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StatusDate).HasColumnType("datetime");

                entity.Property(e => e.Story).IsUnicode(false);

                entity.Property(e => e.SubStatus)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.AssignTo)
                    .WithMany(p => p.VsandStoryAssignTo)
                    .HasForeignKey(d => d.AssignToId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_Story_appxUser");

                entity.HasOne(d => d.PublicationStory)
                    .WithMany(p => p.VsandStory)
                    .HasForeignKey(d => d.PublicationStoryId)
                    .HasConstraintName("FK_vsand_Story_vsand_PublicationStory");

                entity.HasOne(d => d.StatusByNavigation)
                    .WithMany(p => p.VsandStoryStatusByNavigation)
                    .HasForeignKey(d => d.StatusById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_Story_appxUser1");
            });

            modelBuilder.Entity<VsandSystemFormat>(entity =>
            {
                entity.HasKey(e => e.FormatId);

                entity.ToTable("vsand_SystemFormat");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Extension)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FormatClass)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FormatType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.VsandSystemFormat)
                    .HasForeignKey(d => d.SportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_SystemFormat_vsand_Sport");
            });

            modelBuilder.Entity<VsandSystemFormatVariable>(entity =>
            {
                entity.HasKey(e => e.FormatVariableId);

                entity.ToTable("vsand_SystemFormatVariable");

                entity.Property(e => e.ValueType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VariableName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VariableValue)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VsandSystemMessage>(entity =>
            {
                entity.HasKey(e => e.MessageId);

                entity.ToTable("vsand_SystemMessage");

                entity.Property(e => e.Body)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DisplayArea)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EndDisplayDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.StartDisplayDate).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VsandSystemMessageSport>(entity =>
            {
                entity.HasKey(e => e.MessageSportId);

                entity.ToTable("vsand_SystemMessageSport");

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.VsandSystemMessageSport)
                    .HasForeignKey(d => d.MessageId)
                    .HasConstraintName("FK_vsand_SystemMessageSport_vsand_SystemMessage");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.VsandSystemMessageSport)
                    .HasForeignKey(d => d.SportId)
                    .HasConstraintName("FK_vsand_SystemMessageSport_vsand_Sport");
            });

            modelBuilder.Entity<VsandTeam>(entity =>
            {
                entity.HasKey(e => e.TeamId);

                entity.ToTable("vsand_Team");

                entity.HasIndex(e => new { e.TeamId, e.ScheduleYearId })
                    .HasName("idx_vsand_Team_ByScheduleYearIdWithTeamId");

                entity.HasIndex(e => new { e.TeamId, e.Name, e.SchoolId, e.SportId, e.ScheduleYearId })
                    .HasName("vsand_FindSimilarTeamsTeamRecommendSpeed2");

                entity.Property(e => e.AssistantCoaches)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.AthleticDirector)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.HeadCoach)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Principal)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Superintendent)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TeamColors)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TeamNickname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ValidatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.ScheduleYear)
                    .WithMany(p => p.VsandTeam)
                    .HasForeignKey(d => d.ScheduleYearId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_Team_vsand_ScheduleYear");

                entity.HasOne(d => d.School)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.SchoolId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_vsand_Team_vsand_School");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.SportId)
                    .HasConstraintName("FK_vsand_Team_vsand_Sport");
            });

            modelBuilder.Entity<VsandTeamContact>(entity =>
            {
                entity.HasKey(e => e.TeamContactId);

                entity.ToTable("vsand_TeamContact");

                entity.Property(e => e.Address1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Address2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CellPhone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FaxNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HomePhone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.JobTitle)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SchoolPhone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.State)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.VsandTeamContact)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("FK_vsand_TeamContact_vsand_Team");
            });

            modelBuilder.Entity<VsandTeamCustomCode>(entity =>
            {
                entity.HasKey(e => e.CustomCodeId);

                entity.ToTable("vsand_TeamCustomCode");

                entity.HasIndex(e => e.TeamId)
                    .HasName("idx_vsand_TeamCustomCodeTeamId");

                entity.HasIndex(e => new { e.CodeValue, e.CodeName, e.TeamId })
                    .HasName("idx_vsand_TeamCustomCode_LookupSpeed2");

                entity.HasIndex(e => new { e.CodeValue, e.TeamId, e.CodeName })
                    .HasName("idx_vsand_TeamCustomCode_LookupSpeed");

                entity.Property(e => e.CodeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodeValue)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.CustomCodes)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("FK_vsand_TeamCustomCode_vsand_Team");
            });

            modelBuilder.Entity<VsandTeamNotifyList>(entity =>
            {
                entity.HasKey(e => e.NotifyId);

                entity.ToTable("vsand_TeamNotifyList");

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.HasOne(d => d.School)
                    .WithMany(p => p.NotifyList)
                    .HasForeignKey(d => d.SchoolId)
                    .HasConstraintName("FK_vsand_TeamNotifyList_vsand_School");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.VsandTeamNotifyList)
                    .HasForeignKey(d => d.SportId)
                    .HasConstraintName("FK_vsand_TeamNotifyList_vsand_Sport");
            });

            modelBuilder.Entity<VsandTeamRoster>(entity =>
            {
                entity.HasKey(e => e.RosterId);

                entity.ToTable("vsand_TeamRoster");

                entity.HasIndex(e => e.PlayerId)
                    .HasName("idx_vsand_TeamRoster1");

                entity.HasIndex(e => e.TeamId)
                    .HasName("idx_RosterTeamId");

                entity.Property(e => e.AwayJerseyNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.JerseyNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.TeamRosters)
                    .HasForeignKey(d => d.PlayerId)
                    .HasConstraintName("FK_vsand_TeamRoster_vsand_Player");

                entity.HasOne(d => d.PositionNavigation)
                    .WithMany(p => p.VsandTeamRosterPositionNavigation)
                    .HasForeignKey(d => d.Position)
                    .HasConstraintName("FK_vsand_TeamRoster_vsand_SportPosition");

                entity.HasOne(d => d.Position2Navigation)
                    .WithMany(p => p.VsandTeamRosterPosition2Navigation)
                    .HasForeignKey(d => d.Position2)
                    .HasConstraintName("FK_vsand_TeamRoster_vsand_SportPosition2");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.RosterEntries)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("FK_vsand_TeamRoster_vsand_Team");
            });

            modelBuilder.Entity<VsandTeamRosterCustomCode>(entity =>
            {
                entity.HasKey(e => e.CustomCodeId);

                entity.ToTable("vsand_TeamRosterCustomCode");

                entity.Property(e => e.CodeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodeValue)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Roster)
                    .WithMany(p => p.VsandTeamRosterCustomCode)
                    .HasForeignKey(d => d.RosterId)
                    .HasConstraintName("FK_vsand_RosterCustomCode_vsand_RosterCustomCode");
            });

            modelBuilder.Entity<VsandTeamSchedule>(entity =>
            {
                entity.HasKey(e => e.ScheduleId);

                entity.ToTable("vsand_TeamSchedule");

                entity.HasIndex(e => new { e.ScheduleId, e.TeamId, e.EventMonth, e.EventDay, e.EventYear })
                    .HasName("idx_vsand_TeamSchedule1");

                entity.HasIndex(e => new { e.ScheduleId, e.TeamId, e.EventHour, e.EventMin, e.OpponentId, e.OpponentName, e.HomeAwayFlag, e.Location, e.GameType, e.TournamentId, e.TournamentName, e.RoundId, e.SectionId, e.GroupId, e.TriPlus, e.EventMonth, e.EventDay, e.EventYear })
                    .HasName("vsand_TeamSchedule_EventSpeed");

                entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.Location)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OpponentId).HasColumnName("OpponentID");

                entity.Property(e => e.OpponentName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RoundId).HasColumnName("RoundID");

                entity.Property(e => e.SectionId).HasColumnName("SectionID");

                entity.Property(e => e.TeamId).HasColumnName("TeamID");

                entity.Property(e => e.TournamentId).HasColumnName("TournamentID");

                entity.Property(e => e.TournamentName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Opponent)
                    .WithMany(p => p.VsandTeamScheduleOpponent)
                    .HasForeignKey(d => d.OpponentId)
                    .HasConstraintName("FK_vsand_TeamSchedule_vsand_Team1");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("FK_vsand_TeamSchedule_vsand_Team");
            });

            modelBuilder.Entity<VsandTeamScheduleTeam>(entity =>
            {
                entity.HasKey(e => e.TsteamId);

                entity.ToTable("vsand_TeamScheduleTeam");

                entity.HasIndex(e => e.TeamId)
                    .HasName("idx_vsand_TeamScheduleTeam1");

                entity.HasIndex(e => new { e.TeamId, e.ScheduleId, e.HomeTeam })
                    .HasName("vsand_TeamScheduleTeam_HomeTeamSpeed");

                entity.Property(e => e.TsteamId).HasColumnName("TSTeamID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");

                entity.Property(e => e.TeamId).HasColumnName("TeamID");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.VsandTeamScheduleTeam)
                    .HasForeignKey(d => d.ScheduleId)
                    .HasConstraintName("FK_vsand_TeamScheduleTeam_vsand_TeamSchedule");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.ScheduleTeamEntries)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_vsand_TeamScheduleTeam_vsand_Team");
            });

            modelBuilder.Entity<VsandTeamStatSummaryShim>(entity =>
            {
                entity.HasKey(e => e.TeamId);

                entity.ToTable("vsand_TeamStatSummaryShim");

                entity.Property(e => e.TeamId).ValueGeneratedNever();

                entity.Property(e => e.Team)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VsandUserSport>(entity =>
            {
                entity.HasKey(e => e.UserSportId);

                entity.ToTable("vsand_UserSport");

                entity.HasIndex(e => e.AdminId)
                    .HasName("idx_vsand_UserSport1");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.VsandUserSport)
                    .HasForeignKey(d => d.AdminId)
                    .HasConstraintName("FK_vsand_UserSport_appxUser");

                entity.HasOne(d => d.Sport)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.SportId)
                    .HasConstraintName("FK_vsand_UserSport_vsand_Sport");
            });

            // TODO: Remediate?
            //modelBuilder.Entity<VsandSchoolsWithoutAccounts>(entity =>
            //{
            //    entity.ToTable("vsand_SchoolsWithoutAccounts");
            //});

            // TODO: Remediate;
            //modelBuilder.Entity<TeamsWithDuplicateGameReports>(entity =>
            //{
            //    entity.ToTable("vsand_TeamsWithDuplicateGameRecords");
            //});
        }
    }
}
