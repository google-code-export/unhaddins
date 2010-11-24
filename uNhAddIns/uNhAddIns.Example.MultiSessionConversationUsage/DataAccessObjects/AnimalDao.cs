using System.Collections.Generic;
using uNhAddIns.Example.MultiSessionConversationUsage.Entities;
using NHibernate;

namespace uNhAddIns.Example.MultiSessionConversationUsage.DataAccessObjects
{
	public class AnimalDao : BaseCrudDao<Animal>, IAnimalDao
	{
		public AnimalDao(ISessionFactory factory) : base(factory) {}

		public IList<Animal> GetAll()
		{
			return factory.GetCurrentSession().CreateCriteria<Animal>().List<Animal>();
		}
	}
}