using System;

namespace BankApp.UI.Elements
{
	public abstract class Element
	{
		public string Name { get; }
		public InputGroup Group { get; internal set; }
		public bool Selected => Group?.Selected == this;

		public abstract object Result { get; }
		public abstract void OnInput(ConsoleKeyInfo info);
		public abstract void Draw();

		protected Element(string name)
		{
			this.Name = name;
		}
	}
}