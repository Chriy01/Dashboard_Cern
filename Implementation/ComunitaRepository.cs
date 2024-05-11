using Dashboard.BusinessLayer;
using Dashboard.Database;
using Dashboard.Interfaces;

namespace Dashboard.Implementation
{
    public class ComunitaRepository : IComunitaRepository
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

        public void Add(Comunita comunita)
        {
            _dbContext.Comunita.Add(comunita);
            _dbContext.SaveChanges();
        }

        public void Update(Comunita comunita)
        {
            _dbContext.Comunita.Update(comunita);
            _dbContext.SaveChanges();
        }

        public void Delete(int comunitaId)
        {
            var comunita = GetById(comunitaId);
            if (comunita != null)
            {
                _dbContext.Comunita.Remove(comunita);
                _dbContext.SaveChanges();
            }
        }
    }
}
