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

	public int GetRankCount() {
		using MySqlDataReader reader = _database.Execute("SELECT count(*) FROM record");
		reader.Read();
		var count = reader.GetInt32(0);
		return count;
	}

	public List<Record> GetRanks(int page = 0, int recordsPerPage = 5) {
		var pageParam = (new MySqlParameter("@page", MySqlDbType.Int32), page * recordsPerPage);
		var recordsParam = (new MySqlParameter("@records", MySqlDbType.Int32), recordsPerPage);
		using MySqlDataReader reader = _database.Execute("SELECT * FROM record ORDER BY distance LIMIT @page,@records", pageParam, recordsParam);
		var records = new List<Record>();
		while (reader.Read()) {
			var id = reader.GetString("user_id");
			var distance = reader.GetFloat("distance");
			var time = reader.GetDateTime("created_at");
			records.Add(new Record(id, distance, time));
		}
		return records;
	}
}
