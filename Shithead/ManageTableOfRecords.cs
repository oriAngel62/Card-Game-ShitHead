using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
//not using unit4 anymore
using System.Collections.Generic;
using System.Windows.Forms;
using Shithead.DatabaseCommunication;
using System.Data.SqlClient;

namespace Shithead
{
    public class ManageTableOfRecords
    {
        public string ConnectionString { get; set; }
        public List<PlayerData> PlayerData { get; set; } = new List<PlayerData>();
        //public static TableOfRecords TableOfRecords { get; set; }
        public string Name { get; set; }
        public bool Win { get; set; }

        private SqlConnection connection;
        public ManageTableOfRecords(string name, bool win)
        {
            Name = name;
            Win = win;
            ConnectionString= @"Data Source=(localdb)\ProjectModels;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            CoonnectToDB();
            
        }


        public void CoonnectToDB()
        {
            using (connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                PlayerData = new List<PlayerData>();
                SqlCommand command = new SqlCommand("SELECT Name, Win, Lose, Score FROM Word", connection);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read()) // until we go through all the records
                {
                    PlayerData.Add(new PlayerData((string)dataReader["Name"], (int)dataReader["Win"], (int)dataReader["Lose"], (int)dataReader["Score"]));
                }
            }
                
                if(Name != "")
                {
                    WriteToSQL();
                    ReadDataFromSQL();
                }
            ShowTableOfRecordsForm();
        }
        public void ReadDataFromSQL()
        {
            PlayerData.Clear();
            using (connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                PlayerData = new List<PlayerData>();
                SqlCommand command = new SqlCommand("SELECT Name, Win, Lose, Score FROM Word", connection);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read()) // until we go through all the records
                {
                    PlayerData.Add(new PlayerData((string)dataReader["Name"], (int)dataReader["Win"], (int)dataReader["Lose"], (int)dataReader["Score"]));
                }
            }
        }
        public void ShowTableOfRecordsForm()
        {
            TableOfRecords records = new TableOfRecords();
            var source = new BindingSource();
            source.DataSource = PlayerData;
            records.DataGrid.AutoGenerateColumns = true;
            records.DataGrid.DataSource = source;
            records.Show();
        }

        public bool NameInList(string name)
        {
           bool b = PlayerData.Exists(playerData => playerData.Name.Equals(name));
            return b;
        }

        private void WriteToSQL()
        {
            if (!NameInList(Name))
            {
                InserNewNameToSQL();
            }
            else
            {
                UpdateNameSQL();
            }
        }

        private void UpdateNameSQL()
        {
            using (connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string cmdString = "";
                PlayerData player = PlayerData.Find(playerData => playerData.Name.Contains(Name));
                if (Win)
                {
                    cmdString = $"UPDATE Word SET Win = {player.Win + 1}  Where Name = @name";
                }
                else
                {
                    cmdString = $"UPDATE Word SET Lose = {player.Lose + 1} Where Name = @name";
                }
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = cmdString;
                    command.Parameters.AddWithValue("@name", Name);
                    command.ExecuteNonQuery();
                }
            }
        }


        private void InserNewNameToSQL()
        {
            using (connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string cmdString = "INSERT INTO Word (Name,Win,Lose, Score) VALUES (@val1, @val2, @val3, @val4)";
                int numOfWin = 0;
                int numOfLose = 0;

                if (Win)
                    numOfWin = 1;
                else
                    numOfLose = 1;
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = cmdString;
                    command.Parameters.AddWithValue("@val1", Name);
                    command.Parameters.AddWithValue("@val2", numOfWin);
                    command.Parameters.AddWithValue("@val3", numOfLose);
                    command.Parameters.AddWithValue("@val4", numOfWin - numOfLose);
                }
            }
        }
    }
}
