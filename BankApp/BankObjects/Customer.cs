using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using BankApp.IO;
using BankApp.UI;

namespace BankApp.BankObjects
{
	public sealed class Customer : Identified, ISerializable
	{
		public string OrganisationID { get; set; }
		public string OrganisationName { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Region { get; set; }
		public string PostCode { get; set; }
		public string Country { get; set; }
		public string Telephone { get; set; }

		public Customer()
		{}

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
			return db?.Accounts.FindAll(a => a.CustomerID == ID) ?? new List<Account>();
		}

		public Account CreateAccount(Database db)
		{
			var account = new Account(this);
			db.AddAccount(account);
			return account;
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
			UIUtilities.PrintHeader("Information");

			UIUtilities.PrintSegment("Customer nr", ID);
			UIUtilities.PrintSegment("Organisation", OrganisationName);
			UIUtilities.PrintSegment("Organisation nr", OrganisationID);
			UIUtilities.PrintSegment("Address", GetAddress());

			if (!string.IsNullOrWhiteSpace(Telephone))
				UIUtilities.PrintSegment("Telephone", Telephone);

			// Accounts
			Console.WriteLine();
			List<Account> accounts = FetchAccounts(db);

			UIUtilities.PrintHeader($"Accounts ({accounts.Count})");

			if (accounts.Count == 0)
			{
				Console.ForegroundColor = ConsoleColor.DarkYellow;
				Console.WriteLine("< no accounts >");
			}
			else
			{
				UIUtilities.PrintSegment("Total balance", $"{accounts.Select(a => a.Money).Sum():C}");
			}
		}
	}
}