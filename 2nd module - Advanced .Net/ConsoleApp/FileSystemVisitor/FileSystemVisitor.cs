using System;
using System.Collections.Generic;
using System.IO;

namespace Visitor
{
    public class FileSystemVisitor
    {
		private readonly Func<string, bool> _filter;

		public event EventHandler Start, Finish;
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
			this.OnStart(new EventArgs());

			string[] entries = Directory.GetFileSystemEntries(path, "*", SearchOption.AllDirectories);

			for (int i = 0; i < entries.Length; i++)
			{
				string entry = entries[i];
				bool isDirectory = this.IsDirectory(entry);

				if (isDirectory)
				{
					this.OnDirectoryFinded(new FileSystemEntryArgs());
				}
				else
				{
					this.OnFileFinded(new FileSystemEntryArgs());
				}

				if (!_filter(entry))
				{
					if (isDirectory)
					{
						this.OnFilteredDirectoryFinded(new FileSystemEntryArgs());
					}
					else
					{
						this.OnFilteredFileFinded(new FileSystemEntryArgs());
					}

					yield return entry;
				}
			}

			this.OnFinish(new EventArgs());
		}

		protected virtual void OnStart(EventArgs e)
		{
			EventHandler tmp = Start;
			if (tmp != null)
			{
				tmp(this, e);
			}
		}

		protected virtual void OnFinish(EventArgs e)
		{
			EventHandler tmp = Finish;
			if (tmp != null)
			{
				tmp(this, e);
			}
		}

		protected virtual void OnFileFinded(FileSystemEntryArgs e)
		{
			EventHandler<FileSystemEntryArgs> tmp = FileFinded;
			if (tmp != null)
			{
				tmp(this, e);
			}
		}

		protected virtual void OnDirectoryFinded(FileSystemEntryArgs e)
		{
			EventHandler<FileSystemEntryArgs> tmp = DirectoryFinded;
			if (tmp != null)
			{
				tmp(this, e);
			}
		}

		protected virtual void OnFilteredFileFinded(FileSystemEntryArgs e)
		{
			EventHandler<FileSystemEntryArgs> tmp = FilteredFileFinded;
			if (tmp != null)
			{
				tmp(this, e);
			}
		}

		protected virtual void OnFilteredDirectoryFinded(FileSystemEntryArgs e)
		{
			EventHandler<FileSystemEntryArgs> tmp = FilteredDirectoryFinded;
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
	}
}
