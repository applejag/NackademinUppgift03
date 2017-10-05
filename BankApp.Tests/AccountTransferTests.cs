using BankApp.BankObjects;
using BankApp.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankApp.Tests
{
	[TestClass]
	public class AccountTransferTests
	{

		[TestMethod]
		public void AccountTransferSimple()
		{
			var accA = new Account("13093;1024;695.62");
			var accB = new Account("13128;1032;392.20");

			accA.TransferMoney(200m, accB);

			Assert.AreEqual(495.62m, accA.Money);
			Assert.AreEqual(592.20m, accB.Money);
		}

		[TestMethod]
		[ExpectedException(typeof(TransferInsufficientFundsException))]
		public void AccountTransferInsufficientFunds()
		{
			var accA = new Account("13093;1024;695.62");
			var accB = new Account("13128;1032;392.20");

			accA.TransferMoney(1_000_000m, accB);
		}

		[TestMethod]
		[ExpectedException(typeof(TransferNegativeAmountException))]
		public void AccountTransferNegativeAmount()
		{
			var accA = new Account("13093;1024;695.62");
			var accB = new Account("13128;1032;392.20");

			accA.TransferMoney(-100m, accB);
		}

		[TestMethod]
		[ExpectedException(typeof(TransferInvalidReceiverException))]
		public void AccountTransferToSelf()
		{
			var accA = new Account("13093;1024;695.62");

			accA.TransferMoney(100m, accA);
		}

		[TestMethod]
		[ExpectedException(typeof(TransferInvalidReceiverException))]
		public void AccountTransferToNull()
		{
			var accA = new Account("13093;1024;695.62");

			accA.TransferMoney(100m, null);
		}
	}
}