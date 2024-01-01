using Crosscutting;

namespace Imobiliare.UI.Utils
{
    public class EnvironmentService : IEnvironmentService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EnvironmentService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string GetWebRootPath()
        {
            return _webHostEnvironment.WebRootPath;
        }
        // Implement other methods or properties from IEnvironmentService
    }
}
