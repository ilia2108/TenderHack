using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Linq

namespace TenderHack_Back
{
    public class DBConnector
    {
        private const string ConnectionString = "Server=tcp:tenderhack.database.windows.net,1433;Initial Catalog=Tender;Persist Security Info=False;User ID=ryabuily;Password=GosSucks666;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        
        public string GetAll()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var request = "SELECT * FROM KEYS WHERE IS_USED=0";
                var dataAdapter = new SqlDataAdapter(request, connection);

                var command = new SqlCommand(request, connection);
                var dataReader = command.ExecuteReader();
                var key = string.Empty;
                if (dataReader.Read())
                {
                    var id = dataReader.GetValue(0) as Int32?;
                    key = dataReader.GetValue(1) as string;
                    ChangeUsed(id);
                    connection.Close();
                }
                return key==string.Empty? "No valid keys": key;
            }

        }

        public void Add(string key)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                if (key.Contains('\"'))
                    key = key.Replace('\"', '\'');
                else key = '\'' + key + '\'';
                command.CommandText = $"INSERT INTO KEYS(KEY_VALUE, IS_USED) VALUES ({key}, 0)";
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    return;
                }
                
                connection.Close();
            }
        }

        private void ChangeUsed(int? id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = $"UPDATE KEYS SET IS_USED=1 WHERE ID={id}";
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }


    }
}
