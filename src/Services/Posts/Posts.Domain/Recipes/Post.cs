using Posts.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Posts.Domain.Recipes
{
    public class Post : Entity, IAggregateRoot
    {
        public Post(Author author)
        {
            Title = "Nova receita";

            if (author is null)
                throw new PostException("É obrigatório informar quem está fazendo a postagem.");

            Author = author;
            _pictures = new List<Picture>();
            IsDraft = true;
            SetLastModified();
        }


        public string Title { get; private set; }
        public string SubTitle { get; private set; }
        public string Description { get; private set; }
        public Author Author { get; private set; }
        private readonly List<Picture> _pictures;
        public IReadOnlyCollection<Picture> Pictures => _pictures;
        public bool IsDraft { get; private set; }
        public const int MaximumPictures = 5;
        public DateTime LastModified { get; private set; }
        public DateTime? PublishedAt { get; private set; }

        public void SetTitle(string title)
        {
            Title = title;
            SetLastModified();
        }

        public void SetSubTitle(string subTitle)
        {
            SubTitle = subTitle;
            SetLastModified();
        }

        public void SetDescription(string description)
        {
            Description = description;
            SetLastModified();
        }
        public void AddPicture(Picture picture)
        {
            if (_pictures.Count == MaximumPictures)
                throw new PostException($"Um post não pode ter mais de {MaximumPictures} fotos.");

            if (picture.Cover) _pictures.ForEach(x => x.MarkAsNotCover());
            _pictures.Add(picture);
            SetLastModified();
        }
        public void RemovePicture(Picture picture)
        {
            if (!IsDraft && _pictures.Count == 1)
                throw new PostException($"Não é possível remover todas as imagens de um post publicado.");

            _pictures.Remove(picture);
            if (!_pictures.Any(x => x.Cover) && _pictures.Any())
                _pictures.FirstOrDefault().MarkAsCover();

            SetLastModified();
        }

        public void Publish()
        {
            if (IsAlreadyToPublish())
            {
                IsDraft = false;
                PublishedAt = DateTime.Now;
                return;
            }

            throw new PostException("É obrigatório informar, título, subtítulo, descrição e ter ao menos uma foto para publicar.");
        }

        private bool IsAlreadyToPublish() =>
            !string.IsNullOrEmpty(Title) &&
            !string.IsNullOrEmpty(SubTitle) &&
            !string.IsNullOrEmpty(Description) &&
            _pictures.Any();

        private void SetLastModified() => LastModified = DateTime.Now;
    }
}