using RestAPI_ProcessValidated_PartnerInfo.DTO;
using RestAPI_ProcessValidated_PartnerInfo.Entities;
using RestAPI_ProcessValidated_PartnerInfo.Helper;
using RestAPI_ProcessValidated_PartnerInfo.Utils;
using System.Text;

namespace RestAPI_ProcessValidated_PartnerInfo.Service
{
    public interface IValidationService
    {
        Task ValidateRequest(TransactionDetails request);
    }

    public class ValidationService : IValidationService
    {
        private readonly ILoggerService _loggerService;
        private readonly IPartnerService _partnerService;
        private readonly IAesEncryptionService _aesEncryptionService;

        private TransactionDetails _request;

        public ValidationService(ILoggerService loggerService
            , IPartnerService partnerService
            , IAesEncryptionService aesEncryptionService) 
        { 
            this._loggerService = loggerService;
            this._partnerService = partnerService;
            this._aesEncryptionService = aesEncryptionService;
        }

        public async Task ValidateRequest(TransactionDetails request)
        {
            this._request = request;

            await this.ValidateSig();
            await this.ValidateTotalAmount();
            await this.ValidateExpiredRequest();
        }

        private async Task ValidateSig()
        {
            var partner = await this._partnerService.GetPartnerAsync(this._request.PartnerRefNo);

            if (partner is null)
            {
                throw new HttpStatusCodeException(this._loggerService, 401, $"Access Denied!");
            }

            var formSig = SignatureHelper.GenerateSignature(
                timestamp: this._request.TimeStampDate.ToString("yyyyMMddhhmmss")
                , partnerKey: this._request.PartnerKey
                , partnerRefNo: partner.PartnerNo
                , totalAmount: this._request.TotalAmount.ToString()
                , encryptedPartnerCode: Convert.ToBase64String(
                    Encoding.UTF8.GetBytes(partner.PartnerPassword)
                )
            );

            if (this._request.Sig != formSig)
            {
                throw new HttpStatusCodeException(this._loggerService, 401, $"Access Denied!");
            }
        }

        private async Task ValidateTotalAmount()
        {
            if (this._request.Items is null
                ||
                this._request.Items.Count() == 0)
            {
                return;
            }

            if (!this._request.IsTotalAmountMatchingItems)
            {
                throw new HttpStatusCodeException(this._loggerService, 400, $"Invalid total amount");
            }
        }

        private async Task ValidateExpiredRequest()
        {
            DateTimeOffset currentTime = DateTimeOffset.Now;

            var checkTime = currentTime - this._request.TimeStampDate;

            bool TimeExceeded5MinutesRange = checkTime.TotalMinutes < -5 || checkTime.TotalMinutes > 5;

            if (TimeExceeded5MinutesRange)
            {
                throw new HttpStatusCodeException(this._loggerService, 419, $"Expired.");
            }

        }

    }
}
