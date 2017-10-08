using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BankApp.Exceptions;
using BankApp.IO;
using BankApp.UI.Elements;
using JetBrains.Annotations;

namespace BankApp.UI.Menus
{
	public class MenuLoadDatabase : IMenuItem
	{
		public string Title { get; } = "Choose file";

		public string Path => elementInput.Result;
		public string ErrorMessage { get; set; }
		
		private readonly InputGroup menuGroup;
		private readonly TextField elementInput;

		public MenuLoadDatabase()
		{
			elementInput = new TextField("Path");

			menuGroup = new InputGroup(
				elementInput
			);
		}

		public void Run()
		{

			if (!string.IsNullOrEmpty(ErrorMessage))
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ErrorMessage);
				Console.WriteLine();
			}

			List<string> files = PrintFilesInCurrentDirectory();
			elementInput.Result = FileReader.GetPathToLatestDateTimed(files);

			menuGroup.Run();
		}

		private static List<string> PrintFilesInCurrentDirectory()
		{
			List<string> files = FileReader.GetFilesInDirectory(Environment.CurrentDirectory);
			if (files.Count <= 0) return files;

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Suggested paths:");

			Console.ForegroundColor = ConsoleColor.DarkYellow;
			foreach (string file in files.Skip(Math.Max(0, files.Count - 7)))
			{
				Console.WriteLine(file);
			}

			Console.WriteLine();

			return files;
		}

		public bool HasValidPath()
		{
			try
			{
				return File.Exists(Path);
			}
			catch
			{
				return false;
			}
		}
		
		public static Database AskForDatabaseFile()
		{
			var loadfile = new MenuLoadDatabase();

			while (true)
			{
				MenuMain.RunMenuItem(loadfile);

				if (!loadfile.HasValidPath())
				{
					loadfile.ErrorMessage = "Please enter a path to an existing file!";
					continue;
				}

				try
				{
					return new Database(loadfile.Path);
				}
				catch (ParseFileException e)
				{
					loadfile.ErrorMessage = e.Message;
				}
				catch (Exception e)
				{
					loadfile.ErrorMessage = $"Unexpected exception: {e.GetType().Name}\n{e.Message}";
				}
			}

		}
	}
}