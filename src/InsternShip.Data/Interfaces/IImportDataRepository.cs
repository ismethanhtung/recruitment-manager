using InsternShip.Data.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static InsternShip.Data.Model.ImportDataModel;

namespace InsternShip.Data.Interfaces
{
    public interface IImportDataRepository
    {
        Task<ImportUserModel> GetDataUser(IFormFile file);
        Task<IEnumerable<ImportJobPostModel>> GetDataJobPost(IFormFile file);
        Task<IEnumerable<ImportEventPostModel>> GetDataEventPost(IFormFile file);
    }
}
