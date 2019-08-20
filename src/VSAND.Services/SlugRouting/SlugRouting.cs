using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.Repositories;
using VSAND.Services.Cache;

namespace VSAND.Services.SlugRouting
{
    public class SlugRouting : ISlugRouting
    {
        private IUnitOfWork _uow;
        private ICache _cache;

        public SlugRouting(IUnitOfWork uow, ICache cache)
        {
            _uow = uow ?? throw new ArgumentException("Unit of Work is null");
            _cache = cache ?? throw new ArgumentException("Cache is null");
        }

        public async Task<VsandEntitySlug> GetRoute(string path)
        {
            path = path.Trim().ToLowerInvariant();

            string sCacheKey = Cache.Keys.SlugRoute(path);
            var cachedRoute = await _cache.GetAsync<VsandEntitySlug>(sCacheKey);
            if (cachedRoute != null)
            {
                return cachedRoute;
            }

            var oRoute = await _uow.EntitySlugs.Single(es => es.Slug.Equals(path));
            if (oRoute != null)
            {
                await _cache.SetAsync(sCacheKey, oRoute);
            }

            return oRoute;
        }
    }
}
