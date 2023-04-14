using MySql.Data.MySqlClient;
using PushCar.Common.Models;
using PushCar.Server.Database;

namespace PushCar.Server.Repository;

public class RecordRepository {
	private readonly MySqlDatabase _database;

	public RecordRepository(MySqlDatabase database) {
		_database = database;
	}

	public bool AddRecord(Record record) {
		var idParam = (new MySqlParameter("@id", MySqlDbType.VarChar, 32), record.Id);
		var distanceParam = (new MySqlParameter("@distance", MySqlDbType.Float), Math.Round(record.Distance, 2));
		var timeParam = (new MySqlParameter("@time", MySqlDbType.DateTime), record.Time);

		using MySqlDataReader reader = _database.Execute("INSERT record VALUES (@id, @distance, @time)", idParam, distanceParam, timeParam);
		return reader.RecordsAffected > 0;
	}

	public List<Record> GetRecords(int amount = 5) {
		var amountParam = (new MySqlParameter("@amount", MySqlDbType.Int32), amount);
		using MySqlDataReader reader = _database.Execute("SELECT * FROM record ORDER BY distance DESC LIMIT @amount", amountParam);
		var records = new List<Record>();
		while (reader.Read()) {
			var id = reader.GetString("id");
			var distance = reader.GetFloat("distance");
			var time = reader.GetDateTime("created_at");
			records.Add(new Record(id, distance, time));
		}
		return records;
	}
}
