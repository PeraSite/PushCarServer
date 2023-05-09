using System;
using System.Security.Cryptography;
using System.Text;

namespace PushCar.Common.Utils {
	public static class CryptoUtil {
		public static string SHA256(string data) {
			var sha = new SHA256Managed();
			var hash = sha.ComputeHash(Encoding.ASCII.GetBytes(data));
			var sb = new StringBuilder();
			foreach (var b in hash) {
				sb.AppendFormat("{0:x2}", b);
			}
			return sb.ToString();
		}

		public static string GetUniqueString(int length) {
			using var rng = new RNGCryptoServiceProvider();
			var bitCount = length * 6;
			var byteCount = (bitCount + 7) / 8;
			var bytes = new byte[byteCount];
			rng.GetBytes(bytes);
			return Convert.ToBase64String(bytes);
		}
	}
}
