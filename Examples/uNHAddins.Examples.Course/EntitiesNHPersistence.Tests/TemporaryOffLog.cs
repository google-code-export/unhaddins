using System;
using System.Collections.Generic;
using log4net;
using log4net.Core;
using log4net.Repository.Hierarchy;

namespace uNHAddins.Examples.Course.Tests
{
	public class TemporaryOffLog : IDisposable
	{
		private readonly Dictionary<Logger, Level> loggers = new Dictionary<Logger, Level>(5);

		public TemporaryOffLog(string loggersName)
			: this(new string[] { loggersName })
		{
		}

		public TemporaryOffLog(IEnumerable<string> loggersNames)
		{
			foreach (string s in loggersNames)
			{
				ILog log = LogManager.GetLogger(s);
				Logger logger = log.Logger as Logger;
				if (logger != null)
				{
					loggers[logger] = logger.Level;
					logger.Level = Level.Off;
				}
			}
		}

		public void Dispose()
		{
			foreach (KeyValuePair<Logger, Level> logger in loggers)
			{
				logger.Key.Level = logger.Value;
			}
		}
	}
}
