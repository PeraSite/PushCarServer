using System.IO;

namespace PushCar.Common.Packets.Client {
	public class ClientRecordPacket : IPacket {
		public PacketType Type => PacketType.ClientRecord;
		public string Id { get; }
		public float SwipeDistance { get; }

		public ClientRecordPacket(string id, float swipeDistance) {
			Id = id;
			SwipeDistance = swipeDistance;
		}

		public ClientRecordPacket(BinaryReader reader) {
			Id = reader.ReadString();
			SwipeDistance = reader.ReadSingle();
		}

		public void Serialize(BinaryWriter writer) {
			writer.Write(Id);
			writer.Write(SwipeDistance);
		}

		public override string ToString() {
			return $"{nameof(ClientRecordPacket)} {{ {nameof(Id)}: {Id}, {nameof(SwipeDistance)}: {SwipeDistance}}}";
		}
	}
}
