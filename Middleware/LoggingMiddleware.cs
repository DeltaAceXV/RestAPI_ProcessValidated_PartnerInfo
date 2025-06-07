using Newtonsoft.Json;
using RestAPI_ProcessValidated_PartnerInfo.DTO;
using RestAPI_ProcessValidated_PartnerInfo.Service;
using RestAPI_ProcessValidated_PartnerInfo.Utils;

namespace RestAPI_ProcessValidated_PartnerInfo.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerService _logger;

        public LoggingMiddleware(RequestDelegate next
            , ILoggerService logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _logger.Information($"Incoming request: {context.Request.Method}, {context.Request.Path}");
            await this._next(context);
        }

    }
}
