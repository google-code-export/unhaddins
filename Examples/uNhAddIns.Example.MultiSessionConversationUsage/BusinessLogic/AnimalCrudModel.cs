using System;
using System.Collections.Generic;
using uNhAddIns.Example.MultiSessionConversationUsage.DataAccessObjects;
using uNhAddIns.Example.MultiSessionConversationUsage.Entities;
using uNhAddIns.Adapters;

namespace uNhAddIns.Example.MultiSessionConversationUsage.BusinessLogic
{
	[PersistenceConversational]
	public class AnimalCrudModel : IAnimalCrudModel
	{
		private readonly IAnimalDao animalDao;

		public AnimalCrudModel(IAnimalDao animalDao)
		{
			this.animalDao = animalDao;
		}

		[PersistenceConversation]
		public IList<Animal> GetAll()
		{
			return animalDao.GetAll();
		}

		[PersistenceConversation]
		public Animal Save(Animal entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			return animalDao.MakePersistent(entity);
		}

		[PersistenceConversation]
		public void Delete(Animal entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			animalDao.MakeTransient(entity);
		}

		[PersistenceConversation(ConversationEndMode = EndMode.End)]
		public void AcceptAll() { }

		[PersistenceConversation(ConversationEndMode = EndMode.Abort)]
		public void CancelAll() { }
	}
}