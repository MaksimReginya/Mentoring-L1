using System.Configuration;

namespace FileSystemWatcherApp.Configuration
{
	public class Directory : ConfigurationElement
	{
		[ConfigurationProperty("path", IsKey = true)]
		public string Path => (string)base["path"];
	}
}
