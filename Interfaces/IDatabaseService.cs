using Dashboard.Implementation;
using System.Reflection.Metadata;

namespace Dashboard.Interfaces
{
    public interface IDatabaseService
    {
        ParametroRepository ParametroRepository { get; }
        TipologiaParametroRepository TipologiaParametroRepository { get; }
        ComunitaRepository ComunitaRepository { get; }
        UtenteRepository UtenteRepository { get; }
        ConsumerRepository ConsumerRepository { get; }
        ProsumerRepository ProsumerRepository { get; }
        Dati_EconomiciRepository Dati_EconomiciRepository { get; }
        Dati_ImpiantoRepository Dati_ImpiantoRepository { get; }
    }

}
