using System.Collections.Generic;
using FileSystemWatcherApp.Configuration;

namespace FileSystemWatcherApp
{
	public class FileSystemWatcherHelper
	{
		private readonly IEnumerable<Rule> _rules;
		private readonly string _defaultDirectory;

		public FileSystemWatcherHelper(IEnumerable<Rule> rules, string defaultDirectory)
		{
			_rules = rules;
			_defaultDirectory = defaultDirectory;
		}
	}
}
