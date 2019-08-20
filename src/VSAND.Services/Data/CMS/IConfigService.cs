using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;

namespace VSAND.Services.Data.CMS
{
    public interface IConfigService
    {
        Task<AppxConfig> GetConfigItemAsync(string configCat, string configName);
        Task<string> GetConfigValueAsync(string configCat, string configName);
        Task<T> GetConfigValueCachedAsync<T>(string configCat, string configName);
        Task<ServiceResult<AppxConfig>> SaveConfigAsync(string configCat, string configName, string configVal);
    }
}
