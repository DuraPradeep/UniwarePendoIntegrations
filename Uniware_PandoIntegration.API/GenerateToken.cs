using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Uniware_PandoIntegration.API.Folder;
using Uniware_PandoIntegration.Entities;
using Uniware_PandoIntegration.DataAccessLayer;

namespace Uniware_PandoIntegration.API
{
    public class GenerateToken : IUniwarePando
    {
        private readonly IConfiguration iconfiguration;
        private readonly string connectionstring;
        public GenerateToken(IConfiguration iconfiguration, string connectionstring)
        {
            this.iconfiguration = iconfiguration;
            this.connectionstring = connectionstring;
        }

        
        public string GenerateJWTTokens(TokenEntity tokenEntity, out TokenEntity TokenEntity)
        {
            try
            {
                TokenEntity = SPWrapper.Tokencheck(tokenEntity.username, tokenEntity.password);
                //var BLSTOWaybill= BLSTOWaybil();
                if (TokenEntity.username == null)
                {
                    TokenEntity = new TokenEntity();
                    return null;
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.UTF8.GetBytes(iconfiguration["JWT:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                  {
                       new Claim(JwtRegisteredClaimNames.Name, TokenEntity.username),
                        new Claim(JwtRegisteredClaimNames.Sid, TokenEntity.password),
                       new Claim(JwtRegisteredClaimNames.UniqueName, "Pando".ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Environment",TokenEntity.Environment),
                        
                  }),

                    Expires = DateTime.Now.AddHours(24),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var bearer = new JwtSecurityTokenHandler().WriteToken(token);
                return "bearer " + bearer;
                //return bearer;

                //return userName;
            }
            
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //public TokenEntity GetLogin(string? UserName, string? Password)
        //{
        //    var connection = new SqlConnection(connectionstring);
        //    //var Tokenentity = connection.ExecuteQuery<TokenEntity>("[dbo].[sp_tokenvalidate]sp_tokenvalidate", new { @username = UserName, @password = Password }, commandType: CommandType.StoredProcedure).FirstOrDefault();
        //    connection.Open();
        //    connection.co
        //    return Tokenentity;
        //}
    }
}
