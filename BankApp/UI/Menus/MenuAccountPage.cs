using BankApp.BankObjects;

namespace BankApp.UI.Menus
{
	public class MenuAccountPage : IMenuItem
	{
		public string Title { get; } = "Account page";
		public bool Done { get; } = true;

		private readonly Account account;

		public MenuAccountPage(Account account)
		{
			this.account = account;
		}

		public void Run()
		{
			throw new System.NotImplementedException();
		}
	}
}