using Crosscutting;
using Imobiliare.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    public class Logger : ILog
    {
        private readonly Type type;
        //private readonly 

        public Logger(Type type)
        {
            this.type = type;
        }

        public void Debug(string message)
        {
            LogToDatabase(message, "DEBUG");
        }

        public void Error(string error)
        {
            LogToDatabase(error, "ERROR");
        }

        public void Info(string message)
        {
            LogToDatabase(message, "INFO");
        }


        public void Warn(string message)
        {
            LogToDatabase(message, "WARN");
        }

        private static void LogToDatabase(string message, string level)
        {
            //TODO: For SQLServer:
            //var stackTrace = new StackTrace();
            //var frame = stackTrace.GetFrame(2);

            //var MemberName = frame.GetMethod().Name;
            ////var FilePath = frame.GetFileName();
            ////var LineNumber = frame.GetFileLineNumber();

            //string sql = "INSERT INTO Log (Date, Level, Logger, Message, Exception) " +
            //         "VALUES (@Date, @Level, @Logger, @Message, @Exception)";

            //using (var connection = new SqlConnection(ConnectionStrings.ConnectionString))
            //{
            //    using (var command = new SqlCommand(sql, connection))
            //    {
            //        command.Parameters.AddWithValue("@Date", DateTime.Now);
            //        command.Parameters.AddWithValue("@Level", level);
            //        command.Parameters.AddWithValue("@Logger", MemberName);
            //        command.Parameters.AddWithValue("@Message", message);
            //        command.Parameters.AddWithValue("@Exception", "");

            //        connection.Open();
            //        command.ExecuteNonQuery();
            //    }
            //}

            //For SQLite:
            var stackTrace = new StackTrace();
            var frame = stackTrace.GetFrame(2);

            var MemberName = frame.GetMethod().Name;
            //var FilePath = frame.GetFileName();
            //var LineNumber = frame.GetFileLineNumber();

            string sql = "INSERT INTO Log (Date, Level, Logger, Message, Exception) " +
                         "VALUES (@Date, @Level, @Logger, @Message, @Exception)";

            using (var connection = new SqliteConnection(ConnectionStrings.ConnectionString))
            {
                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Date", DateTime.Now);
                    command.Parameters.AddWithValue("@Level", level);
                    command.Parameters.AddWithValue("@Logger", MemberName);
                    command.Parameters.AddWithValue("@Message", message);
                    command.Parameters.AddWithValue("@Exception", "");

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
