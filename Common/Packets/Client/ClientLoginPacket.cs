using System.IO;

namespace PushCar.Common.Packets.Client {
	public class ClientLoginPacket : IPacket {
		public PacketType Type => PacketType.ClientLogin;
		public string Id { get; }
		public string EncryptedPassword { get; }

		public ClientLoginPacket(string id, string encryptedPassword) {
			Id = id;
			EncryptedPassword = encryptedPassword;
		}

		public ClientLoginPacket(BinaryReader reader) {
			Id = reader.ReadString();
			EncryptedPassword = reader.ReadString();
		}

		public void Serialize(BinaryWriter writer) {
			writer.Write(Id);
			writer.Write(EncryptedPassword);
		}

		public override string ToString() {
			return $"{nameof(ClientLoginPacket)} : {{{nameof(Id)}: {Id}, {nameof(EncryptedPassword)}: {EncryptedPassword}}}";
		}
	}
}
