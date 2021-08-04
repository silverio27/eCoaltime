
using Posts.Domain.Recipes;
using Posts.Domain.SeedWork;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Posts.UnitTests.Recipes
{
    public class PicturesTests
    {
        private readonly ITestOutputHelper _output;
        public PicturesTests(ITestOutputHelper testOutputHelper) => _output = testOutputHelper;

        [Fact]
        public void NewPostFailedByInvalidPicture()
        {
            var exeption = Assert.Throws<PostException>(() =>
              new Picture(cover: true, pictureName: ""));

            _output.WriteLine(exeption.Message);

            Assert.Equal("O nome para imagem não pode ser vazia.", exeption.Message);
        }

        [Fact]
        public void AddPicturesInPostTheLastPictureMarkAsCoverIsTheCover()
        {
            var post = Builder.CreatePost();

            post.AddPicture(new(true, "primeira.jpg"));
            post.AddPicture(new(true, "segunda.jpg"));
            post.AddPicture(new(true, "terceira.jpg"));

            Assert.Single(post.Pictures.Where(x => x.Cover));
            Assert.Equal("terceira.jpg", post.Pictures.FirstOrDefault(x => x.Cover).PictureName);
        }
        
        [Fact]
        public void AddPhotosFailedByLimit()
        {
            var post = Builder.CreatePost();

            post.AddPicture(new(true, "primeira.jpg"));
            post.AddPicture(new(true, "segunda.jpg"));
            post.AddPicture(new(true, "terceira.jpg"));
            post.AddPicture(new(true, "quarta.jpg"));
            post.AddPicture(new(true, "quinta.jpg"));

            var exeption = Assert.Throws<PostException>(() =>
             post.AddPicture(new(true, "sexta.jpg")));

            Assert.Equal($"Um post não pode ter mais de {Post.MaximumPictures} fotos.", exeption.Message);
        }

        [Fact]
        public void RemoveAllPictureFailedWhenPostAsPublished()
        {
            var post = Builder.CreatePost();
            post.SetDescription("Salmão na prancha de abacaxi e lemmon pepper.");
            post.SetSubTitle("Salmão na churrasqueira");
            post.SetTitle("Salmão");
            Picture picture = new(true, "img.jpg");
            post.AddPicture(picture);
            post.Publish();
            var exeption = Assert.Throws<PostException>(() =>
             post.RemovePicture(picture));

            Assert.Equal($"Não é possível remover todas as imagens de um post publicado.", exeption.Message);

        }
    }
}
