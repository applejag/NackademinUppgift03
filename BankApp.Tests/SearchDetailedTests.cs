using System;
using BankApp.BankObjects;
using BankApp.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankApp.Tests
{
	[TestClass]
	public class SearchDetailedTests
	{
		public MockDetailed detailedWhitespace;
		public MockDetailed detailedAlphabet;
		public MockDetailed detailedLorem;

		[TestInitialize]
		public void Setup()
		{
			detailedWhitespace = new MockDetailed("   \t  \n\t  \n  ");
			detailedAlphabet = new MockDetailed("ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖ");
			detailedLorem = new MockDetailed("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam sed erat elit. Phasellus tincidunt diam sed turpis condimentum. ac maximus eros malesuada.");
		}

		[TestMethod]
		public void DetailedSearchAllForEmpty()
		{
			const string query = "";

			DetailedResult result1 = DetailedResult.SearchDetailed(detailedWhitespace, query);
			DetailedResult result2 = DetailedResult.SearchDetailed(detailedAlphabet, query);
			DetailedResult result3 = DetailedResult.SearchDetailed(detailedLorem, query);

			Assert.AreEqual(0, result1.Count);
			Assert.IsFalse(result1.IsMatch);
			Assert.AreEqual(0, result2.Count);
			Assert.IsFalse(result2.IsMatch);
			Assert.AreEqual(0, result3.Count);
			Assert.IsFalse(result3.IsMatch);
		}

		[TestMethod]
		public void DetailedSearchAllForSpace()
		{
			const string query = " ";

			DetailedResult result1 = DetailedResult.SearchDetailed(detailedWhitespace, query);
			DetailedResult result2 = DetailedResult.SearchDetailed(detailedAlphabet, query);
			DetailedResult result3 = DetailedResult.SearchDetailed(detailedLorem, query);

			Assert.AreEqual(0, result1.Count);
			Assert.IsFalse(result1.IsMatch);
			Assert.AreEqual(0, result2.Count);
			Assert.IsFalse(result2.IsMatch);
			Assert.AreEqual(0, result3.Count);
			Assert.IsFalse(result3.IsMatch);
		}

		[TestMethod]
		public void DetailedSearchAllForNull()
		{
			const string query = null;

			try
			{
				DetailedResult.SearchDetailed(detailedWhitespace, query);
				Assert.Fail("Threw on {0}", nameof(detailedWhitespace));
			}
			catch (ArgumentNullException)
			{
				try
				{
					DetailedResult.SearchDetailed(detailedAlphabet, query);
					Assert.Fail("Threw on {0}", nameof(detailedAlphabet));
				}
				catch (ArgumentNullException)
				{
					try
					{
						DetailedResult.SearchDetailed(detailedLorem, query);
						Assert.Fail("Threw on {0}", nameof(detailedLorem));
					}
					catch (ArgumentNullException)
					{}
				}
			}
			
		}

		[TestMethod]
		public void DetailedSearchEmptyForA()
		{
			const string query = "A";

			DetailedResult result = DetailedResult.SearchDetailed(detailedWhitespace, query);

			Assert.AreEqual(0, result.Count);
			Assert.IsTrue(result.IsMatch);
		}

		[TestMethod]
		public void DetailedSearchAlphabetForA()
		{
			const string query = "A";

			DetailedResult result = DetailedResult.SearchDetailed(detailedAlphabet, query);
			
			Assert.AreEqual(2, result.Count);
			Assert.IsTrue(result.IsMatch);
		}

		[TestMethod]
		public void DetailedSearchLoremForA()
		{
			const string query = "A";

			DetailedResult result = DetailedResult.SearchDetailed(detailedLorem, query);

			Assert.AreEqual(22, result.Count);
			Assert.IsTrue(result.IsMatch);
		}
	}
}