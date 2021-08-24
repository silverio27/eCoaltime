using Posts.Api.Recipes;
using Posts.Domain.Recipes;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Posts.UnitTests.Recipes
{
    public class DraftTests
    {
        private readonly Mock<IPosts> _posts;

        public DraftTests()
        {
            _posts = new Mock<IPosts>();
        }

        [Fact]
        public async Task ToSketchSuccess()
        {

            var handler = new Draft(_posts.Object);
            Api.Recipes.Author user = new("img.jpg", "Lucas Silvério", Guid.NewGuid());

            CreatePost createSketch = new(user);
            var result = await handler.Handle(createSketch, default);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task ToSketchFailedByException()
        {
            var handler = new Draft(_posts.Object);
            Api.Recipes.Author user = new("img.jpg", "Lucas Silvério", Guid.Empty);
            CreatePost createSketch = new(user);
                
            var result = await handler.Handle(createSketch, default);
            Assert.False(result.Success);
        }
    }
}
