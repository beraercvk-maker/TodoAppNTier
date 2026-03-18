using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TodoAppNTier.Entities.Concrete
{
    public class WorkConfiguration : IEntityTypeConfiguration<Work>
    {
        public void Configure(EntityTypeBuilder<Work> builder)
        {
            builder.HasKey(x => x.Id);
           builder.Property(x => x.Definition).HasMaxLength(200).IsRequired(true);
           builder.Property(x => x.IsCompleted).IsRequired(true);
           
           
          
        }
    }
}