using System;
using System.Collections.Generic;
using System.Text.Json;
using Posts.Domain.Recipes;
using Posts.Domain.SeedWork;
using Xunit;
using Xunit.Abstractions;

namespace Posts.UnitTests.Recipes
{
    public class PostTests
    {
        private readonly ITestOutputHelper _output;
       
        public PostTests(ITestOutputHelper testOutputHelper) => _output = testOutputHelper;

        [Fact]
        public void NewPostSuccess()
        {
            var post = Builder.CreatePost();
            string json = JsonSerializer.Serialize(post, new() { WriteIndented = true });
            _output.WriteLine(json);

            Assert.NotNull(post);
            Assert.Empty(post.Pictures);
            Assert.NotNull(post.Author);
            Assert.Equal("Nova receita", post.Title);
            Assert.True(post.IsDraft);
        }


        [Fact]
        public void NewPostFailedByUser()
        {
            List<Picture> photos = new()
            {
                new Picture(cover: true, pictureName: "https://www.silverio.dev.br/assets/foods/salmao.jpeg"),
                new Picture(cover: false, pictureName: "https://www.silverio.dev.br/assets/foods/bolinho.jpeg")
            };

            var exeption = Assert.Throws<PostException>(() =>
              new Post(
                    author: null)
               );

            _output.WriteLine(exeption.Message);

            Assert.Equal("É obrigatório informar quem está fazendo a postagem.", exeption.Message);
        }

        [Fact]
        public void NewPostFailedByInvalidUser()
        {
            var exeption = Assert.Throws<PostException>(() =>
               new Author(
                avatarImgUrl: "https://www.silverio.dev.br/assets/profile-pic.jpeg",
                name: "Lucas Silvério",
                userId: Guid.Empty));

            _output.WriteLine(exeption.Message);

            Assert.Equal("Usuário inválido.", exeption.Message);
        }

        [Fact]
        public void NewPostFailedByUserWhithoutAvatar()
        {
            var exeption = Assert.Throws<PostException>(() =>
               new Author(
                avatarImgUrl: "",
                name: "Lucas Silvério",
                userId: new Guid("6d0bdd60-912f-4f75-a69a-5c19e8d99eef")));

            _output.WriteLine(exeption.Message);

            Assert.Equal("É obrigatório uma imagem para identificar o usuário.", exeption.Message);
        }

        [Fact]
        public void NewPostFailedByUserWhithoutName()
        {
            var exeption = Assert.Throws<PostException>(() =>
               new Author(
                avatarImgUrl: "https://www.silverio.dev.br/assets/profile-pic.jpeg",
                name: "",
                userId: new Guid("6d0bdd60-912f-4f75-a69a-5c19e8d99eef")));

            _output.WriteLine(exeption.Message);

            Assert.Equal("É obrigatório um nome para identificar o usuário.", exeption.Message);
        }


        [Fact]
        public void PublishSuccess()
        {
            var post = Builder.CreatePost();
            post.SetDescription("Salmão na prancha de abacaxi e lemmon pepper.");
            post.SetSubTitle("Salmão na churrasqueira");
            post.SetTitle("Salmão");
            post.AddPicture(new(true, "img.jpg"));
            post.Publish();
            Assert.False(post.IsDraft);
        }
        [Fact]
        public void PublishFailedByRequiredProperties()
        {
            var post = Builder.CreatePost();
            post.SetDescription("Salmão na prancha de abacaxi e lemmon pepper.");
            post.SetSubTitle("Salmão na churrasqueira");
            post.SetTitle("Salmão");
            var exeption = Assert.Throws<PostException>(() => post.Publish());
            Assert.Equal("É obrigatório informar, título, subtítulo, descrição e ter ao menos uma foto para publicar.", exeption.Message);
        }
    }
}