using RestAPI_ProcessValidated_PartnerInfo.Entities;
using RestAPI_ProcessValidated_PartnerInfo.Service;

namespace RestAPI_ProcessValidated_PartnerInfo.Repository
{

    /* Assume data is extracted from database */
    public interface IPartnerRepository
    {
        public Task<PartnerDetails> GetByNo(string partnerNo);
        public Task<IEnumerable<PartnerDetails>> GetAllAsync();
    }

    public class PartnerRepository : IPartnerRepository
    {
        private readonly ILoggerService _loggerService;
        private readonly List<PartnerDetails> _pseudoDbcontext;

        public PartnerRepository(ILoggerService loggerService) { 
            this._loggerService = loggerService;
            this._pseudoDbcontext = new();

            this._pseudoDbcontext.Add(new(partnerNo: $"FG-00001", allowedPartner: $"FAKEGOOGLE", partnerPassword: $"FAKEPASSWORD1234"));
            this._pseudoDbcontext.Add(new(partnerNo: $"FG-00002", allowedPartner: $"FAKEPEOPLE", partnerPassword: $"FAKEPASSWORD4578"));
        }

        public async Task<PartnerDetails> GetByNo(string partnerNo)
        {
            await this._loggerService.Information($"[IPartnerRepository][GetByNo] - Access to the following partner {partnerNo} was executed");
            return this._pseudoDbcontext.FirstOrDefault(row => row.PartnerNo == partnerNo);
        }

        public async Task<IEnumerable<PartnerDetails>> GetAllAsync()
        {
            await this._loggerService.Information($"[IPartnerRepository][GetByNo] - Access to all partners was executed");
            return this._pseudoDbcontext;
        }
    }
}
