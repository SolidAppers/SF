using System;
using SF.Core.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels;
using SF.Core.Utilities.IoC;
using SF.Core.Utilities.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace SF.Core.CrossCuttingConcerns.Logging.Serilog.Loggers
{
    public class MsTeamsLogger : LoggerServiceBase
    {
        public MsTeamsLogger()
        {
            var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();

            var logConfig = configuration.GetSection("SeriLogConfigurations:MsTeamsConfiguration")
                                .Get<MsTeamsConfiguration>() ??
                            throw new Exception("SeriLogConfigurations:MsTeamsConfiguration section is missing!");

            Logger = new LoggerConfiguration()
                .WriteTo.MicrosoftTeams(logConfig.ChannelHookAddress)
                .CreateLogger();
        }
    }
}