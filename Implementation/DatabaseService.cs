using Dashboard.Database;
using Dashboard.Interfaces;

namespace Dashboard.Implementation
{
    public class DatabaseService : IDatabaseService
    {
        private readonly AppDbContext _dbContext;
        private readonly IParametroRepository _parametroRepository;
        private readonly ITipologiaParametroRepository _tipologiaParametroRepository;
        private readonly IComunitaRepository _comunitaRepository;

        public DatabaseService(AppDbContext dbContext, IParametroRepository parametroRepository, ITipologiaParametroRepository tipologiaParametroRepository, IComunitaRepository comunitaRepository)
        {
            _dbContext = dbContext;
            _parametroRepository = parametroRepository;
            _tipologiaParametroRepository = tipologiaParametroRepository;
            _comunitaRepository = comunitaRepository;
        }

        public IParametroRepository ParametroRepository => _parametroRepository;

        public ITipologiaParametroRepository TipologiaParametroRepository => _tipologiaParametroRepository;

        public IComunitaRepository ComunitaRepository => _comunitaRepository;

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
