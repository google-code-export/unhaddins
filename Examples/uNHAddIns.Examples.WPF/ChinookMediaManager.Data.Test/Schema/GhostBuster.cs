using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ChinookMediaManager.Data.Test.BaseClasses;
using log4net;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Type;
using NUnit.Framework;

namespace ChinookMediaManager.Data.Test.Schema
{
	/// <summary>
	/// ghostbuster
	/// </summary>
	[TestFixture]
	public class GhostBuster 
	{
		private const string DefaultIdName = "Id";
		private static readonly ILog log = LogManager.GetLogger(typeof(GhostBuster));
		private static readonly ILog logError = LogManager.GetLogger(typeof(GhostBuster).FullName + "error");
		private Configuration configuration;
		private ISessionFactory sessions;
		
		[Test, Explicit]
		public void UnexpectedUpdateDeleteOnFetch()
		{
			configuration = new Configuration().Configure("hibernate-readonly.cfg.xml");
			sessions = configuration.BuildSessionFactory();

			log4net.Config.XmlConfigurator.Configure();
			PersistingMappings(null);

			sessions.Dispose();
		}

		private void PersistingMappings(ICollection<string> entitiesFilter)
		{
			var invalidUpdates = new List<string>();
			var nop = new NoUpdateInterceptor(invalidUpdates);

			IEnumerable<string> entitiesToCheck;
			if (entitiesFilter == null)
			{
				entitiesToCheck = configuration.ClassMappings.Select(x => x.EntityName);
			}
			else
			{
				entitiesToCheck = from persistentClass in configuration.ClassMappings
				                  where entitiesFilter.Contains(persistentClass.EntityName)
				                  select persistentClass.EntityName;
			}

			foreach (var entityName in entitiesToCheck)
			{
				EntityPersistenceTest(invalidUpdates, entityName, nop);
			}

			if (invalidUpdates.Count > 0)
			{
				if (logError.IsDebugEnabled)
				{
					logError.Debug("  ");
					logError.Debug("------ INVALID UPDATES -------");
					invalidUpdates.ForEach(x => logError.Debug(x));
					logError.Debug("------------------------------");
				}
			}
			Assert.AreEqual(0, invalidUpdates.Count, "Has unexpected updates.");
		}

		private void EntityPersistenceTest(ICollection<string> invalidUpdates,
		                                   string entityName, IInterceptor nop)
		{
			const string queryTemplate = "select e.{0} from {1} e";
			string msg = "s--------" + entityName;
			log.Debug(msg);

			using (var s = sessions.OpenSession(nop))
			using (var tx = s.BeginTransaction())
			{
				IList entityIds = null;
				try
				{
					string queryString = string.Format(queryTemplate, DefaultIdName, entityName);
					entityIds = s.CreateQuery(queryString).SetMaxResults(1).List();
				}
				catch (Exception e)
				{
					log.Debug("Possible METEORITE:" + e.Message);
				}

				if (entityIds != null)
				{
					if (entityIds.Count == 0 || entityIds[0] == null)
					{
						log.Debug("No instances");
					}
					else
					{
						if (entityIds.Count > 1)
						{
							msg = ">Has " + entityIds.Count + " subclasses";
							log.Debug(msg);
						}
						object entityId = entityIds[0];
						try
						{
							s.Get(entityName, entityId);
							try
							{
								s.Flush();
							}
							catch (Exception ex)
							{
								string emsg = string.Format("EXCEPTION - Flushing entity [#{0}]: {1}", entityId, ex.Message);
								log.Debug(emsg);
								invalidUpdates.Add(emsg);
							}
						}
						catch (Exception ex)
						{
							string emsg = string.Format("EXCEPTION - Getting [#{0}]: {1}", entityId, ex.Message);
							invalidUpdates.Add(emsg);
							log.Debug(emsg);
						}
					}
					tx.Rollback();
				}
			}
			msg = "e--------" + entityName;
			log.Debug(msg);
		}

		private class NoUpdateInterceptor : EmptyInterceptor
		{
			private readonly IList<string> invalidUpdates;

			public NoUpdateInterceptor(IList<string> invalidUpdates)
			{
				this.invalidUpdates = invalidUpdates;
			}

			public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState,
			                                  string[] propertyNames, IType[] types)
			{
				string msg = " FlushDirty :" + entity.GetType().FullName;
				log.Debug(msg);
				invalidUpdates.Add(msg);
				return false;
			}

			public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
			{
				string msg = " Save       :" + entity.GetType().FullName;
				log.Debug(msg);
				invalidUpdates.Add(msg);
				return false;
			}

			public override void OnDelete(object entity, object id, object[] state, string[] propertyNames, IType[] types)
			{
				string msg = " Delete     :" + entity.GetType().FullName;
				log.Debug(msg);
				invalidUpdates.Add(msg);
			}
		}
	}
}