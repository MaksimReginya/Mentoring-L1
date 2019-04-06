using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using FileSystemWatcherApp;
using FileSystemWatcherApp.Configuration;
using FileSystemWatcherApp.LocalizationResources;

namespace ConsoleApp
{
	public class Program
	{
		private static List<string> _directories;
		private static List<Rule> _rules;

		public static void Main(string[] args)
		{
			if (!(ConfigurationManager
				.GetSection("fileSystemWatcherSection") is FileSystemWatcherSection configuration))
			{
				Console.WriteLine(LocalizationResources.ConfigurationSectionWasNotFound);

				return;
			}

			GetConfigurationInfo(configuration);

			FileSystemWatcher watcher = new FileSystemWatcher(_directories, _rules, configuration.Rules.DefaultDirectory);

			watcher.FileCreated += (sender, eventArgs) =>
				Console.WriteLine(string.Format(
					LocalizationResources.CreatedFileFound,
					eventArgs.Name,
					eventArgs.CreationDate.ToShortDateString()));

			Console.WriteLine(LocalizationResources.ExitMessage);
			while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;
		}

		private static void GetConfigurationInfo(FileSystemWatcherSection configuration)
		{
			_directories = new List<string>();
			_rules = new List<Rule>();

			foreach (Directory directory in configuration.Directories)
			{
				_directories.Add(directory.Path);
			}

			foreach (Rule rule in configuration.Rules)
			{
				_rules.Add(rule);
			}

			CultureInfo.DefaultThreadCurrentCulture = configuration.Culture;
			CultureInfo.DefaultThreadCurrentUICulture = configuration.Culture;
		}
	}
}
