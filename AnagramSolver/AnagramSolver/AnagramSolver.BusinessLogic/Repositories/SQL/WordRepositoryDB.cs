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
                ConnectionString = Settings.ConnectionString
            };
        }

        public void AddNewWord(Anagram wordModel)
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
            cmd.Parameters.Add(new SqlParameter("@Category", wordModel.Case));
            cmd.Parameters.Add(new SqlParameter("@SortedWord", String.Concat(wordModel.Word.OrderBy(x => x))));

            cmd.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public List<WordEntity> GetSelectedWordAnagrams(string word)
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
            SqlDataReader reader = cmd.ExecuteReader();

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

        public List<WordEntity> GetAllWords()
        {
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.Text,
                CommandText = "select * from Words"
            };
            SqlDataReader reader = cmd.ExecuteReader();

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

        public WordEntity SelectWordById(int id)
        {
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.Text,
                CommandText = "select Word from Words where Id = @wordIdToGet"
            };
            cmd.Parameters.Add(new SqlParameter("@wordIdToGet", id));
            SqlDataReader reader = cmd.ExecuteReader();

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

        public List<WordEntity> SelectWordsBySearch(string phrase)
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
            SqlDataReader reader = cmd.ExecuteReader();

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

        public void UpdateSelectedWord(int id, Anagram updatedWord)
        {
            var sortedWord = String.Concat(updatedWord.Word.OrderBy(x => x));
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.Text,
                CommandText = "update Words set Category=@case, Word=@word, SortedWord=@sorted where ID=@id"
            };
            cmd.Parameters.Add(new SqlParameter("@case", updatedWord.Case));
            cmd.Parameters.Add(new SqlParameter("@word", updatedWord.Word));
            cmd.Parameters.Add(new SqlParameter("@sorted", sortedWord));
            cmd.Parameters.Add(new SqlParameter("@id", id));
            cmd.ExecuteNonQuery();

            sqlConnection.Close();
        }

        public void DeleteSelectedWord(int id)
        {
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.Text,
                CommandText = "delete from Word where id=@id"
            };
            cmd.Parameters.Add(new SqlParameter("@id", id));
            cmd.ExecuteNonQuery();

            sqlConnection.Close();
        }
    }
}
