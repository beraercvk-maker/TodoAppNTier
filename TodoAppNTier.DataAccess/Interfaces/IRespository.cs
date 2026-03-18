using System.Linq.Expressions;

namespace TodoAppNTier.DataAccess.Interfaces
{
    public interface IRepository<T> where T : class , new()
    {
        Task<List<T>> GetAll();
        Task<T> GetById(object id);
        Task Create(T entity);
        void Update(T entity);
        void Remove(T entity);

        Task<T> GetByFilter(Expression<Func<T, bool>> filter, bool asNoTracking=false);
        
    }

}