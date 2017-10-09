using System;
using System.Globalization;
using BankApp.Exceptions;

namespace BankApp
{
	public static class ValueConverter
	{
		public delegate bool Parser<T>(string value, out T result);

		public static bool TryParseDecimal(string value, out decimal result)
		{
			return decimal.TryParse(value.Replace(',','.'),
				NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
				CultureInfo.InvariantCulture,
				out result);
		}

		public static bool TryParseLong(string value, out long result)
		{
			return long.TryParse(value,
				NumberStyles.AllowLeadingSign,
				CultureInfo.InvariantCulture,
				out result);
		}
		public static bool TryParseULong(string value, out ulong result)
		{
			return ulong.TryParse(value,
				NumberStyles.None,
				CultureInfo.InvariantCulture,
				out result);
		}
		public static bool TryParseInt(string value, out int result)
		{
			return int.TryParse(value,
				NumberStyles.AllowLeadingSign,
				CultureInfo.InvariantCulture,
				out result);
		}
		public static bool TryParseUInt(string value, out uint result)
		{
			return uint.TryParse(value,
				NumberStyles.None,
				CultureInfo.InvariantCulture,
				out result);
		}
		
		public static bool TryParseEnum<T>(string value, out T result) where T : struct, IComparable
		{
			try
			{
				int id = int.Parse(value, CultureInfo.InvariantCulture);
				result = (T)Enum.ToObject(typeof(T), id);
				return true;
			}
			catch
			{
				result = default(T);
				return false;
			}
		}

		public static bool TryParseDateTime(string value, out DateTime result)
		{
			try
			{
				long time = long.Parse(value, CultureInfo.InvariantCulture);
				result = DateTimeOffset.FromUnixTimeSeconds(time).DateTime;
				return true;
			}
			catch
			{
				result = default(DateTime);
				return false;
			}
		}

		public static bool TryParseDateTimeOffset(string value, out DateTimeOffset result)
		{
			try
			{
				long time = long.Parse(value, CultureInfo.InvariantCulture);
				result = DateTimeOffset.FromUnixTimeSeconds(time);
				return true;
			}
			catch
			{
				result = default(DateTimeOffset);
				return false;
			}
		}

		public static string ToParsableString(object obj)
		{
			switch (obj)
			{
				case Enum e:
					return Convert.ToInt32(e).ToString(CultureInfo.InvariantCulture);

				case DateTimeOffset dto:
					return dto.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);

				case DateTime dt:
					return new DateTimeOffset(dt).ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);

				default:
					return Convert.ToString(obj, CultureInfo.InvariantCulture);
			}
		}

	}
}