using System;
using System.Collections.Generic;
using System.Linq;
using BankApp.UI.Elements;

namespace BankApp.UI
{
	public class InputGroup
	{
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

			if (this.elements.Contains(element))
				return false;

			this.elements.Add(element);

			element.Group = this;
			return true;
		}

		public bool RemoveElement(Element element)
		{
			if (element == null)
				throw new ArgumentNullException(nameof(element));

			if (!this.elements.Remove(element)) return false;

			element.Group = null;
			return true;
		}

		public static void RunGroup(params Element[] elements)
		{
			var group = new InputGroup();

			foreach (Element element in elements)
				group.AddElement(element);

			group.Run();
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
		}

		private void Draw()
		{
			for (var i = 0; i < this.elements.Count; i++)
			{
				if (this.selectedIndex == i) continue;

				Console.ResetColor();
				Console.SetCursorPosition(0, this.position + i * 2);
				this.elements[i].Draw();
			}

			if (this.Selected != null)
			{
				Console.ResetColor();
				Console.SetCursorPosition(0, this.position + this.selectedIndex * 2);
				this.Selected.Draw();
			}
		}

		private void Update()
		{
			do
			{
				ConsoleKeyInfo info = Console.ReadKey(true);

				if (info.Key == ConsoleKey.Tab)
				{
					if ((info.Modifiers & ConsoleModifiers.Shift) != 0) SelectPrevious();
					else SelectNext();
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
			selectedIndex = -1;
		}
	}
}