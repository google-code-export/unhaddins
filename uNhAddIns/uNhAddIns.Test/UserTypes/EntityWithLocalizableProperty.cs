using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace uNhAddIns.Test.UserTypes
{
	public class EntityWithLocalizableProperty
	{
		public virtual string Description
		{
			get
			{
				return LocalizableDescriptions.Count > 0
				       	? LocalizableDescriptions.FirstOrDefault(e => e.Key.Equals(Thread.CurrentThread.CurrentCulture)).Value
				       	: null;
			}
			set
			{
				if (value == null)
				{
					LocalizableDescriptions.Remove(Thread.CurrentThread.CurrentCulture);
				}
				else
				{
					LocalizableDescriptions[Thread.CurrentThread.CurrentCulture] = value;
				}
			}
		}

		public virtual Dictionary<CultureInfo, string> LocalizableDescriptions { get; set; }
	}
}