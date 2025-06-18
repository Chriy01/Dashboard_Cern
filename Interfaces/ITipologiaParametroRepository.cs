using Dashboard.BusinessLayer;

namespace Dashboard.Interfaces
{
    public interface ITipologiaParametroRepository
    {
        TipologiaParametro GetById(int tipologiaId);
        void Add(TipologiaParametro tipologiaParametro);
        void Update(TipologiaParametro tipologiaParametro);
        void Delete(int tipologiaId);
       
    }
}
