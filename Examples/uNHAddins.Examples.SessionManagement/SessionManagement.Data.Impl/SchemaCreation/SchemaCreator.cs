using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using SessionManagement.Data.SchemaCreation;

namespace SessionManagement.Data.NH.SchemaCreation
{
	public class SchemaCreator : ISchemaCreator
	{
		#region Implementation of ISchemaCreation

		public void CreateSchema()
		{
			var cfg = new Configuration().Configure();
			new SchemaExport(cfg).Create(false, true);
		}

		public void DropSchema()
		{
			var cfg = new Configuration().Configure();
			new SchemaExport(cfg).Drop(false, true);
		}

		#endregion
	}
}