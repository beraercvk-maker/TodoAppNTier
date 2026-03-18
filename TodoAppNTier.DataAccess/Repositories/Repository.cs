using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TodoAppNTier.DataAccess.Interfaces;
using TodoAppNTier.Entities.Concrete;

namespace TodoAppNTier.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, new()
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
            _context.Set<T>().Update(entity);
        }

        public void Remove(T entity)
        {
           _context.Set<T>().Remove(entity);
        }

      public async Task<T> GetByFilter(Expression<Func<T, bool>> filter, bool asNoTracking = false)
{
    return asNoTracking 
        ? await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(filter)
        : await _context.Set<T>().FirstOrDefaultAsync(filter);
}

        void IRepository<T>.Update(T entity)
        {
            throw new NotImplementedException();
        }

        void IRepository<T>.Remove(T entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetQuery()
        {
            return _context.Set<T>().AsQueryable();
        }
    }
}