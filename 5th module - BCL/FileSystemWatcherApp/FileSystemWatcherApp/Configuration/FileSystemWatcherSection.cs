using System.Configuration;
using System.Globalization;

namespace FileSystemWatcherApp.Configuration
{
	public class FileSystemWatcherSection : ConfigurationSection
	{
		[ConfigurationProperty("culture", DefaultValue = "en-US")]
		public CultureInfo Culture => (CultureInfo)this["culture"];

		[ConfigurationCollection(typeof(Directory), AddItemName = "directory")]
		[ConfigurationProperty("directories")]
		public DirectoriesCollection Directories => (DirectoriesCollection)this["directories"];

		[ConfigurationCollection(typeof(Rule), AddItemName = "rule")]
		[ConfigurationProperty("rules")]
		public RulesCollection Rules => (RulesCollection)this["rules"];
	}
}
