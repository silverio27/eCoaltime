using Posts.Domain.Recipes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Posts.Infra.DataCosmosDB
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToContainer(nameof(Post));
            builder.HasKey(x => x.Id);
            //builder.OwnsOne(x => x.Author,"author", (x)=> { x.) });
            //builder.OwnsMany(x => x.Pictures);

            //builder.Property(x => x.Author.UserId).ToJsonProperty("UserId");
            //builder.HasPartitionKey("$Author.UserId");
            //builder.Property("$Author.UserId").HasConversion(new GuidToStringConverter());
            //builder.HasPartitionKey("id");
            //builder.HasPartitionKey(x => x.Id);
        }
    }
}
