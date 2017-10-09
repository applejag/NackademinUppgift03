using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BankApp.Exceptions;
using BankApp.IO;
using BankApp.UI;

namespace BankApp.BankObjects
{
	public sealed class Account : Identified, ISerializable, ISearchable
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
		/// <exception cref="TransferInsufficientFundsException">Thrown if tries to transfer more than is available in this account.</exception>
		/// <exception cref="TransferNegativeAmountException">Thrown if <paramref name="amount"/> is negative.</exception>
		/// <exception cref="TransferInvalidReceiverException">Thrown if <paramref name="receiver"/> is null or equal to this account.</exception>
		public void TransferMoney(decimal amount, Account receiver)
		{
			if (receiver == null) throw new TransferInvalidReceiverException(this, null, amount);
			if (receiver == this) throw new TransferInvalidReceiverException(this, receiver, amount);

			if (amount < 0) throw new TransferNegativeAmountException(this, receiver, amount);
			if (amount > Money) throw new TransferInsufficientFundsException(this, receiver, amount);

			Money -= amount;
			receiver.Money += amount;
		}

		public Customer FetchCustomer(Database db)
		{
			return db.Customers.SingleOrDefault(c => c.ID == CustomerID);
		}
		
		public string GetSearchDisplay()
		{
			return $"{ID}: {Money}";
		}

		public string GetSearchQueried()
		{
			return string.Join("\n",
				ID, CustomerID
			);
		}

		public void PrintProfile(Database db)
		{
			UIUtilities.PrintHeader("Information");

			UIUtilities.PrintSegment("Account nr", ID);
			UIUtilities.PrintSegment("Balance", $"{Money:C}");
			
			// Accounts
			if (db != null)
			{
				Console.WriteLine();
				Customer customer = FetchCustomer(db);

				UIUtilities.PrintHeader("Owning customer");
				UIUtilities.PrintSegment("Customer nr", customer.GetSearchDisplay());
			}
		}
	}
}