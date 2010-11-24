using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NHibernate.Cfg;
using NHibernate.Cfg.Loquacious;
using uNhAddIns.Audit.TriggerGenerator;
using NHibernate;
using System.Data;
using System.Reflection;
using NHibernate.Dialect;

namespace uNhAddIns.Test.Audit.TriggerGenerator
{

  [TestFixture]
  public class IntegrationFixture
  {

    private Configuration cfg;
    private ISessionFactory sessionFactory;

    [TestFixtureSetUp]
    public void Configure()
    {
      cfg = new Configuration().Configure()
        .DataBaseIntegration(db =>
        {
          db.Dialect<MsSql2008Dialect>();
        });

      cfg.AddResource("uNhAddIns.Test.Audit.TriggerGenerator.Cat.hbm.xml", Assembly.GetExecutingAssembly());
      cfg.AddResource("uNhAddIns.Test.Audit.TriggerGenerator.CatAudit.hbm.xml", Assembly.GetExecutingAssembly());
      cfg.AddTriggerAuditing();
      sessionFactory = cfg.BuildSessionFactory();

      var se = new NHibernate.Tool.hbm2ddl.SchemaExport(cfg);
      se.Execute(false, true, false);
    }

    [SetUp, TearDown]
    public void ClearCats()
    {
      using (var session = sessionFactory.OpenSession())
      using (var tx = session.BeginTransaction())
      {
        session.CreateQuery("delete from Cat")
          .ExecuteUpdate();
        tx.Commit();
      }

      using (var session = sessionFactory.OpenSession())
      using (var tx = session.BeginTransaction())
      {
        session.CreateQuery("delete from CatAudit")
          .ExecuteUpdate();
        tx.Commit();
      }
    }

    [Test]
    public void InsertDoesAudit()
    {
      int catId;
      CatAudit audit;
      using (var session = sessionFactory.OpenSession())
      {
        using (var tx = session.BeginTransaction())
        {
          catId = (int)session.Save(new Cat() { Color = "Black" });
          tx.Commit();
        }

        using (var tx = session.BeginTransaction())
        {
          audit = session.CreateQuery(
            "from CatAudit where Id = :id")
            .SetParameter("id", catId)
            .UniqueResult<CatAudit>();
          tx.Commit();
        }

      }

      Assert.AreEqual(catId, audit.Id);
      Assert.AreEqual("Black", audit.Color);
      Assert.IsNotNullOrEmpty(audit.AuditUser);
      Assert.GreaterOrEqual(audit.AuditTimestamp, DateTime.Now.AddMinutes(-1));
      Assert.AreEqual(TriggerActions.INSERT, audit.AuditOperation);

    }

    [Test]
    public void UpdateDoesAudit()
    {
      var cat = new Cat() { Color = "Black" };
      int catId;
      long auditCount;
      CatAudit audit;
      using (var session = sessionFactory.OpenSession())
      {
        using (var tx = session.BeginTransaction())
        {
          catId = (int)session.Save(cat);
          tx.Commit();
        }

        using (var tx = session.BeginTransaction())
        {
          cat.Color = "Gray";
          tx.Commit();
        }

        using (var tx = session.BeginTransaction())
        {
          audit = session.CreateQuery(
            "from CatAudit where Id = :id and AuditOperation = :op")
            .SetParameter("id", catId)
            .SetParameter("op", TriggerActions.UPDATE)
            .UniqueResult<CatAudit>();
          auditCount = session.CreateQuery("select count(*) from CatAudit")
            .UniqueResult<long>();
          tx.Commit();
        }

      }
      Assert.AreEqual(2, auditCount);
      Assert.AreEqual(catId, audit.Id);
      Assert.AreEqual("Gray", audit.Color);
      Assert.IsNotNullOrEmpty(audit.AuditUser);
      Assert.GreaterOrEqual(audit.AuditTimestamp, DateTime.Now.AddMinutes(-1));
      Assert.AreEqual(TriggerActions.UPDATE, audit.AuditOperation);

    }

    [Test]
    public void DeleteDoesAudit()
    {
      var cat = new Cat() { Color = "Black" };
      int catId;
      long auditCount;
      CatAudit audit;
      using (var session = sessionFactory.OpenSession())
      {
        using (var tx = session.BeginTransaction())
        {
          catId = (int)session.Save(cat);
          tx.Commit();
        }

        using (var tx = session.BeginTransaction())
        {
          session.Delete("from Cat");
          tx.Commit();
        }

        using (var tx = session.BeginTransaction())
        {
          audit = session.CreateQuery(
            "from CatAudit where Id = :id and AuditOperation = :op")
            .SetParameter("id", catId)
            .SetParameter("op", TriggerActions.DELETE)
            .UniqueResult<CatAudit>();
          auditCount = session.CreateQuery("select count(*) from CatAudit")
            .UniqueResult<long>();
          tx.Commit();
        }

      }
      Assert.AreEqual(2, auditCount);
      Assert.AreEqual(catId, audit.Id);
      Assert.AreEqual("Black", audit.Color);
      Assert.IsNotNullOrEmpty(audit.AuditUser);
      Assert.GreaterOrEqual(audit.AuditTimestamp, DateTime.Now.AddMinutes(-1));
      Assert.AreEqual(TriggerActions.DELETE, audit.AuditOperation);

    }

  }
}
