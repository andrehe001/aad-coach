using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace AdventureDay.Runner.Utils
{
    public class RunnerConfiguration
    {
        private readonly Lazy<IConfiguration> _lazyConfiguration;

        public RunnerConfiguration()
        {
            _lazyConfiguration = new Lazy<IConfiguration>(BuildConfiguration);
        }
        
        #region Configuration

        public IConfiguration Configuration => _lazyConfiguration.Value;

        private IConfiguration BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var config = builder.Build();
            return config;
        }

        #endregion
    }

}