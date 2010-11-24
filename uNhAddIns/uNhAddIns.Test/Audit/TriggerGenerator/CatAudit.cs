using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uNhAddIns.Audit.TriggerGenerator;

namespace uNhAddIns.Test.Audit.TriggerGenerator
{

  public class CatAudit
  {

    public virtual int Id { get; set; }
    public virtual string Color { get; set; }
    public virtual string AuditUser { get; set; }
    public virtual DateTime AuditTimestamp { get; set; }
    public virtual TriggerActions AuditOperation { get; set; }

    public override bool Equals(object obj)
    {
      var other = obj as CatAudit;
      if (other == null)
        return false;
      return Id == other.Id 
        && AuditUser == other.AuditUser 
        && AuditTimestamp == other.AuditTimestamp;
    }

    public override int GetHashCode()
    {
      return Id.GetHashCode() ^
        AuditUser.GetHashCode() ^
        AuditTimestamp.GetHashCode();
    }

  }

}
