using TodoAppNTier.DataAccess.Interfaces;
using TodoAppNTier.DataAccess.Repositories;
using TodoAppNTier.Entities.Concrete;
using TodoAppNTier.DataAccess.Contexts;
namespace TodoAppNTier.DataAccess.UnitofWork
{
    public class Uow : IUow
    {

        private readonly TodoContext _context;

        public Uow(TodoContext context)
        {
            _context = context;
        }

        public IRepository<T> GetRepository<T>() where T : BaseEntity // Artık her istediğimizde yeni bir Repository<T> örneği oluşturuyoruz, tek bir Repository<T> örneği değil!
        {
            return new Repository<T>(_context);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }


}