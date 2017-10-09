using System;
using BankApp.BankObjects;

namespace BankApp.Exceptions
{
	public abstract class AccountTransferException : ApplicationException
	{
		public Account Sender { get; }
		public Account Receiver { get; }
		public decimal Amount { get; }

		public override string Message { get; }

		protected AccountTransferException(Account sender, Account receiver, decimal amount, string message)
		{
			Sender = sender;
			Receiver = receiver;
			Amount = amount;
			Message = message;
		}
	}

	public class AccountTransferInsufficientFundsException : AccountTransferException
	{
		public AccountTransferInsufficientFundsException(Account sender, Account receiver, decimal amount)
			: base(sender, receiver, amount, "Insufficient funds!")
		{
		}
	}

	public class AccountTransferInvalidAmountException : AccountTransferException
	{
		public AccountTransferInvalidAmountException(Account sender, Account receiver, decimal amount)
			: base(sender, receiver, amount, amount == 0
				? "You cannot transfer zero!"
				: "You cannot transfer a negative amount!")
		{ }
	}

	public class AccountTransferInvalidReceiverException : AccountTransferException
	{
		public bool IsSelf => Sender == Receiver;
		public bool IsNull => Receiver == null;

		public AccountTransferInvalidReceiverException(Account sender, Account receiver, decimal amount)
			: base(sender, receiver, amount, receiver == sender
				? "You can only transfer to other accounts!"
				: "Invalid receiving account!")
		{ }
	}
}