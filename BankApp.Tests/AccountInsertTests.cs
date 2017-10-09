using System;
using BankApp.BankObjects;
using BankApp.Exceptions;
using BankApp.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankApp.Tests
{
	[TestClass]
	public class AccountInsertTests
	{
		public IDatabase db;

		[TestInitialize]
		public void Initialize()
		{
			db = new MockDatabase();
		}

		[TestMethod]
		[ExpectedException(typeof(AccountInsertInvalidAmountException))]
		public void AccountInsertZero()
		{
			var account = new Account();

			account.InsertMoney(0m, db);
		}

		[TestMethod]
		[ExpectedException(typeof(AccountInsertInvalidAmountException))]
		public void AccountInsertNegative()
		{
			var account = new Account();

			account.InsertMoney(-100m, db);
		}

		[TestMethod]
		public void AccountInsertPositiveOnEmpty()
		{
			var account = new Account();

			account.InsertMoney(100m, db);

			Assert.AreEqual(100m, account.Money);
		}

		[TestMethod]
		public void AccountInsertPositiveOnExisting()
		{
			var account = new Account("13093;1024;695.62");

			account.InsertMoney(100m, db);

			Assert.AreEqual(795.62m, account.Money);
		}
	}
}
