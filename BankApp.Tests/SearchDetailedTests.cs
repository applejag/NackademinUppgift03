using System;
using System.Collections.Generic;
using BankApp.BankObjects;
using BankApp.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankApp.Tests
{
	[TestClass]
	public class SearchDetailedTests
	{

		[TestMethod]
		public void SearchForEmpty()
		{
			const string query = "";
			MockSearchable[] searchable = { new MockSearchable("hello\nworld") };

			List<MockSearchable> result = Database.Search(searchable, query, m => m.data);

			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.Count);
		}

		[TestMethod]
		public void SearchForWhitespace()
		{
			const string query = "  \t  \t \n  ";
			MockSearchable[] searchable = { new MockSearchable("hello\nworld") };

			List<MockSearchable> result = Database.Search(searchable, query, m => m.data);

			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SearchForNull()
		{
			const string query = null;
			MockSearchable[] searchable = { new MockSearchable("hello\nworld") };

			Database.Search(searchable, query, m => m.data);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SearchWithNull()
		{
			const string query = "";
			const MockSearchable[] searchable = null;

			Database.Search(searchable, query, m => m.data);
		}

		[TestMethod]
		public void SearchSimpleMatchWholeWord()
		{
			const string query = "world";
			var obj = new MockSearchable("hello\nworld");
			MockSearchable[] searchables = { obj };

			List<MockSearchable> result = Database.Search(searchables, query, m => m.data);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count);
			Assert.AreSame(obj, result[0]);
		}

		[TestMethod]
		public void SearchSimpleMatchWholeWordWithWhitespace()
		{
			const string query = "\t    \n wo   r\nld   \t  ";
			var obj = new MockSearchable("hello\nworld");
			MockSearchable[] searchables = { obj };

			List<MockSearchable> result = Database.Search(searchables, query, m => m.data);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count);
			Assert.AreSame(obj, result[0]);
		}

		[TestMethod]
		public void SearchSimpleMatchPartOfWord()
		{
			const string query = "rld";
			var obj = new MockSearchable("hello\nworld");
			MockSearchable[] searchables = { obj };

			List<MockSearchable> result = Database.Search(searchables, query, m => m.data);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count);
			Assert.AreSame(obj, result[0]);
		}

		[TestMethod]
		public void SearchSimpleMiss()
		{
			const string query = "wow";
			var obj = new MockSearchable("hello\nworld");
			MockSearchable[] searchables = { obj };

			List<MockSearchable> result = Database.Search(searchables, query, m => m.data);

			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.Count);
		}

		[TestMethod]
		public void SearchWhereMultipleWordsMatch()
		{
			const string query = "rld wo";
			var obj = new MockSearchable("hello\nworld");
			MockSearchable[] searchables = { obj };

			List<MockSearchable> result = Database.Search(searchables, query, m => m.data);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count);
			Assert.AreSame(obj, result[0]);
		}

		[TestMethod]
		public void SearchWhereNotAllWordsMatch()
		{
			const string query = "rld ce";
			var obj = new MockSearchable("hello\nworld");
			MockSearchable[] searchables = { obj };

			List<MockSearchable> result = Database.Search(searchables, query, m => m.data);

			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.Count);
		}

		[TestMethod]
		public void SearchWhereMultipleSearchablesMatchWholeWord()
		{
			const string query = "hello";
			var obj1 = new MockSearchable("hello\nvärld");
			var obj2 = new MockSearchable("hello\nworld");
			MockSearchable[] searchables = { obj1, obj2 };

			List<MockSearchable> result = Database.Search(searchables, query, m => m.data);

			Assert.IsNotNull(result);
			Assert.AreEqual(2, result.Count);
			Assert.AreSame(obj1, result[0]);
			Assert.AreSame(obj2, result[1]);
		}
		[TestMethod]
		public void SearchWhereMultipleSearchablesMatchPartWord()
		{
			const string query = "rld";
			var obj1 = new MockSearchable("hello\nvärld");
			var obj2 = new MockSearchable("hello\nworld");
			MockSearchable[] searchables = { obj1, obj2 };

			List<MockSearchable> result = Database.Search(searchables, query, m => m.data);

			Assert.IsNotNull(result);
			Assert.AreEqual(2, result.Count);
			Assert.AreSame(obj1, result[0]);
			Assert.AreSame(obj2, result[1]);
		}

		[TestMethod]
		public void SearchWhereMultipleSearchablesSingleMatch()
		{
			const string query = "world";
			var obj1 = new MockSearchable("hello\nvärld");
			var obj2 = new MockSearchable("hello\nworld");
			MockSearchable[] searchables = { obj1, obj2 };

			List<MockSearchable> result = Database.Search(searchables, query, m => m.data);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count);
			Assert.AreSame(obj2, result[0]);
		}

		[TestMethod]
		public void SearchWhereMultipleWordsMatchesMultipleSearchables()
		{
			const string query = "rld llo";
			var obj1 = new MockSearchable("hello\nvärld");
			var obj2 = new MockSearchable("hello\nworld");
			MockSearchable[] searchables = { obj1, obj2 };

			List<MockSearchable> result = Database.Search(searchables, query, m => m.data);

			Assert.IsNotNull(result);
			Assert.AreEqual(2, result.Count);
			Assert.AreSame(obj1, result[0]);
			Assert.AreSame(obj2, result[1]);
		}

		[TestMethod]
		public void SearchWhereNotAllWordsMatchesMultipleSearchables()
		{
			const string query = "rld wo";
			var obj1 = new MockSearchable("hello\nvärld");
			var obj2 = new MockSearchable("hello\nworld");
			MockSearchable[] searchables = { obj1, obj2 };

			List<MockSearchable> result = Database.Search(searchables, query, m => m.data);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count);
			Assert.AreSame(obj2, result[0]);
		}
	}
}