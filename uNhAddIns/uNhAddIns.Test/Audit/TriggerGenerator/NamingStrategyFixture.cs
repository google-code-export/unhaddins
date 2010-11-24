using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uNhAddIns.Audit.TriggerGenerator;
using NUnit.Framework;

namespace uNhAddIns.Test.Audit.TriggerGenerator
{
  [TestFixture]
  public class NamingStrategyFixture : AbstractNamingStrategyFixture 
  {

    protected override INamingStrategy GetNamingStrategy()
    {
      return new NamingStrategy();
    }

  }
}
