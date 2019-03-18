using System;
using Moq;
using NLog;
using NUnit.Framework;

namespace Parser.Tests
{
	[TestFixture]
	public class NumberParserTests
	{
		private NumberParser _parser;
		private Mock<ILogger> _loggerMock;

		[SetUp]
		public void Initialize()
		{
			_loggerMock = new Mock<ILogger>();
			_parser = new NumberParser
			{
				Logger = _loggerMock.Object
			};
		}

		[TestCase(null)]
		[TestCase("")]
		public void ConvertToNumber_WhenNullOrEmptyString_ArgumentNullExceptionIsThrown(string number)
		{
			Assert.Throws<ArgumentNullException>(() => _parser.ConvertToNumber(number));

			_loggerMock.Verify(mock => 
				mock.Error(It.IsAny<ArgumentNullException>(), It.IsAny<string>()),
				Times.Once());
		}

		[TestCase("abcdefg")]
		[TestCase("1a2b3c")]
		public void ConvertToNumber_WhenStringContainsLetters_FormatExceptionIsThrown(string number)
		{
			Assert.Throws<FormatException>(() => _parser.ConvertToNumber(number));

			_loggerMock.Verify(mock =>
				mock.Error(It.IsAny<FormatException>(), It.IsAny<string>()),
				Times.Once());
		}

		[TestCase(int.MaxValue)]
		[TestCase(int.MinValue)]
		public void ConvertToNumber_WhenNumberIsTooBigOrSmall_OverflowExceptionIsThrown(int boundaryValue)
		{
			string overflowValue = boundaryValue.ToString() + "0";
			Assert.Throws<OverflowException>(() => _parser.ConvertToNumber(overflowValue));

			_loggerMock.Verify(mock =>
				mock.Error(It.IsAny<OverflowException>(), It.IsAny<string>()),
				Times.Once());
		}

		[TestCase("0", 0)]
		[TestCase("7", 7)]
		[TestCase("-7", -7)]
		[TestCase("17", 17)]
		[TestCase("-17", -17)]
		[TestCase("2147483647", int.MaxValue)]
		[TestCase("-2147483648", int.MinValue)]
		public void ConvertToNumber_WhenStringIsValid_CorrectIntegerIsReturned(string number, int expected)
		{
			int actual = _parser.ConvertToNumber(number);
			Assert.AreEqual(expected, actual);
		}
	}
}
