using System;

namespace Visitor
{
	public class FileSystemEntryArgs : EventArgs
	{
		public bool IsBreakSearch { get; internal set; }

		public bool IsEntryExcluded { get; internal set; }

		public FileSystemEntryArgs(bool isBreakSearch = false, bool isEntryExcluded = false)
		{
			this.IsBreakSearch = isBreakSearch;
			this.IsEntryExcluded = isEntryExcluded;
		}
	}
}
