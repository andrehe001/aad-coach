using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AdventureDay.DataModel;
using AdventureDay.Runner.Utils;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Prometheus;
using Serilog;

namespace AdventureDay.Runner
{
    public static class Program
    {
        public static IConfiguration Configuration { get; private set; }
        
        public static async Task<int> Main(string[] args)
        {
            var runnerConfiguration = new RunnerConfiguration();
            Configuration = runnerConfiguration.Configuration;
            
            var server = new MetricServer(hostname: "localhost", port: Configuration.GetValue<int>("PrometheusPort", 9090));
            server.Start();
            
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .WriteTo.Console() // Always write to console!
                .CreateLogger();
            
            await RunLoop(runnerConfiguration.Configuration);
            return 0;
        }

        private static async Task RunLoop(IConfiguration configuration)
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, args) =>
            {
                args.Cancel = true;
                Console.WriteLine("Running - Press CTRL+C to stop");
                
                // ReSharper disable once AccessToDisposedClosure | endless loop follows.
                cancellationTokenSource.Cancel();
            };

            var container = ConfigureContainer(configuration);
            
            var engine = container.Resolve<RunnerEngine>();

            try
            {
                await engine.Run(cancellationTokenSource.Token);
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Cancelled by user.");
            }
        }

        private static Autofac.IContainer ConfigureContainer(IConfiguration configuration)
        {
            var builder = new ContainerBuilder();
 
            builder.RegisterInstance(configuration).As<IConfiguration>();
            
            // DB Context per Lifetime Scope (multithreading!)
            var optionsBuilder = new DbContextOptionsBuilder<AdventureDayBackendDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DbConnection"));
            builder.RegisterInstance(optionsBuilder.Options).As<DbContextOptions<AdventureDayBackendDbContext>>();
            builder.RegisterType<AdventureDayBackendDbContext>().InstancePerLifetimeScope();
            builder.RegisterType<RunnerEngine>();
            
            var container = builder.Build();
            return container;
        }
    }
}
