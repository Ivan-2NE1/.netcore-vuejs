using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Arc.Data;
using VSAND.Arc.Data.Entities;
using VSAND.Data.Repositories;
using VSAND.Data.ViewModels.News;

namespace VSAND.Arc.Controllers
{
    [Route("[controller]")]
    public class ApiController : Controller
    {
        private readonly ArcContentContext _context;

        public ApiController(ArcContentContext context)
        {
            _context = context;
        }

        [HttpGet("Stories")]
        public async Task<StoryApiResponse> Stories(string publication, List<string> featured, int pageSize = 10, int page = 1)
        {
            var oRet = new StoryApiResponse();

            if (page < 1)
            {
                page = 1;
            }

            if (featured != null && featured.Count > 0)
            {
                // take the first 10 requested stories in the list
                featured = featured.Take(10).ToList();

                var featuredStories = await _context.ContentOperation
                                                .Where(co => co.Published == true && co.Publication == publication && co.BodyType == "story" && featured.Contains(co.ContentOperationId))
                                                .OrderBy(co => featured.IndexOf(co.ContentOperationId))
                                                .ToListAsync();

                oRet.Featured.AddRange(featuredStories.Select(s => s.ToViewModel()));

                // if there are less than 10 featured stories requested, fill the remaining featured stories in reverse chronological order
                if (oRet.Featured.Count < 10)
                {
                    featuredStories = await _context.ContentOperation
                                                        .Where(co => co.Published == true && co.Publication == publication && co.BodyType == "story" && !featured.Contains(co.ContentOperationId))
                                                        .OrderByDescending(co => co.PublishDateUtc)
                                                        .Take(10 - oRet.Featured.Count)
                                                        .ToListAsync();

                    oRet.Featured.AddRange(featuredStories.Select(s => s.ToViewModel()));
                }


                // exclude these stories from the river
                featured = oRet.Featured.Select(s => s.StoryId).ToList();
            }
            else
            {
                featured = new List<string>();
            }

            var revChronStories = await _context.ContentOperation
                                                    .Where(co => co.Published == true && co.Publication == publication && co.BodyType == "story" && !featured.Contains(co.ContentOperationId))
                                                    .OrderByDescending(co => co.PublishDateUtc)
                                                    .Skip((page - 1) * pageSize)
                                                    .Take(pageSize)
                                                    .ToListAsync();

            oRet.River.AddRange(revChronStories.Select(s => s.ToViewModel()));

            return oRet;
        }

        [HttpGet("TaggedStories")]
        public async Task<List<Story>> TaggedStories(string publication, string tag)
        {
            var stories = await _context.ContentOperation.Where(co => co.Published == true && co.Publication == publication && co.BodyType == "story" && co.ContentTags.Any(t => t.Tag == tag))
                                                         .OrderByDescending(co => co.PublishDateUtc)
                                                         .Take(10)
                                                         .ToListAsync();

            return stories.Select(s => s.ToViewModel()).ToList();
        }

        #region VSAND.Backend Story Tagging Endpoints
        [HttpGet("StoryAutocomplete")]
        public async Task<List<Story>> StoryAutocomplete(string publication, string headline)
        {
            var stories = await _context.ContentOperation.Where(co => co.Publication == publication && co.BodyType == "story" && co.Headline.Contains(headline))
                                                         .OrderByDescending(co => co.PublishDateUtc)
                                                         .Take(10)
                                                         .ToListAsync();

            return stories.Select(s => s.ToViewModel()).ToList();
        }

        [HttpPost("ContentTag")]
        public async Task<bool> ContentTag([FromForm] ContentTagRequest request)
        {
            var storyExists = await _context.ContentOperation.AnyAsync(co => co.ContentOperationId == request.StoryId && co.Publication == request.Publication);
            if (!storyExists)
            {
                return false;
            }

            var contentTag = new ContentTag
            {
                ContentOperationId = request.StoryId,
                Publication = request.Publication,
                Tag = request.Tag
            };

            _context.ContentTag.Add(contentTag);

            try
            {
                var iRowsChanged = await _context.SaveChangesAsync();
                if (iRowsChanged != 1)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        [HttpGet("StorySearch")]
        public async Task<PagedResult<Story>> StorySearch(string publication, string headline = "", bool? published = null, DateTime? startDate = null, DateTime? endDate = null, int pageSize = 25, int page = 1)
        {
            if (page < 1)
            {
                page = 1;
            }

            var oQuery = _context.ContentOperation.Where(co => co.Publication == publication && co.BodyType == "story");

            if (!string.IsNullOrEmpty(headline))
            {
                oQuery = oQuery.Where(co => co.Headline.Contains(headline));
            }

            if (published.HasValue && published.Value == true)
            {
                oQuery = oQuery.Where(co => co.Published == published.Value);
            }

            if (startDate.HasValue)
            {
                oQuery = oQuery.Where(co => co.PublishDateUtc > startDate.Value);
            }

            if (endDate.HasValue)
            {
                oQuery = oQuery.Where(co => co.PublishDateUtc < endDate.Value);
            }

            var stories = await oQuery.OrderByDescending(co => co.PublishDateUtc)
                                        .Skip((page - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();

            var totalResults = await oQuery.CountAsync();

            var oRet = new PagedResult<Story>(stories.Select(s => s.ToViewModel()).ToList(), totalResults, pageSize, page);
            return oRet;
        }
        #endregion
    }
}