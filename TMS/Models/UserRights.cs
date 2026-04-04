namespace TMS.Models
{
    public class UserRights
    {
        public int IDForms { get; set; }
        public string? FormName { get; set; }
        public bool AllowToView { get; set; }
        public bool AllowToAdd { get; set; }
        public bool AllowToUpdate { get; set; }
        public bool AllowToDelete { get; set; }
    }

    public class UserRightSaveDto
    {
        public int IDUser { get; set; }
        public List<UserRights> Rights { get; set; }
    }
}
