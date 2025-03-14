using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace nasocket_client
{
	internal class Connection(string serverAddress, int port) : IDisposable
	{
		private readonly IPEndPoint endPoint = new(IPAddress.Parse(serverAddress), port);
		private readonly Socket clientSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

		public void Dispose()
		{
			clientSocket?.Close();
		}

		public bool TryConnect()
		{
			Console.WriteLine("Connecting to server...");
			try {
				clientSocket.Connect(endPoint);
			} catch (Exception exception) {
				Console.WriteLine($"Can't connect to server. Exception message: {exception.Message}.");
				return false;
			}
			Console.WriteLine("Connected.");
			return true;
		}

		public void StartDialog()
		{
			Console.WriteLine("Start dialog.");
			while (
				TryReadMessageFromConsole(out string? message) &&
				TrySendMessage(message) &&
				TryReceiveResponse()
			) {
			}
			Console.WriteLine("Dialog was ended.");
		}

		private bool TrySendMessage(string message)
		{
			Console.WriteLine("Sending message to server...");
			try {
				byte[] messageBytes = Encoding.UTF32.GetBytes(message);
				clientSocket.Send(messageBytes);
			} catch (Exception exception) {
				Console.WriteLine($"Can't send message to server. Exception message: {exception.Message}");
				return false;
			}
			Console.WriteLine("Message was sent.");
			return true;
		}

		private bool TryReceiveResponse()
		{
			Console.WriteLine("Waiting to response...");
			try {
				byte[] buffer = new byte[1024];
				_ = clientSocket.Receive(buffer);
				var response = Encoding.UTF32.GetString(buffer);
				Console.WriteLine($"Response from server: {response}");
			} catch (Exception exception) {
				Console.WriteLine($"Can't get response from server. Exception message: {exception.Message}");
				return false;
			}
			return true;
		}

		private static bool TryReadMessageFromConsole(out string message)
		{
			message = Console.ReadLine() ?? string.Empty;
			if (message == null) {
				Console.WriteLine("Can't read message to server from user.");
				return false;
			}
			return true;
		}
	}
}
