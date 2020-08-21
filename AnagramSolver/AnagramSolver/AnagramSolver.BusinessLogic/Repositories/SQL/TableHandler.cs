using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic.Repositories
{
    public class TableHandler : ITableHandler
    {
        private readonly SqlConnection sqlConnection;

        public TableHandler()
        {
            sqlConnection = new SqlConnection()
            {
                ConnectionString = Settings.ConnectionStringDevelopment
            };
        }
        public async Task ClearSelectedTables(List<string> tables)
        {
            foreach (var table in tables)
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "ClearSelectedTableContent"
                };
                cmd.Parameters.Add(new SqlParameter("@table", table));
                await cmd.ExecuteNonQueryAsync();

                sqlConnection.Close();
            }
        }
    }
}
