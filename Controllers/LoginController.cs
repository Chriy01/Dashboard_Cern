using Dashboard.Database;
using Dashboard.Implementation;
using Dashboard.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CryptSharp;
namespace Dashboard.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly AppDbContext _dbContext;
        private DatabaseService _repo;
        public IActionResult Index()
        {
            var s = Crypter.MD5.Crypt("!demo_2024");
            return View("Login");
        }
        public LoginController(ILogger<LoginController> logger, AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
            _repo = new DatabaseService(dbContext);

        }



        [HttpPost]
        public async Task<IActionResult> checkLogin(Dictionary<string, string> formData)
        {
            var username = formData["username"].ToUpper();
            var password = formData["password"];
            
            var r = await _repo.UtenteRepository.GetByUsernamePassword(username, password);
            
            if ((r != null) &&(r > -1))
            {
                HttpContext.Session.SetInt32("UserID", r);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                HttpContext.Session.SetInt32("UserID", -1);
                return Json("Errore");
            }


             // o un'altra azione
        }
    }
}
