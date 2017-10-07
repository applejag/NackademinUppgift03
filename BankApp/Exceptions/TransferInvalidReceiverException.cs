using BankApp.BankObjects;

namespace BankApp.Exceptions
{
	public class TransferInvalidReceiverException : TransferException
	{
		public bool IsSelf { get; }
		public bool IsNull { get; }

		public TransferInvalidReceiverException(Account sender, Account receiver, decimal amount)
			: base(sender, receiver, amount)
		{
			IsNull = Receiver == null;
			IsSelf = Sender == Receiver;
		}
	}
}