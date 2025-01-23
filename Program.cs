using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace nasocket_client
{
	internal class Program
	{
		static void Main(string[] args)
		{
			const string ServerAddress = "127.0.0.1";
			const int Port = 11177;

			IPEndPoint endPoint = new(IPAddress.Parse(ServerAddress), Port);
			Socket clientSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			try {
				clientSocket.Connect(endPoint);
				Console.WriteLine("Connecting to server...");

				while (true) {
					string? message = Console.ReadLine();
					if (message == null) {
						Console.WriteLine("Stop reading from console");
						break;
					}
					byte[] messageBytes = Encoding.UTF32.GetBytes(message);
					clientSocket.Send(messageBytes);
					Console.WriteLine("Message was sent");

					byte[] buffer = new byte[1024];
					int receivedBytes = clientSocket.Receive(buffer);
					string response = Encoding.UTF32.GetString(buffer);
					Console.WriteLine($"Response from server: {response}");
				}
			} catch (Exception exception) {
				Console.WriteLine($"Fatal error! {exception.Message}.");
			} finally {
				clientSocket.Close();
			}
		}
	}
}
