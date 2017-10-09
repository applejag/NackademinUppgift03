namespace BankApp.BankObjects
{
	public interface IDatabase
	{
		void AddCustomer(Customer customer);
		void AddAccount(Account account);
		void AddTransaction(Transaction transaction);

		void RemoveCustomer(Customer customer);
		void RemoveAccount(Account account);
	}
}