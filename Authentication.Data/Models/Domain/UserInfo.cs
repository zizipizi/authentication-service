namespace Authentication.Data.Models.Domain
{
    public class UserInfo
    {
        public long Id { get; set; }
        public string Login { get; set; }

        public string Role { get; set; }

        public bool IsActive { get; set; }
    }
}
