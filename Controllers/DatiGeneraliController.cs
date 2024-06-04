using Microsoft.AspNetCore.Mvc;
using Dashboard.Utilities;

namespace Dashboard.Controllers
{
    public class DatiGeneraliController : Controller
    {
        public IActionResult Index()
        {
            if ((HttpContext.Session.GetInt32("UserID") != null) || (HttpContext.Session.GetInt32("UserID") > -1))
            {
                return View("DatiGenerali");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
      
        }

        [HttpPost]
        public async Task<IActionResult> SaveDatiGenerali(Dictionary<string, string> formData)
        {


            Global.Zona_Di_Mercato = formData["ZonaMercato"];
            Global.Tipo_Conf = formData["Tipo_Conf"];
            Global.Tasso_Infl = (float)Convert.ToDouble(formData["Inflazione"]);
            Global.Tasso_Int = (float)Convert.ToDouble(formData["Interesse"]);
            Global.Zona_Geo = formData["ZonaGeo"];
            Global.Anno_Rif = formData["Anno"];

            return Json(""); // o un'altra azione
        }

    }
}
