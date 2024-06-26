using Dashboard.BusinessLayer;
using Dashboard.Database;
using Dashboard.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Implementation
{
    public class ConsumerRepository : BaseRepository, IBaseRepositable<Consumer>
    {
        private readonly AppDbContext _dbContext;

        public ConsumerRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Consumer> GetById(int id)
        {
            try
            {
                var con = _dbContext.Consumer.Where(c => c.Consumer_Id == id && c != null).SingleOrDefault();
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
        public async Task<List<Consumer>> GetByComunitaId(int id)
        {
            try
            {
                var con = _dbContext.Consumer.Where(c => c.Comunita_Id == id && c != null).ToListAsync();
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

        public void Add(Consumer entity)
        {
            _dbContext.Consumer.Add(entity);
            _dbContext.SaveChanges();
        }

        public bool Update(Consumer entity)
        {
            _dbContext.Consumer.Update(entity);
            _dbContext.SaveChanges();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var consumer = await GetById(id);
            if (consumer != null)
            {
                _dbContext.Consumer.Remove(consumer);
                _dbContext.SaveChanges();
            }
            return true;
        }
    }
}
