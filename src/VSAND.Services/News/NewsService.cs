using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Data.Repositories;
using VSAND.Data.ViewModels.News;

namespace VSAND.Services.News
{
    public class NewsService : INewsService
    {
        private readonly IConfiguration Configuration;

        public NewsService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<StoryApiResponse> GetNewsStoriesAsync()
        {
            var oRet = new StoryApiResponse();

            var client = new RestClient(Configuration["VsandArcUrl"]);
            var request = new RestRequest("Api/Stories");
            request.AddQueryParameter("publication", Configuration["PubName"]);

            var response = await client.ExecuteTaskAsync(request);
            if (response.IsSuccessful)
            {
                oRet = JsonConvert.DeserializeObject<StoryApiResponse>(response.Content);
            }

            // generate canonical Urls using the PubUrl environment variable
            foreach (var story in oRet.Featured)
            {
                story.CanonicalUrl = Configuration["PubUrl"] + story.CanonicalUrl;
            }

            foreach (var story in oRet.River)
            {
                story.CanonicalUrl = Configuration["PubUrl"] + story.CanonicalUrl;
            }

            return oRet;
        }

        public async Task<PagedResult<Story>> SearchStoriesAsync(string headline = "", bool? published = null, DateTime? startDate = null, DateTime? endDate = null, int pageSize = 25, int page = 1)
        {
            var oRet = new PagedResult<Story>(new List<Story>(), 0, pageSize, page);

            var client = new RestClient(Configuration["VsandArcUrl"]);
            var request = new RestRequest("Api/StorySearch");

            request.AddQueryParameter("publication", Configuration["PubName"]);
            request.AddQueryParameter("pageSize", pageSize.ToString());
            request.AddQueryParameter("pageNumber", page.ToString());

            if (!string.IsNullOrEmpty(headline))
            {
                request.AddQueryParameter("headline", headline);
            }

            if (published.HasValue)
            {
                request.AddQueryParameter("published", published.Value.ToString());
            }

            if (startDate.HasValue)
            {
                request.AddQueryParameter("startDate", startDate.Value.ToString());
            }

            if (endDate.HasValue)
            {
                request.AddQueryParameter("endDate", endDate.Value.ToString());
            }

            var response = await client.ExecuteTaskAsync(request);
            if (response.IsSuccessful)
            {
                oRet = JsonConvert.DeserializeObject<PagedResult<Story>>(response.Content);
            }

            return oRet;
        }
    }
}
