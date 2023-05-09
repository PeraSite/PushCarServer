using System;
using System.IO;
using PushCar.Common.Packets.Client;
using PushCar.Common.Packets.Server;

namespace PushCar.Common {
	public enum PacketType : byte {
		ClientPing,
		ServerPong,

		ClientRequestSalt,
		ServerResponseSalt,
		ClientRegister,
		ClientLogin,
		ServerAuthenticateSuccess,
		ServerAuthenticateFail,

		ClientRecord,

		ClientRequestRank,
		ServerResponseRank
	}

	public static class PacketTypes {
		public static IPacket CreatePacket(this PacketType type, BinaryReader reader) {
			return type switch {
				PacketType.ClientPing => new ClientPingPacket(),
				PacketType.ServerPong => new ServerPongPacket(),

				PacketType.ClientRequestSalt => new ClientRequestSaltPacket(reader),
				PacketType.ServerResponseSalt => new ServerResponseSaltPacket(reader),

				PacketType.ClientRegister => new ClientRegisterPacket(reader),
				PacketType.ClientLogin => new ClientLoginPacket(reader),

				PacketType.ServerAuthenticateSuccess => new ServerAuthenticateSuccessPacket(reader),
				PacketType.ServerAuthenticateFail => new ServerAuthenticateFailPacket(reader),

				PacketType.ClientRecord => new ClientRecordPacket(reader),

				PacketType.ClientRequestRank => new ClientRequestRankPacket(reader),
				PacketType.ServerResponseRank => new ServerResponseRankPacket(reader),
				_ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
			};
		}

		public static TPacket CreatePacket<TPacket>(this PacketType type, BinaryReader reader) where TPacket : IPacket {
			return (TPacket)type.CreatePacket(reader);
		}
	}
}
