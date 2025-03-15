namespace nasocket_client;

internal class Program
{
	static void Main(string[] args)
	{
		const string ServerAddress = "127.0.0.1";
		const int Port = 11177;

		using Connection connection = new(ServerAddress, Port);
		if (connection.TryConnect()) {
			connection.StartDialog();
		}
	}
}
