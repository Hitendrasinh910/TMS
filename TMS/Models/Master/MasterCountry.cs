using TMS.Models.Common;

namespace TMS.Models.Master
{
    public class MasterCountry : AuditFields
    {
        public int IDCountry { get; set; }
        public string Country { get; set; }
    }
}
