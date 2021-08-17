
using Posts.Api.SeedWork;
using Posts.Domain.Recipes;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Posts.Api.Recipes
{
    public record DeletePost(Guid PostId, Guid AuthorId) : IRequest<Response>;
    public class Delete : IRequestHandler<DeletePost, Response>
    {
        private readonly IPosts _posts;

        public Delete(IPosts posts)
        {
            _posts = posts;
        }

        public async Task<Response> Handle(DeletePost command, CancellationToken cancellationToken)
        {
            try
            {
                var post = await _posts.GetAsync(command.PostId, command.AuthorId);
                await _posts.Delete(post);


                return new("Post deletado");
            }
            catch (Exception e)
            {

                return new(e.Message, false);
            }
        }
    }
}
