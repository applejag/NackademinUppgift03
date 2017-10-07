using System.Globalization;
using BankApp.IO;

namespace BankApp.BankObjects
{
	public sealed class Customer : Identified, ISerializable
	{
		public string OrganisationID { get; private set; }
		public string OrganisationName { get; private set; }
		public string Address { get; private set; }
		public string City { get; private set; }
		public string Region { get; private set; }
		public string PostCode { get; private set; }
		public string Country { get; private set; }
		public string Telephone { get; private set; }

		public Customer()
		{ }

		public Customer(FileRow data) : this()
		{
			Deserialize(data);
		}

		public Customer(string data) : this(new FileRow(data))
		{ }

		public FileRow Serialize() => new FileRow (
			ID,
			OrganisationID,
			OrganisationName,
			Address,
			City,
			Region,
			PostCode,
			Country,
			Telephone
		);

		public void Deserialize(FileRow data)
		{
			ID = data.TakeUInt();
			OrganisationID = data.TakeString();
			OrganisationName = data.TakeString();
			Address = data.TakeString();
			City = data.TakeString();
			Region = data.TakeString();
			PostCode = data.TakeString();
			Country = data.TakeString();
			Telephone = data.TakeString();

			data.Close();
		}
	}
}