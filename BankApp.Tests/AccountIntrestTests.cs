using BankApp.BankObjects;
using BankApp.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankApp.Tests
{
	[TestClass]
	public class AccountIntrestTests
	{
		public MockDatabase db;

		[TestInitialize]
		public void Initialize()
		{
			db = new MockDatabase();
		}

		[TestMethod]
		public void AccountIntrestCreditZero()
		{
			var account = new Account("13093;1024;695.62") { CreditRate = 0m, CreditLimit = 10_000m };

			account.ApplyCreditRate(db);

			Assert.AreEqual(695.62m, account.Money);
			Assert.AreEqual(0, db.TransactionsCount);
		}

		[TestMethod]
		public void AccountIntrestCredit10PercentOnNegative()
		{
			var account = new Account("13093;1024;-695.62") { CreditRate = 0.1m, CreditLimit = 10_000m};
			decimal expected = -695.62m - 695.62m * 0.1m / Account.DaysThisYear();

			account.ApplyCreditRate(db);

			Assert.AreEqual(expected, account.Money);
			Assert.AreEqual(1, db.TransactionsCount);
		}

		[TestMethod]
		public void AccountIntrestCredit10PercentOnPositive()
		{
			var account = new Account("13093;1024;695.62") { CreditRate = 0.1m, CreditLimit = 10_000m };
			decimal expected = 695.62m - 695.62m * 0.1m / Account.DaysThisYear();

			account.ApplyCreditRate(db);

			Assert.AreEqual(695.62m, account.Money);
			Assert.AreEqual(0, db.TransactionsCount);
		}

		[TestMethod]
		public void AccountIntrestCreditNegative10PercentOnPositive()
		{
			var account = new Account("13093;1024;695.62") { CreditRate = -0.1m, CreditLimit = 10_000m };

			account.ApplyCreditRate(db);

			Assert.AreEqual(695.62m, account.Money);
			Assert.AreEqual(0, db.TransactionsCount);
		}

		[TestMethod]
		public void AccountIntrestCreditNegative10PercentOnNegative()
		{
			var account = new Account("13093;1024;-695.62") { CreditRate = -0.1m, CreditLimit = 10_000m};

			account.ApplyCreditRate(db);

			Assert.AreEqual(-695.62m, account.Money);
			Assert.AreEqual(0, db.TransactionsCount);
		}

		[TestMethod]
		public void AccountIntrestSavingZero()
		{
			var account = new Account("13093;1024;695.62") { SavingRate = 0m };

			account.ApplySavingRate(db);

			Assert.AreEqual(695.62m, account.Money);
			Assert.AreEqual(0, db.TransactionsCount);
		}

		[TestMethod]
		public void AccountIntrestSaving10PercentOnPositive()
		{
			var account = new Account("13093;1024;695.62") { SavingRate = 0.1m };
			decimal expected = 695.62m + 695.62m * 0.1m / Account.DaysThisYear();

			account.ApplySavingRate(db);

			Assert.AreEqual(expected, account.Money);
			Assert.AreEqual(1, db.TransactionsCount);
		}

		[TestMethod]
		public void AccountIntrestSavingNegative10PercentOnPositive()
		{
			var account = new Account("13093;1024;695.62") { SavingRate = -0.1m };

			account.ApplySavingRate(db);

			Assert.AreEqual(695.62m, account.Money);
			Assert.AreEqual(0, db.TransactionsCount);
		}

		[TestMethod]
		public void AccountIntrestSaving10PercentOnNegative()
		{
			var account = new Account("13093;1024;-695.62") { SavingRate = 0.1m, CreditLimit = 10_000m};

			account.ApplySavingRate(db);

			Assert.AreEqual(-695.62m, account.Money);
			Assert.AreEqual(0, db.TransactionsCount);
		}

		[TestMethod]
		public void AccountIntrestSavingNegative10PercentOnNegative()
		{
			var account = new Account("13093;1024;-695.62") { SavingRate = -0.1m, CreditLimit = 10_000m };

			account.ApplySavingRate(db);

			Assert.AreEqual(-695.62m, account.Money);
			Assert.AreEqual(0, db.TransactionsCount);
		}

	}
}
