using Dashboard.BusinessLayer;
using System.Reflection.Metadata;

namespace Dashboard.Interfaces
{
    public interface IParametroRepository
    {
        Parametro GetById(int parametroId);
        void Add(Parametro parametro);
        void Update(Parametro parametro);
        void Delete(int parametroId);
        // Altre operazioni se necessario
    }
}
