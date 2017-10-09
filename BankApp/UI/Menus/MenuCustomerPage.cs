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
		private readonly Button elementBack;

		public MenuCustomerPage(Customer customer, Database db)
		{
			this.customer = customer;
			this.db = db;

			inputGroup = new InputGroup();
			elementBack = new Button("Back to main menu");
		}

		private void ResetAccountButtons()
		{
			inputGroup.RemoveAll();

			List<Account> accounts = customer.FetchAccounts(db);
			elementsAccounts = new AccountButton[accounts.Count];

			for (int i = 0; i < accounts.Count; i++)
			{
				elementsAccounts[i] = new AccountButton(accounts[i]);
				inputGroup.AddElement(elementsAccounts[i]);
			}

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
				MenuMain.RunMenuItem(new MenuAccountPage(accountButton.account));
			}
			else if (selected == elementBack)
			{
				Done = true;
			}
		}

		private class AccountButton : Button
		{
			public readonly Account account;

			public AccountButton(Account account) : base($"Account {account.ID}: {account.Money:C}")
			{
				this.account = account;
			}
		}
	}
}