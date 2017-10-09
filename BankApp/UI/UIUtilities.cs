using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BankApp.UI.Elements;

namespace BankApp.UI
{
	public class UIUtilities
	{
		public static string PromptActions(string title, params string[] choices)
		{
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.White;
			Console.BackgroundColor = ConsoleColor.DarkRed;
			Console.WriteLine(title);
			Console.WriteLine();

			return InputGroup.RunGroup(choices.Select(c => new Button(c))).Selected.Name;
		}

		public static void PrintHeader(object title)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"{Convert.ToString(title, CultureInfo.InvariantCulture)}: ");
		}

		public static void PrintSegment(object title, object content)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.Write($"{Convert.ToString(title, CultureInfo.InvariantCulture)}: ");
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine($"{Convert.ToString(content, CultureInfo.InvariantCulture)}");
		}
	}
}