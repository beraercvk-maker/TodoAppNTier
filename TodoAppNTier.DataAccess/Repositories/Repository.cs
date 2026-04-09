using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TodoAppNTier.DataAccess.Interfaces;
using TodoAppNTier.Entities.Concrete;
using TodoAppNTier.DataAccess.Contexts;
namespace TodoAppNTier.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly TodoContext _context;
        // _dbSet tanımını kullanmadığın için sildim, kod daha temiz oldu!

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

        // İŞTE YENİ, PERFORMANSLI VE KISACIK UPDATE METODUMUZ!
        public void Update(T entity, T unchanged)
        {
            // Artık kendi içimizde Find() diyerek veritabanına fazladan SELECT atmıyoruz!
            // Service'in bize bulup getirdiği "unchanged" (orijinal) verinin üzerine,
            // kullanıcının gönderdiği "entity" (yeni) veriyi akıllıca yapıştırıyoruz.
            _context.Entry(unchanged).CurrentValues.SetValues(entity);
        }

        public void Remove(object id)
        {
            var deletedEntity = _context.Set<T>().Find(id);
            if (deletedEntity != null)
            {
                _context.Set<T>().Remove(deletedEntity);
            }
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

        public IQueryable<T> GetQuery()
        {
            return _context.Set<T>().AsQueryable();
        }
    }
}