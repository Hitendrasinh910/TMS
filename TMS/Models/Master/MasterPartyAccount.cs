using TMS.Models.Common;

namespace TMS.Models.Master
{
    public class MasterPartyAccount : AuditFields
    {
        public int IDPartyAccount { get; set; }
        public int AccountSrNo { get; set; }
        public string PartyCode { get; set; }
        public int IDAccountType { get; set; }
        public string PartyName { get; set; }
        public string Address { get; set; }
        public int IDState { get; set; }
        public int IDCity { get; set; }
        public string? StateName { get; set; }                    // Get State and City name
        public string? CityName { get; set; }
        public string ContactNo1 { get; set; }
        public string ContactNo2 { get; set; }
        public string Email { get; set; }
        public decimal OpeningBalance { get; set; }
        public int IDBalanceType { get; set; }
        public string GSTNo { get; set; }
        public string PanNo { get; set; }
    }
}
