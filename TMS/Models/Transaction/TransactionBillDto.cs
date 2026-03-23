namespace TMS.Models.Transaction
{
    public class TransactionBillDto
    {
        public TransactionBill Header { get; set; }
        public List<TransactionBillDetail> Details { get; set; }
    }
}
