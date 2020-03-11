namespace Authentication.Host.Results.Enums
{
    public enum UserRepositoryResult
    {
        Ok,
        WrongPassword,
        UserNotFound,
        Error,
        PasswordChangedNeedAuth
    }
}
