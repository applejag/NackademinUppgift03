using System;
using System.Collections.Generic;
using System.Linq;
using BankApp.BankObjects;
using BankApp.UI.Elements;

namespace BankApp.UI.Menus
{
	public class MenuSearchForAccount : IMenuItem
	{
		private const int RESULTS_COUNT = 9;
		public string Title { get; } = "Search for account";
		public bool Done { get; } = true;

		public Account Result { get; private set; }

		private readonly InputGroup inputGroup;
		private readonly TextField elementInput;
		private readonly Database db;
		private readonly AccountButton[] elementsAccounts;
		private readonly Button elementCancel;

		public MenuSearchForAccount(Database db)
		{
			this.db = db;
			inputGroup = new InputGroup();
			elementInput = new TextField("Search");
			elementCancel = new Button("Cancel");

			inputGroup.AddElement(elementInput);
			elementInput.Changed += PrintResults;

			elementsAccounts = new AccountButton[RESULTS_COUNT];
			for (var i = 0; i < elementsAccounts.Length; i++)
			{
				elementsAccounts[i] = new AccountButton {Disabled = true, Padding = false};
				inputGroup.AddElement(elementsAccounts[i]);
			}

			inputGroup.AddElement(elementCancel);
		}

		~MenuSearchForAccount()
		{
			elementInput.Changed -= PrintResults;
		}

		public void Run()
		{
			// Reset
			Result = null;
			elementInput.Result = string.Empty;
			inputGroup.Selected = elementInput;

			foreach (AccountButton btn in elementsAccounts)
				btn.Disabled = true;

			// Run
			inputGroup.Run();

			Element selected = inputGroup.Selected;
			if (selected is AccountButton accountButton)
			{
				Result = accountButton.account;
			}

		}

		private void PrintResults(TextField field)
		{
			List<Account> searchResults = db.SearchAccounts(field.Result);
			
			for (int i = 0; i < elementsAccounts.Length; i++)
			{
				if (i < searchResults.Count)
				{
					Account account = searchResults[i];

					elementsAccounts[i].Disabled = false;
					elementsAccounts[i].Name = account.GetSearchDisplay(db);
					elementsAccounts[i].account = account;
				}
				else
				{
					elementsAccounts[i].Disabled = true;
				}
			}
		}

		private class AccountButton : Button
		{
			public Account account;

			public AccountButton() : base("")
			{}
		}
	}
}