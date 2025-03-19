using InsternShip.Data.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace InsternShip.Data.Repositories
{
    public class GetHtmlBodyRepository: IGetHtmlBodyRepository
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public GetHtmlBodyRepository(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<string> GetBody(string type)
        {
            string wwwrootPath = _hostingEnvironment.WebRootPath;
            string bodyContentPath = Path.Combine(wwwrootPath, "BodyContent");
            string fileName = type;
            string filePath = Path.Combine(bodyContentPath, fileName);
            if (System.IO.File.Exists(filePath))
            {
                string content = System.IO.File.ReadAllText(filePath);
                return await Task.FromResult(content);
            }
            return await Task.FromResult(string.Empty);
        }
    }
}
