namespace TMS.Models.Transaction
{
    public class TransactionLRDetail
    {
        public int IDLRDetail { get; set; }
        public int IDLR { get; set; }
        public string MethodOfPacking { get; set; }
        public string Description { get; set; }
        public string Packages { get; set; }
        public string FreightOn { get; set; }
        public decimal Weight { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
    }
}
