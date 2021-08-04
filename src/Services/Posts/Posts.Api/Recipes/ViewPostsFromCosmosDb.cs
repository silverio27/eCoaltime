using Posts.Api.SeedWork;
using Posts.Infra.DataCosmosDB;
using System.Linq;
using System.Threading.Tasks;

namespace Posts.Api.Recipes
{
    public class ViewPostsFromCosmosDb : IViewPosts
    {
        private readonly PostsContext _context;

        public ViewPostsFromCosmosDb(PostsContext context) => _context = context;
        public async Task<PagedList<dynamic>> Get(PaginateParameters parameters, string urlBase)
        {
            IQueryable<dynamic> query = _context.Posts.OrderBy(x => x.LastModified).Select(x => x.ToView(urlBase));
            return await PagedList<dynamic>.ToPagedList(query, parameters.PageNumber, parameters.PageSize);
        }
    }
}
