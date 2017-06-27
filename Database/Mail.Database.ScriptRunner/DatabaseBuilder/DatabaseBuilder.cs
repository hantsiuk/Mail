using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Mail.Database.ScriptRunner
{
	public class DatabaseBuilder : IDisposable
	{
		private readonly string databaseName;

		private readonly string connectionString;
		
		private readonly SqlConnection connection;

		private const string MasterDatabaseName = "master";

		private readonly static string MasterConnectionString;
		
		static DatabaseBuilder()
		{
			MasterConnectionString = string.Format(ConfigurationManager.ConnectionStrings["Default"].ConnectionString, MasterDatabaseName);
		}

		public DatabaseBuilder(string databaseName)
		{
			this.databaseName = databaseName;
			this.connectionString = string.Format(ConfigurationManager.ConnectionStrings["Default"].ConnectionString, databaseName);
			this.connection = new SqlConnection(connectionString);
			this.connection.Open();
		}
	
		public static bool CkeckIfDatabaseExists(string databaseName)
		{
			using (var masterConnection = new SqlConnection(MasterConnectionString))
			{
				masterConnection.Open();

				var sqlQueryStr = string.Format(@"
					SELECT COUNT(*) FROM [{0}].[sys].[databases] WHERE name='{1}'",
					MasterDatabaseName, databaseName);

				var cmd = new SqlCommand(sqlQueryStr, masterConnection);

				return (int)cmd.ExecuteScalar() > 0;
			}
		}

		public static void CreateDatabase(string databaseName)
		{
			using (var masterConnection = new SqlConnection(MasterConnectionString))
			{
				masterConnection.Open();

				var sqlQueryStr = string.Format(@"CREATE DATABASE {0}", databaseName);
				var cmd = new SqlCommand(sqlQueryStr, masterConnection);

				cmd.ExecuteNonQuery();
			}
		}
	
		public void ExecutSqlQuery(FileInfo[] files, Action<string> logger)
		{
			var scriptHistory = GetScriptHistory();

			foreach (var file in files)
			{
				if (file.Extension == ".sql" && !scriptHistory.Contains(file.Name))
				{
					var sqlQueryStr = File.ReadAllText(file.FullName);

					Server server = new Server(new ServerConnection(connection));
					server.ConnectionContext.ExecuteNonQuery(sqlQueryStr);

					logger(file.Name);
				}
			}
		}

		public void CheckUpdates(FileInfo[] files, Action<string> logger)
		{
			var scriptHistory = GetScriptHistory();

			foreach (var file in files)
			{
				if (file.Extension == ".sql" && !scriptHistory.Contains(file.Name))
				{
					logger(file.Name);
				}
			}
		}

		private IEnumerable<string> GetScriptHistory()
		{
			var scriptHistory = new List<string>();

			var sqlQueryStr = @"
					SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES  
					WHERE TABLE_NAME = N'ScriptHistory'";

			var cmd = new SqlCommand(sqlQueryStr, connection);

			if ((int)cmd.ExecuteScalar() > 0)
			{
				sqlQueryStr = "SELECT ScriptId FROM ScriptHistory";

				cmd = new SqlCommand(sqlQueryStr, connection);

				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						scriptHistory.Add(reader["ScriptId"].ToString());
					}
				}
			}
			else
			{
				CreateSriptHistoryTable();
			}

			return scriptHistory;
		}

		private void CreateSriptHistoryTable()
		{
			var sqlQueryStr = string.Format(@"
					CREATE TABLE [dbo].[ScriptHistory] (
						[ScriptId] [nvarchar] (255) NOT NULL,
						[ExecutionTimeUTC] [datetime] NOT NULL
						CONSTRAINT [PK_ScriptHistory] PRIMARY KEY CLUSTERED ([ScriptId])
					);"
			);

			var cmd = new SqlCommand(sqlQueryStr, connection);

			cmd.ExecuteNonQuery();
		}
		
		public void Dispose()
		{
			connection.Close();
		}
	}
}
