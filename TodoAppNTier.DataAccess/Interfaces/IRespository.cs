using System.Linq.Expressions;
using TodoAppNTier.Entities.Concrete; // BaseEntity'yi tanıması için bu satır gerekebilir

namespace TodoAppNTier.DataAccess.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAll();
        Task<T> GetById(object id);
        Task Create(T entity);
        
        // İŞTE DEĞİŞEN TEK SATIR BURASI: Artık iki parametre alıyor!
        void Update(T entity, T unchanged);
        
        void Remove(object id);
        void Remove(T entity);

        Task<T> GetByFilter(Expression<Func<T, bool>> filter, bool asNoTracking=false);
    }
}