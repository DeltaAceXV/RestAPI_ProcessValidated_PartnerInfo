using System.ComponentModel.DataAnnotations;

namespace RestAPI_ProcessValidated_PartnerInfo.Entities
{
    public class ItemDetail
    {
        [Required, StringLength(50), MinLength(1)]
        public string PartnerItemRef { get; set; }

        [Required, StringLength(100), MinLength(1)]
        public string Name { get; set; }

        [Required]
        public int Qty { get; set; }

        [Required, Range(minimum:1, maximum: 99999)]
        public long UnitPrice { get; set; }
    }
}
