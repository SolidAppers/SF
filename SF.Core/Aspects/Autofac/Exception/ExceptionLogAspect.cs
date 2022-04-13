using System;
using System.Linq;
using Castle.DynamicProxy;
using SF.Core.CrossCuttingConcerns.Logging;
using SF.Core.CrossCuttingConcerns.Logging.Serilog;
using SF.Core.Utilities.Interceptors;
using SF.Core.Utilities.IoC;
using SF.Core.Utilities.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SF.Core.Utilities.Security.Web;
using System.Threading;
using FluentValidation;
using SF.Core.Extensions;

namespace SF.Core.Aspects.Autofac.Exception
{
    public class ExceptionLogAspect:MethodInterception
    {
        private readonly LoggerServiceBase _loggerServiceBase;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExceptionLogAspect(Type loggerService)
        {
            if (loggerService.BaseType != typeof(LoggerServiceBase))
            {
                throw new ArgumentException(AspectMessages.WrongLoggerType);
            }

            _loggerServiceBase = (LoggerServiceBase)Activator.CreateInstance(loggerService);
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }

        protected override void OnException(IInvocation invocation, System.Exception e)
        {
            if (e.GetType() != typeof(ValidationException) && e.GetType() != typeof(NotificationException))
            {
                var logDetailWithException = GetLogDetail(invocation);

                logDetailWithException.ExceptionMessage = e is AggregateException
                    ? string.Join(Environment.NewLine, (e as AggregateException).InnerExceptions.Select(x => x.Message))
                    : e.Message;
                _loggerServiceBase.Error(JsonConvert.SerializeObject(logDetailWithException));
            }
        }

        private LogDetailWithException GetLogDetail(IInvocation invocation)
        {
            var logParameters = invocation.Arguments.Select((t, i) => new LogParameter
            {
                Name = invocation.GetConcreteMethod().GetParameters()[i].Name,
                Value = t,
                Type = t!=null?t.GetType().Name:"null"
            }).ToList();



            var logDetailWithException = new LogDetailWithException
            {
                MethodName = invocation.Method.Name,
                FullName = invocation.InvocationTarget.GetType().FullName,
                Parameters = logParameters,
                User = (_httpContextAccessor.HttpContext == null ||
                        _httpContextAccessor.HttpContext.User.Identity.Name == null)
                    ? "?"
                    : _httpContextAccessor.HttpContext.User.Identity.Name

            };

            return logDetailWithException;
        }
    }
}
