namespace TMS.Models.Transaction
{
    public class TransactionChallanDetail
    {
        public int IDChallanDetail { get; set; }
        public int IDChallan { get; set; }
        public int? IDLR { get; set; }
        public string MethodOfPacking { get; set; }
        public string Description { get; set; }
        public string Packages { get; set; }
        public string FreightOn { get; set; }
        public decimal FixAmt { get; set; }
        public decimal Weight { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }

        // Display Helpers
        public string LRNo { get; set; }
        public DateTime? LRDate { get; set; }
    }
}
