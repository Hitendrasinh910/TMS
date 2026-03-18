using TMS.Models.Common;

namespace TMS.Models.Master
{
    public class MasterTruck : AuditFields
    {
        public int IDTruck { get; set; }
        public string TruckNumber { get; set; }
        public string PanCardHolder { get; set; }
        public string PanCardNo { get; set; }
        public string Remarks { get; set; }
    }
}
