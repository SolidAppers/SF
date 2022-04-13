using System;
using SF.Core.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels;
using SF.Core.Utilities.IoC;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SF.Core.CrossCuttingConcerns.Logging.Serilog.Loggers
{
    public class OracleLogger : LoggerServiceBase
    {
        public OracleLogger()
        {
            var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();

            var logConfig = configuration.GetSection("SeriLogConfigurations:OracleConfiguration")
                                .Get<OracleConfiguration>() ??
                            throw new Exception("SeriLogConfigurations:OracleConfiguration section is missing!");
            // TODO: Not yet supported by netstandard 2.1

            // var seriLogConfig = new LoggerConfiguration()
            // .MinimumLevel.Verbose()
            // .WriteTo.Oracle(cfg =>
            // cfg.WithSettings(connectionString: logConfig.ConnectionString, "Logs")
            // .UseBurstBatch()
            // .CreateSink())
            // .CreateLogger();

            // Logger = seriLogConfig;
        }
    }
}