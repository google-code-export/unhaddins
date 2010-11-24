using System.Collections;
using System.Data;
using System.Linq;
using Iesi.Collections;
using NHibernate.Connection;

namespace ChinookMediaManager.Data.Test.BaseClasses
{
	/// <summary>
	/// Ported from NH oficial tests.
	/// This connection provider keeps a list of all open connections,
	/// it is used when testing to check that tests clean up after themselves.
	/// </summary>
	public class DebugConnectionProvider : DriverConnectionProvider
	{
		private readonly ISet connections = new ListSet();

		public bool HasOpenConnections
		{
			get
			{
				// check to see if all connections that were at one point opened
				// have been closed through the CloseConnection
				// method
				if (connections.IsEmpty)
				{
					// there are no connections, either none were opened or
					// all of the closings went through CloseConnection.
					return false;
				}
				// Disposing of an ISession does not call CloseConnection (should it???)
				// so a Diposed of ISession will leave an IDbConnection in the list but
				// the IDbConnection will be closed (atleast with MsSql it works this way).
				return connections.Cast<IDbConnection>().Any(conn => conn.State != ConnectionState.Closed);

				// all of the connections have been Disposed and were closed that way
				// or they were Closed through the CloseConnection method.
			}
		}

		public override IDbConnection GetConnection()
		{
			IDbConnection connection = base.GetConnection();
			connections.Add(connection);
			return connection;
		}

		public override void CloseConnection(IDbConnection conn)
		{
			base.CloseConnection(conn);
			connections.Remove(conn);
		}

		public void CloseAllConnections()
		{
			while (!connections.IsEmpty)
			{
				IEnumerator en = connections.GetEnumerator();
				en.MoveNext();
				CloseConnection(en.Current as IDbConnection);
			}
		}
	}
}