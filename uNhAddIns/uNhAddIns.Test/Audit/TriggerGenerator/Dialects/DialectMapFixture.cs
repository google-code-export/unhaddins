using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using uNhAddIns.Audit.TriggerGenerator.Dialects;
using NHibernate.Dialect;

namespace uNhAddIns.Test.Audit.TriggerGenerator.Dialects
{
  
  [TestFixture]
  public class DialectMapFixture
  {

    [Test]
    public void UsesSuppliedExtendedDialect()
    {
      var expected = typeof(DummyExtendedDialect);
      var actual = DialectMap.GetExtendedDialectType(expected);
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void UsesMapOfExtendedDialects()
    {
      var supplied = typeof(MsSql2008Dialect);
      var expected = typeof(ExtendedMsSql2008Dialect);
      var actual = DialectMap.GetExtendedDialectType(supplied);
      Assert.AreEqual(expected, actual);
    }

  }

}
