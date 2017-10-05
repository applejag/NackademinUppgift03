using System;

namespace BankApp.Exceptions
{
	public class ParseRowException : ParseException
	{
		public Type ValueType { get; }
		public int Index { get; }
		
		public ParseRowException(Type valueType, string data, int index, Exception innerException)
			: base(data != null
					? $"Failed in parsing <{valueType.Name}> for index [{index}]. Actual: <{data}>"
					: $"Missing <{valueType.Name}> value at index [{index}]."
				, innerException)
		{
			this.Index = index;
			this.ValueType = valueType;
		}

		public ParseRowException(Type valueType, string data, int index)
			: this(valueType, data, index, null)
		{ }

		public ParseRowException(int index, string message)
			: base(message)
		{
			this.Index = index;
			this.ValueType = null;
		}
	}
}