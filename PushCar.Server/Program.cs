using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PushCar.Server;

internal static class Program {
	private const int PORT = 8080;

	private static void Main() {
		Thread serverThread = new Thread(ServerFunction) {
			IsBackground = true
		};
		serverThread.Start();
		Thread.Sleep(500);
		Console.WriteLine("*** 자동차 게임을 위한 게임 서버 시작 ***");
		Console.WriteLine("서버를 종료하려면 아무 키나 누르세요");
		Console.ReadLine();
	}

	private static void ServerFunction(object? obj) {
		Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		socket.Bind(new IPEndPoint(IPAddress.Any, PORT));

		var receiveBuffer = new byte[1024];
		EndPoint clientEndpoint = new IPEndPoint(IPAddress.Any, 0);

		while (true) {
			var length = socket.ReceiveFrom(receiveBuffer, ref clientEndpoint);
			var message = Encoding.UTF8.GetString(receiveBuffer, 0, length);

			// Echoing
			var sendBytes = Encoding.UTF8.GetBytes($"OK CLIENT : ${message}");
			socket.SendTo(sendBytes, clientEndpoint);

			Console.WriteLine($"클라이언트로부터 메시지 수신 : {message}");
		}
	}
}
