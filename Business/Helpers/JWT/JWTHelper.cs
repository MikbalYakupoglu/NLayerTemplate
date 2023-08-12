using DataAccess.Abstract;
using Entity.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Helpers.JWT
{
    public class JWTHelper : ITokenHelper
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRoleDal _userRolesDal;
        public JWTHelper(IConfiguration configuration, IUserRoleDal userRolesDal)
        {
            _configuration = configuration;
            _userRolesDal = userRolesDal;
        }
        public Token CreateToken(User user)
        {
            List<Claim> claims = GetClaims(user);

            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(365),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };


            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            var TokenEntity = new Token()
            {
                token = jwt,
                ExpirationDate = tokenDescriptor.Expires
            };

            return TokenEntity;
        }

        private List<Claim> GetClaims(User user)
        {
            var userRoles = _userRolesDal.GetUserRolesAsync(user.Id).Result;
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            return claims;
        }
    }
}
