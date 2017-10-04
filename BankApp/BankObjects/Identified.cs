using System.Collections.Generic;
using System.Linq;

namespace BankApp.BankObjects
{
	public abstract class Identified
	{
		public long ID { get; protected set; }

		public static long GenerateUniqueID(IEnumerable<Identified> existing)
		{
			return existing.Select(i => i.ID).Max() + 1;
		}
	}
}