using System;
using Parser;

namespace ConsoleApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			NumberParser parser = new NumberParser();

			string number;
			int result;
			if (args.Length > 0)
			{
				number = args[0];
			}
			else
			{
				number = string.Empty;
			}

			try
			{
				result = parser.ConvertToNumber(number);
			}
			catch (ArgumentNullException e)
			{
				Console.WriteLine($"ArgumentNullException catched! Message: {e.Message}");
				return;
			}
			catch (OverflowException e)
			{
				Console.WriteLine($"OverflowException catched! Message: {e.Message}");
				return;
			}
			catch (FormatException e)
			{
				Console.WriteLine($"FormatException catched! Message: {e.Message}");
				return;
			}

			Console.WriteLine($"Converted number: {result}");
		}
	}
}
