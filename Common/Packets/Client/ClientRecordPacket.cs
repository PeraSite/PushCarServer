using System;
using System.IO;
using PushCar.Common.Extensions;

namespace PushCar.Common.Packets.Client {
	public class ClientRecordPacket : IPacket {
		public PacketType Type => PacketType.ClientRecord;
		public Guid Token { get; }
		public float SwipeDistance { get; }

		public ClientRecordPacket(Guid token, float swipeDistance) {
			Token = token;
			SwipeDistance = swipeDistance;
		}

		public ClientRecordPacket(BinaryReader reader) {
			Token = reader.ReadGuid();
			SwipeDistance = reader.ReadSingle();
		}

		public void Serialize(BinaryWriter writer) {
			writer.Write(Token);
			writer.Write(SwipeDistance);
		}

		public override string ToString() {
			return $"{nameof(ClientRecordPacket)} {{ {nameof(Token)}: {Token}, {nameof(SwipeDistance)}: {SwipeDistance}}}";
		}
	}
}
