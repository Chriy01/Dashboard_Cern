using Dashboard.BusinessLayer;
using Dashboard.Database;
using Dashboard.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Implementation
{
    public class Dati_EconomiciRepository : BaseRepository, IBaseRepositable<Dati_Economici>
    {
        private readonly AppDbContext _dbContext;

        public Dati_EconomiciRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Dati_Economici> GetById(int id)
        {
            try
            {
                var con = _dbContext.Dati_Economici.Where(c => c.Dati_Economici_Id == id && c != null).SingleOrDefault();
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
        public async Task<Dati_Economici> GetByUtenzaId_Prosumer(int id,int is_prosumer )
        {
            try
            {
                var con = _dbContext.Dati_Economici.Where(de => de.Utenza_Id == id && de.Is_Prosumer == 1).SingleOrDefault();
                
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

        public void Add(Dati_Economici entity)
        {
            _dbContext.Dati_Economici.Add(entity);
            _dbContext.SaveChanges();
        }

        public bool Update(Dati_Economici entity)
        {
            _dbContext.Dati_Economici.Update(entity);
            _dbContext.SaveChanges();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var Dati_Economici = await GetById(id);
            if (Dati_Economici != null)
            {
                _dbContext.Dati_Economici.Remove(Dati_Economici);
                _dbContext.SaveChanges();
            }
            return true;
        }
    }
}
