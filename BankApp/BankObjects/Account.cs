using System.Globalization;

namespace BankApp.BankObjects
{
	public class Account : Identified, ISerializeable
	{
		public long CustomerID { get; private set; }
		public decimal Money { get; private set; }

		public string[] Serialize() => new[]
		{
			ID.ToString(CultureInfo.InvariantCulture),
			CustomerID.ToString(CultureInfo.InvariantCulture),
			Money.ToString(CultureInfo.InvariantCulture),
		};

		public void Deserialize(string[] data)
		{
			ID = long.Parse(data[0], CultureInfo.InvariantCulture);
			CustomerID = long.Parse(data[1], CultureInfo.InvariantCulture);
			Money = decimal.Parse(data[2], CultureInfo.InvariantCulture);
		}
	}
}