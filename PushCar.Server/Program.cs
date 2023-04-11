using System.Text;
using PushCar.Common.Utils;

namespace PushCar.Server;

internal static class Program {
	private const int LISTEN_PORT = 9000;

	private static void Main() {
		// 콘솔 입출력 한글 깨짐 수정
		Console.InputEncoding = Encoding.Unicode;
		Console.OutputEncoding = Encoding.Unicode;

		Debug.Enabled = true;

		// 서버 시작
		GameServer server = new GameServer(LISTEN_PORT);
		server.Start();
	}
}
