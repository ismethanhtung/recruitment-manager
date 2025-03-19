using InsternShip.Data.Model;

namespace InsternShip.Data.Interfaces
{
    public interface IDecodeRepository
    {
        DecodeModel? Decode(string? token);
    }
}
