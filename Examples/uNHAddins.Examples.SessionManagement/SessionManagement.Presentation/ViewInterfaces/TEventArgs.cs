using System;

namespace SessionManagement.Presentation.ViewInterfaces
{
	public class TEventArgs<T> : EventArgs
	{
		public T Data { get; set; }

		public TEventArgs(T data)
		{
			Data = data;
		}
	}
}