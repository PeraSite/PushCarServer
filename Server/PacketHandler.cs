using PushCar.Common;
using PushCar.Common.Models;
using PushCar.Common.Packets.Client;
using PushCar.Common.Packets.Server;
using PushCar.Server.Repository;

namespace PushCar.Server;

public class PacketHandler {
	private readonly UserRepository _userRepository;
	private readonly RecordRepository _recordRepository;

	public PacketHandler(UserRepository userRepository, RecordRepository recordRepository) {
		_userRepository = userRepository;
		_recordRepository = recordRepository;
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
		if (_userRepository.ExistUser(packet.Id)) {
			var result = _userRepository.Login(packet.Id, packet.EncryptedPassword);
			playerConnection.SendPacket(new ServerAuthenticatePacket(result));
			Console.WriteLine($"[TCP 서버] 클라이언트 {packet.Id} 로그인: {result}");
		} else {
			var result = _userRepository.Register(packet.Id, packet.EncryptedPassword);
			playerConnection.SendPacket(new ServerAuthenticatePacket(result));
			Console.WriteLine($"[TCP 서버] 클라이언트 {packet.Id} 회원가입: {result}");
		}
	}

	private void HandleClientRecordPacket(PlayerConnection playerConnection, ClientRecordPacket packet) {
		// 계산용 상수
		const float carPosition = -7f;
		const float flagPosition = 7.5f;

		var swipeDistance = packet.SwipeDistance;
		var distance = flagPosition - (carPosition + swipeDistance);
		if (distance < 0f) {
			Console.WriteLine($"[TCP 서버] 클라이언트 {packet.Id}의 기록: {distance}m, 올바르지 않음");
			return;
		}
		Console.WriteLine($"[TCP 서버] 클라이언트 {packet.Id}의 기록: {distance}m");
		_recordRepository.AddRecord(new Record(packet.Id, distance));
	}

	private void HandleClientRequestRankPacket(PlayerConnection playerConnection, ClientRequestRankPacket packet) {
		var records = _recordRepository.GetRecords();
		playerConnection.SendPacket(new ServerResponseRankPacket(records));
	}
}
