using TMS.Models.Common;

namespace TMS.Models.Master
{
    public class MasterCity : AuditFields
    {
        public int IDCity { get; set; }
        public int IDState { get; set; }
        public string City { get; set; }
        public string? State { get; set; }
    }
}
