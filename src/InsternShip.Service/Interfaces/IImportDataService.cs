using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsternShip.Service.Interfaces
{
    public interface IImportDataService
    {
        Task<bool> ImportDataUser(IFormFile file);
        Task<bool> ImportDataJobPost(IFormFile file);
        Task<bool> ImportDataEventPost(IFormFile file);
    }
}
