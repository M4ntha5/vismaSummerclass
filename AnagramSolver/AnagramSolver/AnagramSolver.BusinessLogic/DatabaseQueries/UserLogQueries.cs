using AnagramSolver.Contracts.Models;
using AnagramSolver.Contracts.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace AnagramSolver.BusinessLogic.Database
{
    public class UserLogQueries
    {
        private readonly SqlConnection sqlConnection;

        public UserLogQueries()
        {
            sqlConnection = new SqlConnection()
            {
                ConnectionString = Settings.ConnectionString
            };
        }
        public void InsertLog(UserLog log)
        {
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.Text,
                CommandText = "insert into UserLog(Ip, Phrase, Search_time) " +
                                "values (@ip, @phrase, @time)"
            };
            cmd.Parameters.Add(new SqlParameter("@ip", log.Ip));
            cmd.Parameters.Add(new SqlParameter("@phrase", log.SearchPhrase));
            cmd.Parameters.Add(new SqlParameter("@time", log.SearchTime));

            cmd.ExecuteNonQuery();
            sqlConnection.Close();
        }
        public List<UserLog> GetAllLogs()
        {
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.Text,
                CommandText = "select * from UserLog"
            };

            SqlDataReader reader = cmd.ExecuteReader();

            List<UserLog> logs = new List<UserLog>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    logs.Add(
                        new UserLog(
                            reader["Ip"].ToString(),                            
                            reader["Phrase"].ToString(),
                            TimeSpan.Parse(reader["Search_time"].ToString())
                        ));
                }
            }

            reader.Close();
            sqlConnection.Close();
            return logs;
        }


    }
}
