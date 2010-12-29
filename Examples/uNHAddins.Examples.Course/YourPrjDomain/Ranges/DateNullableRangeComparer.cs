using System;
using System.Collections.Generic;

namespace YourPrjDomain.Ranges
{
	[Serializable]
	public class DateNullableRangeComparer: IComparer<DateTime?>
	{
		#region IComparer<DateTime?> Members

		public int Compare(DateTime? x, DateTime? y)
		{
			if (!x.HasValue && !y.HasValue)
				return 0;
			if (!x.HasValue)
				return -1;
			if (!y.HasValue)
				return 1;
			return x.Value.CompareTo(y);
		}

		#endregion
	}
}