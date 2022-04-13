using System.Transactions;
using Castle.DynamicProxy;
using SF.Core.Utilities.Interceptors;

namespace SF.Core.Aspects.Autofac.Transaction
{
    public class TransactionScopeAspect : MethodInterception
    {
        public override void Intercept(IInvocation invocation)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {

                invocation.Proceed();
                transactionScope.Complete();

            }
        }
    }
}
