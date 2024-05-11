using Dashboard.BusinessLayer;
using Dashboard.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Implementation
{
    public class ParametroRepository : IParametroRepository
    {
        private readonly DbContext _dbContext;

        public ParametroRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(Parametro parametro)
        {
            throw new NotImplementedException();
        }

        public void Delete(int parametroId)
        {
            throw new NotImplementedException();
        }

        public Parametro GetById(int parametroId)
        {
            throw new NotImplementedException();
        }

        public void Update(Parametro parametro)
        {
            throw new NotImplementedException();
        }
    }
}
