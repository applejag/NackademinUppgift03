using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using BankApp.IO;

namespace BankApp.BankObjects
{
	public sealed class Customer : Identified, ISerializable, ISearchable
	{
		public string OrganisationID { get; private set; }
		public string OrganisationName { get; private set; }
		public string Address { get; private set; }
		public string City { get; private set; }
		public string Region { get; private set; }
		public string PostCode { get; private set; }
		public string Country { get; private set; }
		public string Telephone { get; private set; }

		public Customer()
		{ }

		public Customer(FileRow data) : this()
		{
			Deserialize(data);
		}

		public Customer(string data) : this(new FileRow(data))
		{ }

		public FileRow Serialize()
		{
			return new FileRow(
				ID,
				OrganisationID,
				OrganisationName,
				Address,
				City,
				Region,
				PostCode,
				Country,
				Telephone
			);
		}

		public void Deserialize(FileRow data)
		{
			ID = data.TakeUInt();
			OrganisationID = data.TakeString();
			OrganisationName = data.TakeString();
			Address = data.TakeString();
			City = data.TakeString();
			Region = data.TakeString();
			PostCode = data.TakeString();
			Country = data.TakeString();
			Telephone = data.TakeString();

			data.Close();
		}

		public string GetSearchDisplay()
		{
			return $"{ID}: {OrganisationName} ({OrganisationID})";
		}

		public string GetSearchQueried()
		{
			return string.Join("\n",
				ID,
				OrganisationID,
				OrganisationName,
				Address,
				City,
				Region,
				PostCode,
				Country
			);
		}

		public List<Account> FetchAccounts(Database db)
		{
			return db.Accounts.FindAll(a => a.CustomerID == ID);
		}

		private static void PrintSegment(object title, object content)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.Write($"{Convert.ToString(title, CultureInfo.InvariantCulture)}: ");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine($"{Convert.ToString(content, CultureInfo.InvariantCulture)}");
		}

		private string GetAddress()
		{
			var sb = new StringBuilder();
			sb.Append($"{Address}, {PostCode} {City}");

			if (!string.IsNullOrWhiteSpace(Country))
				sb.Append($", {Country}");

			if (!string.IsNullOrWhiteSpace(Region))
				sb.Append($" ({Region})");

			return sb.ToString();
		}

		public void PrintProfile(Database db)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Information");

			PrintSegment("Customer nr", ID);
			PrintSegment("Organisation", OrganisationName);
			PrintSegment("Organisation nr", OrganisationID);
			PrintSegment("Address", GetAddress());

			if (!string.IsNullOrWhiteSpace(Telephone))
				PrintSegment("Telephone", Telephone);

			// Accounts
			if (db != null)
			{
				Console.WriteLine();
				List<Account> accounts = FetchAccounts(db);

				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine($"Accounts ({accounts.Count}):");

				Console.ForegroundColor = ConsoleColor.DarkYellow;
				if (accounts.Count == 0)
				{
					Console.WriteLine("< no accounts >");
				}

				foreach (Account account in accounts)
				{
					PrintSegment(account.ID, $"{account.Money:C}");
				}

				Console.WriteLine();
				
				PrintSegment("Total balance", $"{accounts.Select(a => a.Money).Sum():C}");
			}
		}
	}
}