using System;
using System.Threading.Tasks;
using Posts.Domain.SeedWork;

namespace Posts.Domain.Recipes
{
    public interface IPosts : IRepository<Post>
    {
        Task<Post> Add(Post post);
        Task Update(Post post);
        Task<Post> GetAsync(Guid postId, Guid authorId);
        Task Delete(Post post);
    }
}