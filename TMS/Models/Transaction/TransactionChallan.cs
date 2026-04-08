using TMS.Models.Common;

namespace TMS.Models.Transaction
{
    public class TransactionChallan : AuditFields
    {
        public int IDChallan { get; set; }
        public string VoucherNo { get; set; }
        public DateTime? ChallanDate { get; set; }
        public int? IDBill { get; set; }
        public int? IDTruck { get; set; }
        public string? DriverName { get; set; }
        public int? IDTransporter { get; set; }
        public int? IDConsignee { get; set; }
        public string PanCardHolder { get; set; }
        public string PanCardNo { get; set; }
        public string AdvancePayment1 { get; set; }
        public string AdvancePayment2 { get; set; }

        // Calculations
        public decimal FreightAmt { get; set; }
        public decimal ExtraAmt { get; set; }
        public decimal FinalAmt { get; set; }
        public decimal CashAmt { get; set; }
        public decimal Cheque1Amt { get; set; }
        public string Cheque1No { get; set; }
        public DateTime? Cheque1Date { get; set; }
        public decimal Cheque2Amt { get; set; }
        public string Cheque2No { get; set; }
        public DateTime? Cheque2Date { get; set; }
        public decimal AdvanceAmt { get; set; }
        public bool IsPaymentPaidByParty { get; set; }
        public decimal BalanceAmt { get; set; }
        public decimal PaidByPartyAmt { get; set; }

        // Balance Details
        public decimal BalanceChequeAmt { get; set; }
        public string BalanceChequeNo { get; set; }
        public DateTime? BalanceChequeDate { get; set; }
        public string Remarks { get; set; }
        public int? IDChallanEntryBy { get; set; }

        // Relation fields
        public string? TruckNumber { get; set; }
        public string? ConsigneeName { get; set; }
    }
}
