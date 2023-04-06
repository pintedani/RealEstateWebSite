using CoreTestApp.Models;
using CoreTestApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CoreTestApp.Controllers
{
    public class HomeController : Controller
    {
        private IImobilRepository _imobilRepository;

        public HomeController(IImobilRepository imobilRepository)
        {
                _imobilRepository = imobilRepository;
        }

        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel(_imobilRepository.AllImobils);
            return View(homeViewModel);
        }

        public IActionResult Details(int id)
        {
            var anunt = _imobilRepository.GetImobilById(id);
            if(anunt == null)
            {
                return NotFound();
            }

            return View(anunt);
        }

        public IActionResult Search() { return View(); }
    }
}
