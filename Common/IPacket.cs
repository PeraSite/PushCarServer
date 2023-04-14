using System.IO;

namespace PushCar.Common {
	public interface IPacket {
		public PacketType Type { get; }
		public void Serialize(BinaryWriter writer);
	}
}
