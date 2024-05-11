using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Controllers
{
    public class DatiTecniciController : Controller
    {
        public IActionResult Index()
        {
            return View("DatiTecnici");
        }
    }
}
