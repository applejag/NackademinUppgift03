using System;
using System.Globalization;
using BankApp.IO;
using BankApp.UI;
using Console = System.Console;

namespace BankApp.BankObjects
{
	public class Transaction : Identified, ISerializable
	{
		public uint AccountID { get; private set; }
		public uint OtherAccountID { get; private set; }
		public decimal DeltaMoney { get; private set; }
		public TransactionType Type { get; private set; }
		public DateTime Timestamp { get; private set; }

		public Transaction()
		{
			Timestamp = DateTime.Now;
		}

		public static Transaction CreateTransfer(Account source, Account target, decimal amount)
		{
			return new Transaction
			{
				AccountID = source.ID,
				OtherAccountID = target.ID,
				DeltaMoney = amount,
				Type = TransactionType.Transfer,
			};
		}

		public static Transaction CreateInsert(Account account, decimal amount)
		{
			return new Transaction
			{
				AccountID = account.ID,
				DeltaMoney = amount,
				Type = TransactionType.Insert,
			};
		}

		public static Transaction CreateWithdraw(Account account, decimal amount)
		{
			return new Transaction
			{
				AccountID = account.ID,
				DeltaMoney = amount,
				Type = TransactionType.Withdraw,
			};
		}

		public static Transaction CreateInterest(Account account, decimal change)
		{
			return new Transaction
			{
				AccountID = account.ID,
				DeltaMoney = change,
				Type = TransactionType.Interest,
			};
		}

		public FileRow Serialize()
		{
			if (Type == TransactionType.Transfer)
			{
				return new FileRow(
					ID,
					Type,
					DeltaMoney,
					Timestamp,
					AccountID,
					OtherAccountID
				);
			}
			else
			{
				return new FileRow(
					ID,
					Type,
					DeltaMoney,
					Timestamp,
					AccountID
				);
			}
		}

		public void Deserialize(FileRow data)
		{
			ID = data.TakeUInt();
			Type = data.TakeEnum<TransactionType>();
			DeltaMoney = data.TakeDecimal();
			Timestamp = data.TakeDateTime();
			AccountID = data.TakeUInt();

			if (Type == TransactionType.Transfer)
				OtherAccountID = data.TakeUInt();

			data.Close();
		}

		public void PrintTransaction(Account account)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.Write($"{Timestamp:g} - {ID}: ");
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write($"{Type} ");

			if (Type == TransactionType.Transfer)
			{
				Console.Write(AccountID == account.ID
					? $"(to {OtherAccountID}) "
					: $"(from {AccountID}) ");
			}

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.Write(new string('_', 48 - Console.CursorLeft)+' ');
			switch (Type)
			{
				case TransactionType.Withdraw:
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine($"{-DeltaMoney:c2}");
					break;

				case TransactionType.Insert:
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine($"+{DeltaMoney:c2}");
					break;

				case TransactionType.Interest when DeltaMoney >= 0:
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine($"+{DeltaMoney:c2}");
					break;

				case TransactionType.Interest when DeltaMoney < 0:
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine($"{DeltaMoney:c2}");
					break;

				case TransactionType.Transfer when account.ID == OtherAccountID:
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine($"+{DeltaMoney:c2}");
					break;

				case TransactionType.Transfer:
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine($"{-DeltaMoney:c2}");
					break;

				default:
					Console.ForegroundColor = ConsoleColor.White;
					Console.WriteLine(DeltaMoney >= 0
						? $"+{DeltaMoney:c2}"
						: $"{DeltaMoney:c2}");
					break;
			}
		}

		public enum TransactionType
		{
			Transfer = 0,
			Insert = 1,
			Withdraw = 2,
			Interest = 3,
		}
	}
}