using System.Collections.Generic;
using uNhAddIns.Example.MultiSessionConversationUsage.Entities;

namespace uNhAddIns.Example.MultiSessionConversationUsage.BusinessLogic
{
	public interface IAnimalCrudModel
	{
		IList<Animal> GetAll();
		Animal Save(Animal entity);
		void Delete(Animal entity);
		void AcceptAll();
		void CancelAll();
	}
}