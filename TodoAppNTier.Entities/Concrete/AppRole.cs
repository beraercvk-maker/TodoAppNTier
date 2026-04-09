using Microsoft.AspNetCore.Identity;

namespace TodoAppNTier.Entities.Concrete
{
    // DİKKAT: Başına 'public' yazmayı unutmamalıyız ve <int> vermeliyiz!
    public class AppRole : IdentityRole<int>
    {
    }
}