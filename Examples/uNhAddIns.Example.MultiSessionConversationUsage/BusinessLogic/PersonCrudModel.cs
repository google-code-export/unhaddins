using System;
using System.Collections.Generic;
using uNhAddIns.Example.MultiSessionConversationUsage.DataAccessObjects;
using uNhAddIns.Example.MultiSessionConversationUsage.Entities;
using uNhAddIns.Adapters;

namespace uNhAddIns.Example.MultiSessionConversationUsage.BusinessLogic
{
	[PersistenceConversational]
	public class PersonCrudModel : IPersonCrudModel 
	{
		private readonly IPersonDao personDao;

		public PersonCrudModel(IPersonDao personDao)
		{
			this.personDao = personDao;
		}

		[PersistenceConversation]
		public IList<Person> GetAllPersons()
		{
			return personDao.GetAll();
		}

		[PersistenceConversation]
		public Person Save(Person entity)
		{
			return personDao.MakePersistent(entity);
		}

		[PersistenceConversation]
		public void Delete(Person entity)
		{
			personDao.MakeTransient(entity);
		}

		[PersistenceConversation(ConversationEndMode = EndMode.End)]
		public void AcceptAll() { }

		[PersistenceConversation(ConversationEndMode = EndMode.Abort)]
		public void CancelAll() { }

		[PersistenceConversation]
		public IList<Person> GetPersonsWithDocumentStartingWith(int documentStart)
		{
			return personDao.GetByDocumentStart(documentStart);
		}
	}
}