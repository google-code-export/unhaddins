using System.Collections.Generic;
using uNhAddIns.Example.ConversationUsage.Entities;

namespace uNhAddIns.Example.ConversationUsage.DataAccessObjects
{
	public interface IFamilyDao<TAnimal> : ICrudDao<Family<TAnimal>> where TAnimal : Animal
	{
		IList<Family<TAnimal>> WhereTheFatherIs(TAnimal father);
		IList<Family<TAnimal>> GetAll();
	}
}