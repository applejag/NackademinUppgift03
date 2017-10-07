using System;

namespace BankApp.Exceptions
{
	public class ParseFileException : ParseException
	{
		public string FilePath { get; }
		public int FileRow { get; }
		
		public ParseFileException(string filePath, int fileRow, string message)
			: base($"Error in reading file \"{filePath}\" at row {fileRow}! {message}")
		{
			FileRow = fileRow;
			FilePath = filePath;
		}

		public ParseFileException(string filePath, int fileRow, Type type, ParseRowException innerException)
			: base($"Error in reading type <{type.Name}> from file \"{filePath}\" at row {fileRow}! {innerException.Message}")
		{
			FileRow = fileRow;
			FilePath = filePath;
		}
	}
}