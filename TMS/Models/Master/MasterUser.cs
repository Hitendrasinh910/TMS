using TMS.Models.Common;

namespace TMS.Models.Master
{
    public class MasterUser : AuditFields
    {
        public int IDUser { get; set; }
        public string UserType { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string? ContactNo { get; set; }
    }
}
