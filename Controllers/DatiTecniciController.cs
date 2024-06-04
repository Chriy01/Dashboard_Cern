using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Controllers
{
    public class DatiTecniciController : Controller
    {
        public IActionResult Index()
        {
            if ((HttpContext.Session.GetInt32("UserID") != null) || (HttpContext.Session.GetInt32("UserID") > -1))
            {
                return View("DatiTecnici");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
    }
}
