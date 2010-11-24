using System.Collections.Generic;
using uNhAddIns.Example.AopConversationUsage.Entities;

namespace uNhAddIns.Example.AopConversationUsage.BusinessLogic
{
	public interface IFamilyCrudModel<TAnimal> where TAnimal : Animal
	{
		IList<TAnimal> GetExistingComponentsList();
		IList<Family<TAnimal>> GetEntirelyList();
		Family<TAnimal> GetIfAvailable(int id);
		Family<TAnimal> Save(Family<TAnimal> entity);
		void Delete(Family<TAnimal> entity);
		void AcceptAll();
		void CancelAll();
	}
}