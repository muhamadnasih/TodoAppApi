﻿
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ToDoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthenticationController(IConfiguration config)
        {
            this._config = config;
        }

        public record AuthenticationData(string? UserName, string? Password);
        public record UserData(int Id, string FirstName, string LastName, string UserName);

        [HttpPost("token")]
        [AllowAnonymous]
        public ActionResult<string> Authenticate([FromBody] AuthenticationData data)
        {
            var user = ValidateCredential(data);

            if (user is null)
            {
                return Unauthorized();
            }

            var token = GenerateToken(user);
            return Ok(token);
        }

        private string GenerateToken(UserData user)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                _config.GetValue<string>("Authentication:SecretKey")));

            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = new();
            claims.Add(new(JwtRegisteredClaimNames.Sub,user.Id.ToString()));
            claims.Add(new(JwtRegisteredClaimNames.UniqueName, user.UserName));
            claims.Add(new(JwtRegisteredClaimNames.GivenName, user.FirstName));
            claims.Add(new(JwtRegisteredClaimNames.FamilyName, user.LastName));

            var token = new JwtSecurityToken(
                _config.GetValue<string>("Authentication:SecretKey"),
                _config.GetValue<string>("Authentication:SecretKey"),
                claims,             
                DateTime.Now,
                 DateTime.UtcNow.AddDays(1),
                signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserData? ValidateCredential(AuthenticationData data)
        {
            //This is not production code - Replace this with a call to your auth system
            if (CompareValues(data.UserName,"tcorey") && CompareValues(data.Password,"Test123"))
            {
                return new UserData(1,"Tim","Corey", data.UserName!);
            }

            return null;
        }

        private bool CompareValues(string? actual, string expected)
        {
            if (actual is not null)
            {
                if (actual.Equals(expected))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
