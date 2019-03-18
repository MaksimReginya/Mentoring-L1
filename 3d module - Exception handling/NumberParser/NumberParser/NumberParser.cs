using System;

namespace NumberParser
{
    public static class NumberParser
	{
		public static int ConvertToNumber(this string number)
		{
			if (string.IsNullOrEmpty(number))
			{
				throw new ArgumentNullException(nameof(number), $"{nameof(number)} cannot be null or empty");
			}

			char[] digits = number.ToCharArray();
			int result = 0, i = 0;
			if (digits.Length == 1)
			{
				ValidateDigit(digits[0]);
				result = digits[0] - '0';
			}

			bool isNegative = IsNegative(digits[0]);
			if (isNegative)
			{
				i = 1;
			}

			for (; i < digits.Length; i++)
			{
				ValidateDigit(digits[i]);
				int temp = digits[i] - '0';
				if (temp != 0)
				{
					result += temp * (int)Math.Pow(10, (digits.Length - (i + 1)));
				}
			}

			return isNegative ? -result : result;
		}

		private static void ValidateDigit(char el)
		{
			if (!char.IsDigit(el))
			{
				throw new FormatException();
			}
		}

		private static bool IsNegative(char el)
		{
			return '-'.Equals(el);
		}

	}
}
