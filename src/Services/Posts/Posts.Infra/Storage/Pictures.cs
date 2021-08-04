using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Posts.Domain.Recipes;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Posts.Infra.Storage
{
    public class Pictures : IPictures
    {
        private readonly StorageSettings _settings;

        public Pictures(IOptions<StorageSettings> settings)
        {
            _settings = settings.Value;
            var container = new BlobContainerClient(_settings.ConnectionString, _settings.Container);
            container.CreateIfNotExists(PublicAccessType.BlobContainer);
        }

        public async Task<string> UploadAsync(string base64image)
        {
            string name = Guid.NewGuid().ToString() + ".jpg";

            var bytes = Convert.FromBase64String(base64image);
            using MemoryStream stream = new(bytes);
            var blobClient = new BlobClient(_settings.ConnectionString, _settings.Container, name);
            await blobClient.UploadAsync(stream);

            return name;
        }

        


    }
}
