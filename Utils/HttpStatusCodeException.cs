using RestAPI_ProcessValidated_PartnerInfo.Service;

namespace RestAPI_ProcessValidated_PartnerInfo.Utils
{
    public class HttpStatusCodeException : Exception
    {
        private readonly ILoggerService _loggerService;

        public int StatusCode { get; }

        public HttpStatusCodeException(int statusCode, string errorMessage) : base(errorMessage)
        {
            this._loggerService = null!;
            this.StatusCode = statusCode;
        }

        public HttpStatusCodeException(ILoggerService _loggerService, int statusCode, string errorMessage) : base(errorMessage) {
            this._loggerService = _loggerService;
            this._loggerService.Error($"Code: {statusCode}, Message: {errorMessage}", null);
            this.StatusCode = statusCode;
        }

    }
}
