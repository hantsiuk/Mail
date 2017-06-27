using System;
using System.IO;

namespace Mail.Database.ScriptRunner
{
	class Program
	{
		static string MailDatabaseName = "Mail";

		static void Main(string[] args)
		{
			while (true)
			{
				string command;

				if (!DatabaseBuilder.CkeckIfDatabaseExists(MailDatabaseName))
				{
					Console.WriteLine(">Create database: yes | no");
					Console.Write(">");

					command = Console.ReadLine();

					switch (command.ToLower())
					{
						case "yes":
							DatabaseBuilder.CreateDatabase(MailDatabaseName);
							break;
						case "no":
							continue;
						default:
							Console.WriteLine("Invalid command");
							continue;
					}
				}
				
				using (var builder = new DatabaseBuilder(MailDatabaseName))
				{
					var scripts = new DirectoryInfo("Scripts").GetFiles();

					Console.WriteLine(">Commands: run | updates");
					Console.Write(">");

					command = Console.ReadLine();

					switch (command.ToLower())
					{
						case "run":
							builder.ExecutSqlQuery(scripts, Console.WriteLine);
							break;
						case "updates":
							builder.CheckUpdates(scripts, Console.WriteLine);
							break;
						default:
							Console.WriteLine("Invalid command");
							break;
					}

					Console.WriteLine();
				}
			}
		}
	}
}
