namespace KERP.Core.DTOs
{
    public class MassUpdatePurchaseDto
    {
        public string PurchaseOrderNumber { get; set; }
        public int LineNumber { get; set; }
        public int Sequence { get; set; }
        public DateTime? ConfirmedReceiptDate { get; set; }
        public DateTime? ChangedReceiptDate { get; set; }
    }
}
