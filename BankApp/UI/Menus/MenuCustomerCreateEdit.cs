using System;
using BankApp.BankObjects;
using BankApp.UI.Elements;

namespace BankApp.UI.Menus
{
	public class MenuCustomerCreateEdit : IMenuItem
	{
		public string Title => Result?.ID > 0 ? "Edit customer" : "Create customer";
		public bool Done { get; private set; }
		public string ErrorMessage { get; private set; } = null;

		private readonly Database db;
		public Customer Result { get; private set; }

		private readonly TextField elementInputOrgName = new TextField("Organisation name") {PaddingAbove = false};
		private readonly TextField elementInputOrgID = new TextField("Organisation nr") {Validator = TextField.ValidatorNumber, PaddingAbove = false};
		private readonly TextField elementInputAddress = new TextField("Address") { PaddingAbove = false };
		private readonly TextField elementInputCity = new TextField("City") { PaddingAbove = false };
		private readonly TextField elementInputPostCode = new TextField("Post code") {Validator = TextField.ValidatorNumber, PaddingAbove = false};
		private readonly TextField elementInputCountry = new TextField("Country") { PaddingAbove = false };
		private readonly TextField elementInputTelephone = new TextField("Telephone") { PaddingAbove = false };

		private readonly Button elementSubmit = new Button("Submit");
		private readonly Button elementCancel = new Button("Discard") { PaddingAbove = false };
		private readonly InputGroup inputGroup;

		public MenuCustomerCreateEdit(Database db, Customer customer)
			: this(db)
		{
			this.Result = customer;
		}

		public MenuCustomerCreateEdit(Database db)
		{
			this.db = db;

			inputGroup = new InputGroup(
				elementInputOrgName,
				elementInputOrgID,
				elementInputAddress,
				elementInputCity,
				elementInputPostCode,
				elementInputCountry,
				elementInputTelephone,
				elementSubmit,
				elementCancel
			);

			if (Result == null)
			{
				Result = new Customer();
			}

			// Transfer data from customer to fields
			elementInputOrgName.Result = Result.OrganisationName;
			elementInputOrgID.Result = Result.OrganisationID;
			elementInputAddress.Result = Result.Address;
			elementInputCity.Result = Result.City;
			elementInputPostCode.Result = Result.PostCode;
			elementInputCountry.Result = Result.Country;
			elementInputTelephone.Result = Result.Telephone;
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

			inputGroup.Run();

			// Validate
			if (inputGroup.Selected == elementCancel)
			{
				Done = true;
			}
			else if (string.IsNullOrWhiteSpace(elementInputOrgName.Result))
			{
				inputGroup.Selected = elementInputOrgName;
				ErrorMessage = "Organisation name cannot be left empty!";
			}
			else if (string.IsNullOrWhiteSpace(elementInputOrgID.Result))
			{
				inputGroup.Selected = elementInputOrgID;
				ErrorMessage = "Organisation number cannot be left empty!";
			}
			else
			{
				// Transfer data from fields to customer
				Result.OrganisationName = elementInputOrgName.TrimmedResult;
				Result.OrganisationID = elementInputOrgID.TrimmedResult;
				Result.Address = elementInputAddress.TrimmedResult;
				Result.City = elementInputCity.TrimmedResult;
				Result.PostCode = elementInputPostCode.TrimmedResult;
				Result.Country = elementInputCountry.TrimmedResult;
				Result.Telephone = elementInputTelephone.TrimmedResult;

				// If we're editing existing customer
				if (!db.Customers.Contains(Result))
				{
					db.AddCustomer(Result);
					MenuMain.RunMenuItem(new MenuCustomerPage(Result, db));
				}

				Done = true;
			}
		}
	}
}