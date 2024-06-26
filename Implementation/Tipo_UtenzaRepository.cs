using Dashboard.BusinessLayer;
using Dashboard.Database;
using Dashboard.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Implementation
{

    public class Tipo_UtenzaRepository : BaseRepository, IBaseRepositable<Tipo_Utenza>
    {
        private readonly AppDbContext _dbContext;

        public Tipo_UtenzaRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Tipo_Utenza> GetById(int id)
        {
            try
            {
                var com = _dbContext.Tipo_Utenza.Where(tu => tu.Tipo_Utenza_Id == id && tu != null).SingleOrDefault();
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

        public void Add(Tipo_Utenza entity)
        {
            _dbContext.Tipo_Utenza.Add(entity);
            _dbContext.SaveChanges();
        }

        public bool Update(Tipo_Utenza entity)
        {
            _dbContext.Tipo_Utenza.Update(entity);
            _dbContext.SaveChanges();
            return true;
        }

        async Task<bool> IBaseRepositable<Tipo_Utenza>.Delete(int id)
        {
            var tipo_utenza = await GetById(id);
            if (tipo_utenza != null)
            {
                _dbContext.Tipo_Utenza.Remove(tipo_utenza);
                _dbContext.SaveChanges();
            }
            return true;
        }

        public async Task<List<Tipo_Utenza>> GetListTipoUtenza_ById(int Id)
        {
            try
            {
                return await _dbContext.Tipo_Utenza.Where(tu => tu.Tipo_Utenza_Id == Id || Id == -1).ToListAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante l'esecuzione della query: {ex.Message}");
                return null;
            }

        }
    }
    
}
