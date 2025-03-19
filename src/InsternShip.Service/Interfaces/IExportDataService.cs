namespace InsternShip.Service.Interfaces
{
    public interface IExportDataService
    {
        Task<byte[]> CandidateReport();
        Task<byte[]> RecruitmentReport();
        Task<byte[]> EventReport();
        Task<byte[]> InterviewReport();
    }
}
