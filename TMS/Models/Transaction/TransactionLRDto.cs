namespace TMS.Models.Transaction
{
    public class TransactionLRDto
    {
        public TransactionLR Header { get; set; }
        public List<TransactionLRDetail> Details { get; set; }
    }
}
