using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using InsternShip.Service.Interfaces;

namespace InsternShip.Service
{
    public class CVService : ICVService
    {
        private readonly ICVRepository _cVRepository;
        private readonly ICloudinaryRepository _cloudinaryRepository;

        public CVService(ICVRepository cVRepository, ICloudinaryRepository cloudinaryRepository)
        {
            _cVRepository = cVRepository;
            _cloudinaryRepository = cloudinaryRepository;
        }
   
        public async Task<bool> Create(CreateCVModel request)
        {
            //await _candidateRepository.GetById(request.currentId);
            if (request.File == null)
                return false;
            var inforFile = await _cloudinaryRepository.UploadFile(request.File);
            var cv = new CVModel
            {
                CandidateId = request.CurrentId,
                UrlFile = inforFile.UrlFile,
                PublicIdFile = inforFile.PublicIdFile
            };
            var currentCV = await _cVRepository.IsExistFile(cv.CandidateId);
            if(currentCV != null)
            {
                _ = await _cloudinaryRepository.RemoveFile(currentCV.PublicIdFile);
                _ = await _cVRepository.Delete(currentCV.CandidateId);
            }
            return await _cVRepository.Create(cv);
        }
    }
}