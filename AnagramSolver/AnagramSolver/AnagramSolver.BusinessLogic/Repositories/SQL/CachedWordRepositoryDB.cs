using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.Contracts.Utils;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic.Repositories
{
    public class CachedWordRepositoryDB : ICachedWordRepository
    {
        private readonly SqlConnection sqlConnection;

        public CachedWordRepositoryDB()
        {
            sqlConnection = new SqlConnection()
            { 
                ConnectionString = Settings.ConnectionStringDevelopment
            };
        }

        public async Task InsertCachedWord(CachedWord cachedWord)
        {
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.Text,
                CommandText = "insert into CachedWords(Phrase, AnagramsIds) " +
                                "values (@phrase, @anagrams)"
            };
            cmd.Parameters.Add(new SqlParameter("@phrase", cachedWord.SearchPhrase));
            cmd.Parameters.Add(new SqlParameter("@anagrams", cachedWord.AnagramsIds));

            await cmd.ExecuteNonQueryAsync();
            sqlConnection.Close();
        }

        public async Task<CachedWordEntity> GetCachedWord(string phrase)
        {
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.Text,
                CommandText = "select top 1 * from CachedWords " +
                                "where Phrase = @searchPhrase"
            };
            cmd.Parameters.Add(new SqlParameter("@searchPhrase", phrase));

            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            CachedWordEntity result = null;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result = new CachedWordEntity()
                    {
                        ID = int.Parse(reader["ID"].ToString()),
                        Phrase = reader["Phrase"].ToString(),
                        AnagramsIds = reader["AnagramsIds"].ToString()
                    };
                    break;
                }
            }

            reader.Close();
            sqlConnection.Close();
            return result;
        }
    }
}
