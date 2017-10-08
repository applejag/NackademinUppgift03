using BankApp.BankObjects;

namespace BankApp.UI.Menus
{
	public class MenuCustomerPage : IMenuItem
	{
		public string Title => $"Customer page for #{customer.ID}";
		private readonly Customer customer;

		public MenuCustomerPage(Customer customer)
		{
			this.customer = customer;
		}

		public void Run()
		{
			throw new System.NotImplementedException();
		}
	}
}