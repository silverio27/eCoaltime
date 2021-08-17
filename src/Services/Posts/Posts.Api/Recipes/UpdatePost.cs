using Posts.Api.SeedWork;
using Posts.Domain.Recipes;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;

namespace Posts.Api.Recipes
{
    public record UpdatePost(Guid PostId, Guid AuthorId, string Title, string SubTitle, string Description) : IRequest<Response>;
    public class UpdatePostValidator : AbstractValidator<UpdatePost>
    {
        public UpdatePostValidator()
        {
            RuleFor(x => x.PostId).Must(x => x != Guid.Empty).WithMessage("Post inválido.");
            RuleFor(x => x.AuthorId).Must(x => x != Guid.Empty).WithMessage("Autor inválido.");
        }
    }
    public record RemovePicture(Guid PostId, Guid AuthorId, Guid PictureId) : IRequest<Response>;
    public record Publish(Guid PostId, Guid AuthorId) : IRequest<Response>;
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
                var post = await _posts.GetAsync(command.PostId, command.AuthorId);
                post.SetTitle(command.Title);
                post.SetSubTitle(command.SubTitle);
                post.SetDescription(command.Description);

                await _posts.Update(post);
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
                var post = await _posts.GetAsync(command.PostId, command.AuthorId);
                var picture = post.Pictures.FirstOrDefault(x => x.Id == command.PictureId);
                post.RemovePicture(picture);
                await _posts.Update(post);

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
                var post = await _posts.GetAsync(command.PostId, command.AuthorId);
                post.Publish();
                await _posts.Update(post);
                
                return new("Publicado");
            }
            catch (Exception e)
            {

                return new(e.Message, false);
            }
        }
    }
}
