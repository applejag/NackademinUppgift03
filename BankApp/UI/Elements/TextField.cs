using System;

namespace BankApp.UI.Elements
{
	public class TextField : Element
	{
		public int MaxLength { get; set; } = 255;
		public int InputWidth => AvailableWidth - Name.Length - 2;

		public string TrimmedResult => Result.Trim();
		public string Result {
			get => _result;
			set {
				_result = value ?? "";
				cursor = _result.Length;
				UpdateInnerOffset();
			}
		}


		public event InputCallbackEvent Changed;
		public event InputCallbackEvent Submit;

		public delegate void InputCallbackEvent(TextField field);

		private readonly Text textMask;

		private string _result = string.Empty;
		private int cursor;

		public TextField(string name)
			: base(name)
		{
			textMask = new Text(this);
		}

		protected override void OnDraw()
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Write("{0}: ", Name);
;
			textMask.Redraw(Console.CursorLeft, Console.CursorTop);
		}

		public override void OnInput(ConsoleKeyInfo info)
		{
			string oldResult = Result;

			switch (info.Key)
			{
				case ConsoleKey.Backspace when Result.Length > 0:
					_result = Result.Substring(0, cursor - 1) + Result.Substring(cursor);
					cursor--;
					UpdateInnerOffset();
					break;

				case ConsoleKey.Delete when Result.Length > 0:
					_result = Result.Substring(0, cursor) + Result.Substring(cursor + 1);
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

					UpdateInnerOffset();
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

					UpdateInnerOffset();
					break;

				/* Cursor movement */
				case ConsoleKey.LeftArrow:
					cursor--;
					UpdateInnerOffset();
					break;

				case ConsoleKey.RightArrow when cursor < Result.Length:
					cursor++;
					UpdateInnerOffset();
					break;
					
				/* Supaa move */
				case ConsoleKey.PageDown:
				case ConsoleKey.End:
					cursor = Result.Length;
					UpdateInnerOffset();
					break;

				case ConsoleKey.PageUp:
				case ConsoleKey.Home:
					cursor = 0;
					UpdateInnerOffset();
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
					Submit?.Invoke(this);
					break;
			}

			if (!char.IsControl(info.KeyChar))
			{
				if (MaxLength <= 0 || Result.Length < MaxLength)
				{
					_result = Result.Insert(cursor, info.KeyChar.ToString());
					cursor++;
					UpdateInnerOffset();
				}
			}

			if (Result != oldResult)
				Changed?.Invoke(this);
		}

		private void UpdateInnerOffset()
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
			private readonly TextField parent;
			public int offset = 0;

			public Text(TextField parent)
			{
				this.parent = parent;
			}

			protected override void OnDraw()
			{
				Console.ForegroundColor = parent.Selected ? ConsoleColor.White : ConsoleColor.Gray;
				Write(parent.Result.Substring(Math.Min(Math.Max(-offset, 0), parent.Result.Length)) + new string(' ', AvailableWidth));
				
				if (parent.Selected)
				{
					Console.CursorLeft = Math.Min(PositionX + offset + parent.cursor, Console.WindowWidth - 1);
					Console.CursorVisible = IsCursorInside;
				}
			}
		}
	}
}