using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TodoAppNTier.DataAccess.Interfaces;
using TodoAppNTier.Entities.Concrete;

namespace TodoAppNTier.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly TodoContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(TodoContext context)
        {
            _context = context;
            
        }

        public async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(object id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task Create(T entity)
        {
          await _context.Set<T>().AddAsync(entity); 
        }

        public void Update(T entity)
        {
            var updatedEntity = _context.Set<T>().Find(entity.Id);
            _context.Entry(updatedEntity).CurrentValues.SetValues(entity);
           

        }

        public void Remove(object id)
        {
            var deletedEntity = _context.Set<T>().Find(id);
            if (deletedEntity != null) // Silinecek var mı kontrolü yapıldı
            {
                _context.Set<T>().Remove(deletedEntity);
            }
        }

      public async Task<T> GetByFilter(Expression<Func<T, bool>> filter, bool asNoTracking = false)
{
    return asNoTracking 
        ? await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(filter)
        : await _context.Set<T>().FirstOrDefaultAsync(filter);
}

        void IRepository<T>.Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        void IRepository<T>.Remove(object id)
        {
            var deletedEntity = _context.Set<T>().Find(id);
            if (deletedEntity != null) // Silinecek var mı kontrolü yapıldı
            {
                _context.Set<T>().Remove(deletedEntity);
            }
           
        }

        public IQueryable<T> GetQuery()
        {
            return _context.Set<T>().AsQueryable();
        }
    }
}