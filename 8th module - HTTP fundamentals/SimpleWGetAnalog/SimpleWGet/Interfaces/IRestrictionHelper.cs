using System;

namespace SimpleWGet.Interfaces
{
	public interface IRestrictionHelper
	{
		bool IsExtensionRestricted(Uri url);
		bool IsDomainRestricted(Uri url);
	}
}
