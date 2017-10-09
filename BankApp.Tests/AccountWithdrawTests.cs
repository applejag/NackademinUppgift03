using System;
using BankApp.BankObjects;
using BankApp.Exceptions;
using BankApp.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankApp.Tests
{
	[TestClass]
	public class AccountWithdrawTests
	{
		public IDatabase db;

		[TestInitialize]
		public void Initialize()
		{
			db = new MockDatabase();
		}

		[TestMethod]
		[ExpectedException(typeof(AccountWithdrawInvalidAmountException))]
		public void AccountWithdrawZero()
		{
			var account = new Account();

			account.WithdrawMoney(0m, db);
		}

		[TestMethod]
		[ExpectedException(typeof(AccountWithdrawInvalidAmountException))]
		public void AccountWithdrawNegative()
		{
			var account = new Account();

			account.WithdrawMoney(-100m, db);
		}

		[TestMethod]
		[ExpectedException(typeof(AccountWithdrawInsufficientFundsException))]
		public void AccountWithdrawPositiveOnEmpty()
		{
			var account = new Account();

			account.WithdrawMoney(100m, db);
		}

		[TestMethod]
		public void AccountWithdrawPositiveOnExisting()
		{
			var account = new Account("13093;1024;695.62");

			account.WithdrawMoney(100m, db);

			Assert.AreEqual(595.62m, account.Money);
		}
	}
}
