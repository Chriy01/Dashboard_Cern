using Dashboard.BusinessLayer;
using Dashboard.Database;
using Dashboard.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Implementation
{
    public class TipologiaParametroRepository : IBaseRepositable<TipologiaParametro>
    {
        private readonly AppDbContext _dbContext;

        public TipologiaParametroRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TipologiaParametro> GetById(int tipologiaId)
        {
            return _dbContext.TipologiaParametro.Find(tipologiaId);
        }

        public void Add(TipologiaParametro tipologiaParametro)
        {
            _dbContext.TipologiaParametro.Add(tipologiaParametro);
            _dbContext.SaveChanges();
        }

        public bool Update(TipologiaParametro entity)
        {
            _dbContext.TipologiaParametro.Update(entity);
            _dbContext.SaveChanges();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var tipologia = GetById(id);
            if (tipologia != null)
            {
                _dbContext.TipologiaParametro.ExecuteDelete();
                _dbContext.SaveChanges();
            }
            return true;
        }

    }

}
