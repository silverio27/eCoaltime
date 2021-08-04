using Posts.Api.SeedWork;
using System.Threading.Tasks;

namespace Posts.Api.Recipes
{
    public interface IViewPosts
    {
        Task<PagedList<dynamic>> Get(PaginateParameters parameters, string urlBase);
    }
}
