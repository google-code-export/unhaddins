using System;

namespace YourPrjDomain.Ranges
{
	[Serializable]
	public class DateNullableRange : IRange<DateTime?>, IDateRange
	{
		readonly DateNullableRangeComparer comparer = new DateNullableRangeComparer();

		#region IRange<DateTime?> Members
		private DateTime? _lowLimit;
		public DateTime? LowLimit
		{
			get { return _lowLimit; }
			set { _lowLimit = value; }
		}

		private DateTime? _highLimit;
		public DateTime? HighLimit
		{
			get { return _highLimit; }
			set { _highLimit = value; }
		}

		public bool Includes(DateTime? value)
		{
			if (!value.HasValue)
				return false;
			return Includes(value.Value);
		}

		public bool IsValid
		{
			get { return comparer.Compare(_lowLimit, _highLimit) <= 0; }
		}

		public bool IsEmpty
		{
			get
			{
				if (!_lowLimit.HasValue || !_highLimit.HasValue)
					return true;
				return comparer.Compare(_lowLimit, _highLimit) == 0;
			}
		}

		#endregion

		#region IEquatable<IRange<DateTime?>> Members

		public bool Equals(IRange<DateTime?> other)
		{
			if (other == null)
				return false;
			return _lowLimit.Equals(other.LowLimit) && _highLimit.Equals(other.HighLimit);
		}

		#endregion

		#region IDateRange Members

		public TimeSpan Gap()
		{
			AssertIsValid();
			return _highLimit.Value - _lowLimit.Value;
		}

		public bool Includes(IDateRange other)
		{
			return Includes(other.LowLimit) && Includes(other.HighLimit);
		}

		public bool Overlaps(IDateRange other)
		{
			return Includes(other.LowLimit) || Includes(other.HighLimit) || other.Includes(this);
		}

		#endregion

		#region IRange<DateTime> Members

		DateTime IRange<DateTime>.LowLimit
		{
			get { return _lowLimit.GetValueOrDefault(DateTime.MinValue); }
		}

		DateTime IRange<DateTime>.HighLimit
		{
			get { return _highLimit.GetValueOrDefault(DateTime.MaxValue); }
		}

		public bool Includes(DateTime value)
		{
			if (!_lowLimit.HasValue || !_highLimit.HasValue)
				return false;
			return comparer.Compare(value, _lowLimit) >= 0 && comparer.Compare(value, _highLimit) <= 0;
		}

		#endregion

		#region IEquatable<IRange<DateTime>> Members

		public bool Equals(IRange<DateTime> other)
		{
			if (other == null || !_lowLimit.HasValue || !_highLimit.HasValue)
				return false;
			return _lowLimit.Value.Equals(other.LowLimit) && _highLimit.Value.Equals(other.HighLimit);
		}

		#endregion

		private void AssertIsValid()
		{
			if (!IsValid)
				throw new ArgumentOutOfRangeException();
		}
	}
}