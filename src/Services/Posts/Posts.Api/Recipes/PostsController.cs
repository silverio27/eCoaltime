
using Posts.Api.SeedWork;
using Posts.Domain.Recipes;
using Posts.Infra.Storage;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Posts.Api.Recipes
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PostsController : Controller
    {
        private readonly IPosts _posts;
        private readonly IViewPosts _queries;
        private readonly IMediator _mediator;
        private readonly StorageSettings _pictureSettings;

        public PostsController(IPosts posts, IViewPosts queries, IMediator mediator, IOptions<StorageSettings> settings)
        {
            _posts = posts;
            _queries = queries;
            _mediator = mediator;
            _pictureSettings = settings.Value;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreatePost post)
        {
            var result = await _mediator.Send(post);
            if (result.Success) return Created("", result);
            return BadRequest(result);
        }

        [HttpPatch("details/{postId:Guid}")]
        public async Task<ActionResult> Update(Guid postId, UpdatePost post)
        {
            if (postId != post.PostId)
                return BadRequest();

            var result = await _mediator.Send(post);
            if (result.Success) return NoContent();
            return BadRequest(result);
        }

        [HttpPatch("publish/{postId:Guid}")]
        public async Task<ActionResult> Publish(Guid postId, Publish post)
        {
            if (postId != post.PostId)
                return BadRequest();

            var result = await _mediator.Send(post);
            if (result.Success) return NoContent();
            return BadRequest(result);
        }

        [HttpPatch("pictures/add/{postId:Guid}")]
        public async Task<ActionResult> UploadPictures(Guid postId, AddPictures post)
        {
            if (postId != post.PostId)
                return BadRequest();

            var result = await _mediator.Send(post);
            if (result.Success) return NoContent();
            return BadRequest(result);
        }

        [HttpPatch("pictures/remove/{postId:Guid}")]
        public async Task<ActionResult> RemovePicture(Guid postId, RemovePicture picture)
        {
            if (postId != picture.PostId)
                return BadRequest();

            var result = await _mediator.Send(picture);
            if (result.Success) return NoContent();
            return BadRequest(result);
        }

        [HttpDelete("{postId:Guid}")]
        public async Task<ActionResult> RemovePost(Guid postId)
        {
            var result = await _mediator.Send(new DeletePost(postId));
            if (result.Success) return NoContent();
            return BadRequest(result);
        }

        [HttpGet("{postId:Guid}")]
        public async Task<ActionResult> GetPostAsync(Guid postId)
        {
            var post = await _posts.GetAsync(postId);
            if (post is null)
                return NotFound();

            var response = post.ToView(_pictureSettings.UrlBase);
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult> GetPosts([FromQuery] PaginateParameters parameters)
        {
            var posts = await _queries.Get(parameters, _pictureSettings.UrlBase);
            if (posts is null)
                return NotFound();

            var metadata = new
            {
                posts.TotalCount,
                posts.PageSize,
                posts.CurrentPage,
                posts.TotalPages,
                posts.HasNext,
                posts.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(posts);
        }
    }


}
