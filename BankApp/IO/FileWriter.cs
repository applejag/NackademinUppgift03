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
			this.FilePath = path;
			this.FileRow = 0;
			this.writer = new StreamWriter(path);
		}

		public void WriteSerializeableGroup<T>(ICollection<T> group) where T : ISerializable
		{
			this.WriteLine(group.Count);

			foreach (T serializeable in group)
			{
				this.WriteLine(serializeable.Serialize());
			}
		}

		public void WriteLine(string text)
		{
			this.FileRow++;
			this.writer.WriteLine(text);
		}

		public void WriteLine(object arg)
		{
			this.WriteLine(text: Convert.ToString(arg, CultureInfo.InvariantCulture));
		}

		public void Dispose()
		{
			this.writer?.Dispose();
		}
	}
}