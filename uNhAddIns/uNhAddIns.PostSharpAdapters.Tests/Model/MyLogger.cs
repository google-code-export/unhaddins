using System.Collections.Generic;

namespace uNhAddIns.PostSharpAdapters.Tests.Model
{
	public class MyLogger
	{
		private readonly ICollection<string> messages
			= new List<string>();

		public void AddMessage(string message)
		{
			messages.Add(message);
		}

		public IEnumerable<string> Messages { get { return messages; } }
	}
}