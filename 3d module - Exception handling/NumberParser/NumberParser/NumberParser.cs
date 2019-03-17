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

			return 0;
		}
    }
}
