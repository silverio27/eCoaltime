using System.Threading.Tasks;

namespace Posts.Domain.Recipes
{
    public interface IPictures
    {
        Task<string> UploadAsync(string base64image);
    }
}
