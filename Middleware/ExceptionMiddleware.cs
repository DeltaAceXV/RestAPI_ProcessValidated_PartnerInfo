using Newtonsoft.Json;
using RestAPI_ProcessValidated_PartnerInfo.DTO;
using RestAPI_ProcessValidated_PartnerInfo.Service;
using RestAPI_ProcessValidated_PartnerInfo.Utils;

namespace RestAPI_ProcessValidated_PartnerInfo.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILoggerService _logger;

        public ExceptionMiddleware(RequestDelegate next
            , ILoggerService logger)
        {
            this.next = next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
            catch (HttpStatusCodeException ex)
            {
                /* Logic changed to fit the returning response when failed as per specifications */
                //await context.Response.WriteAsync(
                //    ex.Message  
                //);
                
                context.Response.StatusCode = ex.StatusCode;
                context.Response.ContentType = $"application/json";

                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(
                        BaseResult.Failed($"{ex.Message}")
                    )    
                );
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message + (ex.InnerException ?? new()).Message;
                await this._logger.Error($"{errorMessage} {ex.StackTrace}", ex);

                context.Response.ContentType = $"application/json";
                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(
                        BaseResult.Failed($"{errorMessage}")
                    )
                );

            }
        }

    }
}
