using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.Contracts.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic.Repositories
{
    public class UserLogRepositoryDB : IUserLogRepository
    {
        private readonly SqlConnection sqlConnection;

        public UserLogRepositoryDB()
        {
            sqlConnection = new SqlConnection()
            {
                ConnectionString = Settings.ConnectionString
            };
        }

        public async Task InsertLog(UserLog log)
        {
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.Text,
                CommandText = "insert into UserLogs(Ip, Phrase, SearchTime) " +
                                "values (@ip, @phrase, @time)"
            };
            cmd.Parameters.Add(new SqlParameter("@ip", log.Ip));
            cmd.Parameters.Add(new SqlParameter("@phrase", log.SearchPhrase));
            cmd.Parameters.Add(new SqlParameter("@time", log.SearchTime));

            await cmd.ExecuteNonQueryAsync();
            sqlConnection.Close();
        }
        public async Task<List<UserLogEntity>> GetAllLogs()
        {
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.Text,
                CommandText = "select * from UserLogs"
            };

            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            List<UserLogEntity> logs = new List<UserLogEntity>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    logs.Add(
                        new UserLogEntity()
                        {
                            ID = int.Parse(reader["Ip"].ToString()),
                            Ip = reader["Ip"].ToString(),
                            Phrase = reader["Phrase"].ToString(),
                            SearchTime = TimeSpan.Parse(reader["SearchTime"].ToString())
                        });
                }
            }

            reader.Close();
            sqlConnection.Close();
            return logs;
        }

    }
}
