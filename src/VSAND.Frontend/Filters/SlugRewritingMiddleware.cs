using Microsoft.AspNetCore.Http;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Services.SlugRouting;

namespace VSAND.Frontend.Filters
{
    public class SlugRewritingMiddleware
    {
        NLog.ILogger log = LogManager.GetCurrentClassLogger();

        private readonly RequestDelegate _next;
        private ISlugRouting _slugs;

        //Your constructor will have the dependencies needed for database access
        public SlugRewritingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ISlugRouting slugs)
        {
            var path = context.Request.Path.ToUriComponent();

            if (!string.IsNullOrWhiteSpace(path))
            {
                _slugs = slugs;
                await TryHandleRequest(context, path);
            }

            //Let the next middleware (MVC routing) handle the request
            //In case the path was updated, the MVC routing will see the updated path
            await _next.Invoke(context);

        }

        private async Task TryHandleRequest(HttpContext context, string path)
        {
            // we need to get the slug portion out of the requested path
            if (path.StartsWith("/"))
            {
                path = path.Substring(1);
            }
            var oPathData = path.ToLower().Split("/");
            string pathSlug = "";
            string secondDimension = "";
            int keepAfter = 0;

            switch (oPathData[0])
            {
                case "siteapi":
                case "game":
                case "scores":
                case "watch":
                    // Ignore requests for the siteapi, game, etc (Anything that uses default routing!)
                    return;

                case "school":
                    // next part should be school name slug
                    if (oPathData.Length > 1)
                    {
                        pathSlug = oPathData[1];
                        keepAfter = 2;

                        // because school also contains teams, which are just identified by their sport slug
                        if (oPathData.Length > 2)
                        {
                            secondDimension = oPathData[2];
                        }
                    }
                    break;

                case "player":
                    // next part should be the player slug
                    if (oPathData.Length > 1)
                    {
                        pathSlug = oPathData[1];
                        keepAfter = 2;

                        // because player also contains teams, which are just identified by their sport slug & schedule year
                        if (oPathData.Length > 2)
                        {
                            secondDimension = oPathData[2];
                        }
                    }
                    break;

                default:
                    // no other matches, so use the first portion of the path as the slug
                    pathSlug = oPathData[0];
                    break;
            }

            // if we didn't find anything to slug from, exit here
            if (string.IsNullOrEmpty(pathSlug))
            {
                log.Info($"Original Request: {path}");
                log.Info("Path slug is empty, exiting");
                return;
            }

            log.Info($"Getting entity slug for {pathSlug}");

            var slugInfo = await _slugs.GetRoute(pathSlug);
            if (slugInfo != null)
            {
                switch (slugInfo.EntityType.Trim().ToLowerInvariant())
                {
                    case "sport":
                        log.Info("Processing sport slug");
                        context.Items.Add("SportSlug", slugInfo.Slug);
                        keepAfter = 1;
                        context.Request.Path = "/sport/" + slugInfo.EntityId + getKeepRoute(oPathData, keepAfter);

                        // because some sport paths also include season slugs
                        if (oPathData.Length > 1)
                        {
                            secondDimension = oPathData[1];
                        }

                        if (!string.IsNullOrEmpty(secondDimension))
                        {
                            log.Info($"sport has second dimension value: {secondDimension}");
                            // we are checking for things now, like the season indicator for standings
                            switch (secondDimension)
                            {
                                case "standings":
                                    int stdSyId = await processSeasonId(context, oPathData);
                                    if (stdSyId > 0)
                                    {
                                        context.Request.Path = "/sport/" + slugInfo.EntityId + "/standings/" + stdSyId;
                                    }
                                    break;

                                case "powerpoints":
                                    int ppSyId = await processSeasonId(context, oPathData);
                                    if (ppSyId > 0)
                                    {
                                        context.Request.Path = "/sport/" + slugInfo.EntityId + "/powerpoints/" + ppSyId;
                                    }
                                    break;
                            }
                        }

                        break;

                    case "player":
                        context.Items.Add("PlayerSlug", slugInfo.Slug);
                        context.Request.Path = "/player/" + slugInfo.EntityId + getKeepRoute(oPathData, keepAfter);
                        if (!string.IsNullOrEmpty(secondDimension))
                        {
                            // this has to be a sport slug, followed by a season/
                            var secondSlugInfo = await _slugs.GetRoute(secondDimension);
                            if (secondSlugInfo != null && secondSlugInfo.EntityType.Equals("sport", StringComparison.OrdinalIgnoreCase))
                            {
                                keepAfter = 3;
                                context.Items.Add("SportSlug", secondSlugInfo.Slug);
                                // what we are trying to load here is a specific team
                                context.Request.Path = "/player/" + slugInfo.EntityId + "/" + secondSlugInfo.EntityId + "/0" + getKeepRoute(oPathData, keepAfter);

                                var seasonIdx = Array.IndexOf(oPathData, "season");
                                var seasonValueIdx = -1;
                                if (seasonIdx >= 0)
                                {
                                    seasonValueIdx = seasonIdx + 1;
                                    if (seasonValueIdx >= oPathData.Length)
                                    {
                                        seasonValueIdx = -1;
                                    }
                                }
                                if (seasonValueIdx >= 0)
                                {
                                    var seasonSlug = oPathData[seasonValueIdx];
                                    var seasonInfo = await _slugs.GetRoute(seasonSlug);
                                    context.Items.Add("SeasonSlug", seasonInfo.Slug);

                                    // remove season AND season slug from the path data
                                    int[] remIdx = { seasonIdx, seasonValueIdx };
                                    oPathData = oPathData.Where((source, index) => !remIdx.Contains(index)).ToArray();

                                    context.Request.Path = "/player/" + slugInfo.EntityId + "/" + secondSlugInfo.EntityId + "/" + seasonInfo.EntityId + getKeepRoute(oPathData, keepAfter);
                                }
                            }
                        }
                        break;


                    case "school":
                        //TODO: Need list of valid second-path actions that are available for the school (to skip sport sub-processing step to get to team)
                        context.Items.Add("SchoolSlug", slugInfo.Slug);
                        context.Request.Path = "/school/" + slugInfo.EntityId + getKeepRoute(oPathData, keepAfter);
                        if (!string.IsNullOrEmpty(secondDimension))
                        {
                            if (secondDimension.Equals("schedule", StringComparison.OrdinalIgnoreCase))
                            {
                                log.Info("Working with school schedule default date handling");
                                // they are requesting a specific date to view
                                // there should be 3 args after this for year, month, and day
                                //3,4, 5 -> 4, 5, 3
                                if (oPathData.Length >= 5)
                                {
                                    context.Request.Path = "/school/" + slugInfo.EntityId;
                                    var oParams = new List<KeyValuePair<string, string>>();
                                    oParams.Add(new KeyValuePair<string, string>("viewdate", oPathData[4] + "/" + oPathData[5] + "/" + oPathData[3]));
                                    var oQs = QueryString.Create(oParams);
                                    context.Request.QueryString = oQs;
                                    log.Info("Rewrite path to " + context.Request.QueryString.ToString());
                                }
                                return;
                            }

                            // if the second dimension is NOT in the list of defined routes for the school, it is most likely the setup to a team entry
                            var secondSlugInfo = await _slugs.GetRoute(secondDimension);
                            if (secondSlugInfo != null && secondSlugInfo.EntityType.Equals("sport", StringComparison.OrdinalIgnoreCase))
                            {
                                keepAfter = 3;
                                context.Items.Add("SportSlug", secondSlugInfo.Slug);
                                // what we are trying to load here is a TEAM!
                                context.Request.Path = "/teams/" + slugInfo.EntityId + "/" + secondSlugInfo.EntityId + "/0" + getKeepRoute(oPathData, keepAfter);

                                var seasonIdx = Array.IndexOf(oPathData, "season");
                                var seasonValueIdx = -1;
                                if (seasonIdx >= 0)
                                {
                                    seasonValueIdx = seasonIdx + 1;
                                    if (seasonValueIdx >= oPathData.Length)
                                    {
                                        seasonValueIdx = -1;
                                    }
                                }
                                if (seasonValueIdx >= 0)
                                {
                                    var seasonSlug = oPathData[seasonValueIdx];
                                    var seasonInfo = await _slugs.GetRoute(seasonSlug);
                                    context.Items.Add("SeasonSlug", seasonInfo.Slug);

                                    // remove season AND season slug from the path data
                                    int[] remIdx = { seasonIdx, seasonValueIdx };
                                    oPathData = oPathData.Where((source, index) => !remIdx.Contains(index)).ToArray();

                                    context.Request.Path = "/teams/" + slugInfo.EntityId + "/" + secondSlugInfo.EntityId + "/" + seasonInfo.EntityId + getKeepRoute(oPathData, keepAfter);
                                }
                            }
                        }
                        break;
                }
            }

            log.Info($"Rewritten Request: {context.Request.Path}");
        }

