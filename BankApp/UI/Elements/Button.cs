using System;

namespace BankApp.UI.Elements
{
	public class Button : Element
	{
		public override object Result { get; } = null;

		public Button(string name)
			: base(name)
		{ }

		public override void OnInput(ConsoleKeyInfo info)
		{
			switch (info.Key)
			{
				case ConsoleKey.Enter:
				case ConsoleKey.Spacebar:
					this.Group.SelectNextOrSubmit();
					break;

				case ConsoleKey.LeftArrow:
				case ConsoleKey.UpArrow:
					this.Group.SelectPrevious();
					break;

				case ConsoleKey.RightArrow:
				case ConsoleKey.DownArrow:
					this.Group.SelectNext();
					break;
			}
		}

		public override void Draw()
		{
			Console.BackgroundColor = ConsoleColor.DarkCyan;
			Console.ForegroundColor = ConsoleColor.White;

			Console.Write(this.Selected
				? $"[ {this.Name} ]"
				: $"  {this.Name}  ");
		}
	}
}