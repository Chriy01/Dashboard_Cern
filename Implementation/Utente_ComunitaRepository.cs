using Dashboard.BusinessLayer;
using Dashboard.Database;
using Dashboard.Interfaces;
using Microsoft.EntityFrameworkCore;
using CryptSharp;
using NuGet.Versioning;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.AspNetCore.Identity;

namespace Dashboard.Implementation
{
    public class Utente_ComunitaRepository : BaseRepository,IBaseRepositable<Utente>
    {
        private readonly AppDbContext _dbContext;

        public Utente_ComunitaRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Utente entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Utente> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Utente_Comunita>> GetUtente_ComunitaByIdUtente(int Utente_Id)
        {
            try
            {                              
                return await _dbContext.Utente_Comunita.Where(uc => uc.Utente_Id == Utente_Id).ToListAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante l'esecuzione della query: {ex.Message}");
                return null;
            }

        }

        public bool Update(Utente entity)
        {
            throw new NotImplementedException();
        }
    }
}
