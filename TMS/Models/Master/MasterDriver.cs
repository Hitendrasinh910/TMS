using TMS.Models.Common;

namespace TMS.Models.Master
{
    public class MasterDriver : AuditFields
    {
        public int IDDriver { get; set; }
        public string DriverName { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string EmergencyContactNo { get; set; }
        public string DrivingLicenceNo { get; set; }
        public DateTime? DLValidTill { get; set; }
    }
}
