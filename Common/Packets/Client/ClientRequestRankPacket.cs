using System.IO;

namespace PushCar.Common.Packets.Client {
	public class ClientRequestRankPacket : IPacket {
		public PacketType Type => PacketType.ClientRequestRank;
		public void Serialize(BinaryWriter writer) { }

		public override string ToString() {
			return $"{nameof(ClientRequestRankPacket)} {{}}";
		}
	}
}
