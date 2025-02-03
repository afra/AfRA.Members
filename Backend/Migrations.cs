using AFRA.Members.Backend.database;
using AFRA.Members.Backend.database.Tables;
using LinqToDB;
using LinqToDB.Data;

namespace AFRA.Members.Backend;

public static class Migrations {
	private const int DbVer = 2;

	private static readonly List<Migration> _migrations = new() {
		new Migration(1, "update members set founding = replace(founding, 't', '1')"),
		new Migration(1, "update members set founding =  replace(founding, 'f', '0')"),
		new Migration(1, "update members set send_donation_receipt =  replace(send_donation_receipt, 't', '1')"),
		new Migration(1, "update members set send_donation_receipt =  replace(send_donation_receipt, 'f', '0')"),
		new Migration(1, "update members set non_voting =  replace(non_voting, 't', '1')"),
		new Migration(1, "update members set non_voting =  replace(non_voting, 'f', '0')"),
		new Migration(2, "update members set type_of_payment =  replace(type_of_payment, 'Überweisung', 'Bank transfer')"),
		new Migration(2, "update members set type_of_payment =  replace(type_of_payment, 'Bar', 'Cash')"),
	};

	public static void RunMigrations() {
		using var db     = new Database.DbConn();
		var       ccolor = Console.ForegroundColor;

		if (!db.DataProvider.GetSchemaProvider().GetSchema(db).Tables.Any()) {
			Console.ForegroundColor = ConsoleColor.DarkCyan;
			Console.Write("Running migration: ");
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Initialize Database");

			db.CreateTable<DbInfo>();
			db.CreateTable<Member>();

			db.InsertWithIdentity(new DbInfo { DbVer = DbVer });
		}
		else if (db.DataProvider.GetSchemaProvider().GetSchema(db).Tables.All(t => t.TableName != "DbInfo")) {
			db.CreateTable<DbInfo>();
			db.InsertWithIdentity(new DbInfo { DbVer = 0 });
		}

		if(!db.DataProvider.GetSchemaProvider().GetSchema(db).Tables.Any(t => t.TableName == "Memberships")) {
			db.CreateTable<Membership>();
		}

		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine($"Database version: {db.DbInfo.ToList().First().DbVer}");

		var migrationsToRun = _migrations.FindAll(p => p.IntroducedWithDbVer > db.DbInfo.First().DbVer);
		if (migrationsToRun.Count == 0) {
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("No migrations to run.");
		}
		else {
			new Migration(0, "BEGIN TRANSACTION").Run(db);
			try {
				migrationsToRun.ForEach(p => p.Run(db));
			}
			catch {
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.WriteLine($"Migrating to database version {DbVer} failed.");
				new Migration(0, "ROLLBACK TRANSACTION").Run(db);
				Console.ForegroundColor = ConsoleColor.DarkYellow;
				Console.WriteLine("Rolled back migrations.");
				Environment.Exit(1);
			}

			new Migration(0, "COMMIT TRANSACTION").Run(db);

			var newdb  = new Database.DbConn();
			var dbinfo = newdb.DbInfo.First();
			dbinfo.DbVer = DbVer;
			newdb.Update(dbinfo);

			Console.ForegroundColor = ConsoleColor.DarkGreen;
			Console.WriteLine($"Database version is now: {DbVer}");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Finished running migrations.");
		}

		Console.ForegroundColor = ccolor;
	}

	private class Migration {
		private readonly string _sql;
		public readonly  int    IntroducedWithDbVer;

		public Migration(int introducedWithDbVer, string sql) {
			IntroducedWithDbVer = introducedWithDbVer;
			_sql                = sql;
		}

		public void Run(DataConnection db) {
			Console.ForegroundColor = ConsoleColor.DarkCyan;
			Console.Write("Running migration: ");
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine(_sql);
			db.Execute(_sql);
		}
	}
}
