using Dashboard.BusinessLayer;

namespace Dashboard.Interfaces
{
    public interface IBaseRepositable<T>
    {
        Task<T> GetById(int id);
        void Add(T entity);
        bool Update(T entity);
        Task<bool> Delete(int id);



    }
}
