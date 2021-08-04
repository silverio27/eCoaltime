using System;
using Posts.Domain.SeedWork;

namespace Posts.Domain.Recipes
{
    public class Author : ValueObject
    {
        public Author(string avatarImgUrl, string name, Guid userId)
        {
            if (userId == Guid.Empty)
                throw new PostException("Usu�rio inv�lido.");

            if (string.IsNullOrEmpty(avatarImgUrl))
                throw new PostException("� obrigat�rio uma imagem para identificar o usu�rio.");

            if (string.IsNullOrEmpty(name))
                throw new PostException("� obrigat�rio um nome para identificar o usu�rio.");

            AvatarImgUrl = avatarImgUrl;
            Name = name;
            UserId = userId;
        }

        public string AvatarImgUrl { get; private set; }
        public string Name { get; private set; }
        public Guid UserId { get; private set; }
    }
}