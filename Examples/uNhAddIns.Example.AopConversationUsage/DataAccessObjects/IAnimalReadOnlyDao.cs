using System.Collections.Generic;
using uNhAddIns.Example.AopConversationUsage.Entities;

namespace uNhAddIns.Example.AopConversationUsage.DataAccessObjects
{
	public interface IAnimalReadOnlyDao<TAnimal> : IDao<TAnimal> where TAnimal : Animal
	{
		IList<TAnimal> GetAll();
	}
}