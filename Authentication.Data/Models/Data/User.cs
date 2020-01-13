namespace Authentication.Data.Models.Data
{
    public class User
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public bool IsActive { get; set; }

        public string Role { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }
    }
}
