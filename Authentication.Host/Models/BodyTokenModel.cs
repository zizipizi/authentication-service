namespace Authentication.Host.Models
{
    public class BodyTokenModel
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
