using System.IO;

namespace PushCar.Common.Packets.Client {
	public class ClientHandshakePacket : IPacket {
		public PacketType Type => PacketType.ClientHandshake;
		public void Serialize(BinaryWriter writer) { }
		public void Deserialize(BinaryReader reader) { }
	}
}
