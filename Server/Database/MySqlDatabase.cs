using MySql.Data.MySqlClient;

namespace PushCar.Server.Database;

public class MySqlDatabase : IDisposable {
	private readonly MySqlConnection _connection;

	public MySqlDatabase(
		string server,
		uint port,
		string database,
		string user,
		string password
	) {
		MySqlConnectionStringBuilder connectionString = new MySqlConnectionStringBuilder {
			Server = server,
			Port = port,
			Database = database,
			UserID = user,
			Password = password
		};
		_connection = new MySqlConnection(connectionString.ToString());
		_connection.Open();
	}

	~MySqlDatabase() {
		Dispose(false);
	}

	public MySqlDataReader Execute(string query, params (MySqlParameter param, object value)[] parameters) {
		MySqlCommand cmd = new MySqlCommand(query, _connection);

		foreach ((MySqlParameter param, var value) in parameters)
			cmd.Parameters.Add(param).Value = value;

		MySqlDataReader rdr = cmd.ExecuteReader();
		return rdr;
	}

	public int ExecuteNonQuery(string query, params (MySqlParameter param, object value)[] parameters) {
		MySqlCommand cmd = new MySqlCommand(query, _connection);

		foreach ((MySqlParameter param, var value) in parameters)
			cmd.Parameters.Add(param).Value = value;

		return cmd.ExecuteNonQuery();
	}

	private void Dispose(bool disposing) {
		if (disposing) {
			_connection.Dispose();
		}
	}

	public void Dispose() {
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}
