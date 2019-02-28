using System;

namespace StandartClassLibrary
{
	public static class HelloBuilder
	{
		public static string BuildHelloMessage(string name)
		{
			string currentTime = DateTime.Now.ToShortTimeString();

			return $"{currentTime} Hello, {name}!";
		}
	}
}
