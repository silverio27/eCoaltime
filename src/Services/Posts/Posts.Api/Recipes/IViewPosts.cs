using Posts.Api.SeedWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Posts.Api.Recipes
{
    public class PaginatePost : PaginateParameters { public Guid AuthorId { get; set; } }
    public interface IViewPosts
    {
        Task<dynamic> Get(PaginatePost parameters, string urlBase);
    }
}
