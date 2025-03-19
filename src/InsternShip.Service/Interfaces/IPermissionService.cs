using InsternShip.Data.Model;

namespace InsternShip.Service.Interfaces
{
    public interface IPermissionService
    {
        Task<TokenModel> GetInfoToken();
    }
}
