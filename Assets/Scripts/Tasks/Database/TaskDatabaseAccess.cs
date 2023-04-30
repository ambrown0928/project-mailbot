using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

namespace Tasks.Database
{
    public static class TaskDatabaseAccess
    {
        public static IDbConnection CreateAndOpenDatabase()
        {
            string dbUri = "URI=file:TaskLog.sqlite";
            IDbConnection dbConnection = new SqliteConnection(dbUri);
            dbConnection.Open();

            IDbCommand dbCommandCreateTasks = dbConnection.CreateCommand();
            dbCommandCreateTasks.CommandText = "CREATE TABLE IF NOT EXISTS Tasks (id INTEGER PRIMARY KEY UNIQUE, taskName TEXT)";
            dbCommandCreateTasks.ExecuteReader();

            return dbConnection;
        }
    }
}
