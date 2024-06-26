using Dashboard.BusinessLayer;
using Dashboard.Database;
using Dashboard.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Implementation
{
    public class ImpiantoRepository : BaseRepository, IBaseRepositable<Impianto>
    {
        private readonly AppDbContext _dbContext;

        public ImpiantoRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Impianto> GetById(int id)
        {
            try
            {
                var con = _dbContext.Impianto.Where(c => c.Impianto_Id == id && c != null).SingleOrDefault();
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
        public async Task<List<Impianto>> GetByComunitaId(int id)
        {
            try
            {
                var con = _dbContext.Impianto.Where(c => c.Comunita_Id == id && c != null).ToListAsync();
                return await con;
                // Operazioni aggiuntive...
            }
            catch (Exception ex)
            {
                // Registra l'errore o gestiscilo di conseguenza
                Console.WriteLine($"Si è verificato un errore durante l'esecuzione della query: {ex.Message}");
                return null;
            }

        }

        public void Add(Impianto entity)
        {
            _dbContext.Impianto.Add(entity);
            _dbContext.SaveChanges();
        }

        public bool Update(Impianto entity)
        {
            _dbContext.Impianto.Update(entity);
            _dbContext.SaveChanges();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var Impianto = await GetById(id);
            if (Impianto != null)
            {
                _dbContext.Impianto.Remove(Impianto);
                _dbContext.SaveChanges();
            }
            return true;
        }
    }
}
