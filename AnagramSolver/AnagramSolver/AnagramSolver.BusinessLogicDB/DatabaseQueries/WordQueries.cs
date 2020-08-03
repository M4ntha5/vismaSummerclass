using AnagramSolver.Contracts.Models;
using AnagramSolver.Contracts.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AnagramSolver.BusinessLogicDB.Database
{
    public class WordQueries
    {
        private readonly SqlConnection sqlConnection;

        public WordQueries()
        {
            sqlConnection = new SqlConnection()
            {
                ConnectionString = Settings.ConnectionString
            };
        }

        public void InsertWord(Anagram wordModel)
        {
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.Text,
                CommandText = "insert into Word(Word, Category, Sorted_word) " +
                                "values (@Word, @Category, @Sorted_word)"
            };
            cmd.Parameters.Add(new SqlParameter("@Word", wordModel.Word));
            cmd.Parameters.Add(new SqlParameter("@Category", wordModel.Case));
            cmd.Parameters.Add(new SqlParameter("@Sorted_word", String.Concat(wordModel.Word.OrderBy(x => x))));

            cmd.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public List<Anagram> SelectWordAnagrams(string word)
        {
            var sortedWord = String.Concat(word.OrderBy(x => x));
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.Text,
                CommandText = "select * from Word where Sorted_word = @sortedWord"
            };
            cmd.Parameters.Add(new SqlParameter("@sortedWord", sortedWord));
            SqlDataReader reader = cmd.ExecuteReader();

            List<Anagram> anagrams = new List<Anagram>();
            if(reader.HasRows)
            {
                while(reader.Read())
                {
                    anagrams.Add(
                        new Anagram()
                        {
                            Id = int.Parse(reader["Id"].ToString()),
                            Case = reader["Category"].ToString(),
                            Word = reader["Word"].ToString(),
                            SortedWord = reader["Sorted_word"].ToString(),
                        });
                }
            }

            reader.Close();
            sqlConnection.Close();
            return anagrams;
        }

        public List<Anagram> SelectAllWords()
        {
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.Text,
                CommandText = "select * from Word"
            };
            SqlDataReader reader = cmd.ExecuteReader();

            List<Anagram> anagrams = new List<Anagram>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    anagrams.Add(
                        new Anagram()
                        {
                            Id = int.Parse(reader["Id"].ToString()),
                            Case = reader["Category"].ToString(),
                            Word = reader["Word"].ToString(),
                            SortedWord = reader["Sorted_word"].ToString(),
                        });
                }
            }

            reader.Close();
            sqlConnection.Close();
            return anagrams;
        }

        public string SelectWordById(string id)
        {
            var idToSearch = int.Parse(id);
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.Text,
                CommandText = "select Word from Word where Id = @wordIdToGet"
            };
            cmd.Parameters.Add(new SqlParameter("@wordIdToGet", idToSearch));
            SqlDataReader reader = cmd.ExecuteReader();

            string anagram = null;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    anagram = reader["Word"].ToString();
                    break;
                }
            }

            reader.Close();
            sqlConnection.Close();
            return anagram;
        }

        public bool ClearWordTable()
        {
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "ClearWordTableContent"
            };
            cmd.ExecuteNonQuery();

            sqlConnection.Close();
            return true;
        }

        public List<Anagram> SelectWordsBySearch(string phrase)
        {
            sqlConnection.Open();
            var fixedPhrase = Regex.Replace(phrase, @"([%_\[])", @"[$1]");
            SqlCommand cmd = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.Text,
                CommandText = "select * from Word where Word like @phrase"
            };
            cmd.Parameters.Add(new SqlParameter("@phrase", "%"+ fixedPhrase + "%"));
            SqlDataReader reader = cmd.ExecuteReader();

            List<Anagram> anagrams = new List<Anagram>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    anagrams.Add(
                        new Anagram()
                        {
                            Id = int.Parse(reader["Id"].ToString()),
                            Case = reader["Category"].ToString(),
                            Word = reader["Word"].ToString(),
                            SortedWord = reader["Sorted_word"].ToString(),
                        });
                }
            }

            reader.Close();
            sqlConnection.Close();
            return anagrams;
        }
    }
}
