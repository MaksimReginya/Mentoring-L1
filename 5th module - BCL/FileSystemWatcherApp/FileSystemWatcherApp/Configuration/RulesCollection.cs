using System.Configuration;

namespace FileSystemWatcherApp.Configuration
{
	public class RulesCollection : ConfigurationElementCollection
	{
		[ConfigurationProperty("defaultDirectory")]
		public string DefaultDirectory => (string)this["defaultDirectory"];

		protected override ConfigurationElement CreateNewElement() => new Rule();

		protected override object GetElementKey(ConfigurationElement element) =>
			((Rule)element).Template;
	}
}
