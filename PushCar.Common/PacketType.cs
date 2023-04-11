using System;
using System.IO;

namespace PushCar.Common {
	public enum PacketType : byte {
		ClientHandshake,
		ServerHandshake,

		ClientPing,
		ServerPong,
	}

	public static class PacketTypes {
		private static IPacket CreatePacket(this PacketType type, BinaryReader reader) {
			return type switch {
				// TODO
				_ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
			};
		}

		public static TPacket CreatePacket<TPacket>(this PacketType type, BinaryReader reader) where TPacket : IPacket {
			return (TPacket)type.CreatePacket(reader);
		}
	}
}
