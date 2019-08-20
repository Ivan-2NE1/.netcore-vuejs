using System.Threading.Tasks;

namespace VSAND.Services.Cache
{
    public interface ICache
    {
        Task<T> GetAsync<T>(string key);

        Task SetAsync<T>(string key, T value);

        Task RemoveAsync(string key);
    }
}
