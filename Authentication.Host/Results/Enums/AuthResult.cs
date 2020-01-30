﻿namespace Authentication.Host.Results.Enums
{
    public enum AuthResult
    {
        Ok,
        WrongLoginOrPass,
        UserBlocked,
        UserNotFound,
        TokenValidationProblem,
        TokenExpired,
        Error
    }
}