using System.Collections.Generic;
using System.IO;
using PushCar.Common.Models;

namespace PushCar.Common.Packets.Server {
	public class ServerResponseRankPacket : IPacket {
		public PacketType Type => PacketType.ServerResponseRank;
		public List<Record> Records { get; }

		public ServerResponseRankPacket(List<Record> records) {
			Records = records;
		}

		public ServerResponseRankPacket(BinaryReader reader) {
			Records = new List<Record>();
			var count = reader.ReadInt32();
			for (var i = 0; i < count; i++) {
				Records.Add(new Record(reader));
			}
		}

		public void Serialize(BinaryWriter writer) {
			writer.Write(Records.Count);
			foreach (var record in Records) {
				record.Serialize(writer);
			}
		}

		public override string ToString() {
			return $"{nameof(ServerPongPacket)} {{}}";
		}
	}
}
