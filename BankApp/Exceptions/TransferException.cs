using System;
using BankApp.BankObjects;

namespace BankApp.Exceptions
{
	public abstract class TransferException : ApplicationException
	{
		public Account Sender { get; }
		public Account Receiver { get; }
		public decimal Amount { get; }

		public TransferException(Account sender, Account receiver, decimal amount)
		{
			Sender = sender;
			Receiver = receiver;
			Amount = amount;
		}
	}
}