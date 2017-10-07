using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BankApp.BankObjects
{
	public class DetailedResult : IReadOnlyList<DetailedResult.Result>
	{
		private readonly List<Result> results;
		public readonly bool IsMatch;

		public int Count => results.Count;
		public Result this[int index] => results[index];

		public DetailedResult(IEnumerable<Result> results)
		{
			this.results = results?.ToList() ?? new List<Result>();
			IsMatch = this.results.Any(r => r.IsMatch);
		}

		public DetailedResult()
		{
			results = new List<Result>();
			IsMatch = false;
		}

		public static DetailedResult SearchDetailed(IDetailed detailed, string query)
		{
			if (string.IsNullOrWhiteSpace(query)) return new DetailedResult();

			throw new NotImplementedException();
		}

		public override string ToString()
		{
			return results.Select(r => r.Value).Aggregate((a, b) => a + b);
		}

		public void PrintResults(ConsoleColor defaultColor, ConsoleColor matchColor)
		{
			foreach (Result result in results)
			{
				Console.ForegroundColor = result.IsMatch ? matchColor : defaultColor;
				Console.Write(result.Value);
			}
		}

		public IEnumerator<Result> GetEnumerator()
		{
			return results.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public struct Result
		{
			public IDetailed Source;
			public bool IsMatch;
			public string Value;
			public int StartsAt;

			public override string ToString()
			{
				return Value;
			}
		}

	}
}
