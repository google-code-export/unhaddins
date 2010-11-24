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

  public abstract class AbstractAuditColumnSourceFixture
  {

    protected abstract IAuditColumnSource GetAuditColumnSource();

    [Test]
    public void CreatesNewColumnsWithEachCall()
    {
      var source = GetAuditColumnSource();
      var dialect = new ExtendedMsSql2008Dialect();
      var first = source.GetAuditColumns(new Table("AuditTable1"),
        dialect);
      var second = source.GetAuditColumns(new Table("AuditTable1"),
        dialect);

      var same = from firstCol in first
                 from secondCol in second
                 where ReferenceEquals(firstCol, secondCol)
                 select firstCol;
      
      Assert.AreEqual(0, same.Count());
    }

  }
}
