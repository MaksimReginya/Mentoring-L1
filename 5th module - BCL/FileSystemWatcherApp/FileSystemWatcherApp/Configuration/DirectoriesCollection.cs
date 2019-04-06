using System.Collections.Generic;
using System.Configuration;

namespace FileSystemWatcherApp.Configuration
{
	public class DirectoriesCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement() => new Directory();

		protected override object GetElementKey(ConfigurationElement element) =>
			((Directory)element).Path;
	}
}
