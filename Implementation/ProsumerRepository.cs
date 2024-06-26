using Dashboard.BusinessLayer;
using Dashboard.Database;
using Dashboard.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Implementation
{
    public class ProsumerRepository : BaseRepository, IBaseRepositable<Prosumer>
    {
        private readonly AppDbContext _dbContext;

        public ProsumerRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Prosumer> GetById(int id)
        {
            try
            {
                var con = _dbContext.Prosumer.Where(c => c.Prosumer_Id == id && c != null).SingleOrDefault();
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
        public async Task<List<Prosumer>> GetByComunitaId(int id)
        {
            try
            {
                var con = _dbContext.Prosumer.Where(c => c.Comunita_Id == id && c != null).ToListAsync();
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

        public void Add(Prosumer entity)
        {
            _dbContext.Prosumer.Add(entity);
            _dbContext.SaveChanges();
        }

        public bool Update(Prosumer entity)
        {
            _dbContext.Prosumer.Update(entity);
            _dbContext.SaveChanges();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var Prosumer = await GetById(id);
            if (Prosumer != null)
            {
                _dbContext.Prosumer.Remove(Prosumer);
                _dbContext.SaveChanges();
            }
            return true;
        }
    }
}
