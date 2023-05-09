using System.IO;

namespace PushCar.Common.Packets.Client {
	public class ClientRequestSaltPacket : IPacket {
		public PacketType Type => PacketType.ClientRequestSalt;
		public string Id { get; }

		public ClientRequestSaltPacket(string id) {
			Id = id;
		}

		public ClientRequestSaltPacket(BinaryReader reader) {
			Id = reader.ReadString();
		}

		public void Serialize(BinaryWriter writer) {
			writer.Write(Id);
		}

		public override string ToString() {
			return $"{nameof(ClientRequestSaltPacket)} : {{{nameof(Id)}: {Id}}}";
		}
	}
}
