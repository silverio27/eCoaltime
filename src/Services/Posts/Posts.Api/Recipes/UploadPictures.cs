using Posts.Api.SeedWork;
using Posts.Domain.Recipes;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Posts.Api.Recipes
{
    public record Picture(bool Cover, string Base64image);
    public record AddPictures(Guid PostId, IList<Picture> Pictures) : IRequest<Response>;
    public class PicturesValidator : AbstractValidator<AddPictures>
    {
        public PicturesValidator()
        {
            RuleForEach(x => x.Pictures).Must(x => IsBase64String(x.Base64image)).WithMessage("Uma das imagens não é válida.");
            RuleFor(x => x.Pictures).Must(x => x.Count <= Post.MaximumPictures).WithMessage($"O post não pode ter mais de {Post.MaximumPictures} fotos.");
        }
        private static bool IsBase64String(string imageBase64)
        {
            imageBase64 = imageBase64.Trim();
            return (imageBase64.Length % 4 == 0) && Regex.IsMatch(imageBase64, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }
    }

    public class UploadPictures : IRequestHandler<AddPictures, Response>
    {
        private readonly IPosts _posts;
        private readonly IPictures _pictures;
        public UploadPictures(IPosts posts, IPictures pictures)
        {
            _posts = posts;
            _pictures = pictures;
        }
        public async Task<Response> Handle(AddPictures command, CancellationToken cancellationToken)
        {
            try
            {
                var post = await _posts.GetAsync(command.PostId);
                if (post is null)
                    return new("O Post a ser atualizado não existe", false);

                if (post.Pictures.Count >= Post.MaximumPictures)
                    return new($"O Post já possui {Post.MaximumPictures}, remova uma imagem para adicionar uma nova.");

                foreach (var picture in command.Pictures)
                {
                    string name = await _pictures.UploadAsync(picture.Base64image);
                    Domain.Recipes.Picture photo = new(picture.Cover, name);
                    post.AddPicture(photo);
                }

                _posts.Update(post);
                await _posts.UnitOfWork.SaveEntitiesAsync(default);

                return new("Post atualizado");
            }
            catch (Exception e)
            {

                return new(e.Message, false);
            }
        }
    }
}
