using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.Repositories;
using VSAND.Data.ViewModels;
using VSAND.Services.Cache;

namespace VSAND.Services.Data.CMS
{
    public class ConfigService : IConfigService
    {
        private IUnitOfWork _uow;
        private ICache _cache;

        public ConfigService(IUnitOfWork uow, ICache cache)
        {
            _uow = uow;
            _cache = cache;
        }

        public async Task<T> GetConfigValueCachedAsync<T>(string configCat, string configName)
        {
            string cacheKey = Cache.Keys.CMSConfig(configCat, configName);
            T retval = await _cache.GetAsync<T>(cacheKey);
            if (retval == null || string.IsNullOrWhiteSpace(retval.ToString()))
            {
                string val = await GetConfigValueAsync(configCat, configName);
                var oRet = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(val);
                if (val != null && !string.IsNullOrWhiteSpace(val))
                {
                    await _cache.SetAsync(cacheKey, oRet);
                }
                return oRet;
            }
            return default;
        }

        public async Task<AppxConfig> GetConfigItemAsync(string configCat, string configName)
        {
            return await _uow.AppxConfigs.Single(c => c.ConfigCat.Equals(configCat) && c.ConfigName.Equals(configName));
        }

        public async Task<string> GetConfigValueAsync(string configCat, string configName)
        {
            string sRet = "";
            var oC = await _uow.AppxConfigs.Single(c => c.ConfigCat.Equals(configCat) && c.ConfigName.Equals(configName));
            if (oC != null)
            {
                if (oC.ConfigVal != null)
                {
                    sRet = oC.ConfigVal;
                }
            }
            return sRet;
        }

        public async Task<ServiceResult<AppxConfig>> SaveConfigAsync(string configCat, string configName, string configVal)
        {
            string cacheKey = Cache.Keys.CMSConfig(configCat, configName);

            var oConfig = await _uow.AppxConfigs.Single(c => c.ConfigCat.Equals(configCat) && c.ConfigName.Equals(configName));
            if (oConfig == null)
            {
                oConfig = new AppxConfig()
                {
                    ConfigCat = configCat,
                    ConfigName = configName,
                    ConfigVal = configVal
                };
                await _uow.AppxConfigs.Insert(oConfig);
            }
            else
            {
                oConfig.ConfigVal = configVal;
                _uow.AppxConfigs.Update(oConfig);
            }

            var oRet = new ServiceResult<AppxConfig>();
            oRet.Success = await _uow.Save();

            if (oRet.Success)
            {
                await _cache.RemoveAsync(cacheKey);
            }

            oRet.Id = oConfig.ConfigId;
            oRet.obj = oConfig;
            return oRet;
        }
    }
}
