using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using FluentValidation;
using SF.Core.CrossCuttingConcerns.Validation;
using SF.Core.Utilities.Interceptors;
using SF.Core.Utilities.Messages;

namespace SF.Core.Aspects.Autofac.Validation
{
    public class ValidationAspect : MethodInterception
    {
        private Type _validatorType;
        public ValidationAspect(Type validatorType)
        {
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
            {
                throw new System.Exception(AspectMessages.WrongValidationType);
            }

            _validatorType = validatorType;
        }
        protected override void OnBefore(IInvocation invocation)
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType);
            var entityType = _validatorType.BaseType.GetGenericArguments()[0];
            var entities = invocation.Arguments.Where(t => t?.GetType() == entityType);

            foreach (var entity in entities)
            {
                if (validator.CanValidateInstancesOfType(entity.GetType()))
                {
                    ValidationTool.Validate(validator, entity);
                }
            }


        }
    }


    public class ValidationAspectList : MethodInterception
    {
        private List<Type> _validatorTypeList;
        public ValidationAspectList(List<Type> validatorTypeList)
        {
            foreach (var vType in validatorTypeList)
            {
                if (!typeof(IValidator).IsAssignableFrom(vType))
                {
                    throw new System.Exception(AspectMessages.WrongValidationType);
                }
            }

            _validatorTypeList = validatorTypeList;
        }
        protected override void OnBefore(IInvocation invocation)
        {
            foreach (var vType in _validatorTypeList) {
                var validator = (IValidator)Activator.CreateInstance(vType);
                var entityType = vType.BaseType.GetGenericArguments()[0];
                var entities = invocation.Arguments.Where(t => t?.GetType() == entityType);

                foreach (var entity in entities)
                {
                    if (validator.CanValidateInstancesOfType(entity.GetType()))
                    {
                        ValidationTool.Validate(validator, entity);
                    }
                }

            }

        }
    }
}
