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
    }

}
