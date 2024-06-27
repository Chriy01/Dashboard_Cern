using Dashboard.BusinessLayer;
using Dashboard.Database;
using Dashboard.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Implementation
{
    public class Dati_ImpiantoRepository : BaseRepository, IBaseRepositable<Dati_Impianto>
    {
        private readonly AppDbContext _dbContext;

        public Dati_ImpiantoRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Dati_Impianto> GetById(int id)
        {
            try
            {
                var con = _dbContext.Dati_Impianto.Where(c => c.Dati_Impianto_Id == id && c != null).SingleOrDefault();
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
        public async Task<Dati_Impianto> GetByUtenzaId_Prosumer(int id, int is_prosumer)
        {
            try
            {
                var con = _dbContext.Dati_Impianto.Where(de => de.Utenza_Id == id && de.Is_Prosumer == is_prosumer).SingleOrDefault();

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

        public void Add(Dati_Impianto entity)
        {
            _dbContext.Dati_Impianto.Add(entity);
            _dbContext.SaveChanges();
        }

        public bool Update(Dati_Impianto entity)
        {
            _dbContext.Dati_Impianto.Update(entity);
            _dbContext.SaveChanges();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var Dati_Impianto = await GetById(id);
            if (Dati_Impianto != null)
            {
                _dbContext.Dati_Impianto.Remove(Dati_Impianto);
                _dbContext.SaveChanges();
            }
            return true;
        }
    }
}
