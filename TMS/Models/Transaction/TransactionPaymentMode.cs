using TMS.Models.Common;

namespace TMS.Models.Transaction
{
    public class TransactionPaymentMode : AuditFields
    {
        public int IDPaymentMode { get; set; }
        public string PaymentMode { get; set; }
    }
}
