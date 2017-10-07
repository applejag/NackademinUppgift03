using System;

namespace BankApp.UI.Elements
{
	public class TextField : Element
	{
		public string Result { get; set; } = string.Empty;
		public int MaxLength { get; set; } = 255;
		public int InputWidth => Width - Name.Length - 2;

		private readonly Text textMask;

		private int cursor;

		public TextField(string name)
			: base(name)
		{
			textMask = new Text(this);
		}

		protected override void OnDraw()
		{

			Write("{0}: ", Name);
;
			textMask.Redraw(Console.CursorLeft, Console.CursorTop);

			if (Selected)
			{
				Console.CursorLeft = Math.Min(textMask.PositionX + textMask.offset + cursor, Console.WindowWidth - 1);
				Console.CursorVisible = IsCursorInside;
			}
		}

		public override void OnInput(ConsoleKeyInfo info)
		{
			switch (info.Key)
			{
				case ConsoleKey.Backspace when Result.Length > 0:
					Result = Result.Substring(0, cursor - 1) + Result.Substring(cursor);
					cursor--;
					UpdateCarret();
					break;

				case ConsoleKey.Delete when Result.Length > 0:
					Result = Result.Substring(0, cursor) + Result.Substring(cursor + 1);
					break;

				/* Cursor block movement */
				case ConsoleKey.LeftArrow when info.Modifiers.HasFlag(ConsoleModifiers.Control):
					int len = Result.Length;
					cursor--;
					for (int i = cursor; i > 0; i--)
					{
						if (i < len && !char.IsLetter(Result[i]))
							break;
						cursor--;
					}

					UpdateCarret();
					break;

				case ConsoleKey.RightArrow when cursor < Result.Length && info.Modifiers.HasFlag(ConsoleModifiers.Control):
					len = Result.Length;
					cursor++;
					for (int i = cursor; i < len; i++)
					{
						if (i < len && !char.IsLetter(Result[i]))
							break;
						cursor++;
					}

					UpdateCarret();
					break;

				/* Cursor movement */
				case ConsoleKey.LeftArrow:
					cursor--;
					UpdateCarret();
					break;

				case ConsoleKey.RightArrow when cursor < Result.Length:
					cursor++;
					UpdateCarret();
					break;
					
				/* Supaa move */
				case ConsoleKey.PageDown:
				case ConsoleKey.End:
					cursor = Result.Length;
					UpdateCarret();
					break;

				case ConsoleKey.PageUp:
				case ConsoleKey.Home:
					cursor = 0;
					UpdateCarret();
					break;

				/* Element navigation */
				case ConsoleKey.UpArrow:
					Group.SelectPrevious();
					break;

				case ConsoleKey.DownArrow:
					Group.SelectNext();
					break;

				case ConsoleKey.Enter:
					Group.SelectNextOrSubmit();
					break;
			}

			if (!char.IsControl(info.KeyChar))
			{
				if (MaxLength <= 0 || Result.Length < MaxLength)
				{
					Result = Result.Insert(cursor, info.KeyChar.ToString());
					cursor++;
					UpdateCarret();
				}
			}
		}

		private void UpdateCarret()
		{
			// Too far to the right?
			if (cursor >= InputWidth - textMask.offset)
				textMask.offset = InputWidth - cursor - 1;

			// Too far to the left?
			if (cursor + textMask.offset <= 0)
				textMask.offset = -cursor;
		}

		private class Text : Mask
		{
			public override int Width => parent.InputWidth;
			private readonly TextField parent;
			public int offset = 0;

			public Text(TextField parent)
			{
				this.parent = parent;
			}

			protected override void OnDraw()
			{
				Console.ForegroundColor = parent.Selected ? ConsoleColor.White : ConsoleColor.Gray;
				Write(parent.Result.Substring(Math.Min(Math.Max(-offset, 0), parent.Result.Length)) + new string(' ', Width));
			}
		}
	}
}