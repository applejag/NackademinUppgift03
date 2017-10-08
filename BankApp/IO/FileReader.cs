using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using BankApp.BankObjects;
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

		/// <exception cref="ParseFileException">Thrown on any parsing errors</exception>
		public List<T> ReadSerializeableGroup<T>() where T : Identified, ISerializable, new()
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

					if (list.Any(o => o.ID == obj.ID))
						throw new ParseFileException(FilePath, FileRow, $"Found second item with ID <{obj.ID}>.");

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

		public static List<string> GetFilesInDirectory(string directory, string extension = ".txt")
		{
			var filenames = new List<string>();

			void RecursiveSearch(string dirr)
			{
				FileSystemInfo[] files = new DirectoryInfo(Path.Combine(directory, dirr)).GetFileSystemInfos();

				foreach (FileSystemInfo file in files)
				{
					if (file.Attributes.HasFlag(FileAttributes.Hidden)) continue;
					
					if (file.Attributes.HasFlag(FileAttributes.Directory))
						RecursiveSearch(Path.Combine(dirr, file.Name));
					else if (file.Extension == extension)
						filenames.Add(Path.Combine(dirr, file.Name));
				}
			}

			RecursiveSearch(string.Empty);

			filenames.Sort();
			return filenames;
		}

		public static string GetPathToLatestDateTimed(List<string> files)
		{
			string filename = null;
			DateTime? latest = null;

			foreach (string file in files)
			{
				string name = Path.GetFileNameWithoutExtension(file);
				if (DateTime.TryParseExact(name, "yyyyMMdd-HHmm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime time))
				{
					if (latest == null || time > latest.Value)
					{
						latest = time;
						filename = file;
					}
				}
			}

			return filename;
		}
	}
}