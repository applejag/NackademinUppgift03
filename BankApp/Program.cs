using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankApp.BankObjects;

namespace BankApp
{
	class Program
	{
		static void Main(string[] args)
		{
			const string path = @"Data/bankdata-small.txt";

			Console.WriteLine("Loading \"{0}\"...", Path.GetFullPath(path));
			Database db;
			try
			{
				db = new Database(path);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.ReadKey();
				return;
			}

			Console.WriteLine("Loading complete!");

			Console.WriteLine("\nCustomers ({0}):", db.Customers.Count);

			foreach (var cust in db.Customers)
			{
				List<Account> accounts = db.Accounts.FindAll(a => a.CustomerID == cust.ID);
				Console.WriteLine("- {0}, {1}, {2}. Money: ({3} accounts) {4} kr",
					cust.ID,
					cust.OrganisationID,
					cust.OrganisationName,
					accounts.Count,
					accounts.Select(a=>a.Money).Sum());
			}

			Console.WriteLine("\nAccounts ({0}):", db.Accounts.Count);

			foreach (var acc in db.Accounts)
			{
				Customer cust = db.Customers.Single(c => c.ID == acc.CustomerID);
				Console.WriteLine("- {0}, {1} kr, belong to {2}",
					acc.ID,
					acc.Money,
					cust.OrganisationName);
			}

			Console.ReadKey();
		}
	}
}
