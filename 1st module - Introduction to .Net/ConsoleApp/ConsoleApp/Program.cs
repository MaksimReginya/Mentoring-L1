using System;
using StandartClassLibrary;

namespace ConsoleApp
{
	public class Program
	{
		static void Main(string[] args)
		{
			string helloMessage = HelloBuilder.BuildHelloMessage(args[0]);

			Console.WriteLine(helloMessage);
		}
	}
}
