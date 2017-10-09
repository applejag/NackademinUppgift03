using System;
using System.Collections.Generic;
using System.Linq;
using BankApp.BankObjects;
using BankApp.UI.Elements;

namespace BankApp.UI.Menus
{
	public class MenuSearchForCustomer : IMenuItem
	{
		private const int RESULTS_COUNT = 9;
		public string Title { get; } = "Open customer page";
		public bool Done { get; } = true;

		private readonly InputGroup inputGroup;
		private readonly TextField elementInput;
		private readonly Database db;
		private readonly CustomerButton[] elementsCustomers;
		private readonly Button elementCancel;

		public MenuSearchForCustomer(Database db)
		{
			this.db = db;
			inputGroup = new InputGroup();
			elementInput = new TextField("Search");
			elementCancel = new Button("Cancel");

			inputGroup.AddElement(elementInput);
			elementInput.Changed += PrintResults;

			elementsCustomers = new CustomerButton[RESULTS_COUNT];
			for (var i = 0; i < elementsCustomers.Length; i++)
			{
				elementsCustomers[i] = new CustomerButton {Disabled = true};
				inputGroup.AddElement(elementsCustomers[i]);
			}

			inputGroup.AddElement(elementCancel);
		}

		~MenuSearchForCustomer()
		{
			elementInput.Changed -= PrintResults;
		}

		public void Run()
		{
			// Reset
			elementInput.Result = string.Empty;
			inputGroup.Selected = elementInput;

			foreach (CustomerButton btn in elementsCustomers)
				btn.Disabled = true;

			// Run
			inputGroup.Run();

			Element selected = inputGroup.Selected;
			if (selected is CustomerButton customerButton)
			{
				MenuMain.RunMenuItem(new MenuCustomerPage(customerButton.customer, db));
			}

		}

		private void PrintResults(TextField field)
		{
			List<Customer> searchResults = db.SearchCustomers(field.Result);
			
			for (int i = 0; i < elementsCustomers.Length; i++)
			{
				if (i < searchResults.Count)
				{
					Customer customer = searchResults[i];

					elementsCustomers[i].Disabled = false;
					elementsCustomers[i].Name = customer.GetSearchDisplay();
					elementsCustomers[i].customer = customer;
				}
				else
				{
					elementsCustomers[i].Disabled = true;
				}
			}
		}

		private class CustomerButton : Button
		{
			public Customer customer;

			public CustomerButton() : base("")
			{}
		}
	}
}