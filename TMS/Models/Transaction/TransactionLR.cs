using TMS.Models.Common;

namespace TMS.Models.Transaction
{
    public class TransactionLR : AuditFields
    {
        public int IDLR { get; set; }
        public string LRNo { get; set; } = "LR-01";
        public DateTime? LRDate { get; set; }

        // Left Column
        public int? IDConsignor { get; set; }
        public string ConsignorAddress { get; set; }
        public int? IDFromState { get; set; }
        public int? IDFromCity { get; set; }
        public string InvoiceNo { get; set; }

        // Right Column
        public int? IDTruck { get; set; }
        public int? IDConsignee { get; set; }
        public string ConsigneeAddress { get; set; }
        public int? IDToState { get; set; }
        public int? IDToCity { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string SelectTo { get; set; }
        public string EWayBillNo { get; set; }
        public string GSTPaidBy { get; set; }

        public string? BilledTo { get; set; } // Reverted back to string
        public bool ChangeBillToParty { get; set; }

        // Footer
        public string DeclaredValue { get; set; }
        public string Remarks { get; set; }
        public decimal FreightAmt { get; set; }
        public decimal HamaliAmt { get; set; }
        public decimal OtherChargeAmt { get; set; }
        public decimal TotalAmt { get; set; }
    }
}
