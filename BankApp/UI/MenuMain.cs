using System;
using System.Linq;
using System.Text;
using BankApp.Exceptions;
using BankApp.UI.Elements;
using BankApp.UI.Menus;

namespace BankApp.UI
{
	public class MenuMain : IMenuItem
	{
		public string Title { get; } = "Main menu";
		public bool Running { get; private set; } = true;

		private readonly InputGroup group;
		private readonly Database db;

		private readonly MenuItem[] menus =
		{
			new MenuItem(new MenuSearchForCustomer()),
			new MenuItem(new MenuCreateCustomer()),
		};

		private readonly Button elementSaveAndExit;
		private readonly Button elementExitWithoutSaving;

		public MenuMain()
		{
			db = MenuLoadDatabase.AskForDatabaseFile();
			group = new InputGroup(menus.Select(m => m.button));

			elementSaveAndExit = new Button("Save and exit");
			group.AddElement(elementSaveAndExit);
			elementExitWithoutSaving = new Button("Exit without saving");
			group.AddElement(elementExitWithoutSaving);
		}

		public void Run()
		{
			db.PrintStatistics();

			Console.WriteLine();

			group.Run();

			if (group.Selected == elementSaveAndExit)
			{
				// Save and exit
				db.Save();
				Running = false;
			}
			else if (group.Selected == elementExitWithoutSaving)
			{
				var btnYes = new Button("Yes, discard changes");
				var btnNo = new Button("No, return to menu");
				InputGroup result = InputGroup.RunGroup(btnYes, btnNo);

				if (result.Selected == btnYes)
					Running = false;
			}
			else
			{
				RunSubMenu(group.Selected as Button);
			}
		}

		private void RunSubMenu(Button menuButton)
		{
			foreach (MenuItem menu in menus)
			{
				if (menu.button != menuButton) continue;
				
				RunMenuItem(menu.item);	
				
				return;
			}
		}

		public static void RunMenuItem(IMenuItem item)
		{
			if (item == null) return;

			Console.Clear();
			Console.WriteLine();
			Console.BackgroundColor = ConsoleColor.DarkGreen;
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write($" << {ToSuperUpper(item.Title)} >> ");
			Console.ResetColor();
			Console.WriteLine();
			Console.WriteLine();
			item.Run();
		}

		private static string ToSuperUpper(string text)
		{
			var sb = new StringBuilder(text.Length * 2 - 1);

			foreach (char c in text)
				sb.Append((sb.Length > 0 ? " " : "") + char.ToUpper(c));

			return sb.ToString();
		}

		private struct MenuItem
		{
			public readonly IMenuItem item;
			public readonly Button button;

			public MenuItem(IMenuItem item)
			{
				button = new Button(item.Title);
				this.item = item;
			}
		}
	}
}