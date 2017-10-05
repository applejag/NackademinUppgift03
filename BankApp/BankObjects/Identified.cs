using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using BankApp.IO;

namespace BankApp.BankObjects
{
	public abstract class Identified
	{
		public uint ID { get; protected set; }

		public static uint GenerateUniqueID(IEnumerable<Identified> existing)
		{
			return existing.Select(i => i.ID).Max() + 1;
		}
	}
}