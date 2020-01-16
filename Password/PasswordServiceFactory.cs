namespace NSV.Security.Password
{
    public class PasswordServiceFactory
    {
        public static IPasswordService Create(PasswordOptions options)
        {
            return new PasswordService(options);
        }
    }
}
