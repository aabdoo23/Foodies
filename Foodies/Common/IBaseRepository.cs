namespace Foodies.Common
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        public Task<T> Create(T entity);
        public Task<T> Update(T entity);
        public Task<T> Delete(string id);
        public Task<T> GetById(string id);
        public Task<IEnumerable<T>> GetAll();

    }
}
