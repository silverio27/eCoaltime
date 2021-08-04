using Posts.Domain.Recipes;
using System.Linq;

namespace Posts.Api.Recipes
{
    public static class ViewPost
    {
        public static dynamic ToView(this Post post, string urlBase)
        {
            if (post is null) return null;
            return new
            {
                post.Id,
                post.Title,
                post.SubTitle,
                post.Description,
                post.Author,
                post.IsDraft,
                post.LastModified,
                post.PublishedAt,
                Pictures = post.Pictures?.Select(x => new
                {
                    x.Id,
                    x.Cover,
                    PictureUrl = urlBase + x.PictureName
                })
            };
        }
    }

}
