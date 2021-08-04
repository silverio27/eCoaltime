using Posts.Api.SeedWork;
using Posts.Infra.DataSqlServer;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Posts.Api.Recipes
{
    public class ViewPostsFromSqlServer : IViewPosts
    {
        private readonly PostsContext _context;

        public ViewPostsFromSqlServer(PostsContext context) => _context = context;
        public async Task<PagedList<dynamic>> Get(PaginateParameters parameters, string urlBase)
        {
            IQueryable<dynamic> query = _context.Posts.Include(x => x.Author).Include(x => x.Pictures).Select(x => x.ToView(urlBase));
            return await PagedList<dynamic>.ToPagedList(query, parameters.PageNumber, parameters.PageSize);
        }
    }
}
