using InsternShip.Common;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InsternShip.Service
{
    public class ExportDataService : IExportDataService
    { 
        private readonly IExportDataRepository _exportdataRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IApplicationStatusRepository _applicationStatusRepository;
        private readonly IApplicationStatusUpdateRepository _applicationStatusUpdateRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IEventParticipationRepository _eventParticipationRepository;
        private readonly IInterviewSessionRepository _interviewSessionRepository;
        private readonly IInterviewRepository _interviewRepository;

        public ExportDataService(IExportDataRepository exportdataRepository, IApplicationRepository applicationRepository, IApplicationStatusRepository applicationStatusRepository, IApplicationStatusUpdateRepository applicationStatusUpdateRepository, ICandidateRepository candidateRepository, IEventParticipationRepository eventParticipationRepository, IInterviewSessionRepository interviewSessionRepository, IInterviewRepository interviewRepository)
        {
            _exportdataRepository = exportdataRepository;
            _applicationRepository = applicationRepository;
            _applicationStatusRepository = applicationStatusRepository;
            _applicationStatusUpdateRepository = applicationStatusUpdateRepository;
            _candidateRepository = candidateRepository;
            _eventParticipationRepository = eventParticipationRepository;
            _interviewSessionRepository = interviewSessionRepository;
            _interviewRepository = interviewRepository;
        }

        public async Task<byte[]> CandidateReport( )
        {

            var data = await _applicationStatusUpdateRepository.GetCandidateReport();
            return await _exportdataRepository.Export(data.ToList(),null,null,null);
        }
        public async Task<byte[]> RecruitmentReport()
        {
            var data = await _applicationRepository.GetRecruitmentReport();
            return await _exportdataRepository.Export(null, data.ToList(), null, null);
        }
        public async Task<byte[]> EventReport()
        {
            var data = await _eventParticipationRepository.GetEventReport();
            return await _exportdataRepository.Export(null,null, data.ToList(), null);
        }

        public async Task<byte[]> InterviewReport()
        {
            var data = await _interviewRepository.GetInterviewReport();
            return await _exportdataRepository.Export(null, null, null, data.ToList());
        }
    }
}
