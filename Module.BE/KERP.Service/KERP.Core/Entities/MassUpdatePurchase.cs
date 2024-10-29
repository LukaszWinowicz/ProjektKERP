namespace KERP.Core.Entities
{
    public class MassUpdatePurchase
    {
        public int Id { get; set; }
        public required string PurchaseOrderNumber { get; set; }
        public required int LineNumber { get; set; }
        public required int Sequence { get; set; }
        public DateTime? ConfirmedReceiptDate { get; set; }
        public DateTime? ChangedReceiptDate { get; set; }
        public int AddedByUserId { get; set; }
        public bool IsGenerated { get; set; } = false;
        public DateTime? GeneratedDate { get; set; }
    }
}
