using TodoAppNTier.DataAccess.Interfaces;
using TodoAppNTier.Entities.Concrete;

namespace TodoAppNTier.DataAccess.UnitofWork

{
    public interface IUow 
    {
        IRepository<T> GetRepository <T>() where T : BaseEntity;
        Task SaveChanges();
    }
}
    