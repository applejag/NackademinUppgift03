using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using BankApp.Exceptions;

namespace BankApp.IO
{
	public class FileRow
	{
		private readonly Queue<string> dataQueue;

		public string Source { get; }
		public int SourceCount { get; }
		public int Index => SourceCount - Count;
		public int Count => dataQueue.Count;

		public FileRow(string row)
		{
			Source = row;
			dataQueue = new Queue<string>(row.Split(';'));
			SourceCount = dataQueue.Count;
		}

		public FileRow(params object[] args)
		{
			string[] strArgs = args.Select(d => ValueConverter.ToParsableString(d).Replace(";", string.Empty)).ToArray();

			dataQueue = new Queue<string>(strArgs);
			Source = string.Join(";", strArgs);
			SourceCount = dataQueue.Count;
		}

		/// <summary>
		/// Returns the string representation of the queue, seperated with ';'
		/// </summary>
		public override string ToString()
		{
			return Source;
		}

		/// <summary>
		/// Takes one string from the row array queue.
		/// </summary>
		/// <exception cref="ParseRowException"></exception>
		public string TakeString()
		{
			if (Count == 0) throw new ParseRowException(typeof(string), null, Index);

			return dataQueue.Dequeue();
		}

		/// <summary>
		/// Takes one string from the row array queue and tries to convert it to a decimal.
		/// </summary>
		/// <exception cref="ParseRowException"></exception>
		public decimal TakeDecimal()
		{
			return Take<decimal>(ValueConverter.TryParseDecimal);
		}

		/// <summary>
		/// Takes one string from the row array queue and tries to convert it to a long.
		/// </summary>
		/// <exception cref="ParseRowException"></exception>
		public long TakeLong()
		{
			return Take<long>(ValueConverter.TryParseLong);
		}

		/// <summary>
		/// Takes one string from the row array queue and tries to convert it to a uint.
		/// </summary>
		/// <exception cref="ParseRowException"></exception>
		public uint TakeUInt()
		{
			return Take<uint>(ValueConverter.TryParseUInt);
		}

		/// <summary>
		/// Takes one string from the row array queue and tries to convert it to a int.
		/// </summary>
		/// <exception cref="ParseRowException"></exception>
		public int TakeInt()
		{
			return Take<int>(ValueConverter.TryParseInt);
		}

		/// <summary>
		/// Takes one string from the row array queue and tries to convert it to an enum of type <typeparamref name="T"/>.
		/// </summary>
		/// <exception cref="ParseRowException"></exception>
		public T TakeEnum<T>() where T : struct, IComparable
		{
			return Take<T>(ValueConverter.TryParseEnum);
		}

		public DateTime TakeDateTime()
		{
			return Take<DateTime>(ValueConverter.TryParseDateTime);
		}

		public DateTimeOffset TakeDateTimeOffset()
		{
			return Take<DateTimeOffset>(ValueConverter.TryParseDateTimeOffset);
		}

		public T Take<T>(ValueConverter.Parser<T> parser)
		{
			if (Count == 0) throw new ParseRowException(typeof(T), null, Index);

			string data = dataQueue.Dequeue();

			if (parser(data, out T result))
				return result;

			throw new ParseRowException(typeof(T), null, Index - 1);
		}

		public void Close()
		{
			if (Count != 0)
			{
				dataQueue.Clear();
				throw new ParseRowException(Index,
					$"Too many items. Expected <{Index - 1}>, actual <{SourceCount}>.");
			}
		}
		
	}
}