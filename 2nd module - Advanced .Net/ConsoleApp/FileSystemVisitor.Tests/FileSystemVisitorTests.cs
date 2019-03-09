using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Visitor;

namespace FileSystemVisitorTests
{
	[TestFixture]
	public class FileSystemVisitorTests
	{
		private string _baseDirectory;
		private FileSystemVisitor _visitor;

		[SetUp]
		public void Initialize()
		{
			_baseDirectory = AppDomain.CurrentDomain.BaseDirectory + @"..\..\FakeDirectory\";
			_visitor = new FileSystemVisitor();
		}

		[Test]
		public void VisitDirectory_WhenCalled_StartAndFinishEventsAreTriggered()
		{
			// Arrange.
			bool wasStartCalled = false, wasFinishCalled = false;
			_visitor.Start += (object sender, EventArgs e) => wasStartCalled = true;
			_visitor.Finish += (object sender, EventArgs e) => wasFinishCalled = true;

			// Act.
			_visitor.VisitDirectory(_baseDirectory).ToList();

			// Assert.
			Assert.IsTrue(wasStartCalled);
			Assert.IsTrue(wasFinishCalled);
		}

		[Test]
		public void VisitDirectory_WhenDirectoryExists_DirectoryFindedAndFilteredDirectoryFindedAreTriggered()
		{
			// Arrange.
			bool wasDirectoryFindedCalled = false, wasFilteredDirectoryFindedCalled = false;
			_visitor.DirectoryFinded += (object sender, FileSystemEntryArgs e) =>
				wasDirectoryFindedCalled = true;
			_visitor.FilteredDirectoryFinded += (object sender, FileSystemEntryArgs e) =>
				wasFilteredDirectoryFindedCalled = true;

			// Act.
			_visitor.VisitDirectory(_baseDirectory).ToList();

			// Assert.
			Assert.IsTrue(wasDirectoryFindedCalled);
			Assert.IsTrue(wasFilteredDirectoryFindedCalled);
		}

		[Test]
		public void VisitDirectory_WhenFileExists_FileFindedAndFilteredFileFindedAreTriggered()
		{
			// Arrange.
			bool wasFileFindedCalled = false, wasFilteredFileFindedCalled = false;
			_visitor.FileFinded += (object sender, FileSystemEntryArgs e) =>
				wasFileFindedCalled = true;
			_visitor.FilteredFileFinded += (object sender, FileSystemEntryArgs e) =>
				wasFilteredFileFindedCalled = true;
			_baseDirectory += @"DirectoryWithFile\";

			// Act.
			_visitor.VisitDirectory(_baseDirectory).ToList();

			// Assert.
			Assert.IsTrue(wasFileFindedCalled);
			Assert.IsTrue(wasFilteredFileFindedCalled);
		}

		[Test]
		public void VisitDirectory_WhenFilterIsSet_FilesAndDirectoriesAreFiltered()
		{
			// Arrange.
			Func<string, bool> filter = (string path) => true;
			_visitor = new FileSystemVisitor(filter);

			// Act.
			List<string> result = _visitor.VisitDirectory(_baseDirectory).ToList();

			// Assert.
			Assert.AreEqual(0, result.Count);
		}

		[Test]
		public void VisitDirectory_WhenExcludeFlagIsSet_FileOrDirectoryIsExcluded()
		{
			// Arrange.
			_visitor.FileFinded += (object sender, FileSystemEntryArgs e) =>
				e.Action = ActionType.Exclude;
			_baseDirectory += @"DirectoryWithFile\";

			// Act.
			List<string> result = _visitor.VisitDirectory(_baseDirectory).ToList();

			// Assert.
			Assert.AreEqual(0, result.Count);
		}

		[Test]
		public void VisitDirectory_WhenStopFlagIsSet_SearchStops()
		{
			// Arrange.
			_visitor.FileFinded += (object sender, FileSystemEntryArgs e) =>
				e.Action = ActionType.Stop;

			// Act.
			List<string> result = _visitor.VisitDirectory(_baseDirectory).ToList();

			// Assert.
			// Only 'DirectoryWithFile' was found, then search was stopped.
			Assert.AreEqual(1, result.Count);
		}
	}
}
