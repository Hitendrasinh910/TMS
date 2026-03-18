using TMS.Models.Common;

namespace TMS.Models.Master
{
    public class MasterBillToParty : AuditFields
    {
        public int IDPartyBill { get; set; }
        public int SrNo { get; set; }
        public int IDConsignor { get; set; }
        public int IDConsignee { get; set; }
        public int BillTo { get; set; }
        public string Remarks { get; set; }
    }
}
