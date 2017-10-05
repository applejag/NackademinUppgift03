using BankApp.BankObjects;
using BankApp.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankApp.Tests
{
	[TestClass]
	public class CustomerSerializeTests
	{
		[TestMethod]
		public void CustomerSerializeBasic()
		{
			const string data = "1024;556392-8406;Folk och fä HB;Åkergatan 24;Bräcke;;S-844 67;Sweden;0695-34 67 21";

			var acc = new Customer(data);

			Assert.AreEqual(1024u, acc.ID);
			Assert.AreEqual("556392-8406", acc.OrganisationID);
			Assert.AreEqual("Folk och fä HB", acc.OrganisationName);
			Assert.AreEqual("Åkergatan 24", acc.Address);
			Assert.AreEqual("Bräcke", acc.City);
			Assert.AreEqual("", acc.Region);
			Assert.AreEqual("S-844 67", acc.PostCode);
			Assert.AreEqual("Sweden", acc.Country);
			Assert.AreEqual("0695-34 67 21", acc.Telephone);
		}

		[TestMethod]
		public void CustomerSerializeDeserialize()
		{
			const string data = "1024;556392-8406;Folk och fä HB;Åkergatan 24;Bräcke;;S-844 67;Sweden;0695-34 67 21";

			var acc = new Customer(data);
			const string expected = data;
			string actual = acc.Serialize().ToString();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		[ExpectedException(typeof(ParseRowException))]
		public void CustomerSerializeEmpty()
		{
			const string data = "";

			var acc = new Customer(data);
		}

		[TestMethod]
		[ExpectedException(typeof(ParseRowException))]
		public void CustomerSerializeTooMuchData()
		{
			const string data = "1024;556392-8406;Folk och fä HB;Åkergatan 24;Bräcke;;S-844 67;Sweden;0695-34 67 21;13876";

			var acc = new Customer(data);
		}
	}
}