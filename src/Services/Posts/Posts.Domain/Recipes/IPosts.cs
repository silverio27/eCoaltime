using System;
using System.Threading.Tasks;
using Posts.Domain.SeedWork;

namespace Posts.Domain.Recipes
{
    public interface IPosts : IRepository<Post>
    {
        Post Add(Post post);
        void Update(Post post);
        Task<Post> GetAsync(Guid postId);
        void Delete(Post post);
    }
}