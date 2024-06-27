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
        private Utente_ComunitaRepository _utente_comunitaRepository;
        private Tipo_UtenzaRepository _tipoutenzaRepository;
        private ConsumerRepository _consumerRepository;
        private ProsumerRepository _prosumerRepository;
        private ImpiantoRepository _impiantoRepository;
        private Dati_EconomiciRepository _dati_economiciRepository;
        private Dati_ImpiantoRepository _dati_impiantoRepository;
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

        public Utente_ComunitaRepository Utente_ComunitaRepository
        {
            get
            {
                if (_utente_comunitaRepository == null)
                    _utente_comunitaRepository = new Utente_ComunitaRepository(_dbContext);
                return _utente_comunitaRepository;
            }
        }
        public Tipo_UtenzaRepository Tipo_UtenzaRepository
        {
            get
            {
                if (_tipoutenzaRepository == null)
                    _tipoutenzaRepository = new Tipo_UtenzaRepository(_dbContext);
                return _tipoutenzaRepository;
            }
        }
        public ConsumerRepository ConsumerRepository
        {
            get
            {
                if (_consumerRepository == null)
                    _consumerRepository = new ConsumerRepository(_dbContext);
                return _consumerRepository;
            }
        }
        public ProsumerRepository ProsumerRepository
        {
            get
            {
                if (_prosumerRepository == null)
                    _prosumerRepository = new ProsumerRepository(_dbContext);
                return _prosumerRepository;
            }
        }
        public ImpiantoRepository ImpiantoRepository
        {
            get
            {
                if (_impiantoRepository == null)
                    _impiantoRepository = new ImpiantoRepository(_dbContext);
                return _impiantoRepository;
            }
        }
        public Dati_EconomiciRepository Dati_EconomiciRepository
        {
            get
            {
                if (_dati_economiciRepository == null)
                    _dati_economiciRepository = new Dati_EconomiciRepository(_dbContext);
                return _dati_economiciRepository;
            }
        }
        public Dati_ImpiantoRepository Dati_ImpiantoRepository
        {
            get
            {
                if (_dati_impiantoRepository == null)
                    _dati_impiantoRepository = new Dati_ImpiantoRepository(_dbContext);
                return _dati_impiantoRepository;
            }
        }


        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }

}
