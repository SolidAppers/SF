using System.Linq;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using SF.Core.CrossCuttingConcerns.Caching;
using SF.Core.Utilities.Interceptors;
using SF.Core.Utilities.IoC;

namespace SF.Core.Aspects.Autofac.Caching
{
    public class CacheRemoveAspect : MethodInterception
    {
        private string _pattern;
        private ICacheManager _cacheManager;

        /// <summary>
        /// Removes all cache in same namespace if pattern=""
        /// </summary>
        /// <param name="pattern"></param>
        public CacheRemoveAspect(string pattern="")
        {

      
            _pattern = pattern;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }

        protected override void OnSuccess(IInvocation invocation)
        {
            if (string.IsNullOrEmpty(_pattern))
            {
                _pattern = $"{invocation.Method.ReflectedType.FullName}.*";
            }

            _cacheManager.RemoveByPattern(_pattern);
        }
    }
}
