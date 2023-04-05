using CoreTestApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreTestApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IImobilRepository _imobilRepository;

        public SearchController(IImobilRepository imobil)
        {
            _imobilRepository = imobil;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var allPies = _imobilRepository.AllImobils;
            return Ok(allPies);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id) 
        {
            if(!_imobilRepository.AllImobils.Any(i => i.Id == id))
            {
                return NotFound();
            }

            var pie = _imobilRepository.GetImobilById(id);
            return Ok(pie);
        }

        [HttpPost]
        public IActionResult SearchPies([FromBody]string searchQuery) 
        { 
            IEnumerable<Imobil> imobils = new List<Imobil>();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                imobils = _imobilRepository.SearchImobils(searchQuery);
            }

            return new JsonResult(imobils);
        }
    }
}
