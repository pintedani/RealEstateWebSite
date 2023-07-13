using Microsoft.AspNetCore.Mvc;

namespace CoreTestApp.Controllers
{
    public class AnunturiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
