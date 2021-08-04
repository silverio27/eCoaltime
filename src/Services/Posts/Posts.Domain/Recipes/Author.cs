using System;
using Posts.Domain.SeedWork;

namespace Posts.Domain.Recipes
{
    public class Author : ValueObject
    {
        public Author(string avatarImgUrl, string name, Guid userId)
        {
            if (userId == Guid.Empty)
                throw new PostException("Usuário inválido.");

            if (string.IsNullOrEmpty(avatarImgUrl))
                throw new PostException("É obrigatório uma imagem para identificar o usuário.");

            if (string.IsNullOrEmpty(name))
                throw new PostException("É obrigatório um nome para identificar o usuário.");

            AvatarImgUrl = avatarImgUrl;
            Name = name;
            UserId = userId;
        }

        public string AvatarImgUrl { get; private set; }
        public string Name { get; private set; }
        public Guid UserId { get; private set; }
    }
}