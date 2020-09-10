using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NJsonSchema;
using RPSLSGameHub.GameEngine.WebApi.Models;
using RPSLSGameHub.GameEngine.WebApi.Services;

namespace AzureGameDay.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            services.AddControllers().AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            
            services.AddTransient<MatchService>();

            services.AddDbContext<MatchDBContext>(opt =>
               opt.UseSqlServer(Configuration.GetConnectionString("GameEngineDB")));

            services.AddDistributedRedisCache(opt =>
                opt.Configuration = Configuration.GetConnectionString("GameEngineRedis")
            );
            services.AddOpenApiDocument(s =>
            {
                s.Title = "Azure Adventure Day - API";
            }); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
            
            app.UseOpenApi(); 
            app.UseSwaggerUi3();
            app.UseReDoc();
        }
    }
}
