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
		private int selectedIndex = 0;
		private readonly int position = Console.CursorTop;
		private bool running;

		public Element Selected => (selectedIndex >= 0 && selectedIndex < elements.Count)
			? elements[selectedIndex]
			: null;

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

		public static InputGroup RunGroup(params Element[] elements)
		{
			var group = new InputGroup();

			foreach (Element element in elements)
				group.AddElement(element);

			group.Run();

			return group;
		}

		public void Run()
		{
			if (running)
				throw new ApplicationException("Single group can only run once at a time!");

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
			Console.SetCursorPosition(0, position + elements.Count * 2 - 1);
		}

		private void Draw()
		{
			int y = position;
			int x = HORI_PADDING;

			int selectedX = -1;
			int selectedY = -1;

			Element selected = Selected; // in case it's changed during draw call

			Console.CursorVisible = false;

			foreach (Element element in elements)
			{
				if (element != selected)
					element.Redraw(x, y);
				else
				{
					selectedX = x;
					selectedY = y;
				}

				x += element.Width + HORI_PADDING;

				if (x >= Console.WindowWidth)
				{
					y += 1 + VERT_PADDING;
					x = HORI_PADDING;
				}
			}

			// Draw selected last
			if (selectedX != -1 && selectedY != -1)
				selected?.Redraw(selectedX, selectedY);
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
			if (selectedIndex < 0)
				selectedIndex = 0;
			else
				selectedIndex = (selectedIndex + 1) % elements.Count;
		}

		public void SelectPrevious()
		{
			if (selectedIndex < 0)
				selectedIndex = elements.Count - 1;
			else
				selectedIndex = (selectedIndex + elements.Count - 1) % elements.Count;
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