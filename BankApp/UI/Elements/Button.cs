using System;

namespace BankApp.UI.Elements
{
	public class Button : Element
	{
		//public override int Width => Name.Length + 4;

		public Button(string name)
			: base(name)
		{ }

		public override void OnInput(ConsoleKeyInfo info)
		{
			switch (info.Key)
			{
				case ConsoleKey.Enter:
				case ConsoleKey.Spacebar:
					Group.Submit();
					break;

				case ConsoleKey.LeftArrow:
				case ConsoleKey.UpArrow:
					Group.SelectPrevious();
					break;

				case ConsoleKey.RightArrow:
				case ConsoleKey.DownArrow:
					Group.SelectNext();
					break;
			}
		}

		protected override void OnDraw()
		{
			if (Selected)
			{
				Console.BackgroundColor = ConsoleColor.DarkCyan;
				Console.ForegroundColor = ConsoleColor.White;

				Write($"[ {Name} ]");
			}
			else
			{
				Console.BackgroundColor = ConsoleColor.Black;
				Console.ForegroundColor = ConsoleColor.Cyan;

				Write($"< {Name} >");
			}

		}
	}
}