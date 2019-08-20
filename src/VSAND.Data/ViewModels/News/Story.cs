using System;

namespace VSAND.Data.ViewModels.News
{
    public class Story
    {
        public string StoryId { get; set; }

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
    }
}
