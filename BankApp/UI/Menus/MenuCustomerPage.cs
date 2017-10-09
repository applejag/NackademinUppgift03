using System;
using System.Collections.Generic;
using BankApp.BankObjects;
using BankApp.UI.Elements;

namespace BankApp.UI.Menus
{
	public class MenuCustomerPage : IMenuItem
	{
		public string Title { get; } = "Customer page";
		public bool Done { get; private set; }

		private readonly Database db;
		private readonly Customer customer;
		private readonly InputGroup inputGroup;

		private AccountButton[] elementsAccounts;
		private readonly Button elementEditCustomer = new Button("Edit customer");
		private readonly Button elementNewAccount = new Button("Open new account");
		private readonly Button elementBack = new Button("Back to main menu");

		public MenuCustomerPage(Database db)
			: this(MenuMain.RunMenuItem(new MenuSearchForCustomer(db)).Result, db)
		{}

		public MenuCustomerPage(Customer customer, Database db)
		{
			this.customer = customer;
			this.db = db;

			inputGroup = new InputGroup();
		}

		private void ResetAccountButtons()
		{
			inputGroup.RemoveAll();

			List<Account> accounts = customer.FetchAccounts(db);
			elementsAccounts = new AccountButton[accounts.Count];

			for (int i = 0; i < accounts.Count; i++)
			{
				elementsAccounts[i] = new AccountButton(accounts[i]) {Padding = false};
				inputGroup.AddElement(elementsAccounts[i]);
			}

			inputGroup.AddElement(elementEditCustomer);
			inputGroup.AddElement(elementNewAccount);
			inputGroup.AddElement(elementBack);
		}

		public void Run()
		{
			Done = false;

			customer.PrintProfile(db);
			Console.WriteLine();

			ResetAccountButtons();

			inputGroup.Run();

			Element selected = inputGroup.Selected;

			if (selected is AccountButton accountButton)
			{
				// Open account
				MenuMain.RunMenuItem(new MenuAccountPage(accountButton.account, db));
			}
			else if (selected == elementEditCustomer)
			{
				// Edit customer
				MenuMain.RunMenuItem(new MenuCreateCustomer(db, customer));
			}
			else if (selected == elementNewAccount)
			{
				// Create account
				var account = new Account(customer);
				account.GenerateUniqueID(db.Accounts);

				db.Accounts.Add(account);
				Done = false;
			}
			else if (selected == elementBack)
			{
				// Back
				Done = true;
			}
		}

		private class AccountButton : Button
		{
			public readonly Account account;

			public AccountButton(Account account) : base("")
			{
				this.account = account;
			}

			protected override void OnDraw()
			{
				if (Selected)
				{
					Console.BackgroundColor = ConsoleColor.DarkCyan;
					Console.ForegroundColor = ConsoleColor.White;

					Write("[ ");
					WriteAccount();
					Write(" ]");
				}
				else
				{
					Console.BackgroundColor = ConsoleColor.Black;
					Console.ForegroundColor = ConsoleColor.Cyan;

					Write("< ");
					WriteAccount();
					Write(" >");
				}
			}

			private void WriteAccount()
			{
				ConsoleColor fg = Console.ForegroundColor;

				Write("Account ");
				Console.ForegroundColor = Selected ? ConsoleColor.Black : ConsoleColor.DarkYellow;
				Write(account.ID);
				Console.ForegroundColor = Selected ? ConsoleColor.White : ConsoleColor.Green;
				Write($" {account.Money:C}");

				Console.ForegroundColor = fg;
			}
		}
	}
}