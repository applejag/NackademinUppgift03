using System;
using BankApp.BankObjects;
using BankApp.UI.Elements;

namespace BankApp.UI.Menus
{
	public class MenuAccountPage : IMenuItem
	{
		public string Title { get; } = "Account page";
		public bool Done { get; set; } = true;
		private string ErrorMessage { get; set; } = null;

		private readonly Account account;
		private readonly Database db;

		private readonly InputGroup inputGroup;

		private readonly Button elementTransfer = new Button("Transfer money");
		private readonly Button elementCloseAccount = new Button("Close account");
		private readonly Button elementBack = new Button("Back to customer page");

		public MenuAccountPage(Account account, Database db)
		{
			this.account = account;
			this.db = db;

			inputGroup = new InputGroup(
				elementTransfer,
				elementCloseAccount,
				elementBack
			);
		}

		public void Run()
		{
			Done = false;

			if (!string.IsNullOrWhiteSpace(ErrorMessage))
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ErrorMessage);
				Console.WriteLine();
				ErrorMessage = null;
			}

			account.PrintProfile(db);
			Console.WriteLine();

			inputGroup.Run();
			Element selected = inputGroup.Selected;

			if (selected == elementTransfer)
			{
				// Transfer money
				Account accountTarget = MenuMain.RunMenuItem(new MenuSearchForAccount(db)).Result;

				if (accountTarget == account)
				{
					ErrorMessage = "You cannot transfer money to the same account!";
				}
				else if (accountTarget != null)
				{
					MenuMain.RunMenuItem(new MenuAccountTransfer(db, account, accountTarget));
				}
			}
			else if (selected == elementCloseAccount)
			{
				// Close account
				if (account.Money == 0)
				{
					const string title = "This is a permanent action. Are you sure?";
					const string yes = "Yes, remove the account";
					const string no = "No, cancel";

					if (UIUtilities.PromptActions(title, yes, no) == yes)
					{
						db.Accounts.Remove(account);
						Done = true;
					}
				}
				else
					ErrorMessage = "There's still funds remaining on this account.\n" +
					               "Please transfer them before closing the account.";
			}
			else if (selected == elementBack)
			{
				// Back
				Done = true;
			}
		}
	}
}