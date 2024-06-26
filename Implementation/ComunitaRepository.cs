using Dashboard.BusinessLayer;
using Dashboard.Database;
using Dashboard.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Implementation
{
    public class ComunitaRepository : BaseRepository, IBaseRepositable<Comunita>
    {
        private readonly AppDbContext _dbContext;

        public ComunitaRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Comunita> GetById(int id)
        {
            try
            {
                var com = _dbContext.Comunita.Where(c => c.Comunita_Id == id && c != null).SingleOrDefault();
                return com;
                // Operazioni aggiuntive...
            }
            catch (Exception ex)
            {
                // Registra l'errore o gestiscilo di conseguenza
                Console.WriteLine($"Si è verificato un errore durante l'esecuzione della query: {ex.Message}");
                return null;
            }

        }

        public void Add(Comunita entity)
        {
            _dbContext.Comunita.Add(entity);
            _dbContext.SaveChanges();
        }

        public bool Update(Comunita entity)
        {
            _dbContext.Comunita.Update(entity);
            _dbContext.SaveChanges();
            return true;
        }

         async Task<bool> IBaseRepositable<Comunita>.Delete(int id)
        {
            var comunita = await GetById(id);
            if (comunita != null)
            {
                _dbContext.Comunita.Remove(comunita);
                _dbContext.SaveChanges();
            }
            return true;
        }
    }
}
