using TMS.Models.Common;

namespace TMS.Models.Master
{
    public class MasterAccountType : AuditFields
    {
        public int IDAccountType { get; set; }
        public string AccountType { get; set; }
    }
}
