
using Posts.Domain.Recipes;
using System;

namespace Posts.UnitTests.Recipes
{
    public static class Builder
    {
       public static readonly Author ValidUser = new(
        avatarImgUrl: "https://www.silverio.dev.br/assets/profile-pic.jpeg",
        name: "Lucas Silvério",
        userId: new Guid("6d0bdd60-912f-4f75-a69a-5c19e8d99eef"));

        public static  Post CreatePost() => new(ValidUser);
    }
}
