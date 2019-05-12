using System;
using System.Collections.Generic;
using System.Linq;
using SimpleWGet.Interfaces;

namespace ConsoleApp
{
	public class RestrictionHelper : IRestrictionHelper
	{
		private readonly Uri _baseUrl;
		private readonly DomainRestriction _domainRestriction;
		private readonly IEnumerable<string> _extensions;

		public RestrictionHelper(Uri baseUrl, DomainRestriction restriction, IEnumerable<string> extensions)
		{
			_baseUrl = baseUrl;
			_domainRestriction = restriction;
			_extensions = extensions;
		}

		//public bool IsRestricted(Uri url) => this.IsExtensionRestricted(url) || this.IsDomainRestricted(url);

		public bool IsExtensionRestricted(Uri url)
		{
			bool result = false;
			if (_extensions != null)
			{
				string lastSegment = url.Segments.Last();
				result = !_extensions.Any(e => lastSegment.EndsWith(e));
			}

			return result;
		}

		public bool IsDomainRestricted(Uri url)
		{
			switch (_domainRestriction)
			{
				case DomainRestriction.NoRestriction:
					return false;
				case DomainRestriction.CurrentDomainOnly:
					if (_baseUrl.DnsSafeHost == url.DnsSafeHost)
					{
						return false;
					}
					break;
				case DomainRestriction.ChildDomains:
					if (_baseUrl.IsBaseOf(url))
					{
						return false;
					}
					break;
			}

			return true;
		}
	}
}
