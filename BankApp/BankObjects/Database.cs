using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BankApp.IO;
using BankApp.UI;

namespace BankApp.BankObjects
{
	public class Database : IDatabase
	{
		public List<Customer> Customers { get; }
		public List<Account> Accounts { get; }
		public List<Transaction> Transactions { get; }


		public Database(string path)
		{
			using (var file = new FileReader(path))
			{
				Customers = file.ReadSerializeableGroup<Customer>();
				Accounts = file.ReadSerializeableGroup<Account>();
				Transactions = file.ReadSerializeableGroup<Transaction>();
			}
		}

		public void Save()
		{
			Save(path: $@"Data\{DateTime.Now:yyyyMMdd-HHmm}.txt");
		}

		public void Save(string path)
		{
			using (var file = new FileWriter(path))
			{
				file.WriteSerializeableGroup(Customers);
				file.WriteSerializeableGroup(Accounts);
				file.WriteSerializeableGroup(Transactions);
			}
		}

		public List<Customer> SearchCustomers(string query)
		{
			return Search(Customers, query, customer => customer.GetSearchQueried()).OrderBy(i => i.ID).ToList();
		}

		public List<Account> SearchAccounts(string query)
		{
			return Search(Accounts, query, account => account.GetSearchQueried(this)).OrderBy(i => i.ID).ToList();
		}

		public static List<T> Search<T>(IEnumerable<T> list, string query, Func<T, string> selector)
		{
			// Errors
			if (list == null) throw new ArgumentNullException(nameof(list));
			T[] array = list.ToArray();
			if (query == null) throw new ArgumentNullException(nameof(list));
			if (string.IsNullOrWhiteSpace(query)) return new List<T>();
			if (array.Length == 0) return new List<T>();

			// Convert to anon-object array
			var items = array.Where(x => x != null).Select(x => new
			{
				item = x,
				search = selector(x).ToLower()
			}).ToArray();

			// Search algo
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

		public void PrintStatistics()
		{
			UIUtilities.PrintHeader("Statistics");
			UIUtilities.PrintSegment("Nr of customers", Customers.Count);
			UIUtilities.PrintSegment("Nr of accounts", Accounts.Count);
			UIUtilities.PrintSegment("Nr of transactions", Transactions.Count);
			UIUtilities.PrintSegment("Bank total balance",$"{Accounts.Select(a => a.Money).Sum():c}");
		}

		public void AddCustomer(Customer customer)
		{
			customer.GenerateUniqueID(Customers);
			
			Customers.Add(customer);
			customer.CreateAccount(this);
		}

		public void AddAccount(Account account)
		{
			// All accounts throughout the history of transactions
			account.GenerateUniqueID(
				Accounts.Select(a => a.ID)
					.Union(Transactions.Select(t => t.AccountID))
					.Union(Transactions.Where(t => t.Type == Transaction.TransactionType.Transfer)
						.Select(t => t.OtherAccountID))
			);

			if (Customers.All(c => c.ID != account.CustomerID))
			{
				throw new ApplicationException("Account has no customer parent!");
			}
			else
			{
				Accounts.Add(account);
			}
		}

		public void AddTransaction(Transaction transaction)
		{
			transaction.GenerateUniqueID(Transactions);

			if (Accounts.All(a => a.ID != transaction.AccountID))
			{
				throw new ApplicationException("Transaction has no source account!");
			}
			else if (transaction.Type == Transaction.TransactionType.Transfer
			         && Accounts.All(a => a.ID != transaction.OtherAccountID))
			{
				throw new ApplicationException("Transaction has no target account!");
			}
			else
			{
				Transactions.Add(transaction);
			}
		}

		public void RemoveCustomer(Customer customer)
		{
			if (!Customers.Contains(customer))
			{
				throw new ApplicationException("Customer doesn't exist in the database!");
			}
			else if (Math.Round(customer.FetchAccounts(this).Select(a => a.Money).DefaultIfEmpty(0m).Sum(), 2) != 0)
			{
				throw new ApplicationException("There's still money on the customers accounts!");
			}
			else
			{
				Accounts.RemoveAll(a => a.CustomerID == customer.ID);
				Customers.Remove(customer);
			}
		}

		public void RemoveAccount(Account account)
		{
			if (!Accounts.Contains(account))
			{
				throw new ApplicationException("Account doesn't exist in the database!");
			}
			else if (Math.Round(account.Money, 2) != 0)
			{
				throw new ApplicationException("There's still money on this account.");
			}
			else
			{
				Accounts.Remove(account);
			}
		}
	}
}