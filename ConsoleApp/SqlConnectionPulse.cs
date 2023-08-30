using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ConsoleApp
{
    public class SqlConnectionPulse
    {
        public void Run()
        {
            using (var connection = new SqlConnection(Common.GetConnectionString("ToyDb1")))
            {
                connection.Open();

                string sqlQuery = @"
                    SELECT Id, Title 
                    FROM Pulse;";

                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(reader.GetOrdinal("Id"));
                            string name = reader.GetString(reader.GetOrdinal("Title"));

                            Console.WriteLine($"Pulse ID: {id}, Name: {name}");
                        }
                    }
                }
            }
        }
    }
}
