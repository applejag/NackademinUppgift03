using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using BankApp.Exceptions;

namespace BankApp.IO
{
	public class FileReader : IDisposable
	{
		private readonly StreamReader reader;

		public string FilePath { get; }
		public int FileRow { get; private set; }

		public FileReader(string path)
		{
			FilePath = path;
			FileRow = 0;
			reader = new StreamReader(path);
		}

		public List<T> ReadSerializeableGroup<T>() where T : ISerializable, new()
		{
			string countString = ReadLine();
			var list = new List<T>();

			if (!int.TryParse(countString, out int count))
				throw new ParseFileException(FilePath, FileRow, $"Failed in parsing count of elements. Actual <{countString}>");

			for (int i = 0; i < count; i++)
			{
				try
				{
					var data = new FileRow(ReadLine());
					var obj = new T();

					obj.Deserialize(data);

					list.Add(obj);
				}
				catch (ParseRowException e)
				{
					throw new ParseFileException(FilePath, FileRow, typeof(T), e);
				}
			}

			return list;
		}

		public string ReadLine()
		{
			FileRow++;

			if (reader.EndOfStream)
				throw new ParseFileException(FilePath, FileRow, "Unexpected end of file.");

			return reader.ReadLine();
		}

		public void Dispose()
		{
			reader?.Dispose();
		}
	}
}