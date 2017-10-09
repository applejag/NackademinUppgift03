using System;
using BankApp.BankObjects;

namespace BankApp.Exceptions
{
	public abstract class AccountWithdrawException : ApplicationException
	{
		public Account Sender { get; }
		public decimal Amount { get; }

		public override string Message { get; }

		protected AccountWithdrawException(Account sender, decimal amount, string message)
		{
			Sender = sender;
			Amount = amount;
			Message = message;
		}
	}

	public class AccountWithdrawInsufficientFundsException : AccountWithdrawException
	{
		public AccountWithdrawInsufficientFundsException(Account sender, decimal amount)
			: base(sender, amount, "Insufficient funds!")
		{
		}
	}

	public class AccountWithdrawInvalidAmountException : AccountWithdrawException
	{
		public AccountWithdrawInvalidAmountException(Account sender, decimal amount)
			: base(sender, amount, amount == 0
				? "You cannot withdraw zero!"
				: "You cannot withdraw a negative amount!")
		{ }
	}
}