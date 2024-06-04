using Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Globalization;
using CsvHelper.Configuration;
using System.Text;
using Newtonsoft.Json;
using System.Web;
using System.Reflection;
using Dashboard.Utilities;
using Dashboard.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Session;

namespace Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _dbContext;

        double[] ssiData = null;
        double[] sciData = null;

        private CsvConfiguration conf = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
            TrimOptions = TrimOptions.Trim,
            MissingFieldFound = null,
            HeaderValidated = null,
            Delimiter = ";"
        };


        private ParametersMd ModelP;
      

        public HomeController(ILogger<HomeController> logger,AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
                 
        }
        public ActionResult Login()
        {
            HttpContext.Session.SetInt32("UserID", -1);
            return RedirectToAction("Index","Login");
        }

        public IActionResult Index()
        {
            
            if ((HttpContext.Session.GetInt32("UserID") != null) || (HttpContext.Session.GetInt32("UserID") > -1))
            {
                return View("Index");
            }
            else
            {
                return RedirectToAction("Login");
            }
            /*
            List<SelectListItem> options = new List<SelectListItem>
            {
                new SelectListItem { Value = "fixed", Text = "Determinato" },
                new SelectListItem { Value = "parametric", Text = "Parametrico" }
            };
            ViewBag.Options = options;
            ModelP = new ParametersMd();*/
            //  var p = _dbContext.Comunita.ToArray();

           
        }


        [HttpPost]
        public async Task<IActionResult> SaveSelectedValue(string User_Type)
        {
            Global.User_Type = User_Type;
            return Json(""); // o un'altra azione
        }

        // Metodo per la chiamata JavaScript dal C#
        [HttpGet]
        public IActionResult TestFunction()
        {
            Thread.Sleep(2000);

            return Json(new { functionName = "showAlert" });

        }

        [HttpPost]
        public async Task<IActionResult> SendParamsAsync2(ParametersMd VParametersMD)
        {

            if (ModelP == null)
            {
                ModelP = new ParametersMd();
            }
            ModelP.Count_Famiglie = VParametersMD.Count_Famiglie;
            ModelP.Posizione = VParametersMD.Posizione;
            ModelP.Max_power = VParametersMD.Max_power;
            ModelP.Classe_energetica = VParametersMD.Classe_energetica;
            ModelP.Media_metriq = VParametersMD.Media_metriq;
            ModelP.Step = VParametersMD.Step;

            // Costruisci un oggetto JSON con i valori dei parametri
            var paramValues = new Dictionary<string, object>
            {
                { "n_hh", ModelP.Count_Famiglie },
                { "location", ModelP.Posizione },
                { "power_max", ModelP.Max_power },
                { "en_class", ModelP.Classe_energetica },
                { "ftg_avg", ModelP.Media_metriq },
                { "dt_aggr", ModelP.Step }
                // Aggiungi altri parametri se necessario
            };

            // Serializza l'oggetto JSON
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(paramValues);

            // URL del tuo endpoint Flask
            string flaskEndpointUrl = "http://127.0.0.1:5000/process_params";

            using (var client = new HttpClient())
            {
                // Crea il contenuto della richiesta con il JSON
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Invia una richiesta POST all'endpoint Flask
                var response = await client.PostAsync(flaskEndpointUrl, content);

                // Controlla la risposta
                if (response.IsSuccessStatusCode)
                {
                    // Leggi la risposta come stringa JSON
                    string responseJson = await response.Content.ReadAsStringAsync();
                    flaskEndpointUrl = "http://127.0.0.1:5000/executeProgram";
                    response = await client.PostAsync(flaskEndpointUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        // Deserializza il JSON di risposta in un oggetto C#
                        var responseObject = JsonConvert.DeserializeObject<MyResponseObject>(jsonResponse);

                        // Ora puoi accedere ai valori dell'oggetto responseObject


                        ssiData = responseObject.ssi_configurations[0].ToArray();
                        sciData = responseObject.sci_configurations[0].ToArray();
                        
                    }
                    else
                    {
                        // Gestisci errori di connessione, HTTP o altre situazioni di errore
                        // ad esempio, registra il problema o restituisci un messaggio di errore alla vista
                    }
                }
                else
                {
                    // Gestisci errori di connessione, HTTP o altre situazioni di errore
                    // ad esempio, registra il problema o restituisci un messaggio di errore alla vista
                }
            }

            // Ritorna una vista o un'altra azione se necessario
         
           /* string functionName = "TestJs";
            string script = $@"
                                {functionName}({ssiData},{sciData});
                            ";

            // Esegui lo script JavaScript

            ViewBag.DynamicScripts = script; */
            var chartData = new
            {
                ssi_configurations = ssiData,
                sci_configurations = sciData
            };
            return Json(chartData);

            //return Json(new { functionName = "SetChart", additionalData = chartData });
        }

        [HttpPost]
        public IActionResult SendPhotoVoltaicParams(ParametersMd VParametersMD)
        {
            if (ModelP == null)
            {
                ModelP = new ParametersMd();
            }
            ModelP.CountFotovoltaico = VParametersMD.CountFotovoltaico;
            ModelP.TipoFotovoltaico = VParametersMD.TipoFotovoltaico;
            return View("Index");
        }

        [HttpPost]
        public IActionResult SendSimulationParams(ParametersMd VParametersMD)
        {
            ModelP ??= new ParametersMd();
            ModelP.Count = VParametersMD.Count;
            ModelP.Tipo = VParametersMD.Tipo;
            ModelP.Portata_Min = VParametersMD.Portata_Min;
            ModelP.Portata_Max = VParametersMD.Portata_Max;
            StartExecution();
            return View("Index");
        }

        private void StartExecution()
        {
            int i = 0;
            

        }
      
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

      
    }
    public class MyResponseObject
    {
        public List<List<double>> esh_configurations { get; set; }
        public List<List<double>> sci_configurations { get; set; }
        public List<List<double>> ssi_configurations { get; set; }
    }
}