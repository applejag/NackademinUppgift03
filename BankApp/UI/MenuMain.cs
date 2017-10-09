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
		public bool Done { get; } = false;
		public bool Running { get; private set; } = true;

		private readonly InputGroup group;
		private readonly Database db;

		private readonly MainMenuItem[] mainMenus;

		private readonly Button elementSaveAndExit;
		private readonly Button elementExitWithoutSaving;

		public MenuMain()
		{
			db = MenuLoadDatabase.AskForDatabaseFile();

			mainMenus = new [] {
				new MainMenuItem(new MenuSearchForCustomer(db)),
				new MainMenuItem(new MenuCreateCustomer()),
			};

			group = new InputGroup(mainMenus.Select(m => m.button));

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
			foreach (MainMenuItem menu in mainMenus)
			{
				if (menu.button != menuButton) continue;
				
				RunMenuItem(menu.item);	
				
				return;
			}
		}

		public static void RunMenuItem(IMenuItem item)
		{
			if (item == null) return;

			do
			{
				Console.Clear();
				Console.WriteLine();
				Console.BackgroundColor = ConsoleColor.DarkGreen;
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write($" << {ToSuperUpper(item.Title)} >> ");
				Console.ResetColor();
				Console.WriteLine();
				Console.WriteLine();

				Console.Title = $"Bank app > {item.Title}";

				item.Run();
			} while (!item.Done);
		}

		private static string ToSuperUpper(string text)
		{
			var sb = new StringBuilder(text.Length * 2 - 1);

			foreach (char c in text)
				sb.Append((sb.Length > 0 ? " " : "") + char.ToUpper(c));

			return sb.ToString();
		}

		private struct MainMenuItem
		{
			public readonly IMenuItem item;
			public readonly Button button;

			public MainMenuItem(IMenuItem item)
			{
				button = new Button(item.Title);
				this.item = item;
			}
		}
	}
}