using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BankApp.Exceptions;
using BankApp.IO;
using BankApp.UI;

namespace BankApp.BankObjects
{
	public sealed class Account : Identified, ISerializable
	{
		public uint CustomerID { get; private set; }
		public decimal Money { get; private set; }

		#region Serialization

		public Account()
		{ }

		public Account(Customer parent)
		{
			CustomerID = parent.ID;
		}

		public Account(FileRow data) : this()
		{
			Deserialize(data);
		}

		public Account(string data) : this(new FileRow(data))
		{ }


		public FileRow Serialize()
		{
			return new FileRow(
				ID,
				CustomerID,
				Money
			);
		}

		public void Deserialize(FileRow data)
		{
			ID = data.TakeUInt();
			CustomerID = data.TakeUInt();
			Money = data.TakeDecimal();

			data.Close();
		}

		#endregion

		/// <summary>
		/// Transfer money from this account to another account.
		/// </summary>
		/// <param name="amount">Amount of cash to be removed from this account and added to the <paramref name="receiver"/> account.</param>
		/// <param name="receiver">The receiving account.</param>
		/// <param name="db">The database to store the transaction in.</param>
		/// <exception cref="AccountTransferInsufficientFundsException">Thrown if tries to transfer more than is available in this account.</exception>
		/// <exception cref="AccountTransferInvalidAmountException">Thrown if <paramref name="amount"/> is zero or negative.</exception>
		/// <exception cref="AccountTransferInvalidReceiverException">Thrown if <paramref name="receiver"/> is null or equal to this account.</exception>
		public void TransferMoney(decimal amount, Account receiver, IDatabase db)
		{
			if (receiver == null) throw new AccountTransferInvalidReceiverException(this, null, amount);
			if (receiver == this) throw new AccountTransferInvalidReceiverException(this, receiver, amount);

			if (amount <= 0) throw new AccountTransferInvalidAmountException(this, receiver, amount);
			if (amount > Money) throw new AccountTransferInsufficientFundsException(this, receiver, amount);

			Money -= amount;
			receiver.Money += amount;
			db.AddTransaction(Transaction.CreateTransfer(this, receiver, amount));
		}

		/// <summary>
		/// Insert money to this account.
		/// </summary>
		/// <param name="amount">Amount of cash to be inserted.</param>
		/// <param name="db">The database to store the transaction in.</param>
		/// <exception cref="AccountInsertInvalidAmountException">Thrown if <paramref name="amount"/> is zero or negative.</exception>
		public void InsertMoney(decimal amount, IDatabase db)
		{
			if (amount <= 0) throw new AccountInsertInvalidAmountException(this, amount);

			Money += amount;
			db.AddTransaction(Transaction.CreateInsert(this, amount));
		}

		/// <summary>
		/// Withdraw money from this account.
		/// </summary>
		/// <param name="amount">Amount of cash to be withdrawn.</param>
		/// <param name="db">The database to store the transaction in.</param>
		/// <exception cref="AccountWithdrawInsufficientFundsException">Thrown if tries to withdraw more money than is available on this account.</exception>
		/// <exception cref="AccountWithdrawInvalidAmountException">Thrown if <paramref name="amount"/> is zero or negative.</exception>
		public void WithdrawMoney(decimal amount, IDatabase db)
		{
			if (amount <= 0) throw new AccountWithdrawInvalidAmountException(this, amount);
			if (amount > Money) throw new AccountWithdrawInsufficientFundsException(this, amount);

			Money -= amount;
			db.AddTransaction(Transaction.CreateWithdraw(this, amount));
		}

		public Customer FetchCustomer(Database db)
		{
			return db?.Customers.SingleOrDefault(c => c.ID == CustomerID);
		}

		public List<Transaction> FetchTransactions(Database db)
		{
			return db?.Transactions.Where(t =>
				t.AccountID == ID || (t.Type == Transaction.TransactionType.Transfer && t.OtherAccountID == ID))
				.OrderByDescending(t => t.Timestamp).ToList();
		}
		
		public string GetSearchDisplay(Database db)
		{
			Customer customer = FetchCustomer(db);
			return customer != null
				? $"{customer.OrganisationName} ({customer.OrganisationID}), Account {ID}: {Money:C}"
				: $"{ID}: {Money:C}";
		}

		public string GetSearchQueried(Database db)
		{
			return string.Join("\n",
				ID, CustomerID, FetchCustomer(db)?.GetSearchQueried()
			);
		}

		public void PrintProfile(Database db, string header = "Information")
		{
			UIUtilities.PrintHeader(header);

			UIUtilities.PrintSegment("Account nr", ID);
			UIUtilities.PrintSegment("Balance", $"{Money:C}");

			Customer customer = FetchCustomer(db);
			UIUtilities.PrintSegment("Customer", customer?.GetSearchDisplay() ?? "N/A");
		}
	}
}