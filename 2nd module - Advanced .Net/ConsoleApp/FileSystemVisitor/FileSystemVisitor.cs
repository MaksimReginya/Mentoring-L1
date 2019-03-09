using System;
using System.Collections.Generic;
using System.IO;

namespace Visitor
{
    public class FileSystemVisitor
    {
		private readonly Func<string, bool> _filter;

		public event EventHandler<EventArgs> Start, Finish;
		public event EventHandler<FileSystemEntryArgs> FileFinded, DirectoryFinded, FilteredFileFinded, FilteredDirectoryFinded;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filter">filter must return <see cref="true"/> if file/folder should be filtered.</param>
		public FileSystemVisitor(Func<string, bool> filter = null)
		{
			_filter = filter ?? ((string path) => false);
		}

		public IEnumerable<string> VisitFolder(string path)
		{
			this.OnEvent(this.Start, new EventArgs());

			string[] entries = Directory.GetFileSystemEntries(path, "*", SearchOption.AllDirectories);

			foreach (string entry in this.VisitFolder(entries))
			{
				yield return entry;
			}

			this.OnEvent(this.Finish, new EventArgs());
		}

		private IEnumerable<string> VisitFolder(string[] entries)
		{
			for (int i = 0; i < entries.Length; i++)
			{
				string entry = entries[i];
				bool isDirectory = this.IsDirectory(entry);
				string entryName = Path.GetFileName(entry);
				ActionType action;

				if (isDirectory)
				{
					action = this.ProcessEntry(entry, entryName, this.DirectoryFinded, this.FilteredDirectoryFinded);
				}
				else
				{
					action = this.ProcessEntry(entry, entryName, this.FileFinded, this.FilteredFileFinded);
				}

				if (action == ActionType.Stop)
				{
					yield break;
				}
				else if (action == ActionType.Continue)
				{
					yield return entry;
				}
			}
		}

		protected virtual void OnEvent<T>(EventHandler<T> eventHandler, T e)
		{
			EventHandler<T> tmp = eventHandler;
			if (tmp != null)
			{
				tmp(this, e);
			}
		}

		private bool IsDirectory(string path)
		{
			FileAttributes attr = File.GetAttributes(path);

			return attr.HasFlag(FileAttributes.Directory);
		}

		private ActionType ProcessEntry(
			string entry,
			string entryName,
			EventHandler<FileSystemEntryArgs> findHandler,
			EventHandler<FileSystemEntryArgs> filteredFindHandler)
		{
			FileSystemEntryArgs e = new FileSystemEntryArgs(entryName);
			this.OnEvent(findHandler, e);

			if (e.Action != ActionType.Continue)
			{
				return e.Action;
			}

			if (!_filter(entry))
			{
				this.OnEvent(filteredFindHandler, e);
				return e.Action;
			}

			return ActionType.Exclude;
		}
	}
}
