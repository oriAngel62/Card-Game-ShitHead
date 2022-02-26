using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shithead.DatabaseCommunication
{
    public class SQLDatabase : IDisposable
    {
        private SqlConnection _sqlConnection; 

        public SQLDatabase()
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
            _sqlConnection = new SqlConnection(connectionString);
            _sqlConnection.Open();
        }

        public void Dispose()
        {
            if(_sqlConnection?.State == System.Data.ConnectionState.Open)
                _sqlConnection.Close();
        }

        public void CreateDatabase()
        {
            string commandDescription = "CREATE DATABASE ShitheadRecords ON PRIMARY " +
                 "(NAME = ShitheadRecords_Data, " +
                 "FILENAME = 'C:\\ShitheadRecordsData.mdf', " +
                 "SIZE = 2MB, MAXSIZE = 10MB, FILEGROWTH = 10%)" +
                 "LOG ON (NAME = ShitheadRecords_Log, " +
                 "FILENAME = 'C:\\ShitheadRecordsLog.ldf', " +
                 "SIZE = 1MB, " +
                 "MAXSIZE = 5MB, " +
                 "FILEGROWTH = 10%)";

            SqlCommand myCommand = new SqlCommand(commandDescription, _sqlConnection);
            try
            {
                myCommand.ExecuteNonQuery();
                Console.WriteLine("Database Shithead is Created Successfully");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Databse Creation Failed: {ex.ToString()}");
            }
        }
    }
}
