using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Web;
using CsQuery;
using SimpleWGet.Interfaces;

namespace SimpleWGet
{
	public class WGet : ISimpleWGet
	{
		private const int MaxFileNameLength = 260;
		private readonly int _maxDepthLevel;
		private readonly ISaver _saver;
		private readonly IRestrictionHelper _restrictionHelper;
		private readonly ILogger _logger;
		private readonly ISet<Uri> _downloadedUrls = new HashSet<Uri>();

		public WGet(
			ISaver saver,
			IRestrictionHelper restrictionHelper,
			ILogger logger,
			int maxDepthLevel = 0)
		{
			_saver = saver;
			_restrictionHelper = restrictionHelper;
			_logger = logger;
			_maxDepthLevel = maxDepthLevel;
		}

		public void DownloadSite(string url)
		{
			_downloadedUrls.Clear();

			using (HttpClient httpClient = new HttpClient())
			{
				httpClient.BaseAddress = new Uri(url);
				this.ProcessUrl(httpClient, httpClient.BaseAddress, 0);
			}
		}

		private void ProcessUrl(HttpClient httpClient, Uri url, int depthLevel)
		{
			if (depthLevel > _maxDepthLevel
				|| _downloadedUrls.Contains(url)
				|| (!url.Scheme.Equals("http", StringComparison.OrdinalIgnoreCase)
					&& !url.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
				)
			{
				return;
			}

			_downloadedUrls.Add(url);

			HttpResponseMessage response = httpClient.GetAsync(url).Result;

			if (response.Content.Headers.ContentType.MediaType.Equals("text/html", StringComparison.OrdinalIgnoreCase))
			{
				if (_restrictionHelper.IsDomainRestricted(url))
				{
					return;
				}

				_logger?.Log($"Html page founded: {url}");

				this.ProcessHtmlPage(httpClient, response, url, depthLevel);
			}
			else
			{
				if (_restrictionHelper.IsExtensionRestricted(url))
				{
					return;
				}

				_logger?.Log($"Resource founded: {url}");

				Stream stream = response.Content.ReadAsStreamAsync().Result;
				_saver.SaveResource(url, stream);
				stream.Close();
			}
		}

		private void ProcessHtmlPage(HttpClient httpClient, HttpResponseMessage response, Uri url, int depthLevel)
		{
			Stream stream = response.Content.ReadAsStreamAsync().Result;
			MemoryStream memoryStream = new MemoryStream();
			stream.CopyTo(memoryStream);
			stream.Seek(0, SeekOrigin.Begin);

			CQ cq = CQ.Create(stream, Encoding.UTF8);

			_saver.SaveHtmlPage(url, this.GetHtmlPageName(cq), memoryStream);

			foreach (IDomObject el in cq.Find("a"))
			{
				this.ProcessUrl(httpClient, new Uri(httpClient.BaseAddress, el.GetAttribute("href")), depthLevel + 1);
			}

			memoryStream.Close();
			stream.Close();
		}

		private string GetHtmlPageName(CQ cq)
		{
			string extension = ".html";
			string name = cq.Find("title").FirstElement()?.InnerText;
			name = HttpUtility.HtmlDecode(name);

			if (name == null || name.Length + extension.Length > MaxFileNameLength)
			{
				name = Guid.NewGuid().ToString();
			}

			return name + extension;
		}
	}
}
