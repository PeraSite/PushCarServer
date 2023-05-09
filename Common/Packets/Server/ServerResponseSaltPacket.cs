using System.IO;

namespace PushCar.Common.Packets.Server {
	public class ServerResponseSaltPacket : IPacket {
		public PacketType Type => PacketType.ServerResponseSalt;
		public string Salt { get; }

		public ServerResponseSaltPacket(string salt) {
			Salt = salt;
		}

		public ServerResponseSaltPacket(BinaryReader reader) {
			Salt = reader.ReadString();
		}

		public void Serialize(BinaryWriter writer) {
			writer.Write(Salt);
		}

		public override string ToString() {
			return $"{nameof(ServerResponseSaltPacket)} : {{{nameof(Salt)}: {Salt}}}";
		}
	}
}
