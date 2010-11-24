using System;

namespace YourPrjDomain.Ranges
{
	public interface IRange<T>: IEquatable<IRange<T>>
	{
		T LowLimit { get; }
		T HighLimit { get; }
		bool Includes(T value);
		bool IsValid { get;}
		bool IsEmpty { get;}
	}
}