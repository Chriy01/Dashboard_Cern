using System.Reflection.Metadata;

namespace Dashboard.Interfaces
{
    public interface IDatabaseService
    {
        IParametroRepository ParametroRepository { get; }
        ITipologiaParametroRepository TipologiaParametroRepository { get; }
        IComunitaRepository ComunitaRepository { get; }
    }

}
