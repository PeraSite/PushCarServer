using MySql.Data.MySqlClient;
using PushCar.Server.Database;

namespace PushCar.Server.Repository;

public class UserRepository {
	private readonly MySqlDatabase _database;

	public UserRepository(MySqlDatabase database) {
		_database = database;
	}

	public bool ExistUser(string id) {
		var idParam = (new MySqlParameter("@id", MySqlDbType.VarChar, 32), id);

		using MySqlDataReader reader = _database.Execute("SELECT 1 FROM user WHERE id = @id", idParam);
		return reader.HasRows;
	}

	public string GetSalt(string id) {
		var idParam = (new MySqlParameter("@id", MySqlDbType.VarChar, 32), id);

		using MySqlDataReader reader = _database.Execute("SELECT salt FROM user WHERE id = @id", idParam);

		// 존재하지 않는 ID라면 빈 문자열 반환
		return reader.Read() ? reader.GetString(0) : "";
	}

	public bool Login(string id, string passwordHash) {
		var idParam = (new MySqlParameter("@id", MySqlDbType.VarChar, 32), id);
		var passwordParam = (new MySqlParameter("@password", MySqlDbType.VarBinary, 256), pw: passwordHash);

		using MySqlDataReader reader = _database.Execute("SELECT 1 FROM user WHERE id = @id AND password = @password", idParam, passwordParam);
		return reader.HasRows;
	}

	public bool Register(string id, string passwordHash, string salt) {
		var idParam = (new MySqlParameter("@id", MySqlDbType.VarChar, 32), id);
		var passwordParam = (new MySqlParameter("@password", MySqlDbType.VarBinary, 256), passwordHash);
		var saltParam = (new MySqlParameter("@salt", MySqlDbType.VarChar, 32), salt);

		using MySqlDataReader reader = _database.Execute("INSERT user VALUES (@id, @password, @salt)", idParam, passwordParam, saltParam);
		return reader.RecordsAffected >= 1;
	}
}
