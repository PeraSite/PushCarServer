using System.IO;

namespace PushCar.Common.Packets.Client {
	public class ClientRegisterPacket : IPacket {
		public PacketType Type => PacketType.ClientRegister;
		public string Id { get; }
		public string EncryptedPassword { get; }
		public string Salt { get; }

		public ClientRegisterPacket(string id, string encryptedPassword, string salt) {
			Id = id;
			EncryptedPassword = encryptedPassword;
			Salt = salt;
		}

		public ClientRegisterPacket(BinaryReader reader) {
			Id = reader.ReadString();
			EncryptedPassword = reader.ReadString();
			Salt = reader.ReadString();
		}

		public void Serialize(BinaryWriter writer) {
			writer.Write(Id);
			writer.Write(EncryptedPassword);
			writer.Write(Salt);
		}

		public override string ToString() {
			return $"{nameof(ClientRegisterPacket)} : {{{nameof(Id)}: {Id}, {nameof(EncryptedPassword)}: {EncryptedPassword}, {nameof(Salt)}: {Salt}}}";
		}
	}
}
