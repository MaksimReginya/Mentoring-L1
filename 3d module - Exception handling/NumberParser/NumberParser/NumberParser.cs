using System;
using NLog;

namespace Parser
{
	public class NumberParser
	{
		public ILogger Logger { get; set; }

		public NumberParser()
		{
			this.Logger = LogManager.GetCurrentClassLogger();
		}

		public int ConvertToNumber(string number)
		{
			if (string.IsNullOrEmpty(number))
			{
				ArgumentNullException ex = new ArgumentNullException(nameof(number), $"{nameof(number)} cannot be null or empty");
				this.Logger.Error(ex, ex.Message);

				throw ex;
			}

			char[] digits = number.ToCharArray();
			int result = 0, i = 0, numberLength = digits.Length;
			if (numberLength == 1)
			{
				this.ValidateDigit(digits[0]);
				result = this.CharToInt(digits[0]);

				return result;
			}

			bool isNegative = this.IsNegative(number);
			if (isNegative)
			{
				i = 1;
			}

			for (; i < numberLength; i++)
			{
				this.ValidateDigit(digits[i]);
				int temp = this.CharToInt(digits[i]);
				if (temp != 0)
				{
					result += this.ProcessNextDigit(temp, numberLength, i); 
				}
			}

			return isNegative ? -result : result;
		}

		private int ProcessNextDigit(int digit, int numberLength, int index)
		{
			int result;

			try
			{
				checked
				{
					result = digit * (int)Math.Pow(10, numberLength - (index + 1));
				}
			}
			catch (OverflowException ex)
			{
				this.Logger.Error(ex, "Provided number is out of Int32 range.");

				throw;
			}

			return result;
		}

		private void ValidateDigit(char el)
		{
			if (!char.IsDigit(el))
			{
				FormatException ex = new FormatException("Provided string is not a valid integer.");
				this.Logger.Error(ex, ex.Message);

				throw ex;
			}
		}

		private bool IsNegative(string number)
		{
			return '-'.Equals(number[0]);
		}

		private int CharToInt(char el)
		{
			return el - '0';
		}
	}
}
