using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BankApp.Exceptions;
using BankApp.IO;
using BankApp.UI.Elements;
using JetBrains.Annotations;

namespace BankApp.UI.Menus
{
	public class MenuLoadDatabase : IMenuItem
	{
		public string Title { get; } = "Choose file";
		public bool Done => DB != null;
		public Database DB { get; private set; }

		public string Path => EscapePath(elementInput.TrimmedResult);
		public string ErrorMessage { get; set; } = null;
		
		private readonly InputGroup menuGroup;
		private readonly TextField elementInput;

		public MenuLoadDatabase()
		{
			elementInput = new TextField("Path");

			menuGroup = new InputGroup(
				elementInput
			);
		}

		private static string EscapePath(string path)
		{
			var sb = new StringBuilder(path.Length);
			char[] invalidChars = System.IO.Path.GetInvalidPathChars();

			foreach (char c in path)
			{
				if (Array.IndexOf(invalidChars, c) == -1)
					sb.Append(c);
			}

			return sb.ToString();
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

			TryGetDatabase();
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
			
			MenuMain.RunMenuItem(loadfile);

			return loadfile.DB;
		}

		private void TryGetDatabase()
		{

			if (!HasValidPath())
			{
				ErrorMessage = "Please enter a path to an existing file!";
			}

			try
			{
				DB = new Database(Path);
			}
			catch (ParseFileException e)
			{
				ErrorMessage = e.Message;
			}
			catch (Exception e)
			{
				ErrorMessage = $"Unexpected exception: {e.GetType().Name}\n{e.Message}";
			}
		}
	}
}