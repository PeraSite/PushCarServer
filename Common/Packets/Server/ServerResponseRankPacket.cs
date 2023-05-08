using System.Collections.Generic;
using System.IO;
using System.Linq;
using PushCar.Common.Models;

namespace PushCar.Common.Packets.Server {
	public class ServerResponseRankPacket : IPacket {
		public PacketType Type => PacketType.ServerResponseRank;
		public List<Record> Records { get; }
		public int CurrentPage { get; }
		public int MaxPage { get; }

		public ServerResponseRankPacket(List<Record> records, int currentPage, int maxPage) {
			Records = records;
			CurrentPage = currentPage;
			MaxPage = maxPage;
		}

		public ServerResponseRankPacket(BinaryReader reader) {
			Records = new List<Record>();
			var count = reader.ReadInt32();
			for (var i = 0; i < count; i++) {
				Records.Add(new Record(reader));
			}
			CurrentPage = reader.ReadInt32();
			MaxPage = reader.ReadInt32();
		}

		public void Serialize(BinaryWriter writer) {
			writer.Write(Records.Count);
			foreach (var record in Records) {
				record.Serialize(writer);
			}
			writer.Write(CurrentPage);
			writer.Write(MaxPage);
		}

		public override string ToString() {
			var recordStr = string.Join(", ", Records.Select(x => $"{x.Id}:{x.Distance}"));
			return $"{nameof(ServerResponseRankPacket)} {{{nameof(Records)}: {recordStr}, {nameof(CurrentPage)}: {CurrentPage}, {nameof(MaxPage)}: {MaxPage} }}";
		}
	}
}
