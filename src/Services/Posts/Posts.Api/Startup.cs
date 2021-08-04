
using Posts.Api.Recipes;
using Posts.Api.SeedWork;
using Posts.Domain.Recipes;
//using Coaltime.Infra.DataSqlServer;
using Posts.Infra.DataCosmosDB;
using Posts.Infra.Storage;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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

            //in memory
            //services.AddDbContext<PostsContext>(opt => opt.UseInMemoryDatabase("DataBase"));
            //cosmos

            services.AddDbContext<PostsContext>(opt =>
            {
                opt.UseCosmos(connectionString:Configuration["CosmosDb:ConnectionString"], databaseName: Configuration["CosmosDb:DatabaseName"]);
            });

            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            services.AddControllers(x => x.Filters.Add(typeof(ValidatorActionFilter)))
                    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Startup>());

  

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Posts.Api", Version = "v1" });
            });

            services.AddTransient<IPosts, Infra.DataCosmosDB.Posts>();
            services.AddMediatR(typeof(Startup));
            services.AddTransient<IPictures, Pictures>();
            services.AddTransient<IViewPosts, ViewPostsFromCosmosDb>();

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
    }
}
