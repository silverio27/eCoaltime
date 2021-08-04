using Posts.Domain.Recipes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Posts.Infra.DataSqlServer
{
    public class PictureConfiguration : IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> builder)
        {
            builder.Property<Guid>("PostId")
                   .IsRequired();
        }
    }
}
