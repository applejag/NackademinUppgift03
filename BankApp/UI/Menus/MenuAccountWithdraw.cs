using System;
using System.Globalization;
using BankApp.BankObjects;
using BankApp.Exceptions;
using BankApp.UI.Elements;

namespace BankApp.UI.Menus
{
	public class MenuAccountWithdraw : IMenuItem
	{
		public string Title { get; } = "Withdraw money";
		public bool Done => ErrorMessage == null;
		public string ErrorMessage { get; set; }

		private readonly Account account;
		private readonly Database db;

		private readonly InputGroup inputGroup;

		private readonly TextField elementInput = new TextField("Amount") {Validator = TextField.ValidatorNumber};
		private readonly Button elementOK = new Button("Withdraw");
		private readonly Button elementCancel = new Button("Cancel");

		public MenuAccountWithdraw(Account account, Database db)
		{
			this.db = db;
			this.account = account;

			inputGroup = new InputGroup(
				elementInput,
				elementOK,
				elementCancel
			);
		}

		public void Run()
		{
			if (ErrorMessage != null)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ErrorMessage);
				Console.WriteLine();
				ErrorMessage = null;
			}

			account.PrintProfile(db);
			Console.WriteLine();

			UIUtilities.PrintHeader("Withdraw money");

			inputGroup.Run();
			Element selected = inputGroup.Selected;

			if (selected == elementOK)
			{
				string result = elementInput.TrimmedResult.Replace(',','.');
				if (decimal.TryParse(result, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
					CultureInfo.InvariantCulture, out decimal amount))
				{
					try
					{
						account.WithdrawMoney(amount, db);

						UIUtilities.PromptSuccess($"Successfully withdrew {amount:C}");
					}
					catch (AccountWithdrawException e)
					{
						ErrorMessage = e.Message;
					}
				}
				else
				{
					ErrorMessage = "Unable to parse number! Please check your input.";
				}
			}
		}
	}
}