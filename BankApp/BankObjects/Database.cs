using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BankApp.BankObjects;
using BankApp.IO;

namespace BankApp
{
	public class Database
	{
		public List<Customer> Customers { get; }
		public List<Account> Accounts { get; }

		public Database(string path)
		{
			using (var file = new FileReader(path))
			{
				Customers = file.ReadSerializeableGroup<Customer>();
				Accounts = file.ReadSerializeableGroup<Account>();
			}
		}

		public void Save(string path)
		{
			using (var file = new FileWriter(path))
			{
				file.WriteSerializeableGroup(Customers);
				file.WriteSerializeableGroup(Accounts);
			}
		}

		public List<Customer> SearchCustomers(string query)
		{
			return Search(Customers, query).OrderBy(i => i.ID).ToList();
		}

		public List<Account> SearchAccounts(string query)
		{
			return Search(Accounts, query).OrderBy(i => i.ID).ToList();
		}

		public List<ISearchable> SearchCustomersAndAccounts(string query)
		{
			var all = new List<ISearchable>();
			all.AddRange(Customers);
			all.AddRange(Accounts);

			return Search(all, query);
		}

		public static List<T> Search<T>(IEnumerable<T> list, string query) where T : ISearchable
		{
			if (list == null) throw new ArgumentNullException(nameof(list));
			T[] array = list.ToArray();
			if (query == null) throw new ArgumentNullException(nameof(list));
			if (string.IsNullOrWhiteSpace(query)) return new List<T>();
			if (array.Length == 0) return new List<T>();

			var items = array.Where(x => x != null).Select(x => new
			{
				item = x,
				search = x.GetSearchQueried().ToLower()
			}).ToArray();

			string[] words = query.ToLower().Split();

			List<T> allMatches = null;

			foreach (string word in words)
			{
				var wordMatches = new List<T>();
				foreach (var aobj in items)
				{
					if (aobj.search.Contains(word))
						wordMatches.Add(aobj.item);
				}

				allMatches = allMatches?.Intersect(wordMatches).ToList() ?? wordMatches;
			}
			
			return allMatches ?? new List<T>();
		}
	}
}