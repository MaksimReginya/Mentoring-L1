using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using CsQuery;
using SimpleWGet.Interfaces;

namespace SimpleWGet
{
	public class WGet : ISimpleWGet
	{
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
			if (depthLevel > _maxDepthLevel || _downloadedUrls.Contains(url))
			{
				return;
			}

			_downloadedUrls.Add(url);

			if (_restrictionHelper.IsRestricted(url))
			{
				return;
			}

			HttpResponseMessage response = httpClient.GetAsync(url).Result;

			if (response.Content.Headers.ContentType.MediaType.Equals("text/html", StringComparison.OrdinalIgnoreCase))
			{
				_logger?.Log($"Html page founded: {url}");

				this.ProcessHtmlPage(httpClient, response, url, depthLevel);
			}
			else
			{
				_logger?.Log($"Resource founded: {url}");

				System.IO.Stream stream = response.Content.ReadAsStreamAsync().Result;
				_saver.SaveResource(url, stream);
				stream.Close();
			}
		}

		private void ProcessHtmlPage(HttpClient httpClient, HttpResponseMessage response, Uri url, int depthLevel)
		{
			System.IO.Stream stream = response.Content.ReadAsStreamAsync().Result;
			CQ cq = CQ.Create(stream, Encoding.UTF8);

			string name = cq.Find("title").FirstElement().InnerText + ".html";
			_saver.SaveHtmlPage(url, name, stream);

			foreach (IDomObject el in cq.Find("a"))
			{
				this.ProcessUrl(httpClient, new Uri(httpClient.BaseAddress, el.GetAttribute("href")), depthLevel + 1);
			}

			stream.Close();
		}
	}
}
