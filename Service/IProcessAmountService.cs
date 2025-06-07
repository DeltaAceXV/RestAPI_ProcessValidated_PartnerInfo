using RestAPI_ProcessValidated_PartnerInfo.DTO;
using RestAPI_ProcessValidated_PartnerInfo.Entities;
using RestAPI_ProcessValidated_PartnerInfo.Utils;

namespace RestAPI_ProcessValidated_PartnerInfo.Service
{
    public interface IProcessAmountService
    {
        Task<SummaryAmountResponse> ProcessAmount(TransactionDetails request);
    }

    public class ProcessAmountService : IProcessAmountService
    {
        private readonly ILoggerService _loggerService;

        public ProcessAmountService(ILoggerService loggerService)
        {
            this._loggerService = loggerService;
        }

        public Task<SummaryAmountResponse> ProcessAmount(TransactionDetails request)
        {
            var summaryAmountResponse = SummaryAmountResponse.Success(request.TotalAmount);

            if (summaryAmountResponse.TotalAmount < 0)
            {
                throw new HttpStatusCodeException(this._loggerService, 400, $"The total amount cannot be negative {summaryAmountResponse.TotalAmount}");
            }
            if (summaryAmountResponse.TotalDiscount < 0)
            {
                throw new HttpStatusCodeException(this._loggerService, 400, $"The total discount cannot be negative {summaryAmountResponse.TotalDiscount}");
            }
            if (summaryAmountResponse.FinalAmount < 0)
            {
                throw new HttpStatusCodeException(this._loggerService, 400, $"The final amount cannot be negative {summaryAmountResponse.FinalAmount}");
            }

            return Task.FromResult(summaryAmountResponse);
        }

    }
}
