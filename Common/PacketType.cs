using System;
using System.IO;
using PushCar.Common.Packets.Client;
using PushCar.Common.Packets.Server;

namespace PushCar.Common {
	public enum PacketType : byte {
		ClientPing,
		ServerPong,

		ClientAuthenticate,
		ServerAuthenticate,
	}

	public static class PacketTypes {
		public static IPacket CreatePacket(this PacketType type, BinaryReader reader) {
			return type switch {
				PacketType.ClientPing => new ClientPingPacket(),
				PacketType.ServerPong => new ServerPongPacket(),

				_ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
			};
		}

		public static TPacket CreatePacket<TPacket>(this PacketType type, BinaryReader reader) where TPacket : IPacket {
			return (TPacket)type.CreatePacket(reader);
		}
	}
}
