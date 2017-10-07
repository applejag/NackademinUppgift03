using BankApp.BankObjects;

namespace BankApp.Tests.Mocks
{
	public class MockDetailed : IDetailed
	{
		private readonly string data;

		public MockDetailed(string data)
		{
			this.data = data;
		}

		public string GetDetailed()
		{
			return data;
		}
	}
}