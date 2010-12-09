using System;
using System.Collections.Generic;
using uNhAddIns.Adapters;
using uNhAddIns.Example.AopConversationUsage.DataAccessObjects;
using uNhAddIns.Example.AopConversationUsage.Entities;

namespace uNhAddIns.Example.AopConversationUsage.BusinessLogic
{
	[PersistenceConversational]
	public class FamilyCrudModel<TAnimal> : IFamilyCrudModel<TAnimal> where TAnimal : Animal
	{
		private readonly IAnimalReadOnlyDao<TAnimal> animalDao;
		private readonly IFamilyDao<TAnimal> familyDao;

		public FamilyCrudModel(IDaoFactory factory)
		{
			animalDao = factory.GetDao<IAnimalReadOnlyDao<TAnimal>>();
			familyDao = factory.GetDao<IFamilyDao<TAnimal>>();
		}

		#region Implementation of IFamilyCrudModel<TAnimal>

		[PersistenceConversation]
		public IList<TAnimal> GetExistingComponentsList()
		{
			return animalDao.GetAll();
		}

		[PersistenceConversation]
		public IList<Family<TAnimal>> GetEntirelyList()
		{
			return familyDao.GetAll();
		}

		[PersistenceConversation]
		public Family<TAnimal> GetIfAvailable(int id)
		{
			return familyDao.Get(id);
		}

		[PersistenceConversation]
		public Family<TAnimal> Save(Family<TAnimal> entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			return familyDao.MakePersistent(entity);
		}

		[PersistenceConversation]
		public void Delete(Family<TAnimal> entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			familyDao.MakeTransient(entity);
		}

		[PersistenceConversation(ConversationEndMode = EndMode.End)]
		public void AcceptAll() { }

		[PersistenceConversation(ConversationEndMode = EndMode.Abort)]
		public void CancelAll() { }

		#endregion
	}
}