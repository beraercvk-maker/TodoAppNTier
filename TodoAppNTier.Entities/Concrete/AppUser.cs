using Microsoft.AspNetCore.Identity;

namespace TodoAppNTier.Entities.Concrete
{
    // DİKKAT: Burası IdentityRole DEĞİL, IdentityUser olacak!
    public class AppUser : IdentityUser<int>
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public List<Work>? Works { get; set; } 
    }
}