using System.Text;
using PushCar.Common.Utils;
using PushCar.Server.Database;
using PushCar.Server.Repository;

namespace PushCar.Server;

internal static class Program {
	private const int LISTEN_PORT = 9000;

	private static void Main() {
		// 콘솔 입출력 한글 깨짐 수정
		Console.InputEncoding = Encoding.Unicode;
		Console.OutputEncoding = Encoding.Unicode;

		// 디버깅 출력 설정
		Debug.Enabled = true;

		// 환경변수 가져오기
		var ip = GetEnvironmentVariable("MYSQL_SERVER");
		var port = uint.Parse(GetEnvironmentVariable("MYSQL_PORT"));
		var databaseName = GetEnvironmentVariable("MYSQL_DATABASE");
		var sqlUser = GetEnvironmentVariable("MYSQL_USER");
		var password = GetEnvironmentVariable("MYSQL_PASSWORD");

		// DB 초기화
		using MySqlDatabase database = new MySqlDatabase(ip, port, databaseName, sqlUser, password);
		UserRepository userRepository = new UserRepository(database);

		// 서버 시작
		GameServer server = new GameServer(LISTEN_PORT, userRepository);
		server.Start();
	}

	private static string GetEnvironmentVariable(string key)
		=> Environment.GetEnvironmentVariable(key) ?? throw new Exception($"{key} is not set");
}
