using System;
using YourPrjDomain.Ranges;

namespace YourPrjDomain.Ranges
{
	public interface IDateRange: IRange<DateTime>
	{
		TimeSpan Gap();
		bool Includes(IDateRange other);
		bool Overlaps(IDateRange other);
	}
}