namespace SessionManagement.Data.SchemaCreation
{
	public interface ISchemaCreator
	{
		void CreateSchema();
		void DropSchema();
	}
}
