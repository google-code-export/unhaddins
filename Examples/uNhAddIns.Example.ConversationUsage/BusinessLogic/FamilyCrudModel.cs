using System;
using System.Collections.Generic;
using uNhAddIns.Example.ConversationUsage.DataAccessObjects;
using uNhAddIns.Example.ConversationUsage.Entities;
using uNhAddIns.SessionEasier.Conversations;

namespace uNhAddIns.Example.ConversationUsage.BusinessLogic
{
	public class FamilyCrudModel<TAnimal> : PersistenceConversationalModel, IFamilyCrudModel<TAnimal>
		where TAnimal : Animal
	{
		private readonly IAnimalReadOnlyDao<TAnimal> animalDao;
		private readonly IFamilyDao<TAnimal> familyDao;

		public FamilyCrudModel(IConversationsContainerAccessor cca, IConversationFactory cf, IDaoFactory factory)
			: base(cca, cf)
		{
			animalDao = factory.GetDao<IAnimalReadOnlyDao<TAnimal>>();
			familyDao = factory.GetDao<IFamilyDao<TAnimal>>();
		}

		#region Implementation of IFamilyCrudModel<TAnimal>

		public IList<TAnimal> GetExistingComponentsList()
		{
			try
			{
				using (GetConversationCaregiver())
				{
					return animalDao.GetAll();
				}
			}
			catch (Exception e)
			{
				ManageException(e);
				throw;
			}
		}

		public IList<Family<TAnimal>> GetEntirelyList()
		{
			try
			{
				using (GetConversationCaregiver())
				{
					return familyDao.GetAll();
				}
			}
			catch (Exception e)
			{
				ManageException(e);
				throw;
			}
		}

		public Family<TAnimal> GetIfAvailable(int id)
		{
			try
			{
				using (GetConversationCaregiver())
				{
					return familyDao.Get(id);
				}
			}
			catch (Exception e)
			{
				ManageException(e);
				throw;
			}
		}

		public Family<TAnimal> Save(Family<TAnimal> entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			try
			{
				using (GetConversationCaregiver())
				{
					return familyDao.MakePersistent(entity);
				}
			}
			catch (Exception e)
			{
				ManageException(e);
				throw;
			}
		}

		public void Delete(Family<TAnimal> entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			try
			{
				using (GetConversationCaregiver())
				{
					familyDao.MakeTransient(entity);
				}
			}
			catch (Exception e)
			{
				ManageException(e);
				throw;
			}
		}

		public void AcceptAll()
		{
			try
			{
				EndPersistenceConversation();
			}
			catch (Exception e)
			{
				ManageException(e);
				throw;
			}
		}

		public void CancelAll()
		{
			try
			{
				AbortPersistenceConversation();
			}
			catch (Exception e)
			{
				ManageException(e);
				throw;
			}
		}

		#endregion
	}
}