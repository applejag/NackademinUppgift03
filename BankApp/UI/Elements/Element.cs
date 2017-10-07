using System;

namespace BankApp.UI.Elements
{
	public abstract class Element : Mask
	{
		public string Name { get; }
		public InputGroup Group { get; internal set; }
		public bool Selected => Group?.Selected == this;

		public abstract void OnInput(ConsoleKeyInfo info);

		protected Element(string name)
		{
			Name = name;
		}
	}
}