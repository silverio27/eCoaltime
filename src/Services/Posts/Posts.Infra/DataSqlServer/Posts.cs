using Posts.Domain.Recipes;
using Posts.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Posts.Infra.DataSqlServer
{
    public class Posts : IPosts
    {
        private readonly PostsContext _context;

        public Posts(PostsContext context) => _context = context;

        public IUnitOfWork UnitOfWork => _context;

        public Post Add(Post post) => _context.Posts.Add(post).Entity;

        public void Delete(Post post) => _context.Posts.Remove(post);

        public async Task<Post> GetAsync(Guid postId)
        {
            var post = await _context.Posts
                .Include(x => x.Author)
                .FirstOrDefaultAsync(x => x.Id == postId);

            if (post is not null)
                await _context.Entry(post)
                   .Collection(i => i.Pictures).LoadAsync();

            return post;
        }

        public void Update(Post post)
        {

            var existingPhotos = _context.Photos.Where(x => x.PostId == post.Id).ToList();
            if (existingPhotos.Any())
            {
                _context.Photos.AddRange(post.Pictures.Except(existingPhotos));
                _context.Photos.RemoveRange(existingPhotos.Except(post.Pictures));
            }
            else _context.Photos.AddRange(post.Pictures);
            _context.Entry(post).State = EntityState.Modified;

        }
    }
}
