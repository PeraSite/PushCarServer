using System;
using System.IO;

namespace PushCar.Common.Extensions {
	public static class BinaryReaderExtensions {
		public static Guid ReadGuid(this BinaryReader reader) {
			return new Guid(reader.ReadBytes(16));
		}
	}
}
