namespace Authentication.Host.Results.Enums
{
    public enum UserResult
    {
        Ok,
        WrongPassword,
        RefreshTokenExpired,
        RefreshNotMatchAccess,
        UserNotFound,
        Error,
        PasswordChangedNeedAuth
    }
}
