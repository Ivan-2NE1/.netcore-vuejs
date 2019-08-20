using System.Threading.Tasks;

namespace VSAND.Services.Razor
{
    public interface IRazorViewRenderer
    {
        Task<string> RenderAsync(string name);
        Task<string> RenderAsync<TModel>(string name, TModel model);
        Task<string> RenderInlineAsync(string name);
        Task<string> RenderInlineAsync<TModel>(string name, TModel model);
    }
}
