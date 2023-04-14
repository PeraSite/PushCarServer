using System.Net;
using System.Net.Sockets;
using PushCar.Common;
using PushCar.Common.Extensions;
using PushCar.Common.Utils;

namespace PushCar.Server;

public class PlayerConnection : IDisposable {
	public TcpClient Client { get; }
	public NetworkStream Stream { get; }
	public BinaryReader Reader { get; }
	public BinaryWriter Writer { get; }
	public IPEndPoint IP => (IPEndPoint)Client.Client.RemoteEndPoint!;

	public PlayerConnection(TcpClient client) {
		Client = client;

		Stream = Client.GetStream();
		Writer = new BinaryWriter(Stream);
		Reader = new BinaryReader(Stream);
	}

	public IPacket ReadPacket() {
		try {
			var id = Stream.ReadByte();
			// 읽을 수 없다면(데이터가 끝났다면 리턴)
			if (id == -1) throw new IOException("EOF");

			// 타입에 맞는 패킷 객체 생성
			var packetType = (PacketType)id;
			var packet = packetType.CreatePacket(Reader);
			Debug.Log($"[C({ToString()}) -> S] {packet}");

			return packet;
		}
		catch (IOException e) {
			throw;
		}
		catch (Exception e) {
			Console.WriteLine(e);
			throw;
		}
	}

	public void SendPacket(IPacket packet) {
		if (!Stream.CanRead) return;
		if (!Stream.CanWrite) return;
		if (!Client.Connected) {
			Debug.Log($"[S -> C({ToString()})] Cannot send packet due to disconnected: {packet}");
			return;
		}
		Debug.Log($"[S -> C({ToString()})] {packet}");
		Writer.Write(packet);
	}

	public override string ToString() => $"{IP.Address}:{IP.Port}";

	public void Dispose() {
		Stream.Dispose();
		Reader.Dispose();
		Writer.Dispose();
		Client.Dispose();
		GC.SuppressFinalize(this);
	}
}
