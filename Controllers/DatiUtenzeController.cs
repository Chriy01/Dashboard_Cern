using Dashboard.Utilities;
using Microsoft.AspNetCore.Mvc;
using Dashboard.Utilities;
using Dashboard.Database;
using Dashboard.Implementation;
using Microsoft.EntityFrameworkCore;
using Dashboard.BusinessLayer;

namespace Dashboard.Controllers
{

    public class DatiUtenzeController : Controller
    {
        private readonly ILogger<DatiUtenzeController> _logger;
        private readonly AppDbContext _dbContext;
        private DatabaseService _repo;
        private Comunita _comunita;
        private Consumer _consumer;
        private Prosumer _prosumer;
        private Impianto _impianto;
        private Int32? id_;
        private int ConsumerID;
        private int ProsumerID;
        private int ImpiantoID;
        public IActionResult Index(string Id)
        {
            if ((HttpContext.Session.GetInt32("UserID") != null) && (HttpContext.Session.GetInt32("UserID") > -1))
            {
                id_ = HttpContext.Session.GetInt32("Comunita_Id");
                ConsumerID = -1;
                ProsumerID = -1;
                ImpiantoID = -1;


                return View("DatiUtenze");
                
            }
            else
            {
                return RedirectToAction("Login","Home");
            }
        }

  
        public  DatiUtenzeController(ILogger<DatiUtenzeController> logger, AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
            _repo = new DatabaseService(dbContext);
        }


