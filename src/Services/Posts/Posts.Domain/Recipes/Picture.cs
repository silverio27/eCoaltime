using Newtonsoft.Json;
using Posts.Domain.SeedWork;
using System;

namespace Posts.Domain.Recipes
{
    public class Picture : Entity
    {
        public Picture(bool cover, string pictureName)
        {
            if (string.IsNullOrEmpty(pictureName))
                throw new PostException("O nome para imagem não pode ser vazia.");

            Cover = cover;
            PictureName = pictureName;
        }
        protected Picture() { }
        public bool Cover { get; private set; }
        public string PictureName { get; private set; }
        public void MarkAsCover() => Cover = true;
        public void MarkAsNotCover() => Cover = false;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}