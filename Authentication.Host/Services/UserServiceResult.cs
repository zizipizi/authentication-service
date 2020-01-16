using Authentication.Data.Models.Domain;
using NSV.Security.JWT;

namespace Authentication.Host.Services
{
    public struct UserServiceResult
    {
        public UserResult Result { get; set; }

        public User User { get; set; }

        public TokenModel Token { get; set; }


        public UserServiceResult(UserResult result = UserResult.Ok, User user = null, TokenModel token = null)
        {
            Result = result;
            User = user;
            Token = token;
        }

        public enum UserResult
        {
            Ok,
            UserNotFound,
            Blocked,
            Exist
        }

        internal static UserServiceResult Ok(TokenModel token, User user)
        {
            return new UserServiceResult
                (
                    result: UserResult.Ok,
                    user: user,
                    token: token
                );
        }

        internal static UserServiceResult Ok()
        {
            return new UserServiceResult(UserResult.Ok);
        }

        internal static UserServiceResult UserNotFound()
        {
            return new UserServiceResult(UserResult.UserNotFound);
        }

        internal static UserServiceResult Blocked()
        {
            return new UserServiceResult(UserResult.Blocked);
        }

        internal static UserServiceResult UserExist()
        {
            return new UserServiceResult(UserResult.Exist);
        }
    }
}