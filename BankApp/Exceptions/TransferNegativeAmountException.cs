using BankApp.BankObjects;

namespace BankApp.Exceptions
{
	public class TransferNegativeAmountException : TransferException
	{
		public TransferNegativeAmountException(Account sender, Account receiver, decimal amount)
			: base(sender, receiver, amount)
		{
		}
	}
}