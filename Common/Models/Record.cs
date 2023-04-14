using System;
using System.IO;

namespace PushCar.Common.Models {
	public struct Record {
		public string Id { get; }
		public float Distance { get; }
		public DateTime Time { get; }

		public Record(string id, float distance) {
			Id = id;
			Distance = distance;
			Time = DateTime.Now;
		}

		public Record(string id, float distance, DateTime time) {
			Id = id;
			Distance = distance;
			Time = time;
		}

		public Record(BinaryReader reader) {
			Id = reader.ReadString();
			Distance = reader.ReadSingle();
			Time = DateTime.FromBinary(reader.ReadInt64());
		}

		public void Serialize(BinaryWriter writer) {
			writer.Write(Id);
			writer.Write(Distance);
			writer.Write(Time.ToBinary());
		}
	}
}
