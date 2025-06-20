using RestAPI_ProcessValidated_PartnerInfo.Attributes;
using RestAPI_ProcessValidated_PartnerInfo.Helper;
using System.ComponentModel.DataAnnotations;

namespace RestAPI_ProcessValidated_PartnerInfo.Entities
{
    public class TransactionDetails
    {
        public TransactionDetails() { 
        
        }

        [Required]
        [StringLength(50)]
        public string PartnerKey { get; set; }

        [Required]
        [StringLength(50)]
        public string PartnerRefNo { get; set; }

        [Required]
        [Sensitive]
        [StringLength(50)]
        public string PartnerPassword { get; set; }

        [Required]
        [Range(0, long.MaxValue, ErrorMessage = "The total amount should not be less than 0")]
        public long TotalAmount { get; set; }

        public ItemDetail[] Items { get; set; }

        [Required, MinLength(1)]
        public string TimeStamp { get; set; }

        [Required, MinLength(1)]
        public string Sig { get; set; }

        public DateTime TimeStampDate { 
            get {
                if (string.IsNullOrWhiteSpace(this.TimeStamp))
                {
                    return default;
                }

                return this.TimeStamp.ConvertStringToDateTime();
            }
        }

        public bool IsTotalAmountMatchingItems
        {
            get {
                if (this.Items == null || this.Items.Length == 0)
                {
                    return false;
                }

                long totalAmount = this.Items.Sum(item => item.UnitPrice * item.Qty);

                return this.TotalAmount == totalAmount;
            }
        }
    }
}
