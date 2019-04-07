using System;

namespace FileSystemWatcherApp
{
	public class FileCreatedEventArgs : EventArgs
	{
		public string Name { get; set; }

		public string FullPath { get; set; }

		public DateTime CreationDate { get; set; }
	}
}
