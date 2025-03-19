using InsternShip.Data.ViewModels;

namespace InsternShip.Service.Interfaces
{
    public interface IInterviewSessionService
    {
        Task<InterviewSessionDetailViewModel> GetDetail(Guid interviewId);
    }
}
