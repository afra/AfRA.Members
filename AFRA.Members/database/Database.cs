using AFRA.Members.database.Tables;
using LinqToDB;
using LinqToDB.Configuration;
using LinqToDB.Data;

namespace AFRA.Members.database;

public class Database {
	public class ConnectionStringSettings : IConnectionStringSettings {
		public string ConnectionString { get; set; } = null!;
		public string Name             { get; set; } = null!;
		public string ProviderName     { get; set; } = null!;
		public bool   IsGlobal         => false;
	}

	public class Settings : ILinqToDBSettings {
		public IEnumerable<IDataProviderSettings> DataProviders => Enumerable.Empty<IDataProviderSettings>();

		public string DefaultConfiguration => "SQLite";
		public string DefaultDataProvider  => "SQLite";

		public IEnumerable<IConnectionStringSettings> ConnectionStrings {
			get { yield return new ConnectionStringSettings { Name = "db", ProviderName = "SQLite", ConnectionString = @"Data Source=database.db;" }; }
		}
	}

	public class DbConn : DataConnection {
		public DbConn() : base("db") { }
		public ITable<DbInfo> DbInfo => this.GetTable<DbInfo>();
		public ITable<Member>  Members => this.GetTable<Member>();
	}
}
