using System;

namespace BankApp.Exceptions
{
	public abstract class ParseException : ApplicationException
	{
		public ParseException(string message)
			: base(message)
		{ }

		public ParseException(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}
}