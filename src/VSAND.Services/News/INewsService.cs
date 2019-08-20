using System;
using System.Threading.Tasks;
using VSAND.Data.Repositories;
using VSAND.Data.ViewModels.News;

namespace VSAND.Services.News
{
    public interface INewsService
    {
        Task<StoryApiResponse> GetNewsStoriesAsync();
        Task<PagedResult<Story>> SearchStoriesAsync(string headline = "", bool? published = null, DateTime? startDate = null, DateTime? endDate = null, int pageSize = 25, int page = 1);
    }
}
