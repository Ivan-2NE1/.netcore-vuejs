using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using VSAND.Arc.Data.Entities;

namespace VSAND.Arc.ViewModels.Arc.Slim
{
    public class SlimContentOperation
    {
        public string Id { get; set; }

        public Body Body { get; set; }

        public string Operation { get; set; }

        public DateTime Date { get; set; }

        public bool Published { get; set; }

        public string ContentOperationObject { get; set; }

        public ContentOperation ToEntityModel(string sRecord)
        {
            string byLine = null;
            var credit = this.Body.Credits.By.FirstOrDefault();
            if (credit != null && credit.AdditionalProperties != null && credit.AdditionalProperties.ContainsKey("original"))
            {
                byLine = credit.AdditionalProperties["original"].ByLine;
            }

            return new ContentOperation
            {
                ContentOperationId = this.Id,
                Publication = this.Body.CanonicalWebsite,
                LastOperation = this.Operation,
                LastOperationDateUtc = this.Date, // THIS SHOULD BE READ FROM THE RECORD OBJECT
                Published = this.Published,
                PublishDateUtc = this.Body.PublishDate,
                BodyType = this.Body.Type,
                CanonicalUrl = this.Body.CanonicalUrl,
                Headline = this.Body.Headlines.Basic,
                FeatureImageUrl = this.Body.ContentElements.FirstOrDefault(ce => ce.Type == "image")?.Url,
                ByLine = byLine,
                ContentOperationObject = sRecord
            };
        }
    }

    public class Body
    {
        [JsonProperty("canonical_website")]
        public string CanonicalWebsite { get; set; }

        [JsonProperty("publish_date")]
        public DateTime? PublishDate { get; set; }

        public string Type { get; set; }

        [JsonProperty("canonical_url")]
        public string CanonicalUrl { get; set; }

        public Headlines Headlines { get; set; }

        [JsonProperty("content_elements")]
        public List<ContentElement> ContentElements { get; set; } = new List<ContentElement>();

        public Credits Credits { get; set; }

        public Dictionary<string, Website> Websites { get; set; } = new Dictionary<string, Website>();
    }

    public class Headlines
    {
        public string Basic { get; set; }
    }

    public class ContentElement
    {
        public string Type { get; set; }

        public string Url { get; set; }
    }

    public class Credits
    {
        public List<Credit> By { get; set; }
    }

    public class Credit
    {
        [JsonProperty("additional_properties")]
        public Dictionary<string, Author> AdditionalProperties { get; set; }
    }

    public class Author
    {
        public string ByLine { get; set; }
    }

    public class Website
    {
        [JsonProperty("website_section")]
        public WebsiteSection WebsiteSection { get; set; }
    }

    public class WebsiteSection
    {
        [JsonProperty("_id")]
        public string Id { get; set; }
    }
}