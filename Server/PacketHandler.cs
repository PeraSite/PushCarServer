using System.Net;
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
			case ClientAuthenticatePacket packet: {
				HandleClientAuthenticatePacket(playerConnection, packet);
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

	private void HandleClientAuthenticatePacket(PlayerConnection playerConnection, ClientAuthenticatePacket packet) {
		var existUser = _userRepository.ExistUser(packet.Id);

		var authSuccess = existUser
			? _userRepository.Login(packet.Id, packet.EncryptedPassword)
			: _userRepository.Register(packet.Id, packet.EncryptedPassword);

		if (authSuccess) {
			var token = Guid.NewGuid();

			playerConnection.ID = packet.Id;
			playerConnection.Token = token;

			_gameServer.AddToken(playerConnection, token);
			playerConnection.SendPacket(new ServerAuthenticateSuccessPacket(token));
			Console.WriteLine($"[TCP 서버] 클라이언트 {packet.Id} 접속 성공 - {token}");
		} else {
			playerConnection.SendPacket(new ServerAuthenticateFailPacket("아이디 또는 비밀번호가 올바르지 않습니다."));
			Console.WriteLine($"[TCP 서버] 클라이언트 {packet.Id} 접속 실패");
		}
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
		var records = _recordRepository.GetRecords();
		playerConnection.SendPacket(new ServerResponseRankPacket(records));
	}
}
