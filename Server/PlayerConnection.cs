using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using PushCar.Common;
using PushCar.Common.Extensions;
using PushCar.Common.Utils;

namespace PushCar.Server;

public class PlayerConnection : IDisposable {
	public TcpClient Client { get; }
	public SslStream Stream { get; }
	public BinaryReader Reader { get; }
	public BinaryWriter Writer { get; }
	public IPEndPoint IP => (IPEndPoint)Client.Client.RemoteEndPoint!;

	public PlayerConnection(TcpClient client) {
		Client = client;

		// Create Ssl Stream
		Stream = new SslStream(Client.GetStream(), false);
		var certificate = new X509Certificate2("server.pfx", "password");

		Stream.AuthenticateAsServer(certificate, false,
			System.Security.Authentication.SslProtocols.Tls12, false);

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
