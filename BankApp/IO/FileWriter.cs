using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BankApp.IO
{
	public class FileWriter : IDisposable
	{
		private readonly StreamWriter writer;
		public string FilePath { get; }
		public int FileRow { get; private set; }

		public FileWriter(string path)
		{
			FilePath = path;
			FileRow = 0;
			writer = new StreamWriter(path);
		}

		public void WriteSerializeableGroup<T>(ICollection<T> group) where T : ISerializable
		{
			WriteLine(group.Count);

			foreach (T serializeable in group)
			{
				WriteLine(serializeable.Serialize());
			}
		}

		public void WriteLine(string text)
		{
			FileRow++;
			writer.WriteLine(text);
		}

		public void WriteLine(object arg)
		{
			WriteLine(text: Convert.ToString(arg, CultureInfo.InvariantCulture));
		}

		public void Dispose()
		{
			writer?.Dispose();
		}
	}
}