namespace TMS.Models.Common
{
    public class AuditFields
    {
        public DateTime? E_Date { get; set; }
        public string? E_By { get; set; }
        public int? E_ById { get; set; }

        public DateTime? U_Date { get; set; }
        public string? U_By { get; set; }
        public int? U_ById { get; set; }

        public DateTime? D_Date { get; set; }
        public string? D_By { get; set; }
        public bool IsDeleted { get; set; }
    }
}
