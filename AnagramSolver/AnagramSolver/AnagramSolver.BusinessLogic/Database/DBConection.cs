using AnagramSolver.Contracts.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace AnagramSolver.BusinessLogic
{
    public class DBConection
    {
        private const string ConnectionString = "Data Source=.;Initial Catalog=AnagramSolver;Integrated Security=True";
        private readonly SqlConnection sqlConnection = new SqlConnection
        {
            ConnectionString = ConnectionString
        };

        public void InsertWord(WordModel wordModel)
        {
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand
            {
                Connection = sqlConnection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "InsertWord"
            };
            cmd.Parameters.Add(new SqlParameter("@Word", wordModel.Word));
            cmd.Parameters.Add(new SqlParameter("@Category", wordModel.Category));
            cmd.Parameters.Add(new SqlParameter("@Sorted_word", wordModel.Category));
            cmd.ExecuteNonQuery();
            sqlConnection.Close();
        }
    }
}
