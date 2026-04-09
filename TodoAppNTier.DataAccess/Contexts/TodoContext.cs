using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoAppNTier.Entities.Concrete; // AppUser, AppRole ve Work sınıflarını görebilmesi için

namespace TodoAppNTier.DataAccess.Contexts // DÜZELTME 1: Doğru Adres!
{
    // DÜZELTME 2: Artık normal DbContext değil, Güvenlikli IdentityDbContext!
    public class TodoContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // DÜZELTME 3: Bu satır olmazsa Identity tabloları veritabanında ASLA oluşmaz!
            base.OnModelCreating(modelBuilder); 
            
            modelBuilder.ApplyConfiguration(new WorkConfiguration());
        }

        public DbSet<Work> Works { get; set; }
    }
}