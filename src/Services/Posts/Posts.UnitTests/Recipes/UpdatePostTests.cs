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
            _posts.Setup(x => x.UnitOfWork.SaveEntitiesAsync(default))
                .Returns(Task.FromResult(true));

            Domain.Recipes.Author user = new("img.jpg", "lucas", Guid.NewGuid());
            Post post = new(user);
            _posts.Setup(x => x.GetAsync(post.Id))
                .Returns(Task.FromResult(post));

            var handler = new Update(_posts.Object);
            UpdatePost command = new(post.Id, "Salmão", "Salmão grelhado", "Muito gostoso");
            var response = await handler.Handle(command, default);
            Assert.True(response.Success);
        }

        [Fact]
        public async Task UpdatePostFailed()
        {
            _posts.Setup(x => x.UnitOfWork.SaveEntitiesAsync(default))
                .Returns(Task.FromResult(true));

            var handler = new Update(_posts.Object);
            UpdatePost command = new(Guid.Empty, "Salmão", "Salmão grelhado", "Muito gostoso");
            var response = await handler.Handle(command, default);
            Assert.False(response.Success);
        }
    }
}
