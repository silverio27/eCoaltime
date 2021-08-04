using Posts.Domain.Recipes;
using Posts.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Threading;
using System.Threading.Tasks;

namespace Posts.Infra.DataCosmosDB
{
    public class PostsContext : DbContext, IUnitOfWork
    {
        public DbSet<Post> Posts { get; set; }
        public PostsContext(DbContextOptions options) : base(options){
            base.Database.EnsureCreated();
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await base.SaveChangesAsync(cancellationToken);
            return true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PostConfiguration());

        }

    }
}
