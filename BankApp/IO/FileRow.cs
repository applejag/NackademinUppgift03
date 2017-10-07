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
			string[] strArgs = args.Select(d => 
				Convert.ToString(d, CultureInfo.InvariantCulture).Replace(";", string.Empty)
			).ToArray();

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
			if (Count == 0) throw new ParseRowException(typeof(decimal), null, Index);

			string data = dataQueue.Dequeue();
			try
			{
				return decimal.Parse(data.Replace(',', '.'), CultureInfo.InvariantCulture);
			}
			catch (Exception e)
			{
				throw new ParseRowException(typeof(decimal), data, Index-1, e);
			}
		}

		/// <summary>
		/// Takes one string from the row array queue and tries to convert it to a long.
		/// </summary>
		/// <exception cref="ParseRowException"></exception>
		public long TakeLong()
		{
			if (Count == 0) throw new ParseRowException(typeof(long), null, Index);

			string data = dataQueue.Dequeue();
			try
			{
				return long.Parse(data, CultureInfo.InvariantCulture);
			}
			catch (Exception e)
			{
				throw new ParseRowException(typeof(long), data, Index - 1, e);
			}
		}

		/// <summary>
		/// Takes one string from the row array queue and tries to convert it to a uint.
		/// </summary>
		/// <exception cref="ParseRowException"></exception>
		public uint TakeUInt()
		{
			if (Count == 0) throw new ParseRowException(typeof(uint), null, Index);

			string data = dataQueue.Dequeue();
			try
			{
				return uint.Parse(data, CultureInfo.InvariantCulture);
			}
			catch (Exception e)
			{
				throw new ParseRowException(typeof(uint), data, Index - 1, e);
			}
		}

		/// <summary>
		/// Takes one string from the row array queue and tries to convert it to a int.
		/// </summary>
		/// <exception cref="ParseRowException"></exception>
		public int TakeInt()
		{
			if (Count == 0) throw new ParseRowException(typeof(int), null, Index);

			string data = dataQueue.Dequeue();
			try
			{
				return int.Parse(data, CultureInfo.InvariantCulture);
			}
			catch (Exception e)
			{
				throw new ParseRowException(typeof(int), data, Index - 1, e);
			}
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