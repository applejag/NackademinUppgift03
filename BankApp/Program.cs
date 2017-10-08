using System;
using BankApp.UI;

namespace BankApp
{
	class Program
	{
		static void Main(string[] args)
		{
			var controller = new MenuMain();

			while (controller.Running)
			{
				MenuMain.RunMenuItem(controller);
			}

		}
	}
}
