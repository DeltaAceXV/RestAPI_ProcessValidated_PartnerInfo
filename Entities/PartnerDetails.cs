namespace RestAPI_ProcessValidated_PartnerInfo.Entities
{
    public class PartnerDetails
    {
        public PartnerDetails(string partnerNo, string allowedPartner, string partnerPassword) { 
            this.PartnerNo = partnerNo;
            this.AllowedPartner = allowedPartner;
            this.PartnerPassword = partnerPassword;
        }

        public string PartnerNo { get; private set; }
        public string AllowedPartner {  get; private set; }
        public string PartnerPassword { get; private set; }
    }
}
