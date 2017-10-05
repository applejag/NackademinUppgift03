using BankApp.BankObjects;
using BankApp.Exceptions;
using BankApp.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankApp.Tests
{
	[TestClass]
	public class AccountSerializeTests
	{
		[TestMethod]
		public void AccountSerializeBasic()
		{
			const string data = "13093;1024;695.62";

			var acc = new Account(data);

			Assert.AreEqual(13093u, acc.ID);
			Assert.AreEqual(1024u, acc.CustomerID);
			Assert.AreEqual(695.62m, acc.Money);
		}

		[TestMethod]
		public void AccountSerializeDeserialize()
		{
			const string data = "13093;1024;695.62";

			var acc = new Account(data);
			const string expected = data;
			string actual = acc.Serialize().ToString();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		[ExpectedException(typeof(ParseRowException))]
		public void AccountSerializeEmpty()
		{
			const string data = "";

			var acc = new Account(data);
		}

		[TestMethod]
		[ExpectedException(typeof(ParseRowException))]
		public void AccountSerializeTooMuchData()
		{
			const string data = "13093;1024;695.62;1326";

			var acc = new Account(data);
		}

	}
}
