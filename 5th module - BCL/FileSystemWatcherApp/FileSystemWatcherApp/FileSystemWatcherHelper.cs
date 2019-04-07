using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using FileSystemWatcherApp.Configuration;
using IODirectory = System.IO.Directory;

namespace FileSystemWatcherApp
{
	public class FileSystemWatcherHelper
	{
		private readonly IEnumerable<Rule> _rules;
		private readonly ILogger _logger;
		private readonly string _defaultDirectory;
		private int _currentIndexNumber = 1;

		public FileSystemWatcherHelper(IEnumerable<Rule> rules, string defaultDirectory, ILogger logger)
		{
			_logger = logger;
			_rules = rules;
			_defaultDirectory = defaultDirectory;
		}

		public void ShiftFile(string fileName, string filePath)
		{
			foreach (Rule rule in _rules)
			{
				Regex template = new Regex(rule.Template);
				if (!template.IsMatch(fileName))
				{
					continue;
				}

				_logger?.Log(LocalizationResources.LocalizationResources.FoundMatchingRule);
				string dest = this.BuildNewFilePath(fileName, rule);
				this.TransferFile(filePath, dest);
				_logger?.Log(LocalizationResources.LocalizationResources.FileTransfered);

				return;
			}

			string defaultDest = Path.Combine(_defaultDirectory, fileName);
			_logger?.Log(LocalizationResources.LocalizationResources.NotFoundMatchingRule);
			this.TransferFile(filePath, defaultDest);
			_logger?.Log(LocalizationResources.LocalizationResources.FileTransfered);
		}

		private string BuildNewFilePath(string fileName, Rule rule)
		{
			StringBuilder result = new StringBuilder();

			if (rule.IsShiftDateRequired)
			{
				result.Append($"{DateTime.Now.ToShortDateString()}_");
			}

			if (rule.IsIndexNumberRequired)
			{
				result.Append($"{_currentIndexNumber}_");
			}

			_currentIndexNumber++;
			result.Append(fileName);
			return Path.Combine(rule.DestinationDirectory, result.ToString());
		}

		private void TransferFile(string from, string to)
		{
			string directoryName = Path.GetDirectoryName(to);
			if (!IODirectory.Exists(directoryName))
			{
				IODirectory.CreateDirectory(directoryName);
			}

			if (File.Exists(to))
			{
				File.Delete(to);
			}

			File.Move(from, to);
		}
	}
}
