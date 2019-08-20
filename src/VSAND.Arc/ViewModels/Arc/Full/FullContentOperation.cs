
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace VSAND.Arc.ViewModels.Arc.Full
{
    public class FullContentOperation
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("operation")]
        public string Operation { get; set; }

        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty("organization_id")]
        public string OrganizationId { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("branch")]
        public string Branch { get; set; }

        [JsonProperty("published")]
        public bool Published { get; set; }

        [JsonProperty("created")]
        public bool Created { get; set; }

        [JsonProperty("trigger")]
        public Trigger Trigger { get; set; }

        [JsonProperty("body")]
        public Body Body { get; set; }

        public static FullContentOperation FromJson(string json) => JsonConvert.DeserializeObject<FullContentOperation>(json, Converter.Settings);
    }

    public partial class Body
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("content_elements")]
        public BodyContentElement[] ContentElements { get; set; }

        [JsonProperty("created_date")]
        public DateTimeOffset CreatedDate { get; set; }

        [JsonProperty("revision")]
        public Revision Revision { get; set; }

        [JsonProperty("last_updated_date")]
        public DateTimeOffset LastUpdatedDate { get; set; }

        [JsonProperty("headlines")]
        public Headlines Headlines { get; set; }

        [JsonProperty("owner")]
        public Owner Owner { get; set; }

        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public LabelClass Address { get; set; }

        [JsonProperty("workflow", NullValueHandling = NullValueHandling.Ignore)]
        public Workflow Workflow { get; set; }

        [JsonProperty("subheadlines", NullValueHandling = NullValueHandling.Ignore)]
        public Description Subheadlines { get; set; }

        [JsonProperty("description")]
        public Description Description { get; set; }

        [JsonProperty("language", NullValueHandling = NullValueHandling.Ignore)]
        public string Language { get; set; }

        [JsonProperty("source")]
        public BodySource Source { get; set; }

        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
        public LabelClass Label { get; set; }

        [JsonProperty("taxonomy")]
        public BodyTaxonomy Taxonomy { get; set; }

        [JsonProperty("related_content", NullValueHandling = NullValueHandling.Ignore)]
        public RelatedContent RelatedContent { get; set; }

        [JsonProperty("distributor")]
        public BodyDistributor Distributor { get; set; }

        [JsonProperty("canonical_website")]
        public string CanonicalWebsite { get; set; }

        [JsonProperty("planning", NullValueHandling = NullValueHandling.Ignore)]
        public BodyPlanning Planning { get; set; }

        [JsonProperty("credits")]
        public BodyCredits Credits { get; set; }

        [JsonProperty("websites")]
        public BodyWebsites Websites { get; set; }

        [JsonProperty("additional_properties")]
        public BodyAdditionalProperties AdditionalProperties { get; set; }

        [JsonProperty("_website_ids")]
        public string[] WebsiteIds { get; set; }

        [JsonProperty("_website_urls")]
        public string[] WebsiteUrls { get; set; }

        [JsonProperty("publishing")]
        public Publishing Publishing { get; set; }

        [JsonProperty("promo_items", NullValueHandling = NullValueHandling.Ignore)]
        public BodyPromoItems PromoItems { get; set; }

        [JsonProperty("canonical_url", NullValueHandling = NullValueHandling.Ignore)]
        public string CanonicalUrl { get; set; }

        [JsonProperty("display_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? DisplayDate { get; set; }

        [JsonProperty("first_publish_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? FirstPublishDate { get; set; }

        [JsonProperty("publish_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? PublishDate { get; set; }

        [JsonProperty("subtype", NullValueHandling = NullValueHandling.Ignore)]
        public string Subtype { get; set; }

        [JsonProperty("slug", NullValueHandling = NullValueHandling.Ignore)]
        public string Slug { get; set; }
    }

    public partial class BodyAdditionalProperties
    {
        [JsonProperty("clipboard", NullValueHandling = NullValueHandling.Ignore)]
        public Clipboard Clipboard { get; set; }

        [JsonProperty("has_published_copy")]
        public bool HasPublishedCopy { get; set; }

        [JsonProperty("is_published", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsPublished { get; set; }

        [JsonProperty("publish_date", NullValueHandling = NullValueHandling.Ignore)]
        public PublishDate? PublishDate { get; set; }

        [JsonProperty("comment_thread_id", NullValueHandling = NullValueHandling.Ignore)]
        public string CommentThreadId { get; set; }
    }

    public partial class Clipboard
    {
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public Text[] Text { get; set; }
    }

    public partial class Text
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("additional_properties")]
        public TextAdditionalProperties AdditionalProperties { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }
    }

    public partial class TextAdditionalProperties
    {
        [JsonProperty("comments")]
        public object[] Comments { get; set; }

        [JsonProperty("inline_comments")]
        public object[] InlineComments { get; set; }

        [JsonProperty("_id")]
        public long Id { get; set; }
    }

    public partial class LabelClass
    {
    }

    public partial class BodyContentElement
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("additional_properties", NullValueHandling = NullValueHandling.Ignore)]
        public PurpleAdditionalProperties AdditionalProperties { get; set; }

        [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
        public string Content { get; set; }

        [JsonProperty("level", NullValueHandling = NullValueHandling.Ignore)]
        public long? Level { get; set; }

        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public ContentElementAddress Address { get; set; }

        [JsonProperty("caption", NullValueHandling = NullValueHandling.Ignore)]
        public string Caption { get; set; }

        [JsonProperty("created_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? CreatedDate { get; set; }

        [JsonProperty("credits", NullValueHandling = NullValueHandling.Ignore)]
        public FluffyCredits Credits { get; set; }

        [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
        public long? Height { get; set; }

        [JsonProperty("image_type", NullValueHandling = NullValueHandling.Ignore)]
        public string ImageType { get; set; }

        [JsonProperty("last_updated_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? LastUpdatedDate { get; set; }

        [JsonProperty("licensable", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Licensable { get; set; }

        [JsonProperty("owner", NullValueHandling = NullValueHandling.Ignore)]
        public Owner Owner { get; set; }

        [JsonProperty("source", NullValueHandling = NullValueHandling.Ignore)]
        public FluffySource Source { get; set; }

        [JsonProperty("subtitle", NullValueHandling = NullValueHandling.Ignore)]
        public string Subtitle { get; set; }

        [JsonProperty("taxonomy", NullValueHandling = NullValueHandling.Ignore)]
        public FluffyTaxonomy Taxonomy { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
        public string Version { get; set; }

        [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
        public long? Width { get; set; }

        [JsonProperty("copyright", NullValueHandling = NullValueHandling.Ignore)]
        public string Copyright { get; set; }

        [JsonProperty("slug", NullValueHandling = NullValueHandling.Ignore)]
        public string Slug { get; set; }

        [JsonProperty("subtype", NullValueHandling = NullValueHandling.Ignore)]
        public string Subtype { get; set; }

        [JsonProperty("raw_oembed", NullValueHandling = NullValueHandling.Ignore)]
        public RawOembed RawOembed { get; set; }

        [JsonProperty("referent", NullValueHandling = NullValueHandling.Ignore)]
        public ContentElementReferent Referent { get; set; }

        [JsonProperty("alignment", NullValueHandling = NullValueHandling.Ignore)]
        public string Alignment { get; set; }

        [JsonProperty("canonical_url", NullValueHandling = NullValueHandling.Ignore)]
        public string CanonicalUrl { get; set; }

        [JsonProperty("canonical_website", NullValueHandling = NullValueHandling.Ignore)]
        public string CanonicalWebsite { get; set; }

        [JsonProperty("content_restrictions", NullValueHandling = NullValueHandling.Ignore)]
        public ContentRestrictions ContentRestrictions { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public Description Description { get; set; }

        [JsonProperty("display_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? DisplayDate { get; set; }

        [JsonProperty("distributor", NullValueHandling = NullValueHandling.Ignore)]
        public ContentElementDistributor Distributor { get; set; }

        [JsonProperty("first_publish_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? FirstPublishDate { get; set; }

        [JsonProperty("headlines", NullValueHandling = NullValueHandling.Ignore)]
        public Description Headlines { get; set; }

        [JsonProperty("promo_items", NullValueHandling = NullValueHandling.Ignore)]
        public ContentElementPromoItems PromoItems { get; set; }

        [JsonProperty("publish_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? PublishDate { get; set; }

        [JsonProperty("websites", NullValueHandling = NullValueHandling.Ignore)]
        public ContentElementWebsites Websites { get; set; }

        [JsonProperty("content_elements", NullValueHandling = NullValueHandling.Ignore)]
        public BasicElement[] ContentElements { get; set; }

        [JsonProperty("list_type", NullValueHandling = NullValueHandling.Ignore)]
        public string ListType { get; set; }

        [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        public Item[] Items { get; set; }

        [JsonProperty("workflow", NullValueHandling = NullValueHandling.Ignore)]
        public Workflow Workflow { get; set; }
    }

    public partial class PurpleAdditionalProperties
    {
        [JsonProperty("comments", NullValueHandling = NullValueHandling.Ignore)]
        public Comment[] Comments { get; set; }

        [JsonProperty("inline_comments", NullValueHandling = NullValueHandling.Ignore)]
        public object[] InlineComments { get; set; }

        [JsonProperty("_id", NullValueHandling = NullValueHandling.Ignore)]
        public Id? Id { get; set; }

        [JsonProperty("fullSizeResizeUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string FullSizeResizeUrl { get; set; }

        [JsonProperty("galleries", NullValueHandling = NullValueHandling.Ignore)]
        public PurpleGallery[] Galleries { get; set; }

        [JsonProperty("ingestionMethod", NullValueHandling = NullValueHandling.Ignore)]
        public string IngestionMethod { get; set; }

        [JsonProperty("keywords", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Keywords { get; set; }

        [JsonProperty("mime_type", NullValueHandling = NullValueHandling.Ignore)]
        public string MimeType { get; set; }

        [JsonProperty("originalName", NullValueHandling = NullValueHandling.Ignore)]
        public string OriginalName { get; set; }

        [JsonProperty("originalUrl", NullValueHandling = NullValueHandling.Ignore)]
        public Uri OriginalUrl { get; set; }

        [JsonProperty("owner", NullValueHandling = NullValueHandling.Ignore)]
        public string Owner { get; set; }

        [JsonProperty("proxyUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string ProxyUrl { get; set; }

        [JsonProperty("published", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Published { get; set; }

        [JsonProperty("resizeUrl", NullValueHandling = NullValueHandling.Ignore)]
        public Uri ResizeUrl { get; set; }

        [JsonProperty("restricted", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Restricted { get; set; }

        [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
        public long? Version { get; set; }

        [JsonProperty("countryId", NullValueHandling = NullValueHandling.Ignore)]
        public long? CountryId { get; set; }

        [JsonProperty("iptc_job_identifier", NullValueHandling = NullValueHandling.Ignore)]
        public string IptcJobIdentifier { get; set; }

        [JsonProperty("iptc_source", NullValueHandling = NullValueHandling.Ignore)]
        public string IptcSource { get; set; }

        [JsonProperty("iptc_title", NullValueHandling = NullValueHandling.Ignore)]
        public string IptcTitle { get; set; }

        [JsonProperty("takenOn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? TakenOn { get; set; }

        [JsonProperty("has_published_copy", NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasPublishedCopy { get; set; }

        [JsonProperty("roles", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Roles { get; set; }

        [JsonProperty("usage_instructions", NullValueHandling = NullValueHandling.Ignore)]
        public string UsageInstructions { get; set; }

        [JsonProperty("class", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Class { get; set; }

        [JsonProperty("data-slide", NullValueHandling = NullValueHandling.Ignore)]
        public string DataSlide { get; set; }

        [JsonProperty("data-injectedad", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PurpleParseStringConverter))]
        public bool? DataInjectedad { get; set; }
    }

    public partial class Comment
    {
        [JsonProperty("start")]
        public long Start { get; set; }

        [JsonProperty("end")]
        public long End { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty("replies", NullValueHandling = NullValueHandling.Ignore)]
        public Reply[] Replies { get; set; }
    }

    public partial class Reply
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }
    }

    public partial class PurpleGallery
    {
        [JsonProperty("headlines")]
        public Description Headlines { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }
    }

    public partial class Description
    {
        [JsonProperty("basic")]
        public string Basic { get; set; }
    }

    public partial class ContentElementAddress
    {
        [JsonProperty("locality", NullValueHandling = NullValueHandling.Ignore)]
        public string Locality { get; set; }

        [JsonProperty("region", NullValueHandling = NullValueHandling.Ignore)]
        public string Region { get; set; }

        [JsonProperty("country_name", NullValueHandling = NullValueHandling.Ignore)]
        public string CountryName { get; set; }

        [JsonProperty("street_address", NullValueHandling = NullValueHandling.Ignore)]
        public string StreetAddress { get; set; }
    }

    public partial class BasicElement
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("additional_properties")]
        public FluffyAdditionalProperties AdditionalProperties { get; set; }

        [JsonProperty("address")]
        public ContentElementAddress Address { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("copyright", NullValueHandling = NullValueHandling.Ignore)]
        public string Copyright { get; set; }

        [JsonProperty("credits")]
        public PurpleCredits Credits { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("image_type")]
        public string ImageType { get; set; }

        [JsonProperty("licensable")]
        public bool Licensable { get; set; }

        [JsonProperty("owner")]
        public Owner Owner { get; set; }

        [JsonProperty("slug", NullValueHandling = NullValueHandling.Ignore)]
        public string Slug { get; set; }

        [JsonProperty("source")]
        public PurpleSource Source { get; set; }

        [JsonProperty("subtitle", NullValueHandling = NullValueHandling.Ignore)]
        public string Subtitle { get; set; }

        [JsonProperty("taxonomy")]
        public PurpleTaxonomy Taxonomy { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }
    }

    public partial class FluffyAdditionalProperties
    {
        [JsonProperty("fullSizeResizeUrl")]
        public string FullSizeResizeUrl { get; set; }

        [JsonProperty("galleries")]
        public FluffyGallery[] Galleries { get; set; }

        [JsonProperty("galleryOrder")]
        public long GalleryOrder { get; set; }

        [JsonProperty("ingestionMethod")]
        public string IngestionMethod { get; set; }

        [JsonProperty("keywords")]
        public string[] Keywords { get; set; }

        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        [JsonProperty("originalName")]
        public string OriginalName { get; set; }

        [JsonProperty("originalUrl")]
        public Uri OriginalUrl { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("proxyUrl")]
        public string ProxyUrl { get; set; }

        [JsonProperty("published")]
        public bool Published { get; set; }

        [JsonProperty("resizeUrl")]
        public Uri ResizeUrl { get; set; }

        [JsonProperty("restricted")]
        public bool Restricted { get; set; }

        [JsonProperty("takenOn")]
        public DateTimeOffset TakenOn { get; set; }

        [JsonProperty("version")]
        public long Version { get; set; }

        [JsonProperty("iptc_source", NullValueHandling = NullValueHandling.Ignore)]
        public string IptcSource { get; set; }

        [JsonProperty("countryId", NullValueHandling = NullValueHandling.Ignore)]
        public long? CountryId { get; set; }

        [JsonProperty("iptc_job_identifier", NullValueHandling = NullValueHandling.Ignore)]
        public string IptcJobIdentifier { get; set; }

        [JsonProperty("iptc_title", NullValueHandling = NullValueHandling.Ignore)]
        public string IptcTitle { get; set; }

        [JsonProperty("usage_instructions", NullValueHandling = NullValueHandling.Ignore)]
        public string UsageInstructions { get; set; }
    }

    public partial class FluffyGallery
    {
        [JsonProperty("headlines")]
        public LabelClass Headlines { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }
    }

    public partial class PurpleCredits
    {
        [JsonProperty("affiliation")]
        public PurpleAffiliation[] Affiliation { get; set; }

        [JsonProperty("by", NullValueHandling = NullValueHandling.Ignore)]
        public PurpleBy[] By { get; set; }
    }

    public partial class PurpleAffiliation
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class PurpleBy
    {
        [JsonProperty("byline", NullValueHandling = NullValueHandling.Ignore)]
        public string Byline { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("referent", NullValueHandling = NullValueHandling.Ignore)]
        public ByReferent Referent { get; set; }
    }

    public partial class ByReferent
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("provider", NullValueHandling = NullValueHandling.Ignore)]
        public string Provider { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("website", NullValueHandling = NullValueHandling.Ignore)]
        public string Website { get; set; }
    }

    public partial class Owner
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("sponsored", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Sponsored { get; set; }
    }

    public partial class PurpleSource
    {
        [JsonProperty("edit_url")]
        public Uri EditUrl { get; set; }

        [JsonProperty("system")]
        public string System { get; set; }
    }

    public partial class PurpleTaxonomy
    {
        [JsonProperty("associated_tasks", NullValueHandling = NullValueHandling.Ignore)]
        public object[] AssociatedTasks { get; set; }
    }

    public partial class ContentRestrictions
    {
        [JsonProperty("content_code")]
        public string ContentCode { get; set; }
    }

    public partial class FluffyCredits
    {
        [JsonProperty("affiliation", NullValueHandling = NullValueHandling.Ignore)]
        public FluffyAffiliation[] Affiliation { get; set; }

        [JsonProperty("by", NullValueHandling = NullValueHandling.Ignore)]
        public FluffyBy[] By { get; set; }
    }

    public partial class FluffyAffiliation
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("additional_properties", NullValueHandling = NullValueHandling.Ignore)]
        public LabelClass AdditionalProperties { get; set; }
    }

    public partial class FluffyBy
    {
        [JsonProperty("_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
        public string Version { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("social_links", NullValueHandling = NullValueHandling.Ignore)]
        public SocialLinkElement[] BySocialLinks { get; set; }

        [JsonProperty("socialLinks", NullValueHandling = NullValueHandling.Ignore)]
        public SocialLink[] SocialLinks { get; set; }

        [JsonProperty("additional_properties", NullValueHandling = NullValueHandling.Ignore)]
        public TentacledAdditionalProperties AdditionalProperties { get; set; }

        [JsonProperty("byline", NullValueHandling = NullValueHandling.Ignore)]
        public string Byline { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public Image Image { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("referent", NullValueHandling = NullValueHandling.Ignore)]
        public ByReferent Referent { get; set; }
    }

    public partial class TentacledAdditionalProperties
    {
        [JsonProperty("original", NullValueHandling = NullValueHandling.Ignore)]
        public PurpleOriginal Original { get; set; }
    }

    public partial class PurpleOriginal
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("byline")]
        public string Byline { get; set; }

        [JsonProperty("twitter", NullValueHandling = NullValueHandling.Ignore)]
        public string Twitter { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("mt_author_id")]
        [JsonConverter(typeof(FluffyParseStringConverter))]
        public long MtAuthorId { get; set; }

        [JsonProperty("books")]
        public object[] Books { get; set; }

        [JsonProperty("podcasts")]
        public object[] Podcasts { get; set; }

        [JsonProperty("education")]
        public object[] Education { get; set; }

        [JsonProperty("awards")]
        public object[] Awards { get; set; }

        [JsonProperty("last_updated_date")]
        public DateTimeOffset LastUpdatedDate { get; set; }

        [JsonProperty("firstName", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        [JsonProperty("lastName", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        [JsonProperty("bio", NullValueHandling = NullValueHandling.Ignore)]
        public string Bio { get; set; }

        [JsonProperty("facebook", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Facebook { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Image { get; set; }
    }

    public partial class SocialLinkElement
    {
        [JsonProperty("site")]
        public string Site { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public partial class Image
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }

    public partial class SocialLink
    {
        [JsonProperty("site")]
        public string Site { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("deprecated")]
        public bool Deprecated { get; set; }

        [JsonProperty("deprecation_msg")]
        public string DeprecationMsg { get; set; }
    }

    public partial class ContentElementDistributor
    {
        [JsonProperty("category")]
        public string Category { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("additional_properties")]
        public ItemAdditionalProperties AdditionalProperties { get; set; }

        [JsonProperty("block_properties")]
        public LabelClass BlockProperties { get; set; }
    }

    public partial class ItemAdditionalProperties
    {
        [JsonProperty("comments")]
        public object[] Comments { get; set; }

        [JsonProperty("inline_comments")]
        public object[] InlineComments { get; set; }
    }

    public partial class ContentElementPromoItems
    {
        [JsonProperty("basic")]
        public BasicElement Basic { get; set; }
    }

    public partial class RawOembed
    {
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url { get; set; }

        [JsonProperty("author_name")]
        public string AuthorName { get; set; }

        [JsonProperty("author_url")]
        public Uri AuthorUrl { get; set; }

        [JsonProperty("html")]
        public string Html { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("height")]
        public long? Height { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("cache_age", NullValueHandling = NullValueHandling.Ignore)]
        public string CacheAge { get; set; }

        [JsonProperty("provider_name")]
        public string ProviderName { get; set; }

        [JsonProperty("provider_url")]
        public Uri ProviderUrl { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("additional_properties", NullValueHandling = NullValueHandling.Ignore)]
        public RawOembedAdditionalProperties AdditionalProperties { get; set; }

        [JsonProperty("thumbnail_height", NullValueHandling = NullValueHandling.Ignore)]
        public long? ThumbnailHeight { get; set; }

        [JsonProperty("thumbnail_url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri ThumbnailUrl { get; set; }

        [JsonProperty("thumbnail_width", NullValueHandling = NullValueHandling.Ignore)]
        public long? ThumbnailWidth { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }
    }

    public partial class RawOembedAdditionalProperties
    {
        [JsonProperty("comments")]
        public object[] Comments { get; set; }

        [JsonProperty("_id")]
        public Id Id { get; set; }
    }

    public partial class ContentElementReferent
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("service")]
        public string Service { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("provider")]
        public Uri Provider { get; set; }

        [JsonProperty("referent_properties", NullValueHandling = NullValueHandling.Ignore)]
        public ReferentProperties ReferentProperties { get; set; }
    }

    public partial class ReferentProperties
    {
        [JsonProperty("additional_properties")]
        public RawOembedAdditionalProperties AdditionalProperties { get; set; }
    }

    public partial class FluffySource
    {
        [JsonProperty("edit_url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri EditUrl { get; set; }

        [JsonProperty("system")]
        public string System { get; set; }

        [JsonProperty("source_type", NullValueHandling = NullValueHandling.Ignore)]
        public string SourceType { get; set; }

        [JsonProperty("additional_properties", NullValueHandling = NullValueHandling.Ignore)]
        public SourceAdditionalProperties AdditionalProperties { get; set; }
    }

    public partial class SourceAdditionalProperties
    {
        [JsonProperty("editor", NullValueHandling = NullValueHandling.Ignore)]
        public string Editor { get; set; }
    }

    public partial class FluffyTaxonomy
    {
        [JsonProperty("associated_tasks", NullValueHandling = NullValueHandling.Ignore)]
        public object[] AssociatedTasks { get; set; }

        [JsonProperty("sections", NullValueHandling = NullValueHandling.Ignore)]
        public SectionElement[] Sections { get; set; }

        [JsonProperty("additional_properties", NullValueHandling = NullValueHandling.Ignore)]
        public LabelClass AdditionalProperties { get; set; }
    }

    public partial class SectionElement
    {
        [JsonProperty("referent")]
        public ByReferent Referent { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class ContentElementWebsites
    {
        [JsonProperty("oregonlive", NullValueHandling = NullValueHandling.Ignore)]
        public PurpleOregonlive Oregonlive { get; set; }

        [JsonProperty("masslive", NullValueHandling = NullValueHandling.Ignore)]
        public ClevelandClass Masslive { get; set; }

        [JsonProperty("nj", NullValueHandling = NullValueHandling.Ignore)]
        public ClevelandClass Nj { get; set; }

        [JsonProperty("syracuse", NullValueHandling = NullValueHandling.Ignore)]
        public ClevelandClass Syracuse { get; set; }

        [JsonProperty("al", NullValueHandling = NullValueHandling.Ignore)]
        public ClevelandClass Al { get; set; }

        [JsonProperty("cleveland", NullValueHandling = NullValueHandling.Ignore)]
        public ClevelandClass Cleveland { get; set; }

        [JsonProperty("gulflive", NullValueHandling = NullValueHandling.Ignore)]
        public ClevelandClass Gulflive { get; set; }

        [JsonProperty("lehighvalleylive", NullValueHandling = NullValueHandling.Ignore)]
        public ClevelandClass Lehighvalleylive { get; set; }

        [JsonProperty("mlive", NullValueHandling = NullValueHandling.Ignore)]
        public ClevelandClass Mlive { get; set; }

        [JsonProperty("newyorkupstate", NullValueHandling = NullValueHandling.Ignore)]
        public ClevelandClass Newyorkupstate { get; set; }

        [JsonProperty("pennlive", NullValueHandling = NullValueHandling.Ignore)]
        public ClevelandClass Pennlive { get; set; }

        [JsonProperty("silive", NullValueHandling = NullValueHandling.Ignore)]
        public ClevelandClass Silive { get; set; }
    }

    public partial class ClevelandClass
    {
        [JsonProperty("website_section", NullValueHandling = NullValueHandling.Ignore)]
        public SectionElement WebsiteSection { get; set; }

        [JsonProperty("website_url", NullValueHandling = NullValueHandling.Ignore)]
        public string WebsiteUrl { get; set; }
    }

    public partial class PurpleOregonlive
    {
        [JsonProperty("website_section")]
        public SectionElement WebsiteSection { get; set; }

        [JsonProperty("website_url")]
        public string WebsiteUrl { get; set; }
    }

    public partial class Workflow
    {
        [JsonProperty("status_code")]
        public long StatusCode { get; set; }
    }

    public partial class BodyCredits
    {
        [JsonProperty("by")]
        public TentacledBy[] By { get; set; }
    }

    public partial class TentacledBy
    {
        [JsonProperty("_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
        public string Version { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("social_links", NullValueHandling = NullValueHandling.Ignore)]
        public SocialLinkElement[] BySocialLinks { get; set; }

        [JsonProperty("socialLinks", NullValueHandling = NullValueHandling.Ignore)]
        public SocialLink[] SocialLinks { get; set; }

        [JsonProperty("additional_properties")]
        public StickyAdditionalProperties AdditionalProperties { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public Image Image { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("slug", NullValueHandling = NullValueHandling.Ignore)]
        public string Slug { get; set; }

        [JsonProperty("org", NullValueHandling = NullValueHandling.Ignore)]
        public string Org { get; set; }
    }

    public partial class StickyAdditionalProperties
    {
        [JsonProperty("original")]
        public FluffyOriginal Original { get; set; }
    }

    public partial class FluffyOriginal
    {
        [JsonProperty("_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Status { get; set; }

        [JsonProperty("bio", NullValueHandling = NullValueHandling.Ignore)]
        public string Bio { get; set; }

        [JsonProperty("byline", NullValueHandling = NullValueHandling.Ignore)]
        public string Byline { get; set; }

        [JsonProperty("twitter", NullValueHandling = NullValueHandling.Ignore)]
        public string Twitter { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("mt_author_id", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(FluffyParseStringConverter))]
        public long? MtAuthorId { get; set; }

        [JsonProperty("books", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Books { get; set; }

        [JsonProperty("podcasts", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Podcasts { get; set; }

        [JsonProperty("education", NullValueHandling = NullValueHandling.Ignore)]
        public Education[] Education { get; set; }

        [JsonProperty("awards", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Awards { get; set; }

        [JsonProperty("last_updated_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? LastUpdatedDate { get; set; }

        [JsonProperty("firstName", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        [JsonProperty("lastName", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        [JsonProperty("facebook", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Facebook { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public string Image { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("role", NullValueHandling = NullValueHandling.Ignore)]
        public string Role { get; set; }

        [JsonProperty("affiliations", NullValueHandling = NullValueHandling.Ignore)]
        public string Affiliations { get; set; }

        [JsonProperty("bio_page", NullValueHandling = NullValueHandling.Ignore)]
        public string BioPage { get; set; }

        [JsonProperty("longBio", NullValueHandling = NullValueHandling.Ignore)]
        public string LongBio { get; set; }

        [JsonProperty("slug", NullValueHandling = NullValueHandling.Ignore)]
        public string Slug { get; set; }

        [JsonProperty("native_app_rendering", NullValueHandling = NullValueHandling.Ignore)]
        public bool? NativeAppRendering { get; set; }

        [JsonProperty("fuzzy_match", NullValueHandling = NullValueHandling.Ignore)]
        public bool? FuzzyMatch { get; set; }

        [JsonProperty("contributor", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Contributor { get; set; }

        [JsonProperty("author_type", NullValueHandling = NullValueHandling.Ignore)]
        public string AuthorType { get; set; }

        [JsonProperty("beat", NullValueHandling = NullValueHandling.Ignore)]
        public string Beat { get; set; }

        [JsonProperty("secondLastName", NullValueHandling = NullValueHandling.Ignore)]
        public string SecondLastName { get; set; }

        [JsonProperty("middleName", NullValueHandling = NullValueHandling.Ignore)]
        public string MiddleName { get; set; }
    }

    public partial class Education
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class BodyDistributor
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("subcategory", NullValueHandling = NullValueHandling.Ignore)]
        public string Subcategory { get; set; }
    }

    public partial class Headlines
    {
        [JsonProperty("basic")]
        public string Basic { get; set; }

        [JsonProperty("mobile", NullValueHandling = NullValueHandling.Ignore)]
        public string Mobile { get; set; }

        [JsonProperty("native", NullValueHandling = NullValueHandling.Ignore)]
        public string Native { get; set; }

        [JsonProperty("print", NullValueHandling = NullValueHandling.Ignore)]
        public string Print { get; set; }

        [JsonProperty("tablet", NullValueHandling = NullValueHandling.Ignore)]
        public string Tablet { get; set; }

        [JsonProperty("web", NullValueHandling = NullValueHandling.Ignore)]
        public string Web { get; set; }

        [JsonProperty("meta_title", NullValueHandling = NullValueHandling.Ignore)]
        public string MetaTitle { get; set; }

        [JsonProperty("table", NullValueHandling = NullValueHandling.Ignore)]
        public string Table { get; set; }
    }

    public partial class BodyPlanning
    {
        [JsonProperty("scheduling")]
        public Scheduling Scheduling { get; set; }

        [JsonProperty("internal_note")]
        public string InternalNote { get; set; }
    }

    public partial class Scheduling
    {
        [JsonProperty("planned_publish_date")]
        public DateTimeOffset PlannedPublishDate { get; set; }
    }

    public partial class BodyPromoItems
    {
        [JsonProperty("basic")]
        public Basic Basic { get; set; }
    }

    public partial class Basic
    {
        [JsonProperty("_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("additional_properties")]
        public IndigoAdditionalProperties AdditionalProperties { get; set; }

        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public ContentElementAddress Address { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("created_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? CreatedDate { get; set; }

        [JsonProperty("credits", NullValueHandling = NullValueHandling.Ignore)]
        public TentacledCredits Credits { get; set; }

        [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
        public long? Height { get; set; }

        [JsonProperty("image_type", NullValueHandling = NullValueHandling.Ignore)]
        public string ImageType { get; set; }

        [JsonProperty("last_updated_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? LastUpdatedDate { get; set; }

        [JsonProperty("licensable", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Licensable { get; set; }

        [JsonProperty("owner")]
        public Owner Owner { get; set; }

        [JsonProperty("source")]
        public TentacledSource Source { get; set; }

        [JsonProperty("subtitle", NullValueHandling = NullValueHandling.Ignore)]
        public string Subtitle { get; set; }

        [JsonProperty("taxonomy", NullValueHandling = NullValueHandling.Ignore)]
        public TentacledTaxonomy Taxonomy { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
        public long? Width { get; set; }

        [JsonProperty("copyright", NullValueHandling = NullValueHandling.Ignore)]
        public string Copyright { get; set; }

        [JsonProperty("slug", NullValueHandling = NullValueHandling.Ignore)]
        public string Slug { get; set; }

        [JsonProperty("planning", NullValueHandling = NullValueHandling.Ignore)]
        public BasicPlanning Planning { get; set; }

        [JsonProperty("alt_text", NullValueHandling = NullValueHandling.Ignore)]
        public string AltText { get; set; }

        [JsonProperty("distributor", NullValueHandling = NullValueHandling.Ignore)]
        public ContentElementDistributor Distributor { get; set; }
    }

    public partial class IndigoAdditionalProperties
    {
        [JsonProperty("fullSizeResizeUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string FullSizeResizeUrl { get; set; }

        [JsonProperty("galleries", NullValueHandling = NullValueHandling.Ignore)]
        public PurpleGallery[] Galleries { get; set; }

        [JsonProperty("ingestionMethod", NullValueHandling = NullValueHandling.Ignore)]
        public string IngestionMethod { get; set; }

        [JsonProperty("keywords", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Keywords { get; set; }

        [JsonProperty("mime_type", NullValueHandling = NullValueHandling.Ignore)]
        public string MimeType { get; set; }

        [JsonProperty("originalName", NullValueHandling = NullValueHandling.Ignore)]
        public string OriginalName { get; set; }

        [JsonProperty("originalUrl", NullValueHandling = NullValueHandling.Ignore)]
        public Uri OriginalUrl { get; set; }

        [JsonProperty("owner", NullValueHandling = NullValueHandling.Ignore)]
        public string Owner { get; set; }

        [JsonProperty("proxyUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string ProxyUrl { get; set; }

        [JsonProperty("published")]
        public bool Published { get; set; }

        [JsonProperty("resizeUrl", NullValueHandling = NullValueHandling.Ignore)]
        public Uri ResizeUrl { get; set; }

        [JsonProperty("restricted", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Restricted { get; set; }

        [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
        public long? Version { get; set; }

        [JsonProperty("countryId", NullValueHandling = NullValueHandling.Ignore)]
        public long? CountryId { get; set; }

        [JsonProperty("iptc_job_identifier", NullValueHandling = NullValueHandling.Ignore)]
        public string IptcJobIdentifier { get; set; }

        [JsonProperty("iptc_source", NullValueHandling = NullValueHandling.Ignore)]
        public string IptcSource { get; set; }

        [JsonProperty("iptc_title", NullValueHandling = NullValueHandling.Ignore)]
        public string IptcTitle { get; set; }

        [JsonProperty("takenOn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? TakenOn { get; set; }

        [JsonProperty("usage_instructions", NullValueHandling = NullValueHandling.Ignore)]
        public string UsageInstructions { get; set; }

        [JsonProperty("focal_point", NullValueHandling = NullValueHandling.Ignore)]
        public FocalPoint FocalPoint { get; set; }
    }

    public partial class FocalPoint
    {
        [JsonProperty("min")]
        public long[] Min { get; set; }

        [JsonProperty("max")]
        public long[] Max { get; set; }
    }

    public partial class TentacledCredits
    {
        [JsonProperty("affiliation")]
        public FluffyAffiliation[] Affiliation { get; set; }

        [JsonProperty("by", NullValueHandling = NullValueHandling.Ignore)]
        public StickyBy[] By { get; set; }
    }

    public partial class StickyBy
    {
        [JsonProperty("byline", NullValueHandling = NullValueHandling.Ignore)]
        public string Byline { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
        public string Version { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public Image Image { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("social_links", NullValueHandling = NullValueHandling.Ignore)]
        public SocialLinkElement[] BySocialLinks { get; set; }

        [JsonProperty("socialLinks", NullValueHandling = NullValueHandling.Ignore)]
        public SocialLink[] SocialLinks { get; set; }

        [JsonProperty("additional_properties", NullValueHandling = NullValueHandling.Ignore)]
        public TentacledAdditionalProperties AdditionalProperties { get; set; }
    }

    public partial class BasicPlanning
    {
        [JsonProperty("internal_note")]
        public string InternalNote { get; set; }
    }

    public partial class TentacledSource
    {
        [JsonProperty("edit_url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri EditUrl { get; set; }

        [JsonProperty("system")]
        public string System { get; set; }

        [JsonProperty("additional_properties", NullValueHandling = NullValueHandling.Ignore)]
        public LabelClass AdditionalProperties { get; set; }
    }

    public partial class TentacledTaxonomy
    {
        [JsonProperty("associated_tasks", NullValueHandling = NullValueHandling.Ignore)]
        public object[] AssociatedTasks { get; set; }

        [JsonProperty("additional_properties", NullValueHandling = NullValueHandling.Ignore)]
        public LabelClass AdditionalProperties { get; set; }
    }

    public partial class Publishing
    {
        [JsonProperty("scheduled_operations")]
        public ScheduledOperations ScheduledOperations { get; set; }
    }

    public partial class ScheduledOperations
    {
        [JsonProperty("publish_edition")]
        public PublishEdition[] PublishEdition { get; set; }

        [JsonProperty("unpublish_edition")]
        public object[] UnpublishEdition { get; set; }
    }

    public partial class PublishEdition
    {
        [JsonProperty("operation")]
        public string Operation { get; set; }

        [JsonProperty("operation_date")]
        public DateTimeOffset OperationDate { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class RelatedContent
    {
        [JsonProperty("basic")]
        public object[] Basic { get; set; }

        [JsonProperty("redirect")]
        public object[] Redirect { get; set; }
    }

    public partial class Revision
    {
        [JsonProperty("revision_id")]
        public string RevisionId { get; set; }

        [JsonProperty("parent_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ParentId { get; set; }

        [JsonProperty("editions", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Editions { get; set; }

        [JsonProperty("branch")]
        public string Branch { get; set; }

        [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
        public string UserId { get; set; }

        [JsonProperty("published")]
        public bool Published { get; set; }
    }

    public partial class BodySource
    {
        [JsonProperty("system")]
        public string System { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("source_type", NullValueHandling = NullValueHandling.Ignore)]
        public string SourceType { get; set; }

        [JsonProperty("source_id", NullValueHandling = NullValueHandling.Ignore)]
        public string SourceId { get; set; }
    }

    public partial class BodyTaxonomy
    {
        [JsonProperty("sites", NullValueHandling = NullValueHandling.Ignore)]
        public PrimarySiteClass[] Sites { get; set; }

        [JsonProperty("tags")]
        public Tag[] Tags { get; set; }

        [JsonProperty("sections")]
        public PrimarySectionClass[] Sections { get; set; }

        [JsonProperty("primary_section")]
        public PrimarySectionClass PrimarySection { get; set; }

        [JsonProperty("primary_site", NullValueHandling = NullValueHandling.Ignore)]
        public PrimarySiteClass PrimarySite { get; set; }
    }

    public partial class PrimarySectionClass
    {
        [JsonProperty("_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("_website", NullValueHandling = NullValueHandling.Ignore)]
        public string Website { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
        public string Version { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public string Path { get; set; }

        [JsonProperty("parent_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ParentId { get; set; }

        [JsonProperty("parent", NullValueHandling = NullValueHandling.Ignore)]
        public PrimarySectionParent Parent { get; set; }

        [JsonProperty("additional_properties", NullValueHandling = NullValueHandling.Ignore)]
        public PrimarySectionAdditionalProperties AdditionalProperties { get; set; }

        [JsonProperty("_website_section_id", NullValueHandling = NullValueHandling.Ignore)]
        public string WebsiteSectionId { get; set; }

        [JsonProperty("referent", NullValueHandling = NullValueHandling.Ignore)]
        public ByReferent Referent { get; set; }
    }

    public partial class PrimarySectionAdditionalProperties
    {
        [JsonProperty("original")]
        public TentacledOriginal Original { get; set; }
    }

    public partial class TentacledOriginal
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("site")]
        public OriginalSite Site { get; set; }

        [JsonProperty("_admin")]
        public Admin Admin { get; set; }

        [JsonProperty("navigation")]
        public Navigation Navigation { get; set; }

        [JsonProperty("_website")]
        public string Website { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("order")]
        public PurpleOrder Order { get; set; }

        [JsonProperty("parent")]
        public PurpleParent Parent { get; set; }

        [JsonProperty("ancestors")]
        public PurpleAncestors Ancestors { get; set; }

        [JsonProperty("inactive")]
        public bool Inactive { get; set; }

        [JsonProperty("node_type")]
        public string NodeType { get; set; }
    }

    public partial class Admin
    {
        [JsonProperty("default_content")]
        public string DefaultContent { get; set; }

        [JsonProperty("alias_ids")]
        public string[] AliasIds { get; set; }

        [JsonProperty("tracking_node")]
        public string TrackingNode { get; set; }

        [JsonProperty("commercial_node")]
        public string CommercialNode { get; set; }

        [JsonProperty("url_path")]
        public Uri UrlPath { get; set; }
    }

    public partial class PurpleAncestors
    {
        [JsonProperty("default")]
        public string[] Default { get; set; }

        [JsonProperty("ellipsis", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Ellipsis { get; set; }

        [JsonProperty("toprail", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Toprail { get; set; }

        [JsonProperty("editions", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Editions { get; set; }

        [JsonProperty("footer", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Footer { get; set; }
    }

    public partial class Navigation
    {
        [JsonProperty("nav_title")]
        public string NavTitle { get; set; }

        [JsonProperty("nav_display")]
        [JsonConverter(typeof(PurpleParseStringConverter))]
        public bool? NavDisplay { get; set; }

        [JsonProperty("is_this_a_link_or_a_section")]
        public string IsThisALinkOrASection { get; set; }
    }

    public partial class PurpleOrder
    {
        [JsonProperty("ellipsis")]
        public long Ellipsis { get; set; }

        [JsonProperty("toprail", NullValueHandling = NullValueHandling.Ignore)]
        public long? Toprail { get; set; }

        [JsonProperty("default", NullValueHandling = NullValueHandling.Ignore)]
        public long? Default { get; set; }
    }

    public partial class PurpleParent
    {
        [JsonProperty("default")]
        public string Default { get; set; }

        [JsonProperty("ellipsis")]
        public string Ellipsis { get; set; }

        [JsonProperty("toprail")]
        public string Toprail { get; set; }

        [JsonProperty("editions")]
        public object Editions { get; set; }

        [JsonProperty("footer")]
        public object Footer { get; set; }
    }

    public partial class OriginalSite
    {
        [JsonProperty("site_description")]
        public string SiteDescription { get; set; }

        [JsonProperty("site_keywords")]
        public string SiteKeywords { get; set; }

        [JsonProperty("site_title")]
        public string SiteTitle { get; set; }

        [JsonProperty("headline")]
        public string Headline { get; set; }

        [JsonProperty("site_section")]
        public string SiteSection { get; set; }
    }

    public partial class PrimarySectionParent
    {
        [JsonProperty("default")]
        public string Default { get; set; }
    }

    public partial class PrimarySiteClass
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public object Description { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("parent_id")]
        public string ParentId { get; set; }

        [JsonProperty("additional_properties")]
        public PrimarySiteAdditionalProperties AdditionalProperties { get; set; }
    }

    public partial class PrimarySiteAdditionalProperties
    {
        [JsonProperty("original")]
        public StickyOriginal Original { get; set; }
    }

    public partial class StickyOriginal
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("_admin")]
        public Admin Admin { get; set; }

        [JsonProperty("site")]
        public OriginalSite Site { get; set; }

        [JsonProperty("navigation")]
        public Navigation Navigation { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("parent")]
        public string Parent { get; set; }

        [JsonProperty("inactive")]
        public bool Inactive { get; set; }

        [JsonProperty("node_type")]
        public string NodeType { get; set; }
    }

    public partial class Tag
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }
    }

    public partial class BodyWebsites
    {
        [JsonProperty("nj", NullValueHandling = NullValueHandling.Ignore)]
        public Nj Nj { get; set; }

        [JsonProperty("cleveland", NullValueHandling = NullValueHandling.Ignore)]
        public Cleveland Cleveland { get; set; }

        [JsonProperty("silive", NullValueHandling = NullValueHandling.Ignore)]
        public PennliveClass Silive { get; set; }

        [JsonProperty("oregonlive", NullValueHandling = NullValueHandling.Ignore)]
        public PennliveClass Oregonlive { get; set; }

        [JsonProperty("pennlive", NullValueHandling = NullValueHandling.Ignore)]
        public PennliveClass Pennlive { get; set; }

        [JsonProperty("masslive", NullValueHandling = NullValueHandling.Ignore)]
        public WebsitesAl Masslive { get; set; }

        [JsonProperty("advancelocal", NullValueHandling = NullValueHandling.Ignore)]
        public Advancelocal Advancelocal { get; set; }

        [JsonProperty("syracuse", NullValueHandling = NullValueHandling.Ignore)]
        public PennliveClass Syracuse { get; set; }

        [JsonProperty("mlive", NullValueHandling = NullValueHandling.Ignore)]
        public Mlive Mlive { get; set; }

        [JsonProperty("lehighvalleylive", NullValueHandling = NullValueHandling.Ignore)]
        public Cleveland Lehighvalleylive { get; set; }

        [JsonProperty("al", NullValueHandling = NullValueHandling.Ignore)]
        public WebsitesAl Al { get; set; }
    }

    public partial class Advancelocal
    {
        [JsonProperty("website_section")]
        public AdvancelocalWebsiteSection WebsiteSection { get; set; }
    }

    public partial class AdvancelocalWebsiteSection
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("_website")]
        public string Website { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public object Description { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("parent_id")]
        public string ParentId { get; set; }

        [JsonProperty("parent")]
        public PrimarySectionParent Parent { get; set; }

        [JsonProperty("additional_properties")]
        public IndecentAdditionalProperties AdditionalProperties { get; set; }

        [JsonProperty("_website_section_id")]
        public string WebsiteSectionId { get; set; }
    }

    public partial class IndecentAdditionalProperties
    {
        [JsonProperty("original")]
        public IndigoOriginal Original { get; set; }
    }

    public partial class IndigoOriginal
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("_admin")]
        public Admin Admin { get; set; }

        [JsonProperty("site")]
        public OriginalSite Site { get; set; }

        [JsonProperty("navigation")]
        public Navigation Navigation { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("_website")]
        public string Website { get; set; }

        [JsonProperty("parent")]
        public FluffyParent Parent { get; set; }

        [JsonProperty("ancestors")]
        public FluffyAncestors Ancestors { get; set; }

        [JsonProperty("inactive")]
        public bool Inactive { get; set; }

        [JsonProperty("node_type")]
        public string NodeType { get; set; }

        [JsonProperty("order")]
        public FluffyOrder Order { get; set; }
    }

    public partial class FluffyAncestors
    {
        [JsonProperty("default")]
        public object[] Default { get; set; }

        [JsonProperty("ellipsis")]
        public string[] Ellipsis { get; set; }
    }

    public partial class FluffyOrder
    {
        [JsonProperty("ellipsis")]
        public long Ellipsis { get; set; }
    }

    public partial class FluffyParent
    {
        [JsonProperty("default")]
        public string Default { get; set; }

        [JsonProperty("ellipsis")]
        public string Ellipsis { get; set; }
    }

    public partial class WebsitesAl
    {
        [JsonProperty("website_section")]
        public AlWebsiteSection WebsiteSection { get; set; }
    }

    public partial class AlWebsiteSection
    {
        [JsonProperty("_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("_website", NullValueHandling = NullValueHandling.Ignore)]
        public string Website { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
        public string Version { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public string Path { get; set; }

        [JsonProperty("parent_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ParentId { get; set; }

        [JsonProperty("parent", NullValueHandling = NullValueHandling.Ignore)]
        public PrimarySectionParent Parent { get; set; }

        [JsonProperty("additional_properties", NullValueHandling = NullValueHandling.Ignore)]
        public PrimarySectionAdditionalProperties AdditionalProperties { get; set; }

        [JsonProperty("_website_section_id", NullValueHandling = NullValueHandling.Ignore)]
        public string WebsiteSectionId { get; set; }

        [JsonProperty("referent", NullValueHandling = NullValueHandling.Ignore)]
        public ByReferent Referent { get; set; }
    }

    public partial class Cleveland
    {
        [JsonProperty("website_section")]
        public ClevelandWebsiteSection WebsiteSection { get; set; }

        [JsonProperty("website_url", NullValueHandling = NullValueHandling.Ignore)]
        public string WebsiteUrl { get; set; }
    }

    public partial class ClevelandWebsiteSection
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("referent", NullValueHandling = NullValueHandling.Ignore)]
        public ByReferent Referent { get; set; }

        [JsonProperty("_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("_website", NullValueHandling = NullValueHandling.Ignore)]
        public string Website { get; set; }

        [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
        public string Version { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public string Path { get; set; }

        [JsonProperty("parent_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ParentId { get; set; }

        [JsonProperty("parent", NullValueHandling = NullValueHandling.Ignore)]
        public PrimarySectionParent Parent { get; set; }

        [JsonProperty("additional_properties", NullValueHandling = NullValueHandling.Ignore)]
        public HilariousAdditionalProperties AdditionalProperties { get; set; }

        [JsonProperty("_website_section_id", NullValueHandling = NullValueHandling.Ignore)]
        public string WebsiteSectionId { get; set; }
    }

    public partial class HilariousAdditionalProperties
    {
        [JsonProperty("original")]
        public IndecentOriginal Original { get; set; }
    }

    public partial class IndecentOriginal
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("site")]
        public OriginalSite Site { get; set; }

        [JsonProperty("_admin")]
        public Admin Admin { get; set; }

        [JsonProperty("navigation")]
        public Navigation Navigation { get; set; }

        [JsonProperty("_website")]
        public string Website { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("order")]
        public TentacledOrder Order { get; set; }

        [JsonProperty("parent")]
        public PurpleParent Parent { get; set; }

        [JsonProperty("ancestors")]
        public PurpleAncestors Ancestors { get; set; }

        [JsonProperty("inactive")]
        public bool Inactive { get; set; }

        [JsonProperty("node_type")]
        public string NodeType { get; set; }
    }

    public partial class TentacledOrder
    {
        [JsonProperty("toprail")]
        public long Toprail { get; set; }

        [JsonProperty("ellipsis")]
        public long Ellipsis { get; set; }
    }

    public partial class Mlive
    {
        [JsonProperty("website_section")]
        public MliveWebsiteSection WebsiteSection { get; set; }

        [JsonProperty("website_url", NullValueHandling = NullValueHandling.Ignore)]
        public string WebsiteUrl { get; set; }
    }

    public partial class MliveWebsiteSection
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("_website")]
        public string Website { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("parent_id")]
        public string ParentId { get; set; }

        [JsonProperty("parent")]
        public PrimarySectionParent Parent { get; set; }

        [JsonProperty("additional_properties")]
        public HilariousAdditionalProperties AdditionalProperties { get; set; }

        [JsonProperty("_website_section_id")]
        public string WebsiteSectionId { get; set; }
    }

    public partial class Nj
    {
        [JsonProperty("website_section")]
        public NjWebsiteSection WebsiteSection { get; set; }

        [JsonProperty("website_url", NullValueHandling = NullValueHandling.Ignore)]
        public string WebsiteUrl { get; set; }
    }

    public partial class NjWebsiteSection
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("_website")]
        public string Website { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("parent_id")]
        public string ParentId { get; set; }

        [JsonProperty("parent")]
        public PrimarySectionParent Parent { get; set; }

        [JsonProperty("additional_properties")]
        public PrimarySectionAdditionalProperties AdditionalProperties { get; set; }

        [JsonProperty("_website_section_id")]
        public string WebsiteSectionId { get; set; }
    }

    public partial class PennliveClass
    {
        [JsonProperty("website_section")]
        public AlWebsiteSection WebsiteSection { get; set; }

        [JsonProperty("website_url", NullValueHandling = NullValueHandling.Ignore)]
        public string WebsiteUrl { get; set; }
    }

    public partial class Trigger
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("referent_update")]
        public bool ReferentUpdate { get; set; }

        [JsonProperty("priority")]
        public string Priority { get; set; }
    }

    public partial struct PublishDate
    {
        public bool? Bool;
        public DateTimeOffset? DateTime;

        public static implicit operator PublishDate(bool Bool) => new PublishDate { Bool = Bool };
        public static implicit operator PublishDate(DateTimeOffset DateTime) => new PublishDate { DateTime = DateTime };
    }

    public partial struct Id
    {
        public long? Integer;
        public string String;

        public static implicit operator Id(long Integer) => new Id { Integer = Integer };
        public static implicit operator Id(string String) => new Id { String = String };
    }

    public static class Serialize
    {
        public static string ToJson(this FullContentOperation[] self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                PublishDateConverter.Singleton,
                IdConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class PublishDateConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(PublishDate) || t == typeof(PublishDate?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Boolean:
                    var boolValue = serializer.Deserialize<bool>(reader);
                    return new PublishDate { Bool = boolValue };
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    DateTimeOffset dt;
                    if (DateTimeOffset.TryParse(stringValue, out dt))
                    {
                        return new PublishDate { DateTime = dt };
                    }
                    break;
            }
            throw new Exception("Cannot unmarshal type PublishDate");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (PublishDate)untypedValue;
            if (value.Bool != null)
            {
                serializer.Serialize(writer, value.Bool.Value);
                return;
            }
            if (value.DateTime != null)
            {
                serializer.Serialize(writer, value.DateTime.Value.ToString("o", System.Globalization.CultureInfo.InvariantCulture));
                return;
            }
            throw new Exception("Cannot marshal type PublishDate");
        }

        public static readonly PublishDateConverter Singleton = new PublishDateConverter();
    }

    internal class IdConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Id) || t == typeof(Id?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    var integerValue = serializer.Deserialize<long>(reader);
                    return new Id { Integer = integerValue };
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    return new Id { String = stringValue };
            }
            throw new Exception("Cannot unmarshal type Id");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (Id)untypedValue;
            if (value.Integer != null)
            {
                serializer.Serialize(writer, value.Integer.Value);
                return;
            }
            if (value.String != null)
            {
                serializer.Serialize(writer, value.String);
                return;
            }
            throw new Exception("Cannot marshal type Id");
        }

        public static readonly IdConverter Singleton = new IdConverter();
    }

    internal class PurpleParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(bool) || t == typeof(bool?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            bool b;
            if (Boolean.TryParse(value, out b))
            {
                return b;
            }
            throw new Exception("Cannot unmarshal type bool");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (bool)untypedValue;
            var boolString = value ? "true" : "false";
            serializer.Serialize(writer, boolString);
            return;
        }

        public static readonly PurpleParseStringConverter Singleton = new PurpleParseStringConverter();
    }

    internal class FluffyParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly FluffyParseStringConverter Singleton = new FluffyParseStringConverter();
    }
}
