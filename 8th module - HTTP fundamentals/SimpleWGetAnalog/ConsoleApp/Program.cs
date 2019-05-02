using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SimpleWGet;
using SimpleWGet.Interfaces;

namespace ConsoleApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CommandLineOptions options = new CommandLineOptions();
			if (!CommandLine.Parser.Default.ParseArguments(args, options))
			{
				return;
			}

			DirectoryInfo destDirectory = new DirectoryInfo(options.DestDirectory);
			ISaver saver = new Saver(destDirectory);
			IRestrictionHelper restrictionHelper = GetRestrictionHelper(options);
			ILogger logger = null;
			if (options.Verbose)
			{
				logger = new Logger();
			}

			ISimpleWGet wGet = new WGet(saver, restrictionHelper, logger, options.DepthLevel);

			try
			{
				wGet.DownloadSite(options.Url);
			}
			catch (Exception ex)
			{
				logger.Log($"Some error occured during site downloading: {ex.Message}");
			}

			Console.WriteLine("Finish downloading. Press enter to exit.");
			Console.ReadLine();
		}

		private static IRestrictionHelper GetRestrictionHelper(CommandLineOptions options)
		{
			IEnumerable<string> extensions = options.ResourceExtensions?.Split(',').Select(e => "." + e);
			Uri url = new Uri(options.Url);

			return new RestrictionHelper(url, options.DomainRestriction, extensions);
		}
	}
}
