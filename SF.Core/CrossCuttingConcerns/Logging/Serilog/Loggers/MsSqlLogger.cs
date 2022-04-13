using System;
using SF.Core.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels;
using SF.Core.Utilities.IoC;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace SF.Core.CrossCuttingConcerns.Logging.Serilog.Loggers
{
    public class MsSqlLogger : LoggerServiceBase
    {
        public MsSqlLogger()
        {
            var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();

            var logConfig = configuration.GetSection("SeriLogConfigurations:MsSqlConfiguration")
                                .Get<MsSqlConfiguration>() ??
                            throw new Exception("SeriLogConfigurations:MsSqlConfiguration section is missing!");
            var sinkOpts = new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true };

            var columnOpts = new ColumnOptions();
            columnOpts.Store.Remove(StandardColumn.Message);
            columnOpts.Store.Remove(StandardColumn.Properties);

            var seriLogConfig = new LoggerConfiguration()
                .WriteTo.MSSqlServer(connectionString: logConfig.ConnectionString, sinkOptions: sinkOpts, columnOptions: columnOpts)
                .CreateLogger();

            Logger = seriLogConfig;
        }
    }
}