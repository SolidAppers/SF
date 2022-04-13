using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SF.Core.CrossCuttingConcerns;
using SF.Core.CrossCuttingConcerns.Caching;
using SF.Core.CrossCuttingConcerns.Caching.Microsoft;
using SF.Core.CrossCuttingConcerns.Caching.Redis;
using SF.Core.Utilities.ExtensionMethods;
using SF.Core.Utilities.IoC;

namespace SF.Core.DependencyResolvers
{
    public class CoreModule:ICoreModule
    {
        public void Load(IServiceCollection services)
        {
            if (Config.Configuration["Cache:Distributed"].ToBoolean())
            {
                services.AddSingleton<ICacheManager, RedisCacheManager>();
                
            }
            else {
                services.AddMemoryCache();
                services.AddSingleton<ICacheManager, MemoryCacheManager>();
            }
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<Stopwatch>();
        }
    }
}
