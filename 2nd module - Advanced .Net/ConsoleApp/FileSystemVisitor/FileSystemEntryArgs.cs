using System;

namespace Visitor
{
	public class FileSystemEntryArgs : EventArgs
	{
		public ActionType Action { get; set; }

		public string EntryName { get; internal set; }

		public FileSystemEntryArgs(string name, ActionType action = ActionType.Continue)
		{
			this.EntryName = name;
			this.Action = action;
		}
	}
}
