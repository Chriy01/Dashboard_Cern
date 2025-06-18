using Dashboard.BusinessLayer;

namespace Dashboard.Interfaces
{
    public interface IComunitaRepository
    {
        Comunita GetById(int comunitaId);
        void Add(Comunita comunita);
        void Update(Comunita comunita);
        void Delete(int comunitaId);
       
    }
}
