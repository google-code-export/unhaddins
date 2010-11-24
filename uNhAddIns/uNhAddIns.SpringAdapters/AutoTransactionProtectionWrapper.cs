using NHibernate;
using uNhAddIns.SessionEasier;

namespace uNhAddIns.SpringAdapters
{
	public class AutoTransactionProtectionWrapper : TransactionProtectionWrapper
	{
		private ITransaction autoTransaction;

		public AutoTransactionProtectionWrapper(ISession realSession, SessionCloseDelegate closeDelegate) : base(realSession, closeDelegate)
		{}

		public AutoTransactionProtectionWrapper(ISession realSession, SessionCloseDelegate closeDelegate, SessionDisposeDelegate disposeDelegate) : base(realSession, closeDelegate, disposeDelegate)
		{}

		public override object Invoke(System.Reflection.MethodBase method, object[] args)
		{
			var result = base.Invoke(method, args);
			if (autoTransaction != null)
			{
				autoTransaction.Commit();
				autoTransaction.Dispose();
				autoTransaction = null;
			}
			return result;
		}

		protected override bool HandleMissingTransaction(string methodName)
		{
			autoTransaction = realSession.BeginTransaction();
			return true;
		}
	}
}