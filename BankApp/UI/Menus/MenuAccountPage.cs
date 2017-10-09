using System;
using System.Collections.Generic;
using System.Linq;
using BankApp.BankObjects;
using BankApp.Exceptions;
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

		private readonly Button elementTransfer = new Button("Transfer money") {PaddingAbove = false};
		private readonly Button elementInsert = new Button("Insert money") {PaddingAbove = false};
		private readonly Button elementWithdraw = new Button("Withdraw money") {PaddingAbove = false};

		private readonly Button elementEditAccount = new Button("Edit account") {PaddingAbove = true};
		private readonly Button elementCloseAccount = new Button("Close account") {PaddingAbove = false};
		private readonly Button elementBack = new Button("Back to customer page") {PaddingAbove = true};

		public MenuAccountPage(Account account, Database db)
		{
			this.account = account;
			this.db = db;

			inputGroup = new InputGroup(
				elementTransfer,
				elementInsert,
				elementWithdraw,
				elementEditAccount,
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
			PrintTransactions();
			Console.WriteLine();

			inputGroup.Run();
			Element selected = inputGroup.Selected;

			if (selected == elementTransfer)
			{
				TransferMoney();
			}
			else if (selected == elementInsert)
			{
				MenuMain.RunMenuItem(new MenuAccountInsert(account, db));
			}
			else if (selected == elementWithdraw)
			{
				MenuMain.RunMenuItem(new MenuAccountWithdraw(account, db));
			}
			else if (selected == elementEditAccount)
			{
				MenuMain.RunMenuItem(new MenuAccountCreateEdit(db, account));
			}
			else if (selected == elementCloseAccount)
			{
				CloseAccount();
			}
			else if (selected == elementBack)
			{
				Done = true;
			}
		}

		private void PrintTransactions()
		{
			List<Transaction> allTransaction = account.FetchTransactions(db);
			List<Transaction> transactions = allTransaction.Take(10).ToList();
			int allCount = allTransaction.Count;
			int count = transactions.Count;

			if (count == 0)
			{
				UIUtilities.PrintHeader("Transactions (0)");
				Console.ForegroundColor = ConsoleColor.DarkYellow;
				Console.WriteLine("< no transactions >");
			}
			else
			{
				UIUtilities.PrintHeader(allCount != count
					? $"Transactions ({allCount}, showing {count})"
					: $"Transactions ({count})");

				foreach (Transaction transaction in transactions)
				{
					transaction.PrintTransaction(account);
				}
			}
		}

		private void CloseAccount()
		{
			const string title = "This is a permanent action. Are you sure?";
			const string yes = "Yes, remove the account";
			const string no = "No, cancel";

			if (UIUtilities.PromptWarning(title, yes, no) == yes)
			{
				try
				{
					db.RemoveAccount(account);
					Done = true;
				}
				catch (ApplicationException e)
				{
					ErrorMessage = e.Message;
				}
			}
		}

		private void TransferMoney()
		{
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
	}
}