namespace SessionManagement.Data.Repositories
{
	public interface IRepository<T>
	{
		T Get(object id);
		T Load(object id);
		T MakePersistent(T entity);
		void MakeTransient(T entity);
	}
}