using System.ComponentModel.DataAnnotations;

namespace KERP.Client.DTOs.MassUpdate
{
    public class MassUpdatePurchaseDto
    {
        [Required(ErrorMessage = "Pole Purchase Order jest wymagane.")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "Pole musi mieć dokładnie 9 znaków")]
        public string PurchaseOrder { get; set; } = string.Empty;

        [Required(ErrorMessage = "Pole Line Number jest wymagane.")]
        public int? LineNumber { get; set; }

        [Required(ErrorMessage = "Pole Sequence jest wymagane")]
        public int? Sequence { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ConfirmedReceiptDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ChangedReceiptDate { get; set; }
    }
}
