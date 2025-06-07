using RestAPI_ProcessValidated_PartnerInfo.Entities;
using RestAPI_ProcessValidated_PartnerInfo.Repository;

namespace RestAPI_ProcessValidated_PartnerInfo.Service
{
    public interface IPartnerService
    {
        Task<IEnumerable<PartnerDetails>> GetAllPartnersAsync();

        Task<PartnerDetails> GetPartnerAsync(string partnerNo);
    }

    public class PartnerService : IPartnerService
    {
        private readonly ILoggerService _loggerService;
        private readonly IPartnerRepository _partnerRepository;

        public PartnerService(ILoggerService loggerService
            , IPartnerRepository partnerRepository) 
        { 
            this._loggerService = loggerService;
            this._partnerRepository = partnerRepository;
        }

        public async Task<IEnumerable<PartnerDetails>> GetAllPartnersAsync()
        {
            var partners = await this._partnerRepository.GetAllAsync();
            return partners;
        }

        public async Task<PartnerDetails> GetPartnerAsync(string partnerNo)
        {
            var partner = await this._partnerRepository.GetByNo(partnerNo);

            return partner;
        }


    }

}
