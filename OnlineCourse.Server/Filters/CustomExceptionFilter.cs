using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OnlineCourse.Server.Filters;

public class CustomExceptionFilter(ILogger<CustomExceptionFilter> logger,
    IHostEnvironment environment) : IExceptionFilter
{
    private readonly ILogger<CustomExceptionFilter> _logger = logger;
    private readonly IHostEnvironment _env = environment;
    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "Unhandled exception caught");
        int statusCode = 500;
        string message = "A server error occurred. Please try again later.";

        if (context.Exception != null)
        {
            statusCode = 400;
            message = context.Exception.Message;
        }


        object? response = _env.IsDevelopment()
            ? new
            {
                Message = message,
            }
            :
            new
            {
                Message = message
            };

        context.Result = new JsonResult(response)
        {
            StatusCode = statusCode
        };

        context.ExceptionHandled = true;
    }
}
