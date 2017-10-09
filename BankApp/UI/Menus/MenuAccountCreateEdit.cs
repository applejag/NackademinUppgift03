using System;
using System.Globalization;
using BankApp.BankObjects;
using BankApp.UI.Elements;

namespace BankApp.UI.Menus
{
	public class MenuAccountCreateEdit : IMenuItem
	{
		public string Title { get; } = "Edit Account";
		public bool Done { get; private set; }
		public string ErrorMessage { get; private set; } = null;

		private readonly Database db;
		public Account Result { get; private set; }
		private readonly Customer customer;

		private readonly TextField elementInputSavingRate =
			new TextField("Saving yearly rate") {Validator = TextField.ValidatorNumber, PaddingAbove = false, Suffix = " %"};

		private readonly TextField elementInputCreditRate =
			new TextField("Credit yearly rate") {Validator = TextField.ValidatorNumber, PaddingAbove = false, Suffix = " %" };

		private readonly TextField elementInputCreditLimit =
			new TextField("Credit limit") {Validator = TextField.ValidatorNumber, PaddingAbove = false };

		private readonly Button elementSubmit = new Button("Submit");
		private readonly Button elementCancel = new Button("Discard") { PaddingAbove = false };
		private readonly InputGroup inputGroup;

		public MenuAccountCreateEdit(Database db, Account account)
			: this(db, account.FetchCustomer(db))
		{
			Result = account;
		}

		public MenuAccountCreateEdit(Database db, Customer customer)
		{
			this.db = db;
			this.customer = customer;

			inputGroup = new InputGroup(
				elementInputSavingRate,
				elementInputCreditRate,
				elementInputCreditLimit,
				elementSubmit,
				elementCancel
			);
			
			if (Result == null)
			{
				Result = new Account(customer);
			}

			// Transfer data from Account to fields
			elementInputSavingRate.Result = (Result.SavingRate * 100).ToString(CultureInfo.InvariantCulture);
			elementInputCreditRate.Result = (Result.CreditRate * 100).ToString(CultureInfo.InvariantCulture);
			elementInputCreditLimit.Result = Result.CreditLimit.ToString(CultureInfo.InvariantCulture);
		}

		public void Run()
		{
			Done = false;

			if (ErrorMessage != null)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ErrorMessage);
				Console.WriteLine();
				ErrorMessage = null;
			}

			UIUtilities.PrintHeader("Information");
			UIUtilities.PrintSegment("Customer", customer.GetSearchDisplay());
			UIUtilities.PrintSegment("Balance", $"{Result.Money:c}");
			Console.WriteLine();
			UIUtilities.PrintHeader("Account");

			inputGroup.Run();

			// Validate
			if (inputGroup.Selected == elementCancel)
			{
				Done = true;
			}
			else if (!ValueConverter.TryParseDecimal(elementInputSavingRate.TrimmedResult, out decimal savingRate))
			{
				inputGroup.Selected = elementInputSavingRate;
				ErrorMessage = "Failed to parse saving yearly rate! Please check your input.";
			}
			else if (savingRate < 0)
			{
				inputGroup.Selected = elementInputSavingRate;
				ErrorMessage = "Saving yearly rate cannot be negative!";
			}
			else if (!ValueConverter.TryParseDecimal(elementInputCreditRate.TrimmedResult, out decimal creditRate))
			{
				inputGroup.Selected = elementInputCreditRate;
				ErrorMessage = "Failed to parse credit yearly rate! Please check your input.";
			}
			else if (creditRate < 0)
			{
				inputGroup.Selected = elementInputCreditRate;
				ErrorMessage = "Credit yearly rate cannot be negative!";
			}
			else if (!ValueConverter.TryParseDecimal(elementInputCreditLimit.TrimmedResult, out decimal creditLimit))
			{
				inputGroup.Selected = elementInputCreditLimit;
				ErrorMessage = "Failed to parse credit limit! Please check your input.";
			}
			else if (creditLimit < 0)
			{
				inputGroup.Selected = elementInputCreditLimit;
				ErrorMessage = "Credit limit cannot be negative!";
			}
			else
			{
				// Transfer data from fields to Account
				Result.SavingRate = savingRate * 0.01m;
				Result.CreditRate = creditRate * 0.01m;
				Result.CreditLimit = creditLimit;

				// If we're editing existing Account
				if (!db.Accounts.Contains(Result))
				{
					db.AddAccount(Result);
					MenuMain.RunMenuItem(new MenuAccountPage(Result, db));
				}

				Done = true;
			}
		}
	}
}