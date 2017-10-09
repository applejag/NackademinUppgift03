using BankApp.BankObjects;
using BankApp.UI.Elements;

namespace BankApp.UI.Menus
{
	public class MenuCreateCustomer : IMenuItem
	{
		public string Title { get; } = "Edit customer";
		public bool Done { get; private set; }

		private readonly Database db;
		public Customer Result { get; private set; }

		private readonly TextField elementInputOrgName = new TextField("Organisation name");
		private readonly TextField elementInputOrgID = new TextField("Organisation nr");
		private readonly TextField elementInputAddress = new TextField("Address");
		private readonly TextField elementInputCity = new TextField("City");
		private readonly TextField elementInputPostCode = new TextField("Post code");
		private readonly TextField elementInputCountry = new TextField("Country");
		private readonly TextField elementInputTelephone = new TextField("Telephone");

		private readonly Button elementSubmit = new Button("Submit");
		private readonly Button elementCancel = new Button("Discard");
		private readonly InputGroup inputGroup;

		public MenuCreateCustomer(Database db, Customer customer)
			: this(db)
		{
			this.Result = customer;
		}

		public MenuCreateCustomer(Database db)
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
		}

		public void Run()
		{
			Done = true;

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

			inputGroup.Run();

			// Transfer data from fields to customer
			Result.OrganisationName = elementInputOrgName.TrimmedResult;
			Result.OrganisationID = elementInputOrgID.TrimmedResult;
			Result.Address = elementInputAddress.TrimmedResult;
			Result.City = elementInputCity.TrimmedResult;
			Result.PostCode = elementInputPostCode.TrimmedResult;
			Result.Country = elementInputCountry.TrimmedResult;
			Result.Telephone = elementInputTelephone.TrimmedResult;

			// Validate
			if (inputGroup.Selected == elementCancel)
			{
				Done = true;
			}
			else if (string.IsNullOrWhiteSpace(Result.OrganisationName))
			{
				inputGroup.Selected = elementInputOrgName;
				Done = false;
			}
			else if (string.IsNullOrWhiteSpace(Result.OrganisationID))
			{
				inputGroup.Selected = elementInputOrgID;
				Done = false;
			}
			else
			{
				// If we're editing existing customer
				if (!db.Customers.Contains(Result))
					db.AddCustomer(Result);

				Result = null;
			}
		}
	}
}