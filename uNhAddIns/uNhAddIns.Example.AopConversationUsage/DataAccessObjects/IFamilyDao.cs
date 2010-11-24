using System.Collections.Generic;
using uNhAddIns.Example.AopConversationUsage.Entities;

namespace uNhAddIns.Example.AopConversationUsage.DataAccessObjects
{
	public interface IFamilyDao<TAnimal> : ICrudDao<Family<TAnimal>> where TAnimal : Animal
	{
		IList<Family<TAnimal>> WhereTheFatherIs(TAnimal father);
		IList<Family<TAnimal>> GetAll();
	}
}