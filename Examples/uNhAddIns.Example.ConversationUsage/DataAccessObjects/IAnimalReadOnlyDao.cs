using System.Collections.Generic;
using uNhAddIns.Example.ConversationUsage.Entities;

namespace uNhAddIns.Example.ConversationUsage.DataAccessObjects
{
	public interface IAnimalReadOnlyDao<TAnimal> : IDao<TAnimal> where TAnimal : Animal
	{
		IList<TAnimal> GetAll();
	}
}