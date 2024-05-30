using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.Service.Services.CacheService;
using System.Text;

namespace Store.API.Helper
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int timeToLiveInSecond;

        public CacheAttribute(int timeToLiveInSecond)
        {
            this.timeToLiveInSecond = timeToLiveInSecond;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            var cachekey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var cachedResponse = await cacheService.GetCacheResponseAsync(cachekey);

            if (!string.IsNullOrEmpty(cachedResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cachedResponse,
                    ContentType = "aplication/json",
                    StatusCode = 200,
                };
                context.Result = contentResult;

                return;

            }

            var excutedContext = await next();
            if(excutedContext.Result is OkObjectResult response)
                await cacheService.SetCacheResponseAsync(cachekey,response.Value, TimeSpan.FromSeconds(timeToLiveInSecond));
        }

        private string GenerateCacheKeyFromRequest(HttpRequest Request)
        {
            StringBuilder cachekey = new StringBuilder();

            cachekey.Append($"{Request.Path}");

            foreach (var (key , value) in Request.Query.OrderBy(x=>x.Key))
                cachekey.Append($"|{key}-{value}");

            return cachekey.ToString();
        }
    }
}
