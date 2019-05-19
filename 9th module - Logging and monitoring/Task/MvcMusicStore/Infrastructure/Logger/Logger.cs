using System;
using NLog;

namespace MvcMusicStore.Infrastructure.Logger
{
	public class Logger : ILogger
	{
		private readonly NLog.ILogger _logger = LogManager.GetLogger("MvcMusicStoreLogger");

		public void Trace(string message)
		{
			_logger.Trace(message);
		}

		public void Info(string message)
		{
			_logger.Info(message);
		}

		public void Debug(string message)
		{
			_logger.Debug(message);
		}

		public void Error(string message)
		{
			_logger.Error(message);
		}

		public void Error(string message, Exception ex)
		{
			_logger.Error(ex, message);
		}
	}
}