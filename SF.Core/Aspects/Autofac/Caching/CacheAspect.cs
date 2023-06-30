using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SF.Core.CrossCuttingConcerns.Caching;
using SF.Core.Utilities.ExtensionMethods;
using SF.Core.Utilities.Interceptors;
using SF.Core.Utilities.IoC;

namespace SF.Core.Aspects.Autofac.Caching
{
    public class CacheAspect : MethodInterception
    {
        private int _duration;
        private string _key;
        private ICacheManager _cacheManager;


        public CacheAspect(int duration = 60)
        {
            _duration = duration;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();

        }

        public CacheAspect(string key,int duration = 60)
        {
            _key = key;
            _duration = duration;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();

        }
        public override void Intercept(IInvocation invocation)
        {
            if (string.IsNullOrEmpty(_key))
            {
                _key = $"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}_{invocation.Arguments.ToJson().ToMd5Hash()}";

            }

            if (_cacheManager.IsAdd(_key))
            {

                string a = _cacheManager.Get<string>(_key);
                MethodInfo method = typeof(JsonConvert).GetMethods().FirstOrDefault(method => method.Name == "DeserializeObject" & method.IsGenericMethod == true);

                MethodInfo generic = method.MakeGenericMethod(invocation.Method.ReturnParameter.ParameterType);
                invocation.ReturnValue = generic.Invoke(this, new object[] { a });

                return;
            }
            invocation.Proceed();

            _cacheManager.Add(_key, invocation.ReturnValue.ToJson(), _duration);
        }

     
    }
}
