using PushCar.Common;
using PushCar.Common.Models;
using PushCar.Common.Packets.Client;
using PushCar.Common.Packets.Server;
using PushCar.Server.Repository;

namespace PushCar.Server;

public class PacketHandler {
	private readonly UserRepository _userRepository;
	private readonly RecordRepository _recordRepository;
	private readonly GameServer _gameServer;

	public PacketHandler(UserRepository userRepository, RecordRepository recordRepository, GameServer gameServer) {
		_userRepository = userRepository;
		_recordRepository = recordRepository;
		_gameServer = gameServer;
	}

	public void HandlePacket(IPacket basePacket, PlayerConnection playerConnection) {
		switch (basePacket) {
			case ClientPingPacket packet: {
				HandleClientPingPacket(playerConnection, packet);
				break;
			}
			case ClientLoginPacket packet: {
				HandleClientLoginPacket(playerConnection, packet);
				break;
			}
			case ClientRegisterPacket packet: {
				HandleClientRegisterPacket(playerConnection, packet);
				break;
			}
			case ClientRequestSaltPacket packet: {
				HandleClientRequestSaltPacket(playerConnection, packet);
				break;
			}
			case ClientRecordPacket packet: {
				HandleClientRecordPacket(playerConnection, packet);
				break;
			}
			case ClientRequestRankPacket packet: {
				HandleClientRequestRankPacket(playerConnection, packet);
				break;
			}
			default:
				throw new ArgumentOutOfRangeException(nameof(basePacket));
		}
	}

	private void HandleClientPingPacket(PlayerConnection playerConnection, ClientPingPacket packet) {
		playerConnection.SendPacket(new ServerPongPacket());
	}

	private void HandleClientLoginPacket(PlayerConnection playerConnection, ClientLoginPacket packet) {
		if (!_userRepository.ExistUser(packet.Id)) {
			playerConnection.SendPacket(new ServerAuthenticateFailPacket("아이디 또는 비밀번호가 올바르지 않습니다."));
			return;
		}

		var success = _userRepository.Login(packet.Id, packet.EncryptedPassword);

		if (success) {
			var token = Guid.NewGuid();

			// ID와 토큰 할당
			playerConnection.ID = packet.Id;
			playerConnection.Token = token;

			// Valid한 토큰 목록에 추가
			_gameServer.AddToken(playerConnection, token);

			playerConnection.SendPacket(new ServerAuthenticateSuccessPacket(token));
			Console.WriteLine($"[TCP 서버] 클라이언트 {packet.Id} 로그인 성공 - {token}");
		} else {
			playerConnection.SendPacket(new ServerAuthenticateFailPacket("아이디 또는 비밀번호가 올바르지 않습니다."));
			Console.WriteLine($"[TCP 서버] 클라이언트 {packet.Id} 접속 실패");
		}
	}

	private void HandleClientRegisterPacket(PlayerConnection playerConnection, ClientRegisterPacket packet) {
		if (_userRepository.ExistUser(packet.Id)) {
			playerConnection.SendPacket(new ServerAuthenticateFailPacket("아이디 또는 비밀번호가 올바르지 않습니다."));
			return;
		}

		var success = _userRepository.Register(packet.Id, packet.EncryptedPassword, packet.Salt);

		if (success) {
			var token = Guid.NewGuid();

			// ID와 토큰 할당
			playerConnection.ID = packet.Id;
			playerConnection.Token = token;

			// Valid한 토큰 목록에 추가
			_gameServer.AddToken(playerConnection, token);

			playerConnection.SendPacket(new ServerAuthenticateSuccessPacket(token));
			Console.WriteLine($"[TCP 서버] 클라이언트 {packet.Id} 계정 생성 성공 - {token}");
		} else {
			playerConnection.SendPacket(new ServerAuthenticateFailPacket("계정 생성에 실패했습니다."));
			Console.WriteLine($"[TCP 서버] 클라이언트 {packet.Id} 접속 실패");
		}
	}

	private void HandleClientRequestSaltPacket(PlayerConnection playerConnection, ClientRequestSaltPacket packet) {
		if (!_userRepository.ExistUser(packet.Id)) {
			playerConnection.SendPacket(new ServerAuthenticateFailPacket("아이디 또는 비밀번호가 올바르지 않습니다."));
			return;
		}

		var salt = _userRepository.GetSalt(packet.Id);
		playerConnection.SendPacket(new ServerResponseSaltPacket(salt));
	}

	private void HandleClientRecordPacket(PlayerConnection playerConnection, ClientRecordPacket packet) {
		if (packet.Token != playerConnection.Token) {
			Console.WriteLine($"[TCP 서버] 클라이언트 {packet.Token}의 기록: 토큰 불일치");
			return;
		}

		if (!_gameServer.ExistToken(packet.Token)) {
			Console.WriteLine($"[TCP 서버] 클라이언트 {packet.Token}의 기록: 존재하지 않는 토큰");
			return;
		}

		// 계산용 상수
		const float carPosition = -7f;
		const float flagPosition = 7.5f;

		var swipeDistance = packet.SwipeDistance;
		var distance = flagPosition - (carPosition + swipeDistance);
		if (distance < 0f) {
			Console.WriteLine($"[TCP 서버] 클라이언트 {playerConnection.ID}의 기록: {distance}m, 올바르지 않음");
			return;
		}
		Console.WriteLine($"[TCP 서버] 클라이언트 {playerConnection.ID}의 기록: {distance}m");
		_recordRepository.AddRecord(new Record(playerConnection.ID, distance));
	}

	private void HandleClientRequestRankPacket(PlayerConnection playerConnection, ClientRequestRankPacket packet) {
		var count = _recordRepository.GetRankCount();
		var records = _recordRepository.GetRanks(packet.Page, packet.RecordsPerPage);
		playerConnection.SendPacket(new ServerResponseRankPacket(records, packet.Page, count / packet.RecordsPerPage));
	}
}
