using System;
using System.Linq;
using System.Text;
using BankApp.BankObjects;
using BankApp.Exceptions;
using BankApp.UI.Elements;
using BankApp.UI.Menus;

namespace BankApp.UI
{
	public class MenuMain : IMenuItem
	{
		public string Title { get; } = "Main menu";
		public bool Done { get; private set; } = false;

		private readonly InputGroup inputGroup;
		private readonly Database db;

		private readonly MainMenuItem[] mainMenus;

		private readonly Button elementSaveAndExit = new Button("Save and exit");
		private readonly Button elementExitWithoutSaving = new Button("Exit without saving");

		public MenuMain()
		{
			db = MenuLoadDatabase.AskForDatabaseFile();

			mainMenus = new [] {
				new MainMenuItem("Open customer page", () => RunMenuItem(new MenuCustomerPage(db))),
				new MainMenuItem(new MenuCreateCustomer(db)),
			};

			inputGroup = new InputGroup(mainMenus.Select(m => m.button));

			inputGroup.AddElement(elementSaveAndExit);
			inputGroup.AddElement(elementExitWithoutSaving);
		}

		public void Run()
		{
			Done = false;
			db.PrintStatistics();

			Console.WriteLine();

			inputGroup.Run();

			if (inputGroup.Selected == elementSaveAndExit)
			{
				// Save and exit
				db.Save();
				Done = true;
			}
			else if (inputGroup.Selected == elementExitWithoutSaving)
			{
				const string title = "Unsaved changes will be lost, are you sure?";
				const string yes = "Yes, discard changes";
				const string no = "No, back to main menu";

				if (UIUtilities.PromptWarning(title, yes, no) == yes)
					Done = true;
			}
			else
			{
				RunSubMenu(inputGroup.Selected as Button);
			}
		}

		private void RunSubMenu(Button menuButton)
		{
			foreach (MainMenuItem menu in mainMenus)
			{
				if (menu.button != menuButton) continue;

				menu.onSubmit?.Invoke();
				
				return;
			}
		}

		public static T RunMenuItem<T>(T item) where T : class, IMenuItem
		{
			if (item == null) return null;

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

			return item;
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
			public readonly Action onSubmit;
			public readonly Button button;

			public MainMenuItem(IMenuItem item)
				: this(item.Title, () => RunMenuItem(item))
			{}

			public MainMenuItem(string title, Action onSubmit)
			{
				button = new Button(title);
				this.onSubmit = onSubmit;
			}

		}
	}
}