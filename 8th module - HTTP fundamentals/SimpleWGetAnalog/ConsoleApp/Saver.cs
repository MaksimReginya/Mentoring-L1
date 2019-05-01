using System;
using System.IO;
using System.Linq;
using SimpleWGet.Interfaces;

namespace ConsoleApp
{
	public class Saver : ISaver
	{
		private readonly DirectoryInfo _destDirectory;

		public Saver(DirectoryInfo destDirectory)
		{
			_destDirectory = destDirectory;
		}

		public void SaveHtmlPage(Uri url, string name, Stream stream)
		{
			string directoryPath = this.BuildPath(_destDirectory, url);
			Directory.CreateDirectory(directoryPath);

			name = this.ExcludeInvalidFileNameChars(name);
			string filePath = Path.Combine(directoryPath, name);

			this.CreateFile(stream, filePath);
		}

		public void SaveResource(Uri url, Stream stream)
		{
			string filePath = this.BuildPath(_destDirectory, url);
			string directoryPath = Path.GetDirectoryName(filePath);
			Directory.CreateDirectory(directoryPath);

			if (Directory.Exists(filePath))
			{
				filePath = Path.Combine(filePath, Guid.NewGuid().ToString());
			}

			this.CreateFile(stream, filePath);
		}

		private void CreateFile(Stream stream, string filePath)
		{
			using (FileStream file = File.Create(filePath))
			{
				stream.Seek(0, SeekOrigin.Begin);
				stream.CopyTo(file);
			}
		}

		private string BuildPath(DirectoryInfo destDirectory, Uri url)
		{
			return Path.Combine(destDirectory.FullName, url.Host) + url.LocalPath.Replace("/", @"\");
		}

		private string ExcludeInvalidFileNameChars(string name)
		{
			char[] invalidSymbols = Path.GetInvalidFileNameChars();
			return new string(name.Where(c => !invalidSymbols.Contains(c)).ToArray());
		}
	}
}
