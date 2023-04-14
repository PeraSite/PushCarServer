using System.IO;

namespace PushCar.Common.Packets.Server {
	public class ServerAuthenticatePacket : IPacket {
		public PacketType Type => PacketType.ServerAuthenticate;
		public bool Success { get; }

		public ServerAuthenticatePacket(bool success) {
			Success = success;
		}

		public ServerAuthenticatePacket(BinaryReader reader) {
			Success = reader.ReadBoolean();
		}

		public void Serialize(BinaryWriter writer) {
			writer.Write(Success);
		}

		public override string ToString() {
			return $"{nameof(ServerAuthenticatePacket)} : {{{nameof(Success)}: {Success}}}";
		}
	}
}
