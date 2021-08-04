using Posts.Api.SeedWork;
using Posts.Domain.Recipes;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Posts.Api.Recipes
{
    public record UpdatePost(Guid PostId, string Title, string SubTitle, string Description) : IRequest<Response>;
    public record RemovePicture(Guid PostId, Guid PictureId) : IRequest<Response>;
    public record Publish(Guid PostId) : IRequest<Response>;
    public class Update : IRequestHandler<UpdatePost, Response>,
                          IRequestHandler<RemovePicture, Response>,
                          IRequestHandler<Publish, Response>
    {
        private readonly IPosts _posts;

        public Update(IPosts posts) => _posts = posts;

        public async Task<Response> Handle(UpdatePost command, CancellationToken cancellationToken)
        {
            try
            {
                var post = await _posts.GetAsync(command.PostId);
                post.SetTitle(command.Title);
                post.SetSubTitle(command.SubTitle);
                post.SetDescription(command.Description);

                await _posts.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                return new("Post atualizado");
            }
            catch (Exception e)
            {

                return new(e.Message, false);
            }
        }

        public async Task<Response> Handle(RemovePicture command, CancellationToken cancellationToken)
        {
            try
            {
                var post = await _posts.GetAsync(command.PostId);
                var picture = post.Pictures.FirstOrDefault(x => x.Id == command.PictureId);
                post.RemovePicture(picture);
                _posts.Update(post);
                await _posts.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                return new("Post atualizado");
            }
            catch (Exception e)
            {

                return new(e.Message, false);
            }
        }

        public async Task<Response> Handle(Publish command, CancellationToken cancellationToken)
        {
            try
            {
                var post = await _posts.GetAsync(command.PostId);
                post.Publish();
                _posts.Update(post);
                await _posts.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                return new("Publicado");
            }
            catch (Exception e)
            {

                return new(e.Message, false);
            }
        }
    }
}
