using Posts.Domain.Recipes;
using Posts.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace Posts.Infra.DataCosmosDB
{
    public class Posts : IPosts
    {
        private readonly Container _container;

        public Posts(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task<Post> Add(Post post) => await _container.CreateItemAsync(post, new PartitionKey(post.Author.UserId.ToString()));

        public async Task Delete(Post post) => await _container.DeleteItemAsync<Post>(post.Id.ToString(), new(post.Author.UserId.ToString()));

        public async Task<Post> GetAsync(Guid postId, Guid authorId) => (await _container
            .ReadItemAsync<Post>(postId.ToString(), new(authorId.ToString()))).Resource;

        public async Task Update(Post post) => await _container.UpsertItemAsync(post, new(post.Author.UserId.ToString()));
    }
}
