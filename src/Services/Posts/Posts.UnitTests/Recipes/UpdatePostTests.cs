using Posts.Api.Recipes;
using Posts.Domain.Recipes;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Posts.UnitTests.Recipes
{
    public class UpdatePostTests
    {
        private readonly Mock<IPosts> _posts;

        public UpdatePostTests()
        {
            _posts = new Mock<IPosts>();
        }  

        [Fact]
        public async Task UpdatePostSuccess()
        {

            Domain.Recipes.Author author = new("img.jpg", "lucas", Guid.NewGuid());
            Post post = new(author);
            _posts.Setup(x => x.GetAsync(post.Id, author.UserId))
                .Returns(Task.FromResult(post));

            var handler = new Update(_posts.Object);
            UpdatePost command = new(post.Id, author.UserId, "Salmão", "Salmão grelhado", "Muito gostoso");
            var response = await handler.Handle(command, default);
            Assert.True(response.Success);
        }

        [Fact]
        public async Task UpdatePostFailed()
        {
            var handler = new Update(_posts.Object);
            UpdatePost command = new(Guid.Empty, Guid.NewGuid(), "Salmão", "Salmão grelhado", "Muito gostoso");
            var response = await handler.Handle(command, default);
            Assert.False(response.Success);
        }
    }
}
