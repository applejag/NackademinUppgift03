using BankApp.UI.Elements;

namespace BankApp.UI.Menus
{
	public interface IMenuItem
	{
		string Title { get; }

		void Run();
	}
}