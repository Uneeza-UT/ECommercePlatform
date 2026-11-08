using ECommercePlatform.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommercePlatform.Persistence.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.Property(a => a.FileName)
                .HasMaxLength(200);


            builder.Property(a => a.FilePath)
                .HasMaxLength(500);


            builder.Property(a => a.ContentType)
                .HasMaxLength(100);
        }
    }
}
