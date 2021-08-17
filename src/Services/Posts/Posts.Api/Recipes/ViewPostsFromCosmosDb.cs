using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Posts.Api.SeedWork;
using Posts.Domain.Recipes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Posts.Api.Recipes
{

    public class ViewPostsFromCosmosDb : IViewPosts
    {
        private readonly Container _container;

        public ViewPostsFromCosmosDb(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }
        public async Task<dynamic> Get(PaginatePost parameters, string urlBase)
        {
            var options = new QueryRequestOptions()
            {
                PartitionKey = new(parameters.AuthorId.ToString())
            };
            var querycount = new QueryDefinition($@"select value count(1) from c where c.author.userId like @authorId")
                .WithParameter("@authorId", parameters.AuthorId);
            int count = 0;
            using FeedIterator<int> resultCountIterator = _container.GetItemQueryIterator<int>(querycount, requestOptions: options);
            while (resultCountIterator.HasMoreResults)
            {
                FeedResponse<int> responseCount = await resultCountIterator.ReadNextAsync();
                count = responseCount.FirstOrDefault();
            }
            var results = new List<Post>();
            var query = new QueryDefinition(@$"SELECT * FROM c 
                  where c.author.userId like @authorId
                  order by c.lastModified desc
                  offset @offset limit @limit")
                .WithParameter("@authorId", parameters.AuthorId)
                .WithParameter("@offset", parameters.PageNumber)
                .WithParameter("@limit", parameters.PageSize);

            using FeedIterator<Post> resultSetIterator = _container.GetItemQueryIterator<Post>(query, requestOptions: options);
            while (resultSetIterator.HasMoreResults)
            {
                FeedResponse<Post> responseList = await resultSetIterator.ReadNextAsync();
                results.AddRange(responseList);
            }

            var response = new PagedList<Post>(results, count, parameters.PageNumber, parameters.PageSize);

            return response;
        }
    }
}
