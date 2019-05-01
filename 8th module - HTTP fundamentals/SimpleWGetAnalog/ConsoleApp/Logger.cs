using System;
using SimpleWGet.Interfaces;

namespace ConsoleApp
{
	public class Logger : ILogger
	{
		public void Log(string message)
		{
			Console.WriteLine(message);
		}
	}
}
