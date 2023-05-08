using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using PushCar.Common;
using PushCar.Common.Models;
using PushCar.Common.Packets.Client;
using PushCar.Common.Packets.Server;
using PushCar.Common.Utils;
using PushCar.Server.Repository;

namespace PushCar.Server;

public class GameServer : IDisposable {
	private readonly PacketHandler _packetHandler;
	private readonly TcpListener _server;
	private readonly List<PlayerConnection> _playerConnections;
	private readonly ConcurrentQueue<(PlayerConnection playerConnection, IPacket packet)> _receivedPacketQueue;

	public GameServer(int port, PacketHandler packetHandler) {
		_server = new TcpListener(IPAddress.Any, port);
		_playerConnections = new List<PlayerConnection>();
		_receivedPacketQueue = new ConcurrentQueue<(PlayerConnection playerConnection, IPacket packet)>();

		_packetHandler = packetHandler;
	}

	public void Dispose() {
		Debug.Log("[TCP 서버] 서버 종료");
		_server.Stop();
		GC.SuppressFinalize(this);
	}

	public void Start() {
		// TCP 서버 시작
		_server.Start();

		Debug.Log("[TCP 서버] 서버 시작");

		// 패킷 Dequeue Thread
		var dequeueThread = new Thread(() => {
			try {
				while (true) {
					if (_receivedPacketQueue.TryDequeue(out var tuple)) {
						var (playerConnection, packet) = tuple;

						// handle packet
						_packetHandler.HandlePacket(packet, playerConnection);
					}
				}
			}
			catch (Exception e) {
				Console.WriteLine(e);
				throw;
			}
		});
		dequeueThread.Start();

		try {
			// 서버가 켜진 동안 클라이언트 접속 받기
			while (true) {
				// 새로운 TCP 클라이언트 접속 받기
				var client = _server.AcceptTcpClient();

				// 새 스레드에서 클라이언트 처리
				var thread = new Thread(() => HandleNewClient(client));
				thread.Start();
			}
		}
		catch (Exception e) {
			Console.WriteLine(e);
			throw;
		}
	}

	private void HandleNewClient(TcpClient client) {
		// PlayerConnection 생성
		var playerConnection = new PlayerConnection(client);
		_playerConnections.Add(playerConnection);
		Debug.Log($"[TCP 서버] 클라이언트 접속: {playerConnection}");

		// 패킷 읽기
		try {
			while (client.Connected) {
				// 패킷 읽기
				var packet = playerConnection.ReadPacket();

				// 패킷 큐에 추가
				_receivedPacketQueue.Enqueue((playerConnection, packet));
			}
		}
		catch (IOException) {
			// 클라이언트가 강제 종료했을 때 처리
		}
		catch (Exception e) {
			Console.WriteLine(e);
			throw;
		}
		finally {
			// 클라이언트 접속 종료 처리
			HandleClientQuit(playerConnection);
		}
	}

	private void HandleClientQuit(PlayerConnection playerConnection) {
		var address = playerConnection.IP;

		// PlayerConnection Dictionary 에서 삭제
		_playerConnections.Remove(playerConnection);

		// 클라이언트 닫기
		playerConnection.Dispose();

		Debug.Log("[TCP 서버] 클라이언트 종료: IP 주소={0}, 포트 번호={1}", address.Address, address.Port);
	}
}
