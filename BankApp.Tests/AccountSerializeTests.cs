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
			const string data = "13093;1024;695.62;0.05;0.1;10000";

			var acc = new Account(data);

			Assert.AreEqual(13093u, acc.ID);
			Assert.AreEqual(1024u, acc.CustomerID);
			Assert.AreEqual(695.62m, acc.Money);
			Assert.AreEqual(0.05m, acc.SavingRate);
			Assert.AreEqual(0.1m, acc.CreditRate);
			Assert.AreEqual(10_000m, acc.CreditLimit);
		}

		[TestMethod]
		public void AccountSerializeDeserializeFromOldFormat()
		{
			const string data = "13093;1024;695.62";
			const string expected = "13093;1024;695.62;0;0;0";

			var acc = new Account(data);
			string actual = acc.Serialize().ToString();

			Assert.AreEqual(expected, actual);
		}
		
		[TestMethod]
		public void AccountSerializeDeserializeFromNewFormat()
		{
			const string data = "13093;1024;695.62;0;0;0";
			const string expected = data;

			var acc = new Account(data);
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
