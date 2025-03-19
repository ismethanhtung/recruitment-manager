using AutoMapper;
using InsternShip.Common;
using InsternShip.Data.Entities;
using InsternShip.Data.Interfaces;
using InsternShip.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace InsternShip.Data.Repositories
{
    public class CVRepository : Repository<CV>, ICVRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public CVRepository(RecruitmentDB context, IUnitOfWork uow, IMapper mapper) : base(context)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<bool> Create(CVModel request)
        {
            try
            {
                var cv = _mapper.Map<CV>(request);
                Entities.Add(cv);
                _uow.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<CV?> IsExistFile(Guid? candidateId)
        {
            var data = await Entities.FirstOrDefaultAsync(cv => cv.CandidateId == candidateId);
            return data;
        }

        public async Task<bool> Delete(Guid? canId)
        {
            var data = await Entities.FirstOrDefaultAsync(cv => cv.CandidateId == canId) ?? throw new KeyNotFoundException(ExceptionMessages.FileNotFound);
            try
            {
                Entities.Remove(data);
                _uow.SaveChanges();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<CVModel?> GetByCanId(Guid? canId)
        {
            try
            {
                var data = await Entities.Include(cv => cv.Candidate).FirstOrDefaultAsync(cvc => cvc.CandidateId == canId && cvc.Candidate.IsDeleted == false);
                if (data == null) { return null; }
                var cv = _mapper.Map<CVModel>(data);
                return cv;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}


