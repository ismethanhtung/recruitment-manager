namespace InsternShip.Data.Interfaces
{
    public interface IGetHtmlBodyRepository
    {
        Task<string> GetBody(string type);
    }
}
