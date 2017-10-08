using System;
using System.Globalization;

namespace BankApp.BankObjects
{
	public interface ISearchable
	{
		string GetSearchDisplay();
		string GetSearchQueried();
	}
}