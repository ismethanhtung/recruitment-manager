using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Service.Interfaces;
using Microsoft.AspNetCore.Http;

namespace InsternShip.Service
{
    public class PermissionService: IPermissionService
    {
        private readonly IDecodeRepository _decodeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PermissionService(IDecodeRepository decodeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _decodeRepository = decodeRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TokenModel> GetInfoToken()
        {
            var authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            var token = authHeader.Replace("Bearer ", string.Empty);
            var inforDecode = _decodeRepository.Decode(token);
            var res = new TokenModel
            {
                UserId = new Guid(inforDecode.UserId),
                UserClaimId = new Guid(inforDecode.UserClaimId),
            };
            return await Task.FromResult(res);
        }
    }
}
