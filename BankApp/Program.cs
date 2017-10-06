using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankApp.BankObjects;
using BankApp.UI;
using BankApp.UI.Elements;

namespace BankApp
{
	class Program
	{
		static void Main(string[] args)
		{
			Element[] elements = {
				new Button("hello1"),
				new Button("hello2"),
			};

			InputGroup.RunGroup(elements);
			Console.Clear();

			Console.ReadKey();
		}

		static void TestRead()
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

			Console.WriteLine();
			Console.WriteLine("Customers ({0}):", db.Customers.Count);
			Console.WriteLine();

			foreach (var cust in db.Customers)
			{
				List<Account> accounts = db.Accounts.FindAll(a => a.CustomerID == cust.ID);

				Console.WriteLine("Customer {0}: {2} ({1})",
					cust.ID,
					cust.OrganisationID,
					cust.OrganisationName);


				foreach (var acc in accounts)
				{
					Console.WriteLine("- Account {0}: {1} kr",
						acc.ID,
						acc.Money);
				}

				Console.WriteLine("- Total money: {0} kr",
					accounts.Select(a => a.Money).Sum());

				Console.WriteLine();
			}
		}
	}
}
