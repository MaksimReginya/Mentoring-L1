using System;

namespace MvcMusicStore.Infrastructure.Logger
{
	public interface ILogger
	{
		void Trace(string message);

		void Info(string message);

		void Debug(string message);

		void Error(string message);
		void Error(string message, Exception ex);
	}
}
