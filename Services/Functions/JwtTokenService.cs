using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Functions
{
    public interface IJwtTokenService
    {
        string GenerateToken(int uid, string email, DateTime expireDt);
        int? GetUidFromToken(string token);
    }

    public class JwtTokenService(string jwtKey) : IJwtTokenService
    {
        public string GenerateToken(int uid, string email, DateTime expireDt)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new();
                byte[] key = Encoding.ASCII.GetBytes(jwtKey);
                SecurityTokenDescriptor tokenDescriptor = new()
                {
                    Subject = new ClaimsIdentity([new Claim("uid", uid.ToString()), new Claim(ClaimTypes.Email, email)]),
                    Expires = expireDt,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }catch(Exception ex) { throw ex; }
        }

        public int? GetUidFromToken(string token)
        {
            if (token == null)
                return null;

            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(jwtKey);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
                int uid = int.Parse(jwtToken.Claims.First(x => x.Type == "uid").Value);
                return uid;
            }
            catch
            {
                throw;
            }
        }
    }
}
