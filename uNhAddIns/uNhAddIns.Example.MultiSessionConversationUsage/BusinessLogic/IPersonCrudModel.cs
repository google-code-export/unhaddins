using System.Collections;
using System.Collections.Generic;
using uNhAddIns.Example.MultiSessionConversationUsage.Entities;

namespace uNhAddIns.Example.MultiSessionConversationUsage.BusinessLogic
{
	public interface IPersonCrudModel
	{
		IList<Person> GetAllPersons();
		Person Save(Person entity);
		void Delete(Person entity);
		void AcceptAll();
		void CancelAll();
		IList<Person> GetPersonsWithDocumentStartingWith(int documentStart);
	}
}