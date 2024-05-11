using Dashboard.BusinessLayer;
using Dashboard.Database;
using Dashboard.Interfaces;

namespace Dashboard.Implementation
{
    public class TipologiaParametroRepository : ITipologiaParametroRepository
    {
        private readonly AppDbContext _dbContext;

        public TipologiaParametroRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TipologiaParametro GetById(int tipologiaId)
        {
            return _dbContext.TipologiaParametro.Find(tipologiaId);
        }

        public void Add(TipologiaParametro tipologiaParametro)
        {
            _dbContext.TipologiaParametro.Add(tipologiaParametro);
            _dbContext.SaveChanges();
        }

        public void Update(TipologiaParametro tipologiaParametro)
        {
            _dbContext.TipologiaParametro.Update(tipologiaParametro);
            _dbContext.SaveChanges();
        }

        public void Delete(int tipologiaId)
        {
            var tipologia = GetById(tipologiaId);
            if (tipologia != null)
            {
                _dbContext.TipologiaParametro.Remove(tipologia);
                _dbContext.SaveChanges();
            }
        }
    }

}
