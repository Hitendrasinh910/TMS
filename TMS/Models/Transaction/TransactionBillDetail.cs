namespace TMS.Models.Transaction
{
    public class TransactionBillDetail
    {
        public int IDBillDetail { get; set; }
        public int IDBill { get; set; }
        public int? IDLR { get; set; }
        public string Description { get; set; }
        public int? IDFromCity { get; set; }
        public int? IDToCity { get; set; }
        public string FreightOn { get; set; }
        public decimal FixAmt { get; set; }
        public decimal Weight { get; set; }
        public decimal Rate { get; set; }
        public decimal ExtraCharges { get; set; }
        public decimal Amount { get; set; }
        public string? Remarks { get; set; }

        // Helper properties for UI (not saved to the Detail table directly)
        public string? LRNo { get; set; }
        public DateTime? LRDate { get; set; }
        public string? FromCityName { get; set; }
        public string? ToCityName { get; set; }
    }
}
