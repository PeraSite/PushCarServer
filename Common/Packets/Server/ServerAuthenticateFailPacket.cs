using System.IO;

namespace PushCar.Common.Packets.Server {
	public class ServerAuthenticateFailPacket : IPacket {
		public PacketType Type => PacketType.ServerAuthenticateFail;
		public string Reason { get; }

		public ServerAuthenticateFailPacket(string reason) {
			Reason = reason;
		}

		public ServerAuthenticateFailPacket(BinaryReader reader) {
			Reason = reader.ReadString();
		}

		public void Serialize(BinaryWriter writer) {
			writer.Write(Reason);
		}

		public override string ToString() {
			return $"{nameof(ServerAuthenticateSuccessPacket)} : {{{nameof(Reason)}: {Reason}}}";
		}
	}
}
