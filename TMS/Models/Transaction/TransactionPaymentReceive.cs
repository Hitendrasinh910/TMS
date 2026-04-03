using TMS.Models.Common;

namespace TMS.Models.Transaction
{
    public class TransactionPaymentReceive : AuditFields
    {
        public int IDPayment { get; set; }
        public string ReceiptNo { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? IDPaymentType { get; set; }
        public int? IDParty { get; set; }
        public int? IDBill { get; set; }
        public decimal BillAmount { get; set; }
        public decimal OutstandingAmount { get; set; }
        public int? IDPaymentMode { get; set; } // Stored as NVARCHAR(50) per your SQL
        public decimal AmountReceived { get; set; }
        public decimal TDSAmt { get; set; }
        public decimal BalanceAmt { get; set; }
        public string? Remarks { get; set; }

        // Relation
        public string? PaymentType { get; set; }
        public string? PartyName { get; set; }
        public string? PaymentMode { get; set; }
    }
}
