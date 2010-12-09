using System.Collections.Generic;
using NHibernate;
using NHibernate.Engine;
using uNhAddIns.Example.AopConversationUsage.Entities;

namespace uNhAddIns.Example.AopConversationUsage.DataAccessObjects
{
	public class FamilyDao<TAnimal> : BaseCrudDao<Family<TAnimal>>, IFamilyDao<TAnimal> where TAnimal : Animal
	{
		public FamilyDao(ISessionFactory factory) : base(factory) {}

		#region Implementation of IFamilyDao<TAnimal>

		public IList<Family<TAnimal>> WhereTheFatherIs(TAnimal father)
		{
			return
				factory.GetCurrentSession().GetNamedQuery(GetEntityName() + ".ByFather").SetInt32("fatherId", father.Id).List
					<Family<TAnimal>>();
		}

		public IList<Family<TAnimal>> GetAll()
		{
			return factory.GetCurrentSession().GetNamedQuery(GetEntityName() + ".All").List<Family<TAnimal>>();
		}

		#endregion

		private string GetEntityName()
		{
			return ((ISessionFactoryImplementor) factory).TryGetGuessEntityName(typeof (Family<TAnimal>));
		}
	}
}