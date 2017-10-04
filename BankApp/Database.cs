using System.Collections.Generic;
using BankApp.BankObjects;

namespace BankApp
{
	public class Database
	{
		public List<Account> Accounts { get; } = new List<Account>();
		public List<Customer> Customers { get; } = new List<Customer>();
	}
}