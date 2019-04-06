using System;
using System.Collections.Generic;
using System.IO;
using FileSystemWatcherApp.Configuration;

namespace FileSystemWatcherApp
{
	public class FileSystemWatcher
	{
		private readonly FileSystemWatcherHelper _watcherHelper;
		public event EventHandler<FileCreatedEventArgs> FileCreated;

		public FileSystemWatcher(IEnumerable<string> directories, IEnumerable<Rule> rules, string defaultDirectory)
		{
			_watcherHelper = new FileSystemWatcherHelper(rules, defaultDirectory);
			foreach (string directory in directories)
			{
				this.CreateWatcherForDirectory(directory);
			}
		}

		private void CreateWatcherForDirectory(string directory)
		{
			System.IO.FileSystemWatcher fileSystemWatcher = new System.IO.FileSystemWatcher(directory)
			{
				NotifyFilter = NotifyFilters.CreationTime,
			};

			fileSystemWatcher.Created += (sender, eventArgs) =>
			{
				this.OnFileCreated(eventArgs.Name, eventArgs.FullPath);
			};

			fileSystemWatcher.EnableRaisingEvents = true;
		}

		private void OnFileCreated(string fileName, string filePath)
		{
			FileCreated?.Invoke(this, new FileCreatedEventArgs { Name = fileName, CreationDate = File.GetCreationTime(filePath) });
		}
	}
}
