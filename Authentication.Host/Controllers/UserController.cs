using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Authentication.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private IUserRepository _repo;

        public UserController(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Get()
        {
            try
            {

                var user = await _repo.GetUserAsync(1, CancellationToken.None);
                return Ok(user);
            }
            catch (Exception)
            {
            }
            return BadRequest("User not find");
        }

        [HttpGet]
        public IActionResult Token()
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();

                var key = "sdfijdoaijdSSD231soaisjdoaisjdoiasjiasdkaaasd";

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

                var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

                var identity = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "Login"),
                    new Claim(ClaimTypes.PostalCode, "123456"),
                    new Claim(ClaimTypes.Role, "admin"),
                });

                var token = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Audience = "ApiAudience",
                    Expires = DateTime.UtcNow.AddMinutes(4),
                    Issuer = "ApiIssuer",
                    SigningCredentials = signingCredentials,
                    Subject = identity
                });

                var MyToken = handler.WriteToken(token);

                return Ok(MyToken);
            }
            catch (Exception e)
            {

            }

            return BadRequest("Token not created");
        }
    }
}