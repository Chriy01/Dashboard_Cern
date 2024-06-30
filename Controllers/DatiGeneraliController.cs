using Microsoft.AspNetCore.Mvc;
using Dashboard.Utilities;
using Dashboard.BusinessLayer;
using Dashboard.Database;
using Dashboard.Implementation;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Controllers
{
    public class DatiGeneraliController : Controller
    {
        private readonly ILogger<DatiGeneraliController> _logger;
        private readonly AppDbContext _dbContext;
        private DatabaseService _repo;
        private Comunita _comunita;
        private Utente_Comunita _utente_comunita;
        private Int32 ComunitaID;

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

        public DatiGeneraliController(ILogger<DatiGeneraliController> logger, AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
            _repo = new DatabaseService(dbContext);

        }

        [HttpGet]
        public async Task<JsonResult> ModificaComunita()
        {
            if(HttpContext.Session.GetInt32("Comunita_Id") != null)
            {
                _comunita = HttpContext.Session.GetInt32("Comunita_Id") != null ?
                  await _repo.ComunitaRepository.GetById((int)HttpContext.Session.GetInt32("Comunita_Id")) :
                  new Comunita();

       
                return Json(_comunita);

            }
            else
            {
                return Json(null);
            }

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

        [HttpPost]
        public async Task<IActionResult> SaveDatiGenerali(Dictionary<string, string> formData)
        {
            if (HttpContext.Session.GetInt32("Comunita_Id") == -1)
            {
                _comunita = new Comunita();
            }
            else {
                _comunita = HttpContext.Session.GetInt32("Comunita_Id") != null ?
                     await _repo.ComunitaRepository.GetById((int)HttpContext.Session.GetInt32("Comunita_Id")) :
                     new Comunita();
            }


            var nome_comunita = formData["Nome_Comunita"];
            var zona = formData["ZonaMercato"];
            var tipo_conf = formData["Tipo_Conf"];
            var tasso_infl = (double)Convert.ToDouble(formData["Inflazione"]);
            var tasso_int = (double)Convert.ToDouble(formData["Interesse"]);
            var zona_geo = formData["ZonaGeo"];
            var anno = Convert.ToInt32(formData["Anno"]);
            var prezzo_annuale = (double)Convert.ToDouble(formData["prezzo_annuale"]);
            var prezzo_f1 = (double)Convert.ToDouble(formData["prezzo_f1"]);
            var prezzo_f2 = (double)Convert.ToDouble(formData["prezzo_f2"]);
            var prezzo_f3 = (double)Convert.ToDouble(formData["prezzo_f3"]);
            var prezzo_gennaio = string.IsNullOrEmpty(formData["gennaio"]) ? (double?)null : Convert.ToSingle(formData["gennaio"]);
            var prezzo_febbraio = string.IsNullOrEmpty(formData["Febbraio"]) ? (double?)null : Convert.ToSingle(formData["Febbraio"]);
            var prezzo_marzo = string.IsNullOrEmpty(formData["Marzo"]) ? (double?)null : Convert.ToSingle(formData["Marzo"]);
            var prezzo_aprile = string.IsNullOrEmpty(formData["Aprile"]) ? (double?)null : Convert.ToSingle(formData["Aprile"]);
            var prezzo_maggio = string.IsNullOrEmpty(formData["Maggio"]) ? (double?)null : Convert.ToSingle(formData["Maggio"]);
            var prezzo_giugno = string.IsNullOrEmpty(formData["Giugno"]) ? (double?)null : Convert.ToSingle(formData["Giugno"]);
            var prezzo_luglio = string.IsNullOrEmpty(formData["Luglio"]) ? (double?)null : Convert.ToSingle(formData["Luglio"]);
            var prezzo_agosto = string.IsNullOrEmpty(formData["Agosto"]) ? (double?)null : Convert.ToSingle(formData["Agosto"]);
            var prezzo_settembre = string.IsNullOrEmpty(formData["Settembre"]) ? (double?)null : Convert.ToSingle(formData["Settembre"]);
            var prezzo_ottobre = string.IsNullOrEmpty(formData["Ottobre"]) ? (double?)null : Convert.ToSingle(formData["Ottobre"]);
            var prezzo_novembre = string.IsNullOrEmpty(formData["Novembre"]) ? (double?)null : Convert.ToSingle(formData["Novembre"]);
            var prezzo_dicembre = string.IsNullOrEmpty(formData["Dicembre"]) ? (double?)null : Convert.ToSingle(formData["Dicembre"]);


            // Aggiorna i dati della comunità con i nuovi valori
            _comunita.Nome = nome_comunita;
            _comunita.Zona_di_mercato = zona;
            _comunita.iscomunita = tipo_conf == "1";
            _comunita.tasso_inflazione_mercato = tasso_infl;
            _comunita.tasso_interesse_mercato = tasso_int;
            _comunita.Anno_di_riferimento = anno;
            _comunita.Prezzo_annuale = prezzo_annuale;
            _comunita.Prezzo_Gennaio = prezzo_gennaio;
            _comunita.Prezzo_Febbraio = prezzo_febbraio;
            _comunita.Prezzo_Aprile = prezzo_aprile;
            _comunita.Prezzo_Marzo = prezzo_marzo;
            _comunita.Prezzo_Maggio = prezzo_maggio;
            _comunita.Prezzo_Giugno = prezzo_giugno;
            _comunita.Prezzo_Luglio = prezzo_luglio;
            _comunita.Prezzo_Agosto = prezzo_agosto;
            _comunita.Prezzo_Settembre = prezzo_settembre;
            _comunita.Prezzo_Ottobre = prezzo_ottobre;
            _comunita.Prezzo_Novembre = prezzo_novembre;
            _comunita.Prezzo_Dicembre = prezzo_dicembre;
            _comunita.zona_geografica = zona_geo;

            // Aggiorna o inserisci i dati nel database
            if (_comunita.Comunita_Id != 0)
            {
                _repo.ComunitaRepository.Update(_comunita);
            }
            else
            {
                _repo.ComunitaRepository.Add(_comunita);
                var comunita_utente = new Utente_Comunita();
                comunita_utente.Utente_Id = (int)HttpContext.Session.GetInt32("UserID");
                comunita_utente.Comunita_Id = _comunita.Comunita_Id;
                _repo.Utente_ComunitaRepository.Add(comunita_utente);
            }

            // Salva i cambiamenti nel database
            _repo.SaveChanges();

            // Restituisci una risposta JSON o reindirizza a un'altra azione
            return Json(""); // o un'altra azione
        }

    }
}
