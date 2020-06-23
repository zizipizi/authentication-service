namespace Authentication.Host.Results.Enums
{
    public enum AuthRepositoryResult
    {
        Ok,
        WrongLoginOrPass,
        UserBlocked,
        UserNotFound,
        TokenValidationProblem,
        TokenIsBlocked,
        Error
    }
}
