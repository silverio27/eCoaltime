using Posts.Api.Recipes;
using Posts.Api.SeedWork;
using Posts.Domain.Recipes;
using Posts.Infra.Storage;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Posts.UnitTests.Recipes
{
    public class ControllerTests
    {
        readonly Mock<IPosts> _posts;
        readonly Mock<IViewPosts> _viewPosts;
        readonly IOptions<StorageSettings> _settings;
        readonly Mock<IMediator> _mediator;

        public ControllerTests()
        {
            _posts = new();
            _viewPosts = new();
            _settings = Options.Create<StorageSettings>(new());
            _mediator = new Mock<IMediator>();
        }

        [Fact]
        public async Task ToSketchSucces()
        {
            _mediator.Setup(x => x.Send(It.IsAny<CreatePost>(), default)).ReturnsAsync(new Response("Rascunho criado."));

            var controller = new PostsController(_posts.Object, _viewPosts.Object, _mediator.Object, _settings);

            var result = await controller.Create(new(null)) as CreatedResult;
            Assert.Equal(201, result.StatusCode);
            Assert.Equal("Rascunho criado.", ((Response)result.Value).Message);
        }

        [Fact]
        public async Task ToSketchFailed()
        {
            _mediator.Setup(x => x.Send(It.IsAny<CreatePost>(), default)).ReturnsAsync(new Response("Falhou", false));

            var controller = new PostsController(_posts.Object, _viewPosts.Object, _mediator.Object, _settings);

            var result = await controller.Create(new(null)) as BadRequestObjectResult;
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Falhou", ((Response)result.Value).Message);
        }
    }
}
