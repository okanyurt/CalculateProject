namespace Calculate.Data.Models
{
    public class UserGet
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string AccessToken { get; set; }
        public string Name { get; set; }
        public int RoleID { get; set; }
        public bool IsEnabled { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public int officeIdList { get; set; }

        public string OfficeName { get; set; }
    }
}
