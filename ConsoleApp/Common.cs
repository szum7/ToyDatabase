using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public static class Common
    {
        private static Random random = new Random();

        public static string GetConnectionString(string dbName)
        {
            return $@"Server=(localdb)\MSSQLLocalDB;Database={dbName};Integrated Security=True;";
        }

        public static int GetRandomNumber(int min, int max)
        {
            return random.Next(min, max);
        }

        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static void RunSqlNonQuery(string dbName, string sqlQuery)
        {
            using (var connection = new SqlConnection(Common.GetConnectionString(dbName)))
            {
                connection.Open();

                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    var count = command.ExecuteNonQuery();
                    Console.WriteLine($"Non-query executed, affected rows: {count}");
                }

                connection.Close();
            }
        }

        public static void RunSqlNonQuery(SqlConnection connection, string sqlQuery)
        {
            connection.Open();

            using (var command = new SqlCommand(sqlQuery, connection))
            {
                var count = command.ExecuteNonQuery();
                Console.WriteLine($"Non-query executed, affected rows: {count}");
            }

            connection.Close();
        }

        public static void RunSqlNonQueryNoClose(SqlConnection connection, string sqlQuery)
        {
            using (var command = new SqlCommand(sqlQuery, connection))
            {
                var count = command.ExecuteNonQuery();
                Console.WriteLine($"Non-query executed, affected rows: {count}");
            }
        }
    }
}
