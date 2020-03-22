using System.Linq;
using Authentication.Data.Models.Entities;
using NSV.Security.JWT;

namespace Authentication.Host.Models.Translators
{
    public static class JwtTokenTranslators
    {
        public static BodyTokenModel ToBodyTokenModel(this TokenModel model)
        {
            return new BodyTokenModel
            {
                AccessToken = model.AccessToken.Value,
                RefreshToken = model.RefreshToken.Value
            };
        }

        //TODO Change token.jti to accessToken.Jti
        public static TokenModel ToTokenModel(this RefreshTokenEntity token)
        {
            TokenModel tokenModel;
            if (token.AccessToken == null)
            { 
                tokenModel = new TokenModel(
                    (default, default, default), 
                (token.Token, token.Expired, token.Jti));
            }
            else
            {
                var accessToken = token.AccessToken.First(c => c.RefreshToken == token);
                tokenModel = new TokenModel((accessToken.Token, accessToken.Exprired, token.Jti), (token.Token, token.Expired, token.Jti));
            }

            return tokenModel;
        }


    }
}
