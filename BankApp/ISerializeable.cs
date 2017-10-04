namespace BankApp
{
	public interface ISerializeable
	{
		string[] Serialize();
		void Deserialize(string[] data);
	}
}