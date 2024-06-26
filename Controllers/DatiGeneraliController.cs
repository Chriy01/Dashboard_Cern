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

        [HttpPost]
        public async Task<IActionResult> SaveDatiGenerali(Dictionary<string, string> formData)
        {
            _comunita = HttpContext.Session.GetInt32("Comunita_Id") != null ?
                 await _repo.ComunitaRepository.GetById((int)HttpContext.Session.GetInt32("Comunita_Id")) :
                 new Comunita();



            var nome_comunita = formData["Nome_Comunita"];
            var zona = formData["ZonaMercato"];
            var tipo_conf = formData["Tipo_Conf"];
            var tasso_infl = (float)Convert.ToDouble(formData["Inflazione"]);
            var tasso_int = (float)Convert.ToDouble(formData["Interesse"]);
            var zona_geo = formData["ZonaGeo"];
            var anno = Convert.ToInt32(formData["Anno"]);

            // Aggiorna i dati della comunità con i nuovi valori
            _comunita.Nome = nome_comunita;
            _comunita.Zona_di_mercato = zona;
            _comunita.iscomunita = tipo_conf == "1";
            _comunita.tasso_inflazione_mercato = tasso_infl;
            _comunita.tasso_interesse_mercato = tasso_int;
            _comunita.Zona_di_mercato = zona_geo;
            _comunita.Anno_di_riferimento = anno;

            // Aggiorna o inserisci i dati nel database
            if (_comunita.Comunita_Id != 0)
            {
                _repo.ComunitaRepository.Update(_comunita);
            }
            else
            {
                _repo.ComunitaRepository.Add(_comunita);
            }

            // Salva i cambiamenti nel database
            _repo.SaveChanges();

            // Restituisci una risposta JSON o reindirizza a un'altra azione
            return Json(""); // o un'altra azione
        }

    }
}
