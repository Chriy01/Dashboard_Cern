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
    public class Utente_ComunitaRepository : BaseRepository,IBaseRepositable<Utente_Comunita>
    {
        private readonly AppDbContext _dbContext;

        public Utente_ComunitaRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Utente_Comunita entity)
        {
            _dbContext.Utente_Comunita.Add(entity);
            _dbContext.SaveChanges();
        }

        public async Task<bool> Delete(int id)
        {
            var comunita = await GetById(id);
            if (comunita != null)
            {
                _dbContext.Utente_Comunita.Remove(comunita);
                _dbContext.SaveChanges();
            }
            return true;
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


        public bool Update(Utente_Comunita entity)
        {
            _dbContext.Utente_Comunita.Update(entity);
            _dbContext.SaveChanges();
            return true;
        }

        public async Task<Utente_Comunita> GetById(int id)
        {
            try
            {
                var con = _dbContext.Utente_Comunita.Where(uc => uc.Utente_Comunita_Id == id && uc != null).SingleOrDefault();
                return con;
                // Operazioni aggiuntive...
            }
            catch (Exception ex)
            {
                // Registra l'errore o gestiscilo di conseguenza
                Console.WriteLine($"Si è verificato un errore durante l'esecuzione della query: {ex.Message}");
                return null;
            }

        }
    }
}
