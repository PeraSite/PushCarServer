using System;
using System.IO;
using PushCar.Common.Extensions;

namespace PushCar.Common.Packets.Server {
	public class ServerAuthenticateSuccessPacket : IPacket {
		public PacketType Type => PacketType.ServerAuthenticateSuccess;
		public Guid Token { get; }

		public ServerAuthenticateSuccessPacket(Guid token) {
			Token = token;
		}

		public ServerAuthenticateSuccessPacket(BinaryReader reader) {
			Token = reader.ReadGuid();
		}

		public void Serialize(BinaryWriter writer) {
			writer.Write(Token);
		}

		public override string ToString() {
			return $"{nameof(ServerAuthenticateSuccessPacket)} : {{{nameof(Token)} : {Token}}}";
		}
	}
}
