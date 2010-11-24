using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Dialect;
using uNhAddIns.Audit.TriggerGenerator.Dialects;

namespace uNhAddIns.Test.Audit.TriggerGenerator.Dialects
{

  public class DummyExtendedDialect 
    : Dialect, IExtendedDialect
  {

    public string GetCreateTriggerHeaderString(string triggerName, string dataTableName, uNhAddIns.Audit.TriggerGenerator.TriggerActions action)
    {
      throw new NotImplementedException();
    }

    public string GetCreateTriggerFooterString(string triggerName, string dataTableName, uNhAddIns.Audit.TriggerGenerator.TriggerActions action)
    {
      throw new NotImplementedException();
    }

    public string GetDropTriggerString(string triggerName, string dataTableName, uNhAddIns.Audit.TriggerGenerator.TriggerActions action)
    {
      throw new NotImplementedException();
    }

    public string GetInsertIntoString(string destTable, IEnumerable<string> columnNames, string sourceTable, IEnumerable<string> values)
    {
      throw new NotImplementedException();
    }

    public string QualifyColumn(string tableName, string columnName)
    {
      throw new NotImplementedException();
    }

    public string GetTriggerNewDataAlias()
    {
      throw new NotImplementedException();
    }

    public string GetTriggerOldDataAlias()
    {
      throw new NotImplementedException();
    }

    public string QuoteForTriggerName(string triggerName)
    {
      throw new NotImplementedException();
    }

  }

}
