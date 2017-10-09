using System.Collections.Generic;
using BankApp.BankObjects;

namespace BankApp.Tests.Mocks
{
	public class MockDatabase : IDatabase
	{
		public int TransactionsCount { get; private set; }

		public void AddCustomer(Customer customer)
		{}

		public void AddAccount(Account account)
		{}

		public void AddTransaction(Transaction transaction)
		{
			TransactionsCount++;
		}

		public void RemoveCustomer(Customer customer)
		{}

		public void RemoveAccount(Account account)
		{}
	}
}