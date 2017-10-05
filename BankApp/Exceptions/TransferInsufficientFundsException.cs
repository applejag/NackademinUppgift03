using System;
using BankApp.BankObjects;

namespace BankApp.Exceptions
{
	public class TransferInsufficientFundsException : TransferException
	{
		public TransferInsufficientFundsException(Account sender, Account receiver, decimal amount)
			: base(sender, receiver, amount)
		{
		}
	}
}