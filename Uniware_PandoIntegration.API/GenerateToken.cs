using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Uniware_PandoIntegration.API.Folder;
using Uniware_PandoIntegration.Entities;

namespace Uniware_PandoIntegration.API
{
    public class GenerateToken: IUniwarePando
    {
        private readonly IConfiguration iconfiguration;
        public GenerateToken(IConfiguration iconfiguration) { 
            this.iconfiguration = iconfiguration;
        
        }

        //string IUniwarePando.GenerateJWTTokens { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string GenerateJWTTokens(TokenEntity tokenEntity)
            {
            try
            {
               
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.UTF8.GetBytes(iconfiguration["JWT:Key"]);                
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                  {
                       new Claim(JwtRegisteredClaimNames.Name, tokenEntity.username),
                        new Claim(JwtRegisteredClaimNames.Sid, tokenEntity.password),
                       new Claim(JwtRegisteredClaimNames.UniqueName, "Pando".ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString())
                  }),

                    Expires = DateTime.Now.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                
                return new JwtSecurityTokenHandler().WriteToken(token);

                //return userName;
            }
            catch (Exception ex)
            {
               
                return null;
            }
        }
    }
}
