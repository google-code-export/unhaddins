using System.Collections.Generic;
using uNhAddIns.Example.MultiSessionConversationUsage.Entities;

namespace uNhAddIns.Example.MultiSessionConversationUsage.DataAccessObjects
{
	public interface IAnimalDao : ICrudDao<Animal>
	{
		IList<Animal> GetAll();
	}
}