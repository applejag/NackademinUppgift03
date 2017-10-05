using System;
using System.Collections.Generic;
using System.IO;
using BankApp.BankObjects;
using BankApp.IO;

namespace BankApp
{
	public class Database
	{
		public List<Customer> Customers { get; }
		public List<Account> Accounts { get; }

		public Database(string path)
		{
			using (var file = new FileReader(path))
			{
				this.Customers = file.ReadSerializeableGroup<Customer>();
				this.Accounts = file.ReadSerializeableGroup<Account>();
			}
		}

		public void Save(string path)
		{
			using (var file = new FileWriter(path))
			{
				file.WriteSerializeableGroup(Customers);
				file.WriteSerializeableGroup(Accounts);
			}
		}
	}
}