using System;

namespace Visitor
{
	public class FileSystemEntryNotFoundException : Exception
	{
		public string FileSystemEntryPath { get; set; }

		public FileSystemEntryNotFoundException(string fileSystemEntryPath) : base()
		{
			this.FileSystemEntryPath = fileSystemEntryPath;
		}
	}
}
