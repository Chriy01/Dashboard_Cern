using Dashboard.BusinessLayer;
using Dashboard.Database;
using Dashboard.Interfaces;

namespace Dashboard.Implementation
{
    public class DatabaseService : IDatabaseService
    {
        private readonly AppDbContext _dbContext;
        private ParametroRepository _parametroRepository;
        private TipologiaParametroRepository _tipologiaParametroRepository;
        private ComunitaRepository _comunitaRepository;
        private UtenteRepository _utenteRepository;
        public DatabaseService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ParametroRepository ParametroRepository
        {
            get
            {
                if (_parametroRepository == null)
                    _parametroRepository = new ParametroRepository(_dbContext);
                return _parametroRepository;
            }
        }

        public UtenteRepository UtenteRepository
        {
            get
            {
                if (_utenteRepository == null)
                    _utenteRepository = new UtenteRepository(_dbContext);
                return _utenteRepository;
            }
        }

        public TipologiaParametroRepository TipologiaParametroRepository
        {
            get
            {
                if (_tipologiaParametroRepository == null)
                    _tipologiaParametroRepository = new TipologiaParametroRepository(_dbContext);
                return _tipologiaParametroRepository;
            }
        }

        public ComunitaRepository ComunitaRepository
        {
            get
            {
                if (_comunitaRepository == null)
                    _comunitaRepository = new ComunitaRepository(_dbContext);
                return _comunitaRepository;
            }
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }

}
