using BankApp.BankObjects;

namespace BankApp.Tests.Mocks
{
	public class MockSearchable : ISearchable
	{
		private readonly string data;

		public MockSearchable(string data)
		{
			this.data = data;
		}

		public string GetSearchDisplay()
		{
			throw new System.NotImplementedException();
		}

		public string GetSearchQueried()
		{
			return data;
		}
	}
}