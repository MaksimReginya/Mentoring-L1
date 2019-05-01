using System;
using System.IO;

namespace SimpleWGet.Interfaces
{
	public interface ISaver
	{
		void SaveResource(Uri url, Stream stream);

		void SaveHtmlPage(Uri url, string name, Stream stream);
	}
}