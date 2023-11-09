using Microsoft.AspNetCore.Mvc;

namespace Imobiliare.UI.Controllers
{
    public class ErrorController : Controller
    {
    public ActionResult Index()
    {
      Response.StatusCode = 500;
      return View("Error");
    }

    // GET: Errors
    public ActionResult PageNotFound()
    {
      Response.StatusCode = 404;
      return View();
    }
  }
}