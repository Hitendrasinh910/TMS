using TMS.Models.Common;

namespace TMS.Models.Transaction
{
    public class TransactionBill : AuditFields
    {
        public int IDBill { get; set; }
        public string BillNo { get; set; }
        public DateTime? BillDate { get; set; }
        public int? IDBillToParty { get; set; }
        public int? IDTruck { get; set; }
        public string? DriverMobileNo { get; set; }
        public decimal FinalAmount { get; set; }
        public decimal ToPayAmount { get; set; }
        public decimal CommissionAmt { get; set; }
        public string? Remarks { get; set; }

        // For ID
        public string? BillToPartyName { get; set; }
    }
}
