namespace HSEBank.Repositories
{
    public interface IRepository<T>
    {
        T Add(T entity);
        void Update(T entity);
        void Remove(Guid id);
        T Get(Guid id);
        IReadOnlyCollection<T> GetAll();
        bool Exists(Guid id);
    }
}