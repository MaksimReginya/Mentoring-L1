using CommandLine;
using CommandLine.Text;

namespace ConsoleApp
{
	public class CommandLineOptions
	{
		[Option('u', "url", Required = true, HelpText = "Start url for downloading.")]
		public string Url { get; set; }

		[Option('d', "directory", Required = true, HelpText = "Destination directory path.")]
		public string DestDirectory { get; set; }

		[Option('l', "depthLevel", DefaultValue = 0, HelpText = "Max depth level of site crawling.")]
		public int DepthLevel { get; set; }

		[Option('r', "domainRestriction", DefaultValue = DomainRestriction.CurrentDomainOnly, HelpText = "Sets domain restriction (0 - No restriction, 1 - Current domain only, 3 - Child domains).")]
		public DomainRestriction DomainRestriction { get; set; }

		[Option('e', "resourceExtensions", HelpText = "List of resource extensions, example: \"gif,jpeg,jpg,pdf\".")]
		public string ResourceExtensions { get; set; }

		[Option('v', "verbose", DefaultValue = false, HelpText = "Logs currently processing urls.")]
		public bool Verbose { get; set; }

		[HelpOption]
		public string GetUsage()
		{
			return HelpText.AutoBuild(this);
		}
	}
}
