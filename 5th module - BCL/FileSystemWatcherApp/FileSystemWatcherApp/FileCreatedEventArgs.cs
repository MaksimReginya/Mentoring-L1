using System;

namespace FileSystemWatcherApp
{
	public class FileCreatedEventArgs : EventArgs
	{
		public string Name { get; set; }

		public DateTime CreationDate { get; set; }
	}
}
