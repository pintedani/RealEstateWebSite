using Crosscutting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Imobiliare.UI.Utils
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
        private readonly IEnvironmentService environmentService;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, IEnvironmentService environmentService)
        {
            _next = next;
            this.environmentService = environmentService;
            //_logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception
                //_logger.LogError(ex, "An unhandled exception occurred.");
                LogExceptionToFile(ex);

                // Return a custom error response
                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("An internal server error occurred.");
            }
        }

        private void LogExceptionToFile(Exception exception)
        {
            // Specify the path to the log file
            string logFilePath = Path.Combine(environmentService.GetWebRootPath(),"Logs/exceptions-log.txt"); // Adjust the file path as needed

            // Log the exception to a file using a StreamWriter
            using (var stream = new FileStream(logFilePath, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            {
                writer.WriteLine($"[{DateTime.Now}] An unhandled exception occurred:");
                writer.WriteLine(exception.ToString());
                writer.WriteLine(new string('-', 50));
            }
        }
    }

    public static class GlobalExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
    }

}
