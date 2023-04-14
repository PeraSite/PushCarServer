using System.IO;

namespace PushCar.Common.Packets.Server {
	public class ServerPongPacket : IPacket {
		public PacketType Type => PacketType.ServerPong;
		public void Serialize(BinaryWriter writer) { }

		public override string ToString() {
			return $"{nameof(ServerPongPacket)} {{}}";
		}
	}
}
