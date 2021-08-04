using Posts.Domain.Recipes;
using Posts.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Posts.Infra.DataSqlServer
{
    public class PostsContext : DbContext, IUnitOfWork
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Picture> Photos { get; set; }
        public PostsContext(DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new PictureConfiguration());
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await base.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

}
