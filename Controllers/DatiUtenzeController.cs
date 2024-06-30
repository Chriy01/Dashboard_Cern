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
        //PROSUMER

        [HttpGet]
        public async Task<JsonResult> GetTableProsumer()
        {
            id_ = HttpContext.Session.GetInt32("Comunita_Id");
            var Comunita_Id = id_;
            var prosumerList = new List<Prosumer>();

            if (Comunita_Id != null)
            {
                prosumerList = await _repo.ProsumerRepository.GetByComunitaId((int)Comunita_Id);
            }

            var prosumerInfoList = new List<ConsumerInfo>();

            foreach (var item in prosumerList)
            {
                var _prosumer = await _repo.ProsumerRepository.GetById((int)item.Prosumer_Id);

                if (_prosumer != null)
                {
                    var forma = "";
                    var modalita = "";
                    switch (_prosumer.Forma_Utenza)
                    {
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
                    switch (_prosumer.Modalita_Inserimento)
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

                    var id = _prosumer.Tipo_Utenza_Id;

                    var tipo_utenza = new List<Tipo_Utenza>();


                    tipo_utenza = await _repo.Tipo_UtenzaRepository.GetListTipoUtenza_ById(Convert.ToInt32(id));


                    var consumerInfo = new ConsumerInfo
                    {
                        nome = _prosumer.Descrizione,
                        formautenza = forma,
                        tipoutenza = tipo_utenza.First().Descrizione,
                        modalita = modalita,
                        nutenzecluster = _prosumer.N_Utenze_Cluster.ToString(),
                        btn = "<button type=\"button\" class=\"btn btn-sm mr-2 btn-info\" data-toggle=\"modal\" onclick=\"ModiProsumer(" + _prosumer.Prosumer_Id.ToString() + ")\"><i class=\"fas fa-pencil-alt\"></i></button>" + "<button type=\"button\" id=\"sa-confirm\" onclick=\"_SetIdProsumer('" + _prosumer.Prosumer_Id.ToString() + "')\" class=\"btn btn-sm btn-danger btncancel btncancelProsumer\"><i class=\"fas fa-trash-alt\"></i></button>" // Questo deve essere specificato secondo necessità

                    };
                    prosumerInfoList.Add(consumerInfo);
                }
            }
            //AddConsumer(Nome, Forma_Utenza, Tipo_Utenza, N_Utenze_Cluster, Modalita, Btn)

            return Json(prosumerInfoList);



        }
        [HttpGet]
        public async Task<JsonResult> SetIdProsumer(Dictionary<string, string> formData)
        {
            var id = formData["id"].ToUpper();
            if (id == "")
            {
                id = "-1";
            }

            ProsumerID = Convert.ToInt32(id);



            return Json("");



        }


        [HttpGet]
        public async Task<JsonResult> DeleteProsumer(Dictionary<string, string> formData)
        {
            var id = formData["id"].ToUpper();
            if (id == "")
            {
                id = "-1";
            }

            ProsumerID = Convert.ToInt32(id);
            var cons = new Prosumer();


            await _repo.ProsumerRepository.Delete(Convert.ToInt32(ProsumerID));

            _repo.SaveChanges();


            return Json("");



        }

        [HttpGet]
        public async Task<JsonResult> ModificaProsumer(Dictionary<string, string> formData)
        {
            var id = formData["id"].ToUpper();
            if (id == "")
            {
                id = "-1";
            }
            var cons = new Prosumer();


            cons = await _repo.ProsumerRepository.GetById(Convert.ToInt32(id));
            ProsumerID = Convert.ToInt32(id);



            return Json(cons);



        }

        [HttpGet]
        public async Task<JsonResult> ModificaProsumerImpianto(Dictionary<string, string> formData)
        {
            var id = formData["id"].ToUpper();
            if (id == "")
            {
                id = "-1";
            }
            var cons = new Dati_Impianto();


            cons = await _repo.Dati_ImpiantoRepository.GetByUtenzaId_Prosumer(Convert.ToInt32(id),1);
            ProsumerID = Convert.ToInt32(id);



            return Json(cons);



        }

        [HttpGet]
        public async Task<JsonResult> ModificaProsumerEconomici(Dictionary<string, string> formData)
        {
            var id = formData["id"].ToUpper();
            if (id == "")
            {
                id = "-1";
            }
            var cons = new Dati_Economici();


            cons = await _repo.Dati_EconomiciRepository.GetByUtenzaId_Prosumer(Convert.ToInt32(id), 1);
            ProsumerID = Convert.ToInt32(id);



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
                _prosumer.Prosumer_Id = ProsumerID;
                _repo.ProsumerRepository.Update(_prosumer);
                ProsumerID = -1;
            }
            else
            {
                _repo.ProsumerRepository.Add(_prosumer);
            }

            // Salva i cambiamenti nel database
            _repo.SaveChanges();


            return Json(_prosumer.Prosumer_Id); // o un'altra azione
        }
        [HttpPost]
        public async Task<IActionResult> SaveImpiantoProsumer(Dictionary<string, string> formData)
        {
            var prosumer_id = (int)Convert.ToInt32(formData["prosumer_id"]);
            var longitudine = formData["longitudine"];
            var latitudine = formData["latitudine"];
            var potenza_impianto = (double)Convert.ToDouble(formData["potenza_impianto"]);
            var checkquota = Convert.ToBoolean(formData["checkquota"] == "1");
            var potenza_rinnovabile = (double)Convert.ToDouble(formData["potenza_rinnovabile"]);
            var potenza_inverter = (double)Convert.ToDouble(formData["potenza_inverter"]);
            var capacita_batteria = (double)Convert.ToDouble(formData["capacita_batteria"]);
            var ismodProd = Convert.ToBoolean(formData["ismodProd"] == "1");
            var costokw = (double)Convert.ToDouble(formData["costokw"]);
            var costotot = (double)Convert.ToDouble(formData["costotot"]);
            var escluso_premio = Convert.ToInt32(formData["escluso_premio"]);
            var area_impianto = (double)Convert.ToDouble(formData["area_impianto"]);
            var tipo_impianto = (int)Convert.ToInt32(formData["tipo_impianto"]);
            var data_potenza_inverter = formData["data_potenza_inverter"];
            var is_secondafalda = Convert.ToBoolean(formData["is_secondafalda"] == "1");
            var potenza_sezione = (double)Convert.ToDouble(formData["potenza_sezione"]);
            var angolo_tilt = (double)Convert.ToDouble(formData["angolo_tilt"]);
            var angolo_azimut = (double)Convert.ToDouble(formData["angolo_azimut"]);
            var potenza_sezione_s = (double)Convert.ToDouble(formData["potenza_sezione_s"]);
            var angolo_tilt_s = (double)Convert.ToDouble(formData["angolo_tilt_s"]);
            var angolo_azimut_s = (double)Convert.ToDouble(formData["angolo_azimut_s"]);
            var efficienza_pannello = (double)Convert.ToDouble(formData["efficienza_pannello"]);
            var coefficiente_t = (double)Convert.ToDouble(formData["coefficiente_t"]);
            var prestazioni = (double)Convert.ToDouble(formData["prestazioni"]);
            var efficienza_inverter = (double)Convert.ToDouble(formData["efficienza_inverter"]);
            var costo_ric_batt = (double)Convert.ToDouble(formData["costo_ric_batt"]);
            var perdite = (double)Convert.ToDouble(formData["perdite"]);

            var _datiImpianto = new Dati_Impianto();
            _datiImpianto.Longitudine = longitudine;
            _datiImpianto.Latitudine = latitudine;
            _datiImpianto.Potenza_Impianto = potenza_impianto;
            _datiImpianto.Is_Abilitato_Rinnovabile = checkquota;
            _datiImpianto.Quota_Potenza_Rinnovabile = potenza_rinnovabile;
            _datiImpianto.Potenza_Inverter = potenza_inverter;
            _datiImpianto.Capacita_Batteria = capacita_batteria;
            _datiImpianto.Is_Costo_KW = ismodProd;
            _datiImpianto.Costo_KW = costokw;
            _datiImpianto.Costo_Totale = costotot;
            _datiImpianto.Is_Escluso_Premio = escluso_premio;
            _datiImpianto.Area_Impianto = area_impianto;
            _datiImpianto.Tipologia_Impianto = tipo_impianto;
            _datiImpianto.Data_Esercizio = data_potenza_inverter;
            _datiImpianto.Is_Seconda_Falda = is_secondafalda;
            _datiImpianto.Potenza_Sezione = potenza_sezione;
            _datiImpianto.Angolo_di_tilt = angolo_tilt;
            _datiImpianto.Angolo_di_Azimut = angolo_azimut;
            _datiImpianto.Potenza_Sezione_S = potenza_sezione_s;
            _datiImpianto.Angolo_di_tilt_S = angolo_tilt_s;
            _datiImpianto.Angolo_di_Azimut_S = angolo_azimut_s;
            _datiImpianto.Efficienza = efficienza_pannello;
            _datiImpianto.Coefficiente_T = coefficiente_t;
            _datiImpianto.NOCT = prestazioni;
            _datiImpianto.Efficienza_Inverter = efficienza_inverter;
            _datiImpianto.Costo_Ricambio_Batt = costo_ric_batt;
            _datiImpianto.Altre_Perdite = perdite;
            _datiImpianto.Is_Prosumer = 1; // Assume we are setting it to true
            _datiImpianto.Utenza_Id = prosumer_id;

            // Aggiorna o inserisci i dati nel database
            if (prosumer_id != -1)
            {
                 var dt  = await _repo.Dati_ImpiantoRepository.GetByUtenzaId_Prosumer(prosumer_id, 1);
                _datiImpianto.Dati_Impianto_Id = dt.Dati_Impianto_Id;
                _repo.Dati_ImpiantoRepository.Update(_datiImpianto);
            }
            else
            {
                _repo.Dati_ImpiantoRepository.Add(_datiImpianto);
            }

            // Salva i cambiamenti nel database
            _repo.SaveChanges();

            return Json(""); // o un'altra azione
        }


        public async Task<IActionResult> SaveEconomiciProsumer(Dictionary<string, string> formData)
        {
            var prosumer_id = (int)Convert.ToInt32(formData["prosumer_id"]);
            var isvendita = formData["isvendita"] == "1";
            var isacquisto = Convert.ToBoolean(formData["isacquisto"] == "1");
            var is_detrazione = Convert.ToBoolean(formData["is_detrazione"] == "1");
            var isfinanziamento = Convert.ToBoolean(formData["isfinanziamento"] == "1");
            var isconto = Convert.ToBoolean(formData["isconto"] == "1");
            var tipo_utente = (int)Convert.ToInt32(formData["tipo_utente"]);
            var prezzo_f1 = (double)Convert.ToDouble(formData["prezzo_f1"]);
            var prezzo_f2 = (double)Convert.ToDouble(formData["prezzo_f2"]);
            var prezzo_f3 = (double)Convert.ToDouble(formData["prezzo_f3"]);
            var costo_f1 = (double)Convert.ToDouble(formData["costo_f1"]);
            var costo_f2 = (double)Convert.ToDouble(formData["costo_f2"]);
            var costo_f3 = (double)Convert.ToDouble(formData["costo_f3"]);
            var altre_spese_inv = (double)Convert.ToDouble(formData["altre_spese_inv"]);
            var altre_spese = (double)Convert.ToDouble(formData["altre_spese"]);
            var spese_man = (double)Convert.ToDouble(formData["spese_man"]);
            var finanziamento = (double)Convert.ToDouble(formData["finanziamento"]);
            var interesse = (double)Convert.ToDouble(formData["interesse"]);
            var perce_finanziamento = (double)Convert.ToDouble(formData["perce_finanziamento"]);

            var _datiEconomici = new Dati_Economici();
            _datiEconomici.Tipo_Utenza_Id = tipo_utente;
            _datiEconomici.Modalita_Vendita = isvendita ? 1 : 0;  // Presupponendo che Modalita_Vendita sia un int
            _datiEconomici.Modalita_Acquisto = isacquisto ? 1 : 0;  // Presupponendo che Modalita_Acquisto sia un int
            _datiEconomici.Prezzo_F1 = prezzo_f1;
            _datiEconomici.Prezzo_F2 = prezzo_f2;
            _datiEconomici.Prezzo_F3 = prezzo_f3;
            _datiEconomici.Costo_F1 = costo_f1;
            _datiEconomici.Costo_F2 = costo_f2;
            _datiEconomici.Costo_F3 = costo_f3;
            _datiEconomici.Altre_Spese_Investimento = altre_spese_inv;
            _datiEconomici.Altre_Spese = altre_spese;
            _datiEconomici.Spese_Manutenzione = spese_man;
            _datiEconomici.Is_Detrazione = is_detrazione;
            _datiEconomici.Is_Finanziamento = isfinanziamento;            
            _datiEconomici.Importo_Finanziamento = finanziamento;
            _datiEconomici.Interesse_Finanziamento = interesse;
            _datiEconomici.Is_Conto = isconto;
            _datiEconomici.Conto_Capitale = perce_finanziamento;
            _datiEconomici.Is_Prosumer = 1; // Assume we are setting it to true
            _datiEconomici.Utenza_Id = prosumer_id;

            // Aggiorna o inserisci i dati nel database
            if (prosumer_id != -1)
            {
                var dt = await _repo.Dati_EconomiciRepository.GetByUtenzaId_Prosumer(prosumer_id, 1);
                _datiEconomici.Dati_Economici_Id = dt.Dati_Economici_Id;
                _repo.Dati_EconomiciRepository.Update(_datiEconomici);
            }
            else
            {
                _repo.Dati_EconomiciRepository.Add(_datiEconomici);
            }

            // Salva i cambiamenti nel database
            _repo.SaveChanges();

            return Json(""); // o un'altra azione
        }




        //IMPIANTO

        [HttpGet]
        public async Task<JsonResult> GetTableImpianto()
        {
            id_ = HttpContext.Session.GetInt32("Comunita_Id");
            var Comunita_Id = id_;
            var impiantoList = new List<Impianto>();

            if (Comunita_Id != null)
            {
                impiantoList = await _repo.ImpiantoRepository.GetByComunitaId((int)Comunita_Id);
            }

            var impiantoInfoList = new List<ConsumerInfo>();
            if (impiantoList != null)
            { 

                foreach (var item in impiantoList)
                {
                    var _impianto = await _repo.ImpiantoRepository.GetById((int)item.Impianto_Id);

                    if (_impianto != null)
                    {
                        var forma = "";
                        var modalita = "";
                        switch (_impianto.Forma_Utenza)
                        {
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
                        switch (_impianto.Modalita_Inserimento)
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

                        var id = _impianto.Tipo_Utenza_Id;

                        var tipo_utenza = new List<Tipo_Utenza>();


                        tipo_utenza = await _repo.Tipo_UtenzaRepository.GetListTipoUtenza_ById(Convert.ToInt32(id));


                        var consumerInfo = new ConsumerInfo
                        {
                            nome = _impianto.Descrizione,
                            formautenza = forma,
                            tipoutenza = tipo_utenza.First().Descrizione,
                            modalita = modalita,
                            nutenzecluster = _impianto.N_Utenze_Cluster.ToString(),
                            btn = "<button type=\"button\" class=\"btn btn-sm mr-2 btn-info\" data-toggle=\"modal\" onclick=\"ModiImpianto(" + _impianto.Impianto_Id.ToString() + ")\"><i class=\"fas fa-pencil-alt\"></i></button>" + "<button type=\"button\" id=\"sa-confirm\" onclick=\"_SetIdImpianto('" + _impianto.Impianto_Id.ToString() + "')\" class=\"btn btn-sm btn-danger btncancel btncancelImpianto\"><i class=\"fas fa-trash-alt\"></i></button>" // Questo deve essere specificato secondo necessità

                        };
                        impiantoInfoList.Add(consumerInfo);
                    }
                }
            //AddConsumer(Nome, Forma_Utenza, Tipo_Utenza, N_Utenze_Cluster, Modalita, Btn)
            }

            return Json(impiantoInfoList);



        }
        [HttpGet]
        public async Task<JsonResult> SetIdImpianto(Dictionary<string, string> formData)
        {
            var id = formData["id"].ToUpper();
            if (id == "")
            {
                id = "-1";
            }

            ImpiantoID = Convert.ToInt32(id);



            return Json("");



        }


        [HttpGet]
        public async Task<JsonResult> DeleteImpianto(Dictionary<string, string> formData)
        {
            var id = formData["id"].ToUpper();
            if (id == "")
            {
                id = "-1";
            }

            ImpiantoID = Convert.ToInt32(id);
            var cons = new Impianto();


            await _repo.ImpiantoRepository.Delete(Convert.ToInt32(ImpiantoID));

            _repo.SaveChanges();


            return Json("");



        }

        [HttpGet]
        public async Task<JsonResult> ModificaImpianto(Dictionary<string, string> formData)
        {
            var id = formData["id"].ToUpper();
            if (id == "")
            {
                id = "-1";
            }
            var cons = new Impianto();


            cons = await _repo.ImpiantoRepository.GetById(Convert.ToInt32(id));
            ImpiantoID = Convert.ToInt32(id);



            return Json(cons);

        }

        [HttpGet]
        public async Task<JsonResult> ModificaImpiantoImpianto(Dictionary<string, string> formData)
        {
            var id = formData["id"].ToUpper();
            if (id == "")
            {
                id = "-1";
            }
            var cons = new Dati_Impianto();


            cons = await _repo.Dati_ImpiantoRepository.GetByUtenzaId_Prosumer(Convert.ToInt32(id), 0);
            ImpiantoID = Convert.ToInt32(id);



            return Json(cons);



        }

        [HttpGet]
        public async Task<JsonResult> ModificaImpiantoEconomici(Dictionary<string, string> formData)
        {
            var id = formData["id"].ToUpper();
            if (id == "")
            {
                id = "-1";
            }
            var cons = new Dati_Economici();


            cons = await _repo.Dati_EconomiciRepository.GetByUtenzaId_Prosumer(Convert.ToInt32(id), 0);
            ImpiantoID = Convert.ToInt32(id);



            return Json(cons);



        }


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


            return Json(_impianto.Impianto_Id); // o un'altra azione
        }

        [HttpPost]
        public async Task<IActionResult> SaveImpianto_Impianto(Dictionary<string, string> formData)
        {
            var impianto_id = (int)Convert.ToInt32(formData["impianto_id"]);
            var longitudine = formData["longitudine"];
            var latitudine = formData["latitudine"];
            var potenza_impianto = (double)Convert.ToDouble(formData["potenza_impianto"]);
            var checkquota = Convert.ToBoolean(formData["checkquota"] == "1");
            var potenza_rinnovabile = (double)Convert.ToDouble(formData["potenza_rinnovabile"]);
            var potenza_inverter = (double)Convert.ToDouble(formData["potenza_inverter"]);
            var capacita_batteria = (double)Convert.ToDouble(formData["capacita_batteria"]);
            var ismodProd = Convert.ToBoolean(formData["ismodProd"] == "1");
            var costokw = (double)Convert.ToDouble(formData["costokw"]);
            var costotot = (double)Convert.ToDouble(formData["costotot"]);
            var escluso_premio = Convert.ToInt32(formData["escluso_premio"]);
            var area_impianto = (double)Convert.ToDouble(formData["area_impianto"]);
            var tipo_impianto = (int)Convert.ToInt32(formData["tipo_impianto"]);
            var data_potenza_inverter = formData["data_potenza_inverter"];
            var is_secondafalda = Convert.ToBoolean(formData["is_secondafalda"] == "1");
            var potenza_sezione = (double)Convert.ToDouble(formData["potenza_sezione"]);
            var angolo_tilt = (double)Convert.ToDouble(formData["angolo_tilt"]);
            var angolo_azimut = (double)Convert.ToDouble(formData["angolo_azimut"]);
            var potenza_sezione_s = (double)Convert.ToDouble(formData["potenza_sezione_s"]);
            var angolo_tilt_s = (double)Convert.ToDouble(formData["angolo_tilt_s"]);
            var angolo_azimut_s = (double)Convert.ToDouble(formData["angolo_azimut_s"]);
            var efficienza_pannello = (double)Convert.ToDouble(formData["efficienza_pannello"]);
            var coefficiente_t = (double)Convert.ToDouble(formData["coefficiente_t"]);
            var prestazioni = (double)Convert.ToDouble(formData["prestazioni"]);
            var efficienza_inverter = (double)Convert.ToDouble(formData["efficienza_inverter"]);
            var costo_ric_batt = (double)Convert.ToDouble(formData["costo_ric_batt"]);
            var perdite = (double)Convert.ToDouble(formData["perdite"]);

            var _datiImpianto = new Dati_Impianto();
            _datiImpianto.Longitudine = longitudine;
            _datiImpianto.Latitudine = latitudine;
            _datiImpianto.Potenza_Impianto = potenza_impianto;
            _datiImpianto.Is_Abilitato_Rinnovabile = checkquota;
            _datiImpianto.Quota_Potenza_Rinnovabile = potenza_rinnovabile;
            _datiImpianto.Potenza_Inverter = potenza_inverter;
            _datiImpianto.Capacita_Batteria = capacita_batteria;
            _datiImpianto.Is_Costo_KW = ismodProd;
            _datiImpianto.Costo_KW = costokw;
            _datiImpianto.Costo_Totale = costotot;
            _datiImpianto.Is_Escluso_Premio = escluso_premio;
            _datiImpianto.Area_Impianto = area_impianto;
            _datiImpianto.Tipologia_Impianto = tipo_impianto;
            _datiImpianto.Data_Esercizio = data_potenza_inverter;
            _datiImpianto.Is_Seconda_Falda = is_secondafalda;
            _datiImpianto.Potenza_Sezione = potenza_sezione;
            _datiImpianto.Angolo_di_tilt = angolo_tilt;
            _datiImpianto.Angolo_di_Azimut = angolo_azimut;
            _datiImpianto.Potenza_Sezione_S = potenza_sezione_s;
            _datiImpianto.Angolo_di_tilt_S = angolo_tilt_s;
            _datiImpianto.Angolo_di_Azimut_S = angolo_azimut_s;
            _datiImpianto.Efficienza = efficienza_pannello;
            _datiImpianto.Coefficiente_T = coefficiente_t;
            _datiImpianto.NOCT = prestazioni;
            _datiImpianto.Efficienza_Inverter = efficienza_inverter;
            _datiImpianto.Costo_Ricambio_Batt = costo_ric_batt;
            _datiImpianto.Altre_Perdite = perdite;
            _datiImpianto.Is_Prosumer = 0; // Assume we are setting it to true
            _datiImpianto.Utenza_Id = impianto_id;

            // Aggiorna o inserisci i dati nel database
            if (impianto_id != -1)
            {
                var dt = await _repo.Dati_ImpiantoRepository.GetByUtenzaId_Prosumer(impianto_id, 0);
                _datiImpianto.Dati_Impianto_Id = dt.Dati_Impianto_Id;
                _prosumer.Prosumer_Id = impianto_id;
                _repo.Dati_ImpiantoRepository.Update(_datiImpianto);
            }
            else
            {
                _repo.Dati_ImpiantoRepository.Add(_datiImpianto);
            }

            // Salva i cambiamenti nel database
            _repo.SaveChanges();

            return Json(""); // o un'altra azione
        }

        public async Task<IActionResult> SaveEconomiciImpianto(Dictionary<string, string> formData)
        {
            var impianto_id = (int)Convert.ToInt32(formData["impianto_id"]);
            var isvendita = Convert.ToBoolean(formData["isvendita"] == "1");
            var isacquisto = Convert.ToBoolean(formData["isacquisto"] == "1");
            var is_detrazione = Convert.ToBoolean(formData["is_detrazione"] == "1");
            var isfinanziamento = Convert.ToBoolean(formData["isfinanziamento"] == "1");
            var isconto = Convert.ToBoolean(formData["isconto"] == "1");
            var tipo_utente = (int)Convert.ToInt32(formData["tipo_utente"]);
            var prezzo_f1 = (double)Convert.ToDouble(formData["prezzo_f1"]);
            var prezzo_f2 = (double)Convert.ToDouble(formData["prezzo_f2"]);
            var prezzo_f3 = (double)Convert.ToDouble(formData["prezzo_f3"]);
            var costo_f1 = (double)Convert.ToDouble(formData["costo_f1"]);
            var costo_f2 = (double)Convert.ToDouble(formData["costo_f2"]);
            var costo_f3 = (double)Convert.ToDouble(formData["costo_f3"]);
            var altre_spese_inv = (double)Convert.ToDouble(formData["altre_spese_inv"]);
            var altre_spese = (double)Convert.ToDouble(formData["altre_spese"]);
            var spese_man = (double)Convert.ToDouble(formData["spese_man"]);
            var finanziamento = (double)Convert.ToDouble(formData["finanziamento"]);
            var interesse = (double)Convert.ToDouble(formData["interesse"]);
            var perce_finanziamento = (double)Convert.ToDouble(formData["perce_finanziamento"]);

            var _datiEconomici = new Dati_Economici();
            _datiEconomici.Tipo_Utenza_Id = tipo_utente;
            _datiEconomici.Modalita_Vendita = isvendita ? 1 : 0;  // Presupponendo che Modalita_Vendita sia un int
            _datiEconomici.Modalita_Acquisto = isacquisto ? 1 : 0;  // Presupponendo che Modalita_Acquisto sia un int
            _datiEconomici.Prezzo_F1 = prezzo_f1;
            _datiEconomici.Prezzo_F2 = prezzo_f2;
            _datiEconomici.Prezzo_F3 = prezzo_f3;
            _datiEconomici.Costo_F1 = costo_f1;
            _datiEconomici.Costo_F2 = costo_f2;
            _datiEconomici.Costo_F3 = costo_f3;
            _datiEconomici.Altre_Spese_Investimento = altre_spese_inv;
            _datiEconomici.Altre_Spese = altre_spese;
            _datiEconomici.Spese_Manutenzione = spese_man;
            _datiEconomici.Is_Detrazione = is_detrazione;
            _datiEconomici.Is_Finanziamento = isfinanziamento;
            _datiEconomici.Importo_Finanziamento = finanziamento;
            _datiEconomici.Interesse_Finanziamento = interesse;
            _datiEconomici.Is_Conto = isconto;
            _datiEconomici.Conto_Capitale = perce_finanziamento;
            _datiEconomici.Is_Prosumer = 0; // Assume we are setting it to true
            _datiEconomici.Utenza_Id = impianto_id;

            // Aggiorna o inserisci i dati nel database
            if (impianto_id != -1)
            {
                var dt = await _repo.Dati_EconomiciRepository.GetByUtenzaId_Prosumer(impianto_id, 0);
                _datiEconomici.Dati_Economici_Id = dt.Dati_Economici_Id;
                _repo.Dati_EconomiciRepository.Update(_datiEconomici);
            }
            else
            {
                _repo.Dati_EconomiciRepository.Add(_datiEconomici);
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
