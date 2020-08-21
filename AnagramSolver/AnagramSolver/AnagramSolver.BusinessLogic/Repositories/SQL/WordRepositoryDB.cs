using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.Contracts.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic.Repositories
{
    public class WordRepositoryDB : IWordRepository, IAdditionalWordRepository
    {
        private readonly SqlConnection sqlConnection;

        public WordRepositoryDB()
        {
            sqlConnection = new SqlConnection()
            {
                ConnectionString = Settings.ConnectionStringDevelopment
            };
        }

        public async Task AddNewWord(Anagram wordModel)
        {
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.Text,
                CommandText = "insert into Words(Word, Category, SortedWord) " +
                                "values (@Word, @Category, @SortedWord)"
            };
            cmd.Parameters.Add(new SqlParameter("@Word", wordModel.Word));
            cmd.Parameters.Add(new SqlParameter("@Category", wordModel.Category));
            cmd.Parameters.Add(new SqlParameter("@SortedWord", String.Concat(wordModel.Word.OrderBy(x => x))));

            await cmd.ExecuteNonQueryAsync();
            sqlConnection.Close();
        }

        public async Task<List<WordEntity>> GetSelectedWordAnagrams(string word)
        {
            var sortedWord = String.Concat(word.OrderBy(x => x));
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.Text,
                CommandText = "select * from Words where SortedWord = @sortedWord"
            };
            cmd.Parameters.Add(new SqlParameter("@sortedWord", sortedWord));
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            List<WordEntity> anagrams = new List<WordEntity>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    anagrams.Add(
                        new WordEntity()
                        {
                            ID = int.Parse(reader["Id"].ToString()),
                            Category = reader["Category"].ToString(),
                            Word = reader["Word"].ToString(),
                            SortedWord = reader["SortedWord"].ToString()
                        });
                }
            }

            reader.Close();
            sqlConnection.Close();
            return anagrams;
        }

        public async Task<List<WordEntity>> GetAllWords()
        {
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.Text,
                CommandText = "select * from Words"
            };
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            List<WordEntity> anagrams = new List<WordEntity>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    anagrams.Add(
                        new WordEntity()
                        {
                            ID = int.Parse(reader["Id"].ToString()),
                            Category = reader["Category"].ToString(),
                            Word = reader["Word"].ToString(),
                            SortedWord = reader["SortedWord"].ToString(),
                        });
                }
            }

            reader.Close();
            sqlConnection.Close();
            return anagrams;
        }

        public async ValueTask<WordEntity> SelectWordById(int id)
        {
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.Text,
                CommandText = "select Word from Words where Id = @wordIdToGet"
            };
            cmd.Parameters.Add(new SqlParameter("@wordIdToGet", id));
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            WordEntity anagram =new WordEntity();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    anagram = new WordEntity
                    {
                        Category = reader["Category"].ToString(),
                        Word = reader["Word"].ToString(),
                        SortedWord = reader["SortedWord"].ToString(),
                        ID = int.Parse(reader["ID"].ToString()),
                    };
                    break;
                }
            }

            reader.Close();
            sqlConnection.Close();
            return anagram;
        }

        public async Task<List<WordEntity>> SelectWordsBySearch(string phrase)
        {
            sqlConnection.Open();
            var fixedPhrase = Regex.Replace(phrase, @"([%_\[])", @"[$1]");
            SqlCommand cmd = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.Text,
                CommandText = "select * from Words where Word like @phrase"
            };
            cmd.Parameters.Add(new SqlParameter("@phrase", "%" + fixedPhrase + "%"));
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            List<WordEntity> anagrams = new List<WordEntity>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    anagrams.Add(
                        new WordEntity()
                        {
                            ID = int.Parse(reader["Id"].ToString()),
                            Category = reader["Category"].ToString(),
                            Word = reader["Word"].ToString(),
                            SortedWord = reader["SortedWord"].ToString(),
                        });
                }
            }

            reader.Close();
            sqlConnection.Close();
            return anagrams;
        }

        public async Task UpdateSelectedWord(int id, Anagram updatedWord)
        {
            var sortedWord = String.Concat(updatedWord.Word.OrderBy(x => x));
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.Text,
                CommandText = "update Words set Category=@case, Word=@word, SortedWord=@sorted where ID=@id"
            };
            cmd.Parameters.Add(new SqlParameter("@case", updatedWord.Category));
            cmd.Parameters.Add(new SqlParameter("@word", updatedWord.Word));
            cmd.Parameters.Add(new SqlParameter("@sorted", sortedWord));
            cmd.Parameters.Add(new SqlParameter("@id", id));

            await cmd.ExecuteNonQueryAsync();
            sqlConnection.Close();
        }

        public async Task DeleteSelectedWord(int id)
        {
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.Text,
                CommandText = "delete from Word where id=@id"
            };
            cmd.Parameters.Add(new SqlParameter("@id", id));
            await cmd.ExecuteNonQueryAsync();

            sqlConnection.Close();
        }
    }
}
