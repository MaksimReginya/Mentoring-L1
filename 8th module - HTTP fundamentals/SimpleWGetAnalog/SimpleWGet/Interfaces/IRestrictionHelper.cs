using System;

namespace SimpleWGet.Interfaces
{
	public interface IRestrictionHelper
	{
		bool IsRestricted(Uri url);
	}
}
