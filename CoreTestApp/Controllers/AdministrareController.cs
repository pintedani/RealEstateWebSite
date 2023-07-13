using Microsoft.AspNetCore.Mvc;

namespace CoreTestApp.Controllers
{
    public class AdministrareController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
