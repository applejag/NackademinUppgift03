using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using BankApp.IO;

namespace BankApp.BankObjects
{
	public abstract class Identified
	{
		public uint ID { get; protected set; }

		public void GenerateUniqueID(IEnumerable<Identified> existing)
		{
			GenerateUniqueID(existing.Select(i => i.ID));
		}

		public void GenerateUniqueID(IEnumerable<uint> existing)
		{
			ID = existing.DefaultIfEmpty(0u).Max() + 1u;
		}
	}
}