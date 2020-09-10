using AdventureDayRunner.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AdventureDayRunnerAPI
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
            // Important for all Enum Serializations!
            BsonSerializerSettings.Configure();
            
            services.AddHealthChecks();
            services.AddControllers().AddNewtonsoftJson();
            services.AddSingleton(new AdventureDayPropertiesRepository(
                new AdventureDayDatabaseSettings()
                {
                    ConnectionString = Configuration.GetConnectionString("DbConnection"),
                    DatabaseName = Configuration.GetSection("Parameter").GetSection("DbName").Value,
                    CollectionName = Configuration.GetSection("Parameter").GetSection("DbCollectionName").Value
                }));
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
        }
    }
}
