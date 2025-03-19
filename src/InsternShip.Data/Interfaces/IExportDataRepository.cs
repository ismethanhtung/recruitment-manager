using InsternShip.Data.Model;

namespace InsternShip.Data.Interfaces
{
    public interface IExportDataRepository
    {
        Task<byte[]> Export(List<CandidateReportModel>? canrep, List<RecruitmentReportModel>? recrep, List<EventReportModel>? evrep, List<InterviewReportModel>? invrep);
    }
}
