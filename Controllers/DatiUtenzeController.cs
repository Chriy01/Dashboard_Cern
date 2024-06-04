using Dashboard.Utilities;
using Microsoft.AspNetCore.Mvc;
using Dashboard.Utilities;

namespace Dashboard.Controllers
{
    public class DatiUtenzeController : Controller
    {
        public IActionResult Index()
        {
            if ((HttpContext.Session.GetInt32("UserID") != null) || (HttpContext.Session.GetInt32("UserID") > -1))
            {
                return View("DatiUtenze");
            }
            else
            {
                return RedirectToAction("Login","Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveMonths(Dictionary<string, string> formData)
        {
            /*    ZonaMercato: ZonaMercato,
                Tipo_Conf: Tipo_Conf,
                Inflazione: $('#Inflazione').val(),
                Interesse: $('#Interesse').val(),
                ZonaGeo: $('#ZonaGeo').val(),
                Anno: $('#datepicker').val()
             */
            Global.Consumo_January = (float)Convert.ToDouble(formData["Gennaio"]);
            Global.Consumo_February = (float)Convert.ToDouble(formData["Febbraio"]);
            Global.Consumo_March = (float)Convert.ToDouble(formData["Marzo"]);
            Global.Consumo_April = (float)Convert.ToDouble(formData["Aprile"]);
            Global.Consumo_May = (float)Convert.ToDouble(formData["Maggio"]);
            Global.Consumo_June = (float)Convert.ToDouble(formData["Giugno"]);
            Global.Consumo_July = (float)Convert.ToDouble(formData["Luglio"]);
            Global.Consumo_August = (float)Convert.ToDouble(formData["Agosto"]);
            Global.Consumo_September = (float)Convert.ToDouble(formData["Settembre"]);
            Global.Consumo_October = (float)Convert.ToDouble(formData["Ottobre"]);
            Global.Consumo_November = (float)Convert.ToDouble(formData["Novembre"]);
            Global.Consumo_December = (float)Convert.ToDouble(formData["Dicembre"]);

            return Json(""); // o un'altra azione
        }

        [HttpPost]
        public async Task<IActionResult> SaveDatiUtenze(Dictionary<string, string> formData)
        {


            Global.Forma_Utenza = formData["Forma_Utenza"];
            Global.N_Cluster = (long)Convert.ToDouble(formData["Tipo_Utenza"]); 
            Global.Tipo_Utenza = formData["Tipo_Utenza"];
            Global.Modalita_Inserimento = formData["Modalita_Inserimento"];

            if (Global.User_Type == "c")
            {
                return Json("0"); // o un'altra azione
            }
            else
            {
                return Json("1"); // o un'altra azione
            }
            
        }

    }
}
