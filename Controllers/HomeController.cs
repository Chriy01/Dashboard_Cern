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
using Dashboard.BusinessLayer;
using Dashboard.Implementation;

namespace Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseService _repo;
        private readonly AppDbContext _dbContext;
        private Int32 ComunitaID;

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
            _repo = new DatabaseService(dbContext);
        }
        public ActionResult Login()
        {
            HttpContext.Session.SetInt32("UserID", -1);
            HttpContext.Session.SetInt32("Comunita_Id", -1);
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

        [HttpGet]
        public async Task<IActionResult> nuovaComunita()
        {
            HttpContext.Session.SetInt32("Comunita_Id", -1);
            return Json(""); // o un'altra azione
        }


        [HttpPost]
        public async Task<IActionResult> SaveSelectedValue(string User_Type)
        {
            Global.User_Type = User_Type;
            return Json(""); // o un'altra azione
        }

        [HttpGet]
        public async Task<JsonResult> GetTable()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            var _utente_comunita = new List<Utente_Comunita>();

            if (userId != null)
            {
                _utente_comunita = await _repo.Utente_ComunitaRepository.GetUtente_ComunitaByIdUtente((int)userId);
            }
            
            var comunitaInfoList = new List<ComunitaInfo>();

            foreach (var item in _utente_comunita)
            {
                var _comunita = await _repo.ComunitaRepository.GetById(item.Comunita_Id);

                if (_comunita != null)
                {
                    var comunitaInfo = new ComunitaInfo
                    {
                        Nome = _comunita.Nome,
                        Anno = DateTime.Now.Year, // Supponendo l'anno corrente, puoi modificarlo secondo necessità
                        ZdM = _comunita.Zona_di_mercato,
                        Tipologia = _comunita.iscomunita ? "Comunità" : "",
                        ZG = _comunita.zona_geografica,
                        Btn = "<button type=\"button\" class=\"btn btn-sm mr-2 btn-info\" data-toggle=\"modal\" onclick=\"editComunita(" + _comunita.Comunita_Id.ToString() + ")\"><i class=\"fas fa-pencil-alt\"></i></button>" + "<button type=\"button\" class=\"btn btn-sm mr-2 btn-info\" data-toggle=\"modal\" onclick=\"Modi(" + _comunita.Comunita_Id.ToString() + ")\"><i class=\"fas fa-solid fa-eye\"></i></button>" + "<button type=\"button\" id=\"sa-confirm\" onclick=\"_SetId('" + _comunita.Comunita_Id.ToString() + "')\" class=\"btn btn-sm btn-danger btncancel\"><i class=\"fas fa-trash-alt\"></i></button>" 
                    };
                    comunitaInfoList.Add(comunitaInfo);
                }
            }

            return Json(comunitaInfoList);



        }
        [HttpGet]
        public async Task<JsonResult> SetIdComunita(Dictionary<string, string> formData)
        {
            var id = formData["id"].ToUpper();
            if (id == "")
            {
                id = "-1";
            }

            ComunitaID = Convert.ToInt32(id);



            return Json("");



        }

        [HttpGet]
        public async Task<JsonResult> DeleteComunita(Dictionary<string, string> formData)
        {
            var id = formData["id"].ToUpper();
            if (id == "")
            {
                id = "-1";
            }

            ComunitaID = Convert.ToInt32(id);

            await _repo.ComunitaRepository.Delete(Convert.ToInt32(ComunitaID));

            _repo.SaveChanges();


            return Json("");



        }
        [HttpGet]
        public async Task<IActionResult> EditComunita(Dictionary<string, string> formData)
        {
            var id = formData["id"].ToUpper();
            HttpContext.Session.SetInt32("Comunita_Id", Convert.ToInt32(id));
            return RedirectToAction("Index", "DatiGenerali", new { Id = id });
        }

        [HttpGet]
        public async Task<IActionResult> ModificaComunita(Dictionary<string, string> formData)
        {
            var id = formData["id"].ToUpper();
            HttpContext.Session.SetInt32("Comunita_Id", Convert.ToInt32(id));
            return RedirectToAction("Index", "DatiUtenze", new { Id = id });
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
        private class ComunitaInfo
        {
            public string Nome { get; set; }
            public int Anno { get; set; }
            public string ZdM { get; set; } // Zona di Mercato
            public string Tipologia { get; set; }
            public string ZG { get; set; } // Zona Geografica
            public string Btn { get; set; }
        }


    }
    public class MyResponseObject
    {
        public List<List<double>> esh_configurations { get; set; }
        public List<List<double>> sci_configurations { get; set; }
        public List<List<double>> ssi_configurations { get; set; }
    }

}