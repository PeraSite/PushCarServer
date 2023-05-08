using System.IO;

namespace PushCar.Common.Packets.Client {
	public class ClientRequestRankPacket : IPacket {
		public PacketType Type => PacketType.ClientRequestRank;

		public int Page { get; }
		public int RecordsPerPage { get; }

		public ClientRequestRankPacket(int page, int recordsPerPage) {
			Page = page;
			RecordsPerPage = recordsPerPage;
		}

		public ClientRequestRankPacket(BinaryReader reader) {
			Page = reader.ReadInt32();
			RecordsPerPage = reader.ReadInt32();
		}

		public void Serialize(BinaryWriter writer) {
			writer.Write(Page);
			writer.Write(RecordsPerPage);
		}

		public override string ToString() {
			return $"{nameof(ClientRequestRankPacket)} {{{nameof(Page)}: {Page}, {nameof(RecordsPerPage)}: {RecordsPerPage}}}}}";
		}
	}
}
