using Microsoft.EntityFrameworkCore;

namespace TodoAppNTier.Entities.Concrete
{
    public class TodoContext : DbContext
    {

        public TodoContext (DbContextOptions<TodoContext> options) :base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new WorkConfiguration());
        }
        public DbSet<Work> Works { get; set; }
        
    }
}