using AnagramSolver.Contracts.Models;
using AnagramSolver.Contracts.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace AnagramSolver.BusinessLogicDB.Database
{
    public class CachedWordQueries
    {
        private readonly SqlConnection sqlConnection;

        public CachedWordQueries()
        {
            sqlConnection = new SqlConnection()
            {
                ConnectionString = Settings.ConnectionString
            };
        }

        public void InsertCachedWord(CachedWord cachedWord)
        {
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.Text,
                CommandText = "insert into CachedWord(Phrase, Anagrams_ids) " +
                                "values (@phrase, @anagrams)"
            };
            cmd.Parameters.Add(new SqlParameter("@phrase", cachedWord.SearchPhrase));
            cmd.Parameters.Add(new SqlParameter("@anagrams", cachedWord.Anagrams));

            cmd.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public CachedWord GetCachedWord(string searchWord)
        {
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.Text,
                CommandText = "select top 1 * from CachedWord " +
                                "where phrase = @searchPhrase"
            };
            cmd.Parameters.Add(new SqlParameter("@searchPhrase", searchWord));

            SqlDataReader reader = cmd.ExecuteReader();

            CachedWord result = null;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result = new CachedWord(reader["Phrase"].ToString(), reader["Anagrams_ids"].ToString());
                    break;
                }
            }

            reader.Close();
            sqlConnection.Close();
            return result;
        }
    }
}
