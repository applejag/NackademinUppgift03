namespace BankApp.UI.Menus
{
	public class MenuCreateCustomer : IMenuItem
	{
		public string Title { get; } = "Create customer";
		public bool Done { get; } = true;

		public void Run()
		{
			throw new System.NotImplementedException();
		}
	}
}