using ECommerce.ServicesAbstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace ECommerce.Presentation.Attributes
{
    public class RedisCacheAttribute : ActionFilterAttribute
    {
        private readonly int _durationInMin;

        public RedisCacheAttribute(int DurationInMin = 5)
        {
            _durationInMin = DurationInMin;
        }

        //// Executed After EndPoint
        //public override void OnActionExecuted(ActionExecutedContext context)
        //{
        //    base.OnActionExecuted(context);
        //}

        //// Executed Before EndPoint
        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    base.OnActionExecuting(context);
        //}

        // Executed Asynchronously Before , After EndPoint
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Get Cache Service
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            // Check if Cached Data Exists
            // Create CacheKey
            var cacheKey = CreateCacheKey(context.HttpContext.Request);

            // If Exists => return Cached Data and Skip Excuting EndPoint
            var cacheValue = await cacheService.GetAsync(cacheKey);
            if (cacheValue is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = cacheValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK,
                };

                return;
            }

            // If Not Exists => Continue Excuting the EndPoint
            var ExecutedContext = await next.Invoke();

            if (ExecutedContext.Result is OkObjectResult result)
            {
                // Call SetAsync to Cache
                await cacheService.SetAsync(cacheKey, result.Value!, TimeSpan.FromMinutes(5));
            }
        }

        private string CreateCacheKey(HttpRequest request)
        {
            // api/Products
            StringBuilder Key = new StringBuilder();
            Key.Append(request.Path);
            foreach (var item in request.Query.OrderBy(X => X.Key))
            {
                // api/products/brandId=1&typeId=2
                // api/products/typeId=2&brandId=1
                Key.Append($"|{item.Key}-{item.Value}");
            }
            return Key.ToString();
        }
    }
}
