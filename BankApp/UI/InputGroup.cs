using System;
using System.Collections.Generic;
using System.Linq;
using BankApp.UI.Elements;

namespace BankApp.UI
{
	public class InputGroup
	{
		public const int VERT_PADDING = 1;
		public const int HORI_PADDING = 2;

		private readonly List<Element> elements = new List<Element>();
		private int selectedIndex;
		private int position;
		private int bottom;
		private bool running;

		public Element Selected
		{
			get => (selectedIndex >= 0 && selectedIndex < elements.Count)
				? elements[selectedIndex]
				: null;

			set => selectedIndex = value == null || value.Disabled ? -1 : elements.IndexOf(value);
		}

		public InputGroup()
		{ }

		public InputGroup(params Element[] elements)
			: this()
		{
			foreach (Element element in elements)
				AddElement(element);
		}

		public InputGroup(IEnumerable<Element> elements)
			: this()
		{
			foreach (Element element in elements)
				AddElement(element);
		}

		public bool AddElement(Element element)
		{
			if (element == null)
				throw new ArgumentNullException(nameof(element));

			if (elements.Contains(element))
				return false;

			elements.Add(element);

			element.Group = this;
			return true;
		}

		public bool RemoveElement(Element element)
		{
			if (element == null)
				throw new ArgumentNullException(nameof(element));

			if (!elements.Remove(element)) return false;

			element.Group = null;
			return true;
		}

		public int RemoveAll(Predicate<Element> match)
		{
			if (match == null)
				throw new ArgumentNullException(nameof(match));

			return elements.RemoveAll(match);
		}

		public int RemoveAll()
		{
			int count = elements.Count;
			elements.Clear();
			return count;
		}

		public static InputGroup RunGroup(IEnumerable<Element> elements)
		{
			var group = new InputGroup(elements);

			group.Run();

			return group;
		}

		public static InputGroup RunGroup(params Element[] elements)
		{
			var group = new InputGroup(elements);

			group.Run();

			return group;
		}

		public void Run()
		{
			if (running)
				throw new ApplicationException("Single group can only run once at a time!");

			if (Selected == null && elements.Count > 0) selectedIndex = 0;
			position = Console.CursorTop;
			Draw();
			running = true;
			
			while (running)
			{
				Update();
				Draw();
			}

			// Empty the stream
			while (Console.KeyAvailable) Console.ReadKey(true);
			Console.ResetColor();
			
			Console.SetCursorPosition(0, bottom + VERT_PADDING);
		}

		private void Draw()
		{
			int y = position;
			int x = HORI_PADDING;

			int selectedX = -1;
			int selectedY = -1;

			Element selected = Selected; // in case it's changed during draw call
			Element previous = null;

			Console.CursorVisible = false;

			Console.BackgroundColor = ConsoleColor.Black;
			for (int i = elements.Count * 2 - 1; i >= 0; i--)
			{
				Console.SetCursorPosition(0, y+i);
				Console.Write(new string(' ', Console.WindowWidth));
			}

			foreach (Element element in elements)
			{
				if (element.Disabled) continue;

				if (previous != null)
				{
					if (!previous.Padding && !element.Padding)
						y += 1;
					else
						y += 1 + VERT_PADDING;
				}

				if (element != selected)
					element.Redraw(x, y);
				else
				{
					selectedX = x;
					selectedY = y;
				}

				previous = element;
			}

			// Draw selected last
			if (selectedX != -1 && selectedY != -1)
				selected?.Redraw(selectedX, selectedY);

			bottom = y;
		}

		private void Update()
		{
			do
			{
				ConsoleKeyInfo info = Console.ReadKey(true);

				if (info.Key == ConsoleKey.Tab)
				{
					if ((info.Modifiers & ConsoleModifiers.Shift) != 0)
						SelectPrevious();
					else
						SelectNext();
				}
				else
				{
					Selected?.OnInput(info);
				}

			} while (Console.KeyAvailable && running);
		}

		public void SelectNext()
		{
			foreach (Element _ in elements)
			{
				if (selectedIndex < 0)
					selectedIndex = 0;
				else
					selectedIndex = (selectedIndex + 1) % elements.Count;

				if (Selected != null && !Selected.Disabled)
					break;
			}

			if (Selected == null || Selected.Disabled)
				selectedIndex = -1;
		}

		public void SelectPrevious()
		{
			foreach (Element _ in elements)
			{
				if (selectedIndex < 0)
					selectedIndex = elements.Count - 1;
				else
					selectedIndex = (selectedIndex + elements.Count - 1) % elements.Count;

				if (Selected != null && !Selected.Disabled)
					break;
			}

			if (Selected == null || Selected.Disabled)
				selectedIndex = -1;
		}

		public void SelectNextOrSubmit()
		{
			// Is last?
			if (selectedIndex < elements.Count - 1)
				SelectNext();
			else
				Submit();
		}

		public void Submit()
		{
			running = false;
		}
	}
}