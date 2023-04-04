using Microsoft.AspNetCore.Mvc;

namespace CoreTestApp.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
