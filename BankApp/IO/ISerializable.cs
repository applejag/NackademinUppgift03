namespace BankApp.IO
{
	public interface ISerializable
	{
		FileRow Serialize();
		void Deserialize(FileRow data);
	}
}