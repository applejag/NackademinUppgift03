using System;

namespace BankApp.UI.Elements
{
	public abstract class Element : Mask
	{
		private string _name;

		public string Name
		{
			get => _name;
			set => _name = value ?? string.Empty;
		}

		public InputGroup Group { get; internal set; }
		public bool Selected => Group?.Selected == this;
		public bool Disabled { get; set; }

		public abstract void OnInput(ConsoleKeyInfo info);

		protected Element(string name)
		{
			Name = name;
		}
	}
}