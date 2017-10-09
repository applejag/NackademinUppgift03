using BankApp.BankObjects;
using BankApp.Exceptions;
using BankApp.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankApp.Tests
{
	[TestClass]
	public class AccountTransferTests
	{
		public IDatabase db;

		[TestInitialize]
		public void Initialize()
		{
			db = new MockDatabase();
		}

		[TestMethod]
		public void AccountTransferSimple()
		{
			var accA = new Account("13093;1024;695.62");
			var accB = new Account("13128;1032;392.20");

			accA.TransferMoney(200m, accB, db);

			Assert.AreEqual(495.62m, accA.Money);
			Assert.AreEqual(592.20m, accB.Money);
		}

		[TestMethod]
		[ExpectedException(typeof(AccountTransferInsufficientFundsException))]
		public void AccountTransferInsufficientFunds()
		{
			var accA = new Account("13093;1024;695.62");
			var accB = new Account("13128;1032;392.20");

			accA.TransferMoney(1_000_000m, accB, db);
		}

		[TestMethod]
		[ExpectedException(typeof(AccountTransferInvalidAmountException))]
		public void AccountTransferNegativeAmount()
		{
			var accA = new Account("13093;1024;695.62");
			var accB = new Account("13128;1032;392.20");

			accA.TransferMoney(-100m, accB, db);
		}

		[TestMethod]
		[ExpectedException(typeof(AccountTransferInvalidAmountException))]
		public void AccountTransferZeroAmount()
		{
			var accA = new Account("13093;1024;695.62");
			var accB = new Account("13128;1032;392.20");

			accA.TransferMoney(0m, accB, db);
		}

		[TestMethod]
		[ExpectedException(typeof(AccountTransferInvalidReceiverException))]
		public void AccountTransferToSelf()
		{
			var accA = new Account("13093;1024;695.62");

			accA.TransferMoney(100m, accA, db);
		}

		[TestMethod]
		[ExpectedException(typeof(AccountTransferInvalidReceiverException))]
		public void AccountTransferToNull()
		{
			var accA = new Account("13093;1024;695.62");

			accA.TransferMoney(100m, null, db);
		}
	}
}