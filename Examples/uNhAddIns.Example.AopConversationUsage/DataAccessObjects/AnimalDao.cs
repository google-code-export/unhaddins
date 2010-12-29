using System.Collections.Generic;
using NHibernate;
using uNhAddIns.Example.AopConversationUsage.Entities;

namespace uNhAddIns.Example.AopConversationUsage.DataAccessObjects
{
	public class AnimalDao<TAnimal> : BaseCrudDao<TAnimal>, IAnimalReadOnlyDao<TAnimal> where TAnimal : Animal
	{
		public AnimalDao(ISessionFactory factory) : base(factory) {}

		#region Implementation of IAnimalReadOnlyDao<TAnimal>

		public IList<TAnimal> GetAll()
		{
			return factory.GetCurrentSession().CreateCriteria(typeof (TAnimal)).List<TAnimal>();
		}

		#endregion
	}
}