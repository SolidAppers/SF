using SF.Core.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels;
using SF.Core.Utilities.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;

namespace SF.Core.CrossCuttingConcerns.Logging.Serilog.Loggers
{
    public class MongoDbLogger : LoggerServiceBase
    {
        public MongoDbLogger()
        {
            var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
            var logConfig = configuration.GetSection("SeriLogConfigurations:MongoDbConfiguration").Get<MongoDbConfiguration>()
                                ??
                            throw new Exception("SeriLogConfigurations:MongoDbConfiguration section is missing!");
                
        
            Logger = new LoggerConfiguration().WriteTo.MongoDB(logConfig.ConnectionString, collectionName: logConfig.Collection)
                .CreateLogger();
        }
    }
}