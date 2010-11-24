using System;
using System.ComponentModel;
using System.Linq;
using Castle.DynamicProxy;
using uNhAddIns.Adapters;

namespace uNhAddIns.ComponentBehaviors.Castle
{
	[Behavior(0, typeof(IDataErrorInfo))]
    public class DataErrorInfoBehavior : IInterceptor
    {
        private readonly IEntityValidator _entityValidator;

        public DataErrorInfoBehavior(IEntityValidator entityValidator)
        {
            _entityValidator = entityValidator;
        }

        #region IInterceptor Members

        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.DeclaringType.Equals(typeof (IDataErrorInfo)))
            {
                if ("get_Item".Equals(invocation.Method.Name))
                {
                    string[] invalidvalues = _entityValidator
                        .Validate(invocation.Proxy, (string) invocation.Arguments[0])
                        .Select(iv => iv.Message)
                        .ToArray();

                    invocation.ReturnValue = string.Join(Environment.NewLine, invalidvalues);
                }
                else if ("get_Error".Equals(invocation.Method.Name))
                {
                    string[] invalidValues = _entityValidator.Validate(invocation.Proxy)
                        .Select(iv => iv.Message)
                        .ToArray();

                    invocation.ReturnValue = string.Join(Environment.NewLine, invalidValues);
                }
            }
            else
            {
                invocation.Proceed();
            }
        }

        #endregion

    }
}