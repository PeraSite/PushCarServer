using System.IO;

namespace PushCar.Common.Packets.Client {
	public class ClientAuthenticatePacket : IPacket {
		public PacketType Type => PacketType.ClientAuthenticate;
		public string Id { get; }
		public string EncryptedPassword { get; }

		public ClientAuthenticatePacket(string id, string encryptedPassword) {
			Id = id;
			EncryptedPassword = encryptedPassword;
		}

		public ClientAuthenticatePacket(BinaryReader reader) {
			Id = reader.ReadString();
			EncryptedPassword = reader.ReadString();
		}

		public void Serialize(BinaryWriter writer) {
			writer.Write(Id);
			writer.Write(EncryptedPassword);
		}

		public override string ToString() {
			return $"{nameof(ClientAuthenticatePacket)} : {{{nameof(Id)}: {Id}, {nameof(EncryptedPassword)}: {EncryptedPassword}}}";
		}
	}
}
