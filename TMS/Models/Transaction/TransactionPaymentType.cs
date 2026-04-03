using TMS.Models.Common;

namespace TMS.Models.Transaction
{
    public class TransactionPaymentType : AuditFields
    {
        public int IDPaymentType { get; set; }
        public string PaymentType { get; set; }
    }
}
