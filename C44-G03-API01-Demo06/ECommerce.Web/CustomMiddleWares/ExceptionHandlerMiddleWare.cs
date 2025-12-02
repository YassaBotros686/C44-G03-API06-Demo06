using ECommerce.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.CustomMiddleWares
{
    public class ExceptionHandlerMiddleWare
    {

        // 1. You Must Inject Request Delegate as your Next Middleware in ur CTOR
        // 2. Mist Have InvokeAsync Method HttpContext as Parameter

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleWare> _logger;

        public ExceptionHandlerMiddleWare(RequestDelegate Next, ILogger<ExceptionHandlerMiddleWare> logger)
        {
            _next = Next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                // 404 Not Found
                await HandleNotFoundEndPointAsync(context);
            }
            catch (Exception ex)
            {
                // Logger
                _logger.LogError(ex, $"Something Went Wrong !");
                var Problem = new ProblemDetails()
                {
                    Title = "An Unexpected Error Occurred",
                    Detail = ex.Message,
                    Instance = context.Request.Path,
                    Status = ex switch
                    {
                        NotFoundException => StatusCodes.Status404NotFound,
                        _=> StatusCodes.Status500InternalServerError
                    }
                };
                context.Response.StatusCode = Problem.Status.Value;
                await context.Response.WriteAsJsonAsync(Problem);
            }
        }

        private static async Task HandleNotFoundEndPointAsync(HttpContext context)
        {
            if (context.Response.StatusCode == StatusCodes.Status404NotFound && !context.Response.HasStarted)
            {
                var Problem = new ProblemDetails()
                {
                    Title = "Resource Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = "The Resource You Are Looking For Is Not Found",
                    Instance = context.Request.Path
                };
                await context.Response.WriteAsJsonAsync(Problem);
            }
        }
    }
}
