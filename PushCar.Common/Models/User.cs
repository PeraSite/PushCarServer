namespace PushCar.Common.Models {
	public struct User {
		public string Id { get; }
		public string EncryptedPassword { get; }

		public User(string id, string encryptedPassword) {
			Id = id;
			EncryptedPassword = encryptedPassword;
		}
	}
}
