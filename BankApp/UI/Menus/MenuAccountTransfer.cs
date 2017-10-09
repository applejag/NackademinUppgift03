using BankApp.BankObjects;

namespace BankApp.UI.Menus
{
	public class MenuAccountTransfer : IMenuItem
	{
		public string Title { get; } = "Transfer money";
		public bool Done { get; } = true;

		private readonly Database db;
		private readonly Account accountSource;
		private readonly Account accountTarget;

		public MenuAccountTransfer(Database db, Account accountSource, Account accountTarget)
		{
			this.db = db;
			this.accountSource = accountSource;
			this.accountTarget = accountTarget;
		}

		public void Run()
		{
			throw new System.NotImplementedException();
		}
	}
}