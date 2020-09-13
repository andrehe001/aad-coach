using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using AdventureDayRunner.Utils;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using team_management_api.Data;
using IContainer = Autofac.IContainer;

namespace AdventureDayRunner
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            
            await RunLoop();
            return 0;
        }

        private static async Task RunLoop()
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, args) =>
            {
                args.Cancel = true;
                Console.WriteLine("Running - Press CTRL+C to stop");
                
                // ReSharper disable once AccessToDisposedClosure | endless loop follows.
                cancellationTokenSource.Cancel();
            };

            var runnerConfiguration = new RunnerConfiguration();
            var container = ConfigureContainer(runnerConfiguration.Configuration);
            
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

        private static IContainer ConfigureContainer(IConfiguration configuration)
        {
            var builder = new ContainerBuilder();
 
            builder.RegisterType<RandomNameGenerator>().SingleInstance();
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
