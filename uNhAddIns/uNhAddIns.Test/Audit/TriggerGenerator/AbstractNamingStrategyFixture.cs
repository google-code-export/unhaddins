using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using uNhAddIns.Audit.TriggerGenerator;
using uNhAddIns.Audit.TriggerGenerator.Dialects;
using NHibernate.Mapping;

namespace uNhAddIns.Test.Audit.TriggerGenerator
{

  public abstract class AbstractNamingStrategyFixture
  {

    protected abstract INamingStrategy GetNamingStrategy();

    [Test]
    public void ReturnsUniqueTriggerNamesForActions()
    {
      var strategy = GetNamingStrategy();
      var dataTable = new Table("DataTable");
      var names = new string[] {
        strategy.GetTriggerName(dataTable, TriggerActions.INSERT),
        strategy.GetTriggerName(dataTable, TriggerActions.UPDATE),
        strategy.GetTriggerName(dataTable, TriggerActions.DELETE)};

      Assert.AreEqual(names.Count(), names.Distinct().Count());

    }

    [Test]
    public void ReturnsUniqueTriggerNamesForDifferentTables()
    {
      var strategy = GetNamingStrategy();
      var name1 = strategy.GetTriggerName(new Table("Table1"),
        TriggerActions.INSERT);
      var name2 = strategy.GetTriggerName(new Table("Table2"),
        TriggerActions.INSERT);
      Assert.AreNotEqual(name1, name2);
    }

    [Test]
    public void ReturnsUniqueTableNamesForDifferentTables()
    {
      var strategy = GetNamingStrategy();
      var name1 = strategy.GetAuditTableName(new Table("Table1"));
      var name2 = strategy.GetAuditTableName(new Table("Table2"));
      Assert.AreNotEqual(name1, name2);
    }


  }
}
