using System;
using Visitor;

namespace ConsoleApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			FileSystemVisitor visitor = new FileSystemVisitor();

			visitor.Start += (object sender, EventArgs e) => Console.WriteLine("Start event");
			visitor.Finish += (object sender, EventArgs e) => Console.WriteLine("Finish event");
			visitor.FileFinded += (object sender, FileSystemEntryArgs e) => Console.WriteLine("FileFinded event");
			visitor.DirectoryFinded += (object sender, FileSystemEntryArgs e) => Console.WriteLine("DirectoryFinded event");
			visitor.FilteredFileFinded += (object sender, FileSystemEntryArgs e) => Console.WriteLine("FilteredFileFinded event");
			visitor.FilteredDirectoryFinded += (object sender, FileSystemEntryArgs e) => Console.WriteLine("FilteredDirectoryFinded event");

			foreach (string entry in visitor.VisitFolder(args[0]))
			{
				Console.WriteLine(entry);
			}
		}
	}
}
