using System;
using System.Collections.Generic;
using VSAND.Data.ViewModels.News;

namespace VSAND.Arc.Data.Entities
{
    public partial class ContentOperation
    {
        public ContentOperation()
        {
            ContentTags = new HashSet<ContentTag>();
        }

        public string ContentOperationId { get; set; }

        public string Publication { get; set; }

        public DateTime CreatedDateUtc { get; set; }

        public DateTime ModifiedDateUtc { get; set; }

        public string LastOperation { get; set; }

        public DateTime LastOperationDateUtc { get; set; }

        public bool Published { get; set; }

        public DateTime? PublishDateUtc { get; set; }

        public string BodyType { get; set; }

        public string CanonicalUrl { get; set; }

        public string Headline { get; set; }

        public string FeatureImageUrl { get; set; }

        public string ByLine { get; set; }

        public string ContentOperationObject { get; set; }

        public virtual ICollection<ContentTag> ContentTags { get; set; }

        public Story ToViewModel()
        {
            return new Story
            {
                StoryId = this.ContentOperationId,
                BodyType = this.BodyType,
                ByLine = this.ByLine,
                CanonicalUrl = this.CanonicalUrl,
                CreatedDateUtc = this.CreatedDateUtc,
                FeatureImageUrl = this.FeatureImageUrl,
                Headline = this.Headline,
                LastOperation = this.LastOperation,
                LastOperationDateUtc = this.LastOperationDateUtc,
                ModifiedDateUtc = this.ModifiedDateUtc,
                Publication = this.Publication,
                PublishDateUtc = this.PublishDateUtc,
                Published = this.Published
            };
        }
    }
}
