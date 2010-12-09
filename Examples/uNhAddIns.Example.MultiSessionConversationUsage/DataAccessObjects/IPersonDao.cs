using System.Collections.Generic;
using uNhAddIns.Example.MultiSessionConversationUsage.Entities;

namespace uNhAddIns.Example.MultiSessionConversationUsage.DataAccessObjects
{
	public interface IPersonDao : ICrudDao<Person>
	{
		IList<Person> GetAll();
		IList<Person> GetByDocumentStart(int start);
	}
}