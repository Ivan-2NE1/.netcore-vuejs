using System.Collections.Generic;

namespace VSAND.Data.ViewModels.News
{
    public class StoryApiResponse
    {
        public List<Story> Featured { get; set; }
        public List<Story> River { get; set; }

        public StoryApiResponse()
        {
            this.Featured = new List<Story>();
            this.River = new List<Story>();
        }
    }
}
