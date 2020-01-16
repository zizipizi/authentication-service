namespace NSV.Security.Password
{
    public interface IPasswordService
    {
        PasswordHashResult Hash(string password);

        PasswordValidateResult Validate(
            string verifiedPassword,
            string originPassword);
    }
}
