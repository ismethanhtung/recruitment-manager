using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Data.ViewModels;
using InsternShip.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace InsternShip.Service
{
    public class MeetingService : IMeetingService
    {
        private readonly IMeetingRepository _meetingRepository;

        public MeetingService(IMeetingRepository meetingRepository)
        {
            _meetingRepository= meetingRepository;  
        }
        

        public async Task<MeetingModel> CreateZoomMeeting(CreateMeetingModel request)
        {
            return await _meetingRepository.CreateZoomMeeting(request);
        }
        public async Task<object> GetAll(string? type, int? limit, int? page)
        {
            return await _meetingRepository.GetAll(type, limit, page);
        }
        public async Task<object> GetById(string id)
        {
            return await _meetingRepository.GetById(id);
        }
        public async Task<bool> Delete(string id)
        {
            return await _meetingRepository.Delete(id);
        }

    }
}
