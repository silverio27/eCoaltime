
using Posts.Api.Recipes;
using Posts.Api.SeedWork;
using Posts.Domain.Recipes;

using Posts.Infra.Storage;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace Posts.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {

            services.AddOptions();
            services.Configure<StorageSettings>(Configuration.GetSection("PictureSettings"));


            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            services.AddControllers(x => x.Filters.Add(typeof(ValidatorActionFilter)))
                    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Startup>());

  

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Posts.Api", Version = "v1" });
            });

            services.AddSingleton<IPosts>(InitializePosts(Configuration.GetSection("CosmosDb")).GetAwaiter().GetResult());
            services.AddSingleton<IViewPosts>(InitializeQueries(Configuration.GetSection("CosmosDb")).GetAwaiter().GetResult());

            services.AddMediatR(typeof(Startup));
            services.AddTransient<IPictures, Pictures>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Posts.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static async Task<Infra.DataCosmosDB.Posts> InitializePosts(IConfigurationSection configuration)
        {
            var databaseName = configuration["DatabaseName"];
            var containerName = "Posts";
            var account = configuration["Account"];
            var key = configuration["Key"];

            var options = new CosmosClientOptions()
            {
                SerializerOptions = new CosmosSerializationOptions()
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            };

            CosmosClient client = new(account, key, options);
            var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/author/userId");
            Infra.DataCosmosDB.Posts posts = new(client, databaseName, containerName);
            return posts;
        }

        private static async Task<ViewPostsFromCosmosDb> InitializeQueries(IConfigurationSection configuration)
        {
            var databaseName = configuration["DatabaseName"];
            var containerName = "Posts";
            var account = configuration["Account"];
            var key = configuration["Key"];

            var options = new CosmosClientOptions()
            {
                SerializerOptions = new CosmosSerializationOptions()
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            };

            CosmosClient client = new(account, key, options);
            var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/author/userId");
            ViewPostsFromCosmosDb posts = new(client, databaseName, containerName);
            return posts;
        }
    }
}
