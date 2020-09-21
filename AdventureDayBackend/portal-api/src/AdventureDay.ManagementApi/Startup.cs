using AdventureDay.ManagementApi.Data;
using AdventureDay.ManagementApi.Helpers;
using AdventureDay.ManagementApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;
using Serilog;

namespace AdventureDay.ManagementApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Log.Logger = new LoggerConfiguration()
           .ReadFrom.Configuration(Configuration.GetSection("Logging"))
           .WriteTo.Console() // Always write to console!
           .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.Configure<AppSettings>(Configuration.GetSection("Authentication"));
            services.AddScoped<ITeamDataService, TeamManagementService>();
            services.AddControllers().AddNewtonsoftJson(); // Support Dictionaries!
            services.AddDbContext<AdventureDayBackendDbContext>(options =>
                options.UseSqlServer(AppSettings.GetConnectionString(this.Configuration))
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Redirection handled via Ingress.
            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapMetrics();
            });

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var teamManagementContext = serviceScope.ServiceProvider.GetRequiredService<AdventureDayBackendDbContext>();
                teamManagementContext.Database.EnsureCreated();
            }
        }
    }
}
