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
			visitor.FileFinded += (object sender, FileSystemEntryArgs e) => Console.WriteLine($"FileFinded event: {e.EntryName}");
			visitor.DirectoryFinded += (object sender, FileSystemEntryArgs e) =>
			{
				Console.WriteLine($"DirectoryFinded event: {e.EntryName}");

				// Custom logic for excluding file system entry.
				if (e.EntryName.Length > 7)
				{
					Console.WriteLine($"Excluding {e.EntryName} folder from search");
					e.Action = ActionType.Exclude;
				}
			};
			visitor.FilteredFileFinded += (object sender, FileSystemEntryArgs e) =>
			{
				Console.WriteLine($"FilteredFileFinded event: {e.EntryName}");

				// Custom logic for interrupting search.
				if (e.EntryName.Length == 2)
				{
					Console.WriteLine("Stopping search...");
					e.Action = ActionType.Stop;
				}
			};
			visitor.FilteredDirectoryFinded += (object sender, FileSystemEntryArgs e) =>
				Console.WriteLine($"FilteredDirectoryFinded event: {e.EntryName}");

			foreach (string entry in visitor.VisitFolder(args[0]))
			{
				Console.WriteLine(entry);
			}
		}
	}
}
