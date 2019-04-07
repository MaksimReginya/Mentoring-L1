using System.Collections.Generic;
using System.IO;
using IODirectory = System.IO.Directory;
using FileSystemWatcherApp.Configuration;

namespace FileSystemWatcherApp
{
	public class FileSystemWatcher
	{
		private readonly FileSystemWatcherHelper _watcherHelper;
		private readonly ILogger _logger;

		public FileSystemWatcher(IEnumerable<string> directories, IEnumerable<Rule> rules, string defaultDirectory, ILogger logger)
		{
			_logger = logger;
			_watcherHelper = new FileSystemWatcherHelper(rules, defaultDirectory, logger);

			foreach (string directory in directories)
			{
				this.CreateWatcherForDirectory(directory);
			}
		}

		private void CreateWatcherForDirectory(string directory)
		{
			if (!IODirectory.Exists(directory))
			{
				IODirectory.CreateDirectory(directory);
			}

			System.IO.FileSystemWatcher fileSystemWatcher = new System.IO.FileSystemWatcher(directory)
			{
				NotifyFilter = NotifyFilters.FileName
			};

			fileSystemWatcher.Created += (sender, eventArgs) =>
			{
				this.OnFileCreated(eventArgs.Name, eventArgs.FullPath);
			};

			fileSystemWatcher.EnableRaisingEvents = true;
		}

		private void OnFileCreated(string fileName, string filePath)
		{
			var creationDate = File.GetCreationTime(filePath);
			_logger?.Log(string.Format(
					LocalizationResources.LocalizationResources.CreatedFileFound,
					fileName,
					creationDate.ToShortDateString()));

			_watcherHelper.ShiftFile(fileName, filePath);
		}
	}
}
