using Dashboard.BusinessLayer;
using Dashboard.Database;
using Dashboard.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Implementation
{
    public class ParametroRepository : IBaseRepositable<Parametro>
    {
        private readonly AppDbContext _dbContext;

        public ParametroRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Parametro entity)
        {
            _dbContext.Parametro.Add(entity);
            _dbContext.SaveChanges();
        }

        public async Task<bool> Delete(int id)
        {
            var parametro = await GetById(id);
            if (parametro != null)
            {
                //_dbContext.Parametro.Remove());
                _dbContext.SaveChanges();
            }
            return true;
        }

        public bool Update(Parametro entity)
        {
            _dbContext.Parametro.Update(entity);
            _dbContext.SaveChanges();
            return true;
        }

        public async Task<Parametro> GetById(int id)
        {
            return _dbContext.Parametro.Find(id);
        }
    }
}
