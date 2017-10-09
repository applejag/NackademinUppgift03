using System;
using BankApp.BankObjects;

namespace BankApp.Exceptions
{
	public abstract class AccountInsertException : ApplicationException
	{
		public Account Receiver { get; }
		public decimal Amount { get; }

		public override string Message { get; }

		protected AccountInsertException(Account receiver, decimal amount, string message)
		{
			Receiver = receiver;
			Amount = amount;
			Message = message;
		}
	}

	public class AccountInsertInvalidAmountException : AccountInsertException
	{
		public AccountInsertInvalidAmountException(Account receiver, decimal amount)
			: base(receiver, amount, amount == 0
				? "You cannot insert zero!"
				: "You cannot insert a negative amount!")
		{ }
	}
}