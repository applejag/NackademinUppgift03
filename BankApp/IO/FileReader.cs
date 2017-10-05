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
			this.FilePath = path;
			this.FileRow = 0;
			this.reader = new StreamReader(path);
		}

		public List<T> ReadSerializeableGroup<T>() where T : ISerializable, new()
		{
			string countString = this.ReadLine();
			var list = new List<T>();

			if (!int.TryParse(countString, out int count))
				throw new ParseFileException(this.FilePath, this.FileRow, $"Failed in parsing count of elements. Actual <{countString}>");

			for (int i = 0; i < count; i++)
			{
				try
				{
					var data = new FileRow(this.ReadLine());
					var obj = new T();

					obj.Deserialize(data);

					list.Add(obj);
				}
				catch (ParseRowException e)
				{
					throw new ParseFileException(this.FilePath, this.FileRow, typeof(T), e);
				}
			}

			return list;
		}

		public string ReadLine()
		{
			this.FileRow++;

			if (this.reader.EndOfStream)
				throw new ParseFileException(this.FilePath, this.FileRow, "Unexpected end of file.");

			return this.reader.ReadLine();
		}

		public void Dispose()
		{
			this.reader?.Dispose();
		}
	}
}