namespace Dashboard.Interfaces
{
    public interface IBaseRepositable<T>
    {
        T GetById(int id);
        void Add(T entity);
        bool Update(T entity);
        bool Delete(int id);



    }
}
