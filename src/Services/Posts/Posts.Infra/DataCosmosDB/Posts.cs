using Posts.Domain.Recipes;
using Posts.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Posts.Infra.DataCosmosDB
{
    public class Posts : IPosts
    {
        private readonly PostsContext _context;

        public Posts(PostsContext context) => _context = context;
        public IUnitOfWork UnitOfWork => _context;

        public Post Add(Post post) => _context.Add(post).Entity;

        public void Delete(Post post) => _context.Remove(post);

        public Task<Post> GetAsync(Guid postId) => _context.Posts.FirstOrDefaultAsync(x => x.Id == postId);

        public void Update(Post post) => _context.Posts.Update(post);
    }
}
