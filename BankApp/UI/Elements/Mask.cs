using System;
using JetBrains.Annotations;

namespace BankApp.UI.Elements
{
	public abstract class Mask
	{
		protected abstract void OnDraw();
		
		public int AvailableWidth => Console.WindowWidth - InputGroup.HORI_PADDING - PositionX;
		public int RemainingWidth => AvailableWidth - Console.CursorLeft + PositionX;

		public bool IsCursorInside =>
			Console.CursorLeft >= PositionX
			&& Console.CursorLeft < PositionX + AvailableWidth
			&& Console.CursorTop == PositionY;

		public int PositionX { get; private set; }
		public int PositionY { get; private set; }

		public void Redraw(int x, int y)
		{
			Console.BackgroundColor = ConsoleColor.Black;
			Console.SetCursorPosition(x, y);
			Console.Write(new string(' ', Console.WindowWidth - Console.CursorLeft));

			Console.ResetColor();
			Console.SetCursorPosition(x, y);
			PositionX = x;
			PositionY = y;
			OnDraw();
		}

		public void Clear()
		{
			int x = Console.CursorLeft;
			int y = Console.CursorTop;
			Console.SetCursorPosition(PositionX, PositionY);
			Console.Write(new string(' ', AvailableWidth));
			Console.SetCursorPosition(x, y);
		}

		private void WriteSingleLine(string text)
		{
			if (Console.CursorTop != PositionY) return;

			if (RemainingWidth <= 0) return;

			// Too far to the left
			if (Console.CursorLeft < PositionX)
			{
				int farLeft = PositionX - Console.CursorLeft;

				// Won't even be visible
				if (text.Length < farLeft)
					return;

				text = text.Substring(farLeft);
				Console.CursorLeft = PositionX;
			}

			text = text.Substring(0, Math.Min(RemainingWidth, text.Length));

			Console.Write(text);
		}

		public void Write(string text)
		{
			if (string.IsNullOrEmpty(text)) return;
			bool first = true;

			foreach (string row in text.Split('\n'))
			{
				if (!first)
				{
					Console.CursorTop++;
					Console.CursorLeft = PositionX;
				}

				WriteSingleLine(row);
				first = false;
			}
		}

		public void Write(object value)
		{
			Write(text: value.ToString());
		}

		[StringFormatMethod("format")]
		public void Write(string format, params object[] args)
		{
			Write(text: string.Format(format, args));
		}

	}
}