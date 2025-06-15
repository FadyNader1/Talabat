using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Interfaces;

namespace Talabat.Services.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration confg;

        public TokenService(IConfiguration confg)
        {
            this.confg = confg;
        }
        public async Task<string> GetToken(UserApp user, UserManager<UserApp> userManager)
        {
            //private registration
            var AuthClaims = new List<Claim>()
            {
               new Claim(ClaimTypes.GivenName,user.UserName),
               new Claim(ClaimTypes.Email,user.Email),
            };
            var roles=await userManager.GetRolesAsync(user);
            if (roles != null && roles.Count() > 0)
            {
                foreach (var role in roles)
                {
                    AuthClaims.Add(new Claim(ClaimTypes.Role, role));
                }
            }
            //private key
            var key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(confg["JWT:Key"]));

            var token = new JwtSecurityToken(
                issuer: confg["JWT:Issuer"],
                audience: confg["JWT:Audience"],
                claims: AuthClaims,
                expires: DateTime.Now.AddDays(double.Parse(confg["JWT:Duration"])),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.Aes256CbcHmacSha512)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