        private string getKeepRoute(string[] pathData, int keepAfter)
        {
            var keep = string.Join("/", pathData.ToList().Skip(keepAfter).ToArray());
            if (!string.IsNullOrEmpty(keep))
            {
                keep = "/" + keep;
            }
            return keep;
        }

        private void removeSeasonPathInfo(ref string[] pathData)
        {
            var seasonIdx = Array.IndexOf(pathData, "season");
            var seasonValueIdx = -1;
            if (seasonIdx >= 0)
            {
                seasonValueIdx = seasonIdx + 1;
                if (seasonValueIdx >= pathData.Length)
                {
                    seasonValueIdx = -1;
                }
            }

            if (seasonValueIdx >= 0)
            {
                // remove season AND season slug from the path data
                int[] remIdx = { seasonIdx, seasonValueIdx };
                pathData = pathData.Where((source, index) => !remIdx.Contains(index)).ToArray();
            }
        }

        private async Task<int> processSeasonId(HttpContext context, string[] pathData)
        {
            int scheduleYearId = 0;

            var seasonIdx = Array.IndexOf(pathData, "season");
            var seasonValueIdx = -1;
            if (seasonIdx >= 0)
            {
                seasonValueIdx = seasonIdx + 1;
                if (seasonValueIdx >= pathData.Length)
                {
                    seasonValueIdx = -1;
                }
            }
            if (seasonValueIdx >= 0)
            {
                var seasonSlug = pathData[seasonValueIdx];
                var seasonInfo = await _slugs.GetRoute(seasonSlug);
                if (seasonInfo != null)
                {
                    scheduleYearId = seasonInfo.EntityId;
                    context.Items.Add("SeasonSlug", seasonInfo.Slug);
                }
            }
            return scheduleYearId;
        }
    }
}
