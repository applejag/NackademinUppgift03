using System.Collections.Generic;
using BankApp.BankObjects;

namespace BankApp.Tests.Mocks
{
	public class MockDatabase : IDatabase
	{
		public void AddCustomer(Customer customer)
		{}

		public void AddAccount(Account account)
		{}

		public void AddTransaction(Transaction transaction)
		{}
	}
}