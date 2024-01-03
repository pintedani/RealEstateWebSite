using Imobiliare.Entities;
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

        public void Debug(string message,
                    [CallerMemberName] string memberName = "",
                    [CallerFilePath] string filePath = "",
                    [CallerLineNumber] int lineNumber = 0)
        {
            var stackTrace = new StackTrace();
            var frame = stackTrace.GetFrame(1); // Get the immediate caller

            var MemberName = frame.GetMethod().Name;
            //var FilePath = frame.GetFileName();
            //var LineNumber = frame.GetFileLineNumber();

            //log directly to db

            //string connectionString = _configuration.GetConnectionString("DefaultConnection");

            //string sql = "INSERT INTO LogTable (Message, MemberName, FilePath, LineNumber, LogTime) " +
            //         "VALUES (@Message, @MemberName, @FilePath, @LineNumber, @LogTime)";

            //using (var connection = new SqlConnection(_connectionString))
            //{
            //    using (var command = new SqlCommand(sql, connection))
            //    {
            //        command.Parameters.AddWithValue("@Message", message);
            //        command.Parameters.AddWithValue("@MemberName", memberName);
            //        command.Parameters.AddWithValue("@FilePath", filePath);
            //        command.Parameters.AddWithValue("@LineNumber", lineNumber);
            //        command.Parameters.AddWithValue("@LogTime", DateTime.Now);

            //        connection.Open();
            //        command.ExecuteNonQuery();
            //    }
            //}

        }

        public void Error(string error)
        {

        }

        public void Info(string message)
        {

        }


        public void Warn(string message)
        {
            
        }
    }
}
