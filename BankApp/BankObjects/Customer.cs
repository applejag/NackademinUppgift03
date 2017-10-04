using System.Globalization;

namespace BankApp.BankObjects
{
	public class Customer : Identified, ISerializeable
	{
		public long ID { get; private set; }
		public long OrganisationID { get; private set; }
		public string OrganisationName { get; private set; }
		public string Address { get; private set; }
		public string City { get; private set; }
		public string Region { get; private set; }
		public string PostCode { get; private set; }
		public string Country { get; private set; }
		public string Telephone { get; private set; }

		public string[] Serialize() => new [] { 
			ID.ToString(CultureInfo.InvariantCulture),
			OrganisationID.ToString(CultureInfo.InvariantCulture),
			OrganisationName,
			Address,
			City,
			Region,
			PostCode,
			Country,
			Telephone,
		};

		public void Deserialize(string[] data)
		{
			ID = long.Parse(data[0], CultureInfo.InvariantCulture);
			OrganisationID = long.Parse(data[1], CultureInfo.InvariantCulture);
			OrganisationName = data[2];
			Address = data[3];
			City = data[4];
			Region = data[5];
			PostCode = data[6];
			Country = data[7];
			Telephone = data[8];
		}
	}
}