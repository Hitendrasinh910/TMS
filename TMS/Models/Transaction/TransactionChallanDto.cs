namespace TMS.Models.Transaction
{
    public class TransactionChallanDto
    {
        public TransactionChallan Header { get; set; }
        public List<TransactionChallanDetail> Details { get; set; }
    }
}
