using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BankApp.UI.Elements;

namespace BankApp.UI
{
	public class UIUtilities
	{
		public static string PromptWarning(string title, params string[] choices)
		{
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.White;
			Console.BackgroundColor = ConsoleColor.DarkRed;
			Console.WriteLine($" {title.Trim()} ");
			Console.WriteLine();

			return InputGroup.RunGroup(choices.Select(c => new Button(c))).Selected.Name;
		}

		public static void PromptSuccess(string title, string content = null)
		{
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.White;
			Console.BackgroundColor = ConsoleColor.DarkGreen;
			Console.WriteLine($" {title.Trim()} ");

			if (string.IsNullOrWhiteSpace(content))
			{
				Console.BackgroundColor = ConsoleColor.Black;
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.WriteLine(content);
			}

			Console.WriteLine();

			InputGroup.RunGroup(new Button("OK"));
		}

		public static void PrintHeader(object title)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"[ {Convert.ToString(title, CultureInfo.InvariantCulture)} ]");
		}

		public static void PrintSegment(object title, object content)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.Write($"{Convert.ToString(title, CultureInfo.InvariantCulture)}: ");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine($"{Convert.ToString(content, CultureInfo.InvariantCulture)}");
		}
		
		public static void PrintSegment(object title)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine($"{Convert.ToString(title, CultureInfo.InvariantCulture)}");
		}
	}
}