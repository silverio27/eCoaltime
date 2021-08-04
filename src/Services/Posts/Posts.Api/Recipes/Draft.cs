using Posts.Api.SeedWork;
using Posts.Domain.Recipes;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Posts.Api.Recipes
{
    public record CreatePost(Author Author) : IRequest<Response>;
    public record Author(string AvatarImgUrl, string Name, Guid UserId);
    public class DraftValidator : AbstractValidator<CreatePost>
    {
        public DraftValidator()
        {
            RuleFor(x => x.Author).NotNull().WithMessage("É obrigatório informar um usuário.");
        }
    }
    public class Draft : IRequestHandler<CreatePost, Response>
    {
        private readonly IPosts _posts;
        public Draft(IPosts posts) => _posts = posts;
        public async Task<Response> Handle(CreatePost command, CancellationToken cancellationToken = default)
        {
            try
            {
                Domain.Recipes.Author author = new(
                    avatarImgUrl: command.Author.AvatarImgUrl,
                    name: command.Author.Name,
                    userId: command.Author.UserId);

                Post post = new(author);

                _posts.Add(post);

                await _posts.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                return new Response(Message: "Rascunho criado.", Data: post);
            }
            catch (Exception e)
            {
                return new Response(e.Message, false);
            }
        }
    }
}
