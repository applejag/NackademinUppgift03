using BankApp.UI.Elements;

namespace BankApp.UI.Menus
{
	public interface IMenuItem
	{
		string Title { get; }
		bool Done { get; }

		void Run();
	}
}