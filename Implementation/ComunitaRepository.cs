using Dashboard.BusinessLayer;
using Dashboard.Database;
using Dashboard.Interfaces;

namespace Dashboard.Implementation
{
    public class ComunitaRepository : IBaseRepositable<Comunita>
    {
        private readonly AppDbContext _dbContext;

        public ComunitaRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Comunita GetById(int comunitaId)
        {
            return _dbContext.Comunita.Find(comunitaId);
        }
        Comunita IBaseRepositable<Comunita>.GetById(int id)
        {
            return _dbContext.Comunita.Find(id);
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

        bool IBaseRepositable<Comunita>.Delete(int id)
        {
            var comunita = GetById(id);
            if (comunita != null)
            {
                _dbContext.Comunita.Remove(comunita);
                _dbContext.SaveChanges();
            }
            return true;
        }
    }
}
