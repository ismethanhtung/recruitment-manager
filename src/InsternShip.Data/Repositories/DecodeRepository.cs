using InsternShip.Common;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace InsternShip.Data.Repositories
{
    public class DecodeRepository: IDecodeRepository
    {
        private readonly IConfiguration _configuration;
        public DecodeRepository(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        public DecodeModel? Decode(string? token)
        {
            if (string.IsNullOrEmpty(token)) { throw new KeyNotFoundException(ExceptionMessages.TokenNotFound); }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                ValidAudience = _configuration["JWT:ValidAudience"],
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken) ?? throw new UnauthorizedAccessException();
                var infoDecode = new DecodeModel
                {
                    UserId = principal.FindFirst("UserId").Value,
                    UserClaimId = principal.FindFirst("UserClaimId").Value
                };
                return infoDecode;
            }
            catch
            {
                return null;
            }
        }
    }
}
