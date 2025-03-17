using System;
using System.Net;

namespace nasocket_client;

internal class Program
{
	private static void Main(string[] args)
	{
		const int Port = 11177;

		if (!TryGetServerIpAddressFromUser(out var serverAddress)) {
			return;
		}

		using Connection connection = new(serverAddress, Port);
		if (connection.TryConnect()) {
			connection.StartDialog();
		}
	}

	private static bool TryGetServerIpAddressFromUser(out IPAddress serverAddress)
	{
		Console.Write("Please enter the server IP address: ");
		string enteredString = Console.ReadLine() ?? string.Empty;
		while (!IPAddress.TryParse(enteredString, out serverAddress!)) {
			Console.Write("The entered address is invalid. Please, try again: ");
			enteredString = Console.ReadLine() ?? string.Empty;
		}
		if (serverAddress is null) {
			Console.Write("Can't parse server IP address.");
			return false;
		}
		return true;
	}
}
