using System;
using System.Threading;
using System.Threading.Tasks;
using AdventureDayRunner.Shared;
using Serilog;

namespace AdventureDayRunner
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            
            // Important to handle enum serialization!
            BsonSerializerSettings.Configure();
            
            // TODO: real args, please ;-)
            if (args.Length > 0)
            {
                await Utils.WriteDefaultPropertiesToDb();
            }
            else
            {
                await RunLoop();
            }
            
            return 0;
        }

        private static async Task RunLoop()
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, args) =>
            {
                args.Cancel = true;
                Console.WriteLine("Running - Press CTRL+C to stop");
                cancellationTokenSource.Cancel();
            };

            var engine = new GameDayRunnerEngine();

            try
            {
                await engine.Run(cancellationTokenSource.Token);
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Cancelled by user.");
            }
        }
    }
}
