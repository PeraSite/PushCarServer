using MySql.Data.MySqlClient;
using PushCar.Server.Database;

namespace PushCar.Server.Repository;

public class UserRepository {
	private readonly MySqlDatabase _database;

	public UserRepository(MySqlDatabase database) {
		_database = database;
	}

	public bool IsExistUser(string username) {
		var usernameParam = (new MySqlParameter("@name", MySqlDbType.VarChar, 256), username);
		using MySqlDataReader reader = _database.Execute("SELECT 1 FROM user WHERE name = @name", usernameParam);
		return reader.RecordsAffected > 0;
	}
}
