using RestAPI_ProcessValidated_PartnerInfo.Helper;
using System.ComponentModel.DataAnnotations;

namespace RestAPI_ProcessValidated_PartnerInfo.DTO
{
    public class SummaryAmountResponse : BaseResult
    {
        private const decimal MaximumDiscount = 0.2M;

        protected SummaryAmountResponse(int result, long totalAmount, string? resultMessage) : base(result, resultMessage) { 
            this.Result = result;
            this.TotalAmount = totalAmount;
            this.ResultMessage = resultMessage;
        }

        public long TotalAmount { get; set; }
        public long TotalDiscount => CalculateDiscount();
        public long FinalAmount => this.TotalAmount - this.TotalDiscount;

        public static SummaryAmountResponse Success(long totalAmount) => new SummaryAmountResponse(1, totalAmount: totalAmount, default);

        private long CalculateDiscount() {
            if (this.TotalAmount < 20000)
            {
                return 0;
            }

            decimal baseDiscount = GetBaseDiscount();
            decimal conditionalDiscount = GetConditionalDiscount();
            decimal finalDiscount = baseDiscount + conditionalDiscount;

            finalDiscount = finalDiscount > MaximumDiscount ? MaximumDiscount : finalDiscount;

            var discountedAmount = this.TotalAmount * finalDiscount;
            return (long)discountedAmount;
        }

        private decimal GetBaseDiscount()
        {
            if (this.TotalAmount >= 20000 && this.TotalAmount <= 50000)
            {
                return 0.05M;
            }
            if (this.TotalAmount >= 50100 && this.TotalAmount <= 80000)
            {
                return 0.07M;
            }
            if (this.TotalAmount >= 80100 && this.TotalAmount <= 120000)
            {
                return 0.10M;
            }

            return 0.15M;
        }

        private decimal GetConditionalDiscount()
        {
            if (this.TotalAmount >= 50000 && CalculationHelper.CheckPrimeNumber(this.TotalAmount))
            {
                return 0.08M;
            }

            if (this.TotalAmount >= 90000 && GenericHelper.CheckLastDigitMatch(this.TotalAmount, "5"))
            {
                return 0.10M;
            }

            return 0.00M;
        }


    }
}
