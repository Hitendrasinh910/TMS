using TMS.Models.Common;

namespace TMS.Models.Master
{
    public class MasterState : AuditFields
    {
        public int IDState { get; set; }
        public int IDCountry { get; set; }
        public string State { get; set; }
        public string? CountryName { get; set; }    // Changed due to maping error
    }
}
