namespace TodoAppNTier.Entities.Concrete
{
    public class Work : BaseEntity
    {
        
        public string Definition { get; set; }
        public bool IsCompleted { get; set; }

        public int AppUserId { get; set; } 

        // 2. Entity Framework'ün .Include() yaparken kullanacağı bağlantı nesnesi (Navigation Property)
        public AppUser AppUser { get; set; } = null!;
    }
}