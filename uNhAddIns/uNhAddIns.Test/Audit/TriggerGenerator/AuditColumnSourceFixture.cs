using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using uNhAddIns.Audit.TriggerGenerator;
using NHibernate.Dialect.Function;
using NHibernate;
using NHibernate.Mapping;

namespace uNhAddIns.Test.Audit.TriggerGenerator
{

  [TestFixture]
  public class AuditColumnSourceFixture : AbstractAuditColumnSourceFixture 
  {

    protected override IAuditColumnSource GetAuditColumnSource()
    {
      return new AuditColumnSource();
    }

    [Test]
    public void WhenNoFunctionsExistReturnsOnlyAuditOperationColumn()
    {
      var dialect = new Dialects.DummyExtendedDialect();
      dialect.Functions.Clear();

      var auditColumns = GetAuditColumnSource().GetAuditColumns(
        new Table("AuditTable"), dialect);

      Assert.AreEqual(1, auditColumns.Count());
    }

    [Test]
    public void WhenCurrentUserFunctionExistsReturnsAuditUserColumn()
    {
      var dialect = new Dialects.DummyExtendedDialect();
      dialect.Functions.Clear();
      dialect.Functions.Add(
        AuditColumnSource.CurrentUserFunctionName,
        new NoArgSQLFunction("system_user", NHibernateUtil.AnsiString));

      var auditColumns = GetAuditColumnSource().GetAuditColumns(
        new Table("AuditTable"), dialect);

      Assert.AreEqual(2, auditColumns.Count());
    }

    [Test]
    public void WhenCurrentTimestampFunctionExistsReturnsAuditTimestampColumn()
    {
      var dialect = new Dialects.DummyExtendedDialect();
      dialect.Functions.Clear();
      dialect.Functions.Add(
        AuditColumnSource.CurrentTimestampFunctionName,
        new NoArgSQLFunction("getdate", NHibernateUtil.DateTime));

      var auditColumns = GetAuditColumnSource().GetAuditColumns(
        new Table("AuditTable"), dialect);

      Assert.AreEqual(2, auditColumns.Count());
    }

    [Test]
    public void WhenCurrentTimestampAndCurrentUserFunctionsExistsReturnsThreeColumns()
    {
      var dialect = new Dialects.DummyExtendedDialect();
      dialect.Functions.Clear();
      dialect.Functions.Add(
        AuditColumnSource.CurrentUserFunctionName,
        new NoArgSQLFunction("system_user", NHibernateUtil.AnsiString));
      dialect.Functions.Add(
        AuditColumnSource.CurrentTimestampFunctionName,
        new NoArgSQLFunction("getdate", NHibernateUtil.DateTime));

      var auditColumns = GetAuditColumnSource().GetAuditColumns(
        new Table("AuditTable"), dialect);

      Assert.AreEqual(3, auditColumns.Count());
    }

    [Test]
    public void AuditOperationColumnUniquelyIdentifiesOperation()
    {
      var dialect = new Dialects.DummyExtendedDialect();
      dialect.Functions.Clear();

      var auditColumns = GetAuditColumnSource().GetAuditColumns(
        new Table("AuditTable"), dialect);

      var auditOperation = auditColumns.Single();
      var operations = new TriggerActions[] {
        TriggerActions.INSERT,
        TriggerActions.UPDATE,
        TriggerActions.DELETE 
      };

      var outputs = from op in operations
                    select auditOperation.ValueFunction(op);

      Assert.AreEqual(outputs.Count(), outputs.Distinct().Count());

    }


  }
}
