using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using RestAPI_ProcessValidated_PartnerInfo.DTO;
using RestAPI_ProcessValidated_PartnerInfo.Entities;
using RestAPI_ProcessValidated_PartnerInfo.Service;

namespace RestAPI_ProcessValidated_PartnerInfo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ApiController : ControllerBase
    {

        private readonly IValidationService _validationService;
        private readonly IProcessAmountService _processAmountService;
        private readonly IApiRequestLogger _apiRequestLogger;

        public ApiController(IValidationService validationService
            , IProcessAmountService processAmountService
            , IApiRequestLogger _apiRequestLogger) 
        { 
            this._validationService = validationService;
            this._processAmountService = processAmountService;
            this._apiRequestLogger = _apiRequestLogger;
        }

        [HttpPost("submittrxmessage")]
        public async Task<SummaryAmountResponse> SubmitTrxMessage([FromBody] TransactionDetails request)
        {
            await this._apiRequestLogger.LogRequest(request);
            await this._validationService.ValidateRequest(request);

            return await this._processAmountService.ProcessAmount(request);
        }
    }
}
