using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;


namespace InsternShip.Service.Interfaces
{
    public interface IMeetingService
    {
        Task<MeetingModel> CreateZoomMeeting(CreateMeetingModel request);
        Task<object> GetAll(string? type, int? limit, int? page);
        Task<object> GetById(string id);
        Task<bool> Delete(string id);
    }
}
