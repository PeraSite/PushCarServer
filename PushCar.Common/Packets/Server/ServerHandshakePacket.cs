using System.IO;

namespace PushCar.Common.Packets.Server {
	public class ServerHandshakePacket : IPacket {
		public PacketType Type => PacketType.ServerPong;
		public void Serialize(BinaryWriter writer) { }
		public void Deserialize(BinaryReader reader) { }
	}
}