        private async Task ConfigureComunitaAsync(int Comunita_id)
        {
            int comunitaId = Comunita_id;

            if (comunitaId > -1)
            {
                _comunita = await _repo.ComunitaRepository.GetById(comunitaId);
            }
            else
            {
                _comunita = await _repo.ComunitaRepository.GetById(-1);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveData(Dictionary<string, string> formData)
        {
            /*    ZonaMercato: ZonaMercato,
                Tipo_Conf: Tipo_Conf,
                Inflazione: $('#Inflazione').val(),
                Interesse: $('#Interesse').val(),
                ZonaGeo: $('#ZonaGeo').val(),
                Anno: $('#datepicker').val()
             */
            id_ = HttpContext.Session.GetInt32("Comunita_Id");
            var Comunita_Id = id_;
            
            ConsumerID = (int)Convert.ToInt32(formData["consumer_id"]);
            var forma = (int)Convert.ToInt32(formData["forma"]);
            var tipo_utenza = (int)Convert.ToInt32(formData["tipo_utenza"]);
            var n_utenze = (int)Convert.ToInt32(formData["n_utenze"]);
            var modalita_inserimento = (int)Convert.ToInt32(formData["modalita_inserimento"]);
            var consumo_annuale = (int)Convert.ToInt32(formData["consumo_annuale"]);
            var Consumo_January = (float)Convert.ToDouble(formData["gennaio"]);
            var Consumo_February = (float)Convert.ToDouble(formData["Febbraio"]);
            var Consumo_March = (float)Convert.ToDouble(formData["Marzo"]);
            var Consumo_April = (float)Convert.ToDouble(formData["Aprile"]);
            var Consumo_May = (float)Convert.ToDouble(formData["Maggio"]);
            var Consumo_June = (float)Convert.ToDouble(formData["Giugno"]);
            var Consumo_July = (float)Convert.ToDouble(formData["Luglio"]);
            var Consumo_August = (float)Convert.ToDouble(formData["Agosto"]);
            var Consumo_September = (float)Convert.ToDouble(formData["Settembre"]);
            var Consumo_October = (float)Convert.ToDouble(formData["Ottobre"]);
            var Consumo_November = (float)Convert.ToDouble(formData["Novembre"]);
            var Consumo_December = (float)Convert.ToDouble(formData["Dicembre"]);
            var Descrizione = formData["Descrizione"];

            _consumer = new Consumer();
            _consumer.Forma_Utenza = forma;
            _consumer.Tipo_Utenza_Id = tipo_utenza;
            _consumer.N_Utenze_Cluster = n_utenze;
            _consumer.Modalita_Inserimento = modalita_inserimento;
            _consumer.Consumo_Annuale = consumo_annuale;
            _consumer.Consumo_Gennaio = Consumo_January;
            _consumer.Consumo_Febbraio = Consumo_February;
            _consumer.Consumo_Marzo = Consumo_March;
            _consumer.Consumo_Aprile = Consumo_April;
            _consumer.Consumo_Maggio = Consumo_May;
            _consumer.Consumo_Giugno = Consumo_June;
            _consumer.Consumo_Luglio = Consumo_July;
            _consumer.Consumo_Agosto = Consumo_August;
            _consumer.Consumo_Settembre = Consumo_September;
            _consumer.Consumo_Ottobre = Consumo_October;
            _consumer.Consumo_Novembre = Consumo_November;
            _consumer.Consumo_Dicembre = Consumo_December;
            _consumer.Comunita_Id = Comunita_Id;
            _consumer.Descrizione = Descrizione;
            // Aggiorna o inserisci i dati nel database
            if (ConsumerID != -1)
            {   
                _consumer.Consumer_Id = ConsumerID;
                _repo.ConsumerRepository.Update(_consumer);
                ConsumerID = -1;
            }
            else
            {
                _repo.ConsumerRepository.Add(_consumer);
            }

            // Salva i cambiamenti nel database
            _repo.SaveChanges();


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

        [HttpGet]
        public async Task<JsonResult> GetTipoUtenza(Dictionary<string, string> formData)
        {
            var id = formData["id"].ToUpper();
            if(id == "") {
                id = "-1";
            }
            var tipo_utenza = new List<Tipo_Utenza>();


            tipo_utenza = await _repo.Tipo_UtenzaRepository.GetListTipoUtenza_ById(Convert.ToInt32(id));
            



            return Json(tipo_utenza);



        }


        [HttpGet]
        public async Task<JsonResult> GetTableConsumer()
        {
            id_ = HttpContext.Session.GetInt32("Comunita_Id");
            var Comunita_Id = id_;
            var consumerList = new List<Consumer>();

            if (Comunita_Id != null)
            {
                consumerList = await _repo.ConsumerRepository.GetByComunitaId((int)Comunita_Id);
            }

            var consumerInfoList = new List<ConsumerInfo>();

            foreach (var item in consumerList)
            {
                var _consumer = await _repo.ConsumerRepository.GetById((int)item.Consumer_Id);

                if (_consumer != null)
                {
                    var forma = "";
                    var modalita = "";
                    switch (_consumer.Forma_Utenza) {
                        case 0:
                            forma = "Privato";
                        break;

                        case 1:
                            forma = "Azienda";
                        break;

                        case 2:
                            forma = "Pubblica";
                        break;
                    }
                    switch (_consumer.Modalita_Inserimento)
                    {
                        
                        case 1:
                            modalita = "Consumo Annuale";
                            break;

                        case 2:
                            modalita = "Consumo Mensile";
                            break;

                        case 3:
                            modalita = "Curva Oraria";
                            break;
                    }

                    var id = _consumer.Tipo_Utenza_Id;
               
                    var tipo_utenza = new List<Tipo_Utenza>();


                    tipo_utenza = await _repo.Tipo_UtenzaRepository.GetListTipoUtenza_ById(Convert.ToInt32(id));


                    var consumerInfo = new ConsumerInfo
                    {
                        nome = _consumer.Descrizione,
                        formautenza = forma,
                        tipoutenza = tipo_utenza.First().Descrizione,
                        modalita = modalita,
                        nutenzecluster = _consumer.N_Utenze_Cluster.ToString(),
                        btn =  "<button type=\"button\" class=\"btn btn-sm mr-2 btn-info\" data-toggle=\"modal\" onclick=\"ModiConsumer(" + _consumer.Consumer_Id.ToString() + ")\"><i class=\"fas fa-pencil-alt\"></i></button>" + "<button type=\"button\" id=\"sa-confirm\" onclick=\"_SetIdConsumer('" + _consumer.Consumer_Id.ToString() + "')\" class=\"btn btn-sm btn-danger btncancel btncancelConsumer\"><i class=\"fas fa-trash-alt\"></i></button>" // Questo deve essere specificato secondo necessità

                    };
                    consumerInfoList.Add(consumerInfo);
                }
            }
            //AddConsumer(Nome, Forma_Utenza, Tipo_Utenza, N_Utenze_Cluster, Modalita, Btn)

            return Json(consumerInfoList);



        }

        [HttpGet]
        public async Task<JsonResult> SetIdConsumer(Dictionary<string, string> formData)
        {
            var id = formData["id"].ToUpper();
            if (id == "")
            {
                id = "-1";
            }

            ConsumerID = Convert.ToInt32(id);



            return Json("");



        }


        [HttpGet]
        public async Task<JsonResult> DeleteConsumer(Dictionary<string, string> formData)
        {
            var id = formData["id"].ToUpper();
            if (id == "")
            {
                id = "-1";
            }

            ConsumerID = Convert.ToInt32(id);
            var cons = new Consumer();


            await _repo.ConsumerRepository.Delete(Convert.ToInt32(ConsumerID));
        
            _repo.SaveChanges();


            return Json("");



        }

        [HttpGet]
        public async Task<JsonResult> ModificaConsumer(Dictionary<string, string> formData)
        {
            var id = formData["id"].ToUpper();
            if (id == "")
            {
                id = "-1";
            }
            var cons = new Consumer();


            cons = await _repo.ConsumerRepository.GetById(Convert.ToInt32(id));
            ConsumerID = Convert.ToInt32(id);



            return Json(cons);
           


        }

        [HttpPost]
        public async Task<IActionResult> SaveGeneralProsumer(Dictionary<string, string> formData)
        {
            id_ = HttpContext.Session.GetInt32("Comunita_Id");
            var Comunita_Id = id_;

            ProsumerID = (int)Convert.ToInt32(formData["prosumer_id"]);
            var forma = (int)Convert.ToInt32(formData["forma"]);
            var tipo_utenza = (int)Convert.ToInt32(formData["tipo_utenza"]);
            var n_utenze = (int)Convert.ToInt32(formData["n_utenze"]);
            var modalita_inserimento = (int)Convert.ToInt32(formData["modalita_inserimento"]);
            var consumo_annuale = (int)Convert.ToInt32(formData["consumo_annuale"]);
            var Consumo_January = (float)Convert.ToDouble(formData["gennaio"]);
            var Consumo_February = (float)Convert.ToDouble(formData["Febbraio"]);
            var Consumo_March = (float)Convert.ToDouble(formData["Marzo"]);
            var Consumo_April = (float)Convert.ToDouble(formData["Aprile"]);
            var Consumo_May = (float)Convert.ToDouble(formData["Maggio"]);
            var Consumo_June = (float)Convert.ToDouble(formData["Giugno"]);
            var Consumo_July = (float)Convert.ToDouble(formData["Luglio"]);
            var Consumo_August = (float)Convert.ToDouble(formData["Agosto"]);
            var Consumo_September = (float)Convert.ToDouble(formData["Settembre"]);
            var Consumo_October = (float)Convert.ToDouble(formData["Ottobre"]);
            var Consumo_November = (float)Convert.ToDouble(formData["Novembre"]);
            var Consumo_December = (float)Convert.ToDouble(formData["Dicembre"]);
            var Descrizione = formData["Descrizione"];

            _prosumer = new Prosumer();
            _prosumer.Forma_Utenza = forma;
            _prosumer.Tipo_Utenza_Id = tipo_utenza;
            _prosumer.N_Utenze_Cluster = n_utenze;
            _prosumer.Modalita_Inserimento = modalita_inserimento;
            _prosumer.Consumo_Annuale = consumo_annuale;
            _prosumer.Consumo_Gennaio = Consumo_January;
            _prosumer.Consumo_Febbraio = Consumo_February;
            _prosumer.Consumo_Marzo = Consumo_March;
            _prosumer.Consumo_Aprile = Consumo_April;
            _prosumer.Consumo_Maggio = Consumo_May;
            _prosumer.Consumo_Giugno = Consumo_June;
            _prosumer.Consumo_Luglio = Consumo_July;
            _prosumer.Consumo_Agosto = Consumo_August;
            _prosumer.Consumo_Settembre = Consumo_September;
            _prosumer.Consumo_Ottobre = Consumo_October;
            _prosumer.Consumo_Novembre = Consumo_November;
            _prosumer.Consumo_Dicembre = Consumo_December;
            _prosumer.Comunita_Id = Comunita_Id;
            _prosumer.Descrizione = Descrizione;
            // Aggiorna o inserisci i dati nel database
            if (ProsumerID != -1)
            {
                _prosumer.Prosumer_Id = ConsumerID;
                _repo.ProsumerRepository.Update(_prosumer);
                ProsumerID = -1;
            }
            else
            {
                _repo.ProsumerRepository.Add(_prosumer);
            }

            // Salva i cambiamenti nel database
            _repo.SaveChanges();


            return Json(""); // o un'altra azione
        }




        //IMPIANTO
        [HttpPost]
        public async Task<IActionResult> SaveGeneralImpianto(Dictionary<string, string> formData)
        {
            id_ = HttpContext.Session.GetInt32("Comunita_Id");
            var Comunita_Id = id_;

            ImpiantoID = (int)Convert.ToInt32(formData["Impianto_id"]);
            var forma = (int)Convert.ToInt32(formData["forma"]);
            var tipo_utenza = (int)Convert.ToInt32(formData["tipo_utenza"]);
            var n_utenze = (int)Convert.ToInt32(formData["n_utenze"]);
            var modalita_inserimento = (int)Convert.ToInt32(formData["modalita_inserimento"]);
            var consumo_annuale = (int)Convert.ToInt32(formData["consumo_annuale"]);
            var Consumo_January = (float)Convert.ToDouble(formData["gennaio"]);
            var Consumo_February = (float)Convert.ToDouble(formData["Febbraio"]);
            var Consumo_March = (float)Convert.ToDouble(formData["Marzo"]);
            var Consumo_April = (float)Convert.ToDouble(formData["Aprile"]);
            var Consumo_May = (float)Convert.ToDouble(formData["Maggio"]);
            var Consumo_June = (float)Convert.ToDouble(formData["Giugno"]);
            var Consumo_July = (float)Convert.ToDouble(formData["Luglio"]);
            var Consumo_August = (float)Convert.ToDouble(formData["Agosto"]);
            var Consumo_September = (float)Convert.ToDouble(formData["Settembre"]);
            var Consumo_October = (float)Convert.ToDouble(formData["Ottobre"]);
            var Consumo_November = (float)Convert.ToDouble(formData["Novembre"]);
            var Consumo_December = (float)Convert.ToDouble(formData["Dicembre"]);
            var Descrizione = formData["Descrizione"];

            _impianto = new Impianto();
            _impianto.Forma_Utenza = forma;
            _impianto.Tipo_Utenza_Id = tipo_utenza;
            _impianto.N_Utenze_Cluster = n_utenze;
            _impianto.Modalita_Inserimento = modalita_inserimento;
            _impianto.Consumo_Annuale = consumo_annuale;
            _impianto.Consumo_Gennaio = Consumo_January;
            _impianto.Consumo_Febbraio = Consumo_February;
            _impianto.Consumo_Marzo = Consumo_March;
            _impianto.Consumo_Aprile = Consumo_April;
            _impianto.Consumo_Maggio = Consumo_May;
            _impianto.Consumo_Giugno = Consumo_June;
            _impianto.Consumo_Luglio = Consumo_July;
            _impianto.Consumo_Agosto = Consumo_August;
            _impianto.Consumo_Settembre = Consumo_September;
            _impianto.Consumo_Ottobre = Consumo_October;
            _impianto.Consumo_Novembre = Consumo_November;
            _impianto.Consumo_Dicembre = Consumo_December;
            _impianto.Comunita_Id = Comunita_Id;
            _impianto.Descrizione = Descrizione;
            // Aggiorna o inserisci i dati nel database
            if (ImpiantoID != -1)
            {
                _impianto.Impianto_Id = ImpiantoID;
                _repo.ImpiantoRepository.Update(_impianto);
                ImpiantoID = -1;
            }
            else
            {
                _repo.ImpiantoRepository.Add(_impianto);
            }

            // Salva i cambiamenti nel database
            _repo.SaveChanges();


            return Json(""); // o un'altra azione
        }


        private class ConsumerInfo
        {
            public string nome { get; set; }
            public string formautenza { get; set; }
            public string tipoutenza { get; set; }
            public string nutenzecluster { get; set; } 
            public string modalita { get; set; }
            public string btn {  get; set; }
        }

    }


}
