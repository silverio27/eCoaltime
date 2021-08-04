using Posts.Domain.Recipes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Posts.Infra.DataSqlServer
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.OwnsOne(x => x.Author, y =>
            {
                y.Property<Guid>("PostId");
                y.WithOwner();
            });

            var navigation = builder.Metadata.FindNavigation(nameof(Post.Pictures));

            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
