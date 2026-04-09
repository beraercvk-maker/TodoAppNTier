using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TodoAppNTier.Entities.Concrete;
using TodoAppNTier.DataAccess.Contexts;
namespace TodoAppNTier.DataAccess
{
    public class TodoContextFactory : IDesignTimeDbContextFactory<TodoContext>
    {
        public TodoContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TodoContext>();
            
            // appsettings.json'daki bağlantı dizesinin aynısını buraya yazıyoruz
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TodoAppNTierDb;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new TodoContext(optionsBuilder.Options);
        }
    }
}