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

	public bool Login(string id, string password) {
		var idParam = (new MySqlParameter("@id", MySqlDbType.VarChar, 32), id);
		var passwordParam = (new MySqlParameter("@password", MySqlDbType.VarBinary, 256), pw: password);

		using MySqlDataReader reader = _database.Execute("SELECT 1 FROM user WHERE id = @id AND password = @password", idParam, passwordParam);
		return reader.HasRows;
	}

	public bool Register(string id, string password) {
		var idParam = (new MySqlParameter("@id", MySqlDbType.VarChar, 32), id);
		var passwordParam = (new MySqlParameter("@password", MySqlDbType.VarBinary, 256), password);

		using MySqlDataReader reader = _database.Execute("INSERT user VALUES (@id, @password)", idParam, passwordParam);
		return reader.RecordsAffected >= 1;
	}
}
