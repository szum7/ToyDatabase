using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp._230830_SelectInBatch
{
    public class RunInBatch
    {
        public void Run()
        {
            int batchSize = 100_000;
            int totalRecords;

            using (var connection = new SqlConnection(Common.GetConnectionString("VisaIssueControl")))
            {
                connection.Open();

                totalRecords = getRowCount(connection);

                RunOnClusteredIndexFieldOrder(connection, totalRecords, batchSize);
                RunOnNoIndexFieldOrder(connection, totalRecords, batchSize);

                connection.Close();
            }
        }

        private int getRowCount(SqlConnection connection)
        {
            var query = @"
                SELECT count(*) AS RecordCount 
                FROM [VISA_TRANSACTIONS]";

            using (var countCommand = new SqlCommand(query, connection))
            {
                using (var reader = countCommand.ExecuteReader())
                {
                    reader.Read();
                    return reader.GetInt32(reader.GetOrdinal("RecordCount"));
                }
            }
        }

        private void RunOnClusteredIndexFieldOrder(SqlConnection connection, int totalRecords, int batchSize)
        {
            var result = 0;
            var watch = new Stopwatch();
            watch.Start();

            for (int offset = 0; offset < totalRecords; offset += batchSize)
            {
                var query = @"
                    SELECT * 
                    FROM [VISA_TRANSACTIONS] 
                    ORDER BY [ID] OFFSET @Offset ROWS FETCH NEXT @BatchSize ROWS ONLY";

                using (var selectCommand = new SqlCommand(query, connection))
                {
                    selectCommand.Parameters.AddWithValue("@Offset", offset);
                    selectCommand.Parameters.AddWithValue("@BatchSize", batchSize);

                    using (var reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result += reader.GetInt32(reader.GetOrdinal("AMOUNT"));
                        }

                        Console.WriteLine($"Batch of {batchSize} executed, offset = {offset}. Current sum: {result}");
                    }
                }
            }

            watch.Stop();

            Console.WriteLine($"\nFinal sum = {result}. Time: {watch.Elapsed.ToString(@"m\:ss\.fff")}\n\n");
        }

        private void RunOnNoIndexFieldOrder(SqlConnection connection, int totalRecords, int batchSize)
        {

            var result = 0;
            var watch = new Stopwatch();
            watch.Start();

            for (int offset = 0; offset < totalRecords; offset += batchSize)
            {
                var query = @"
                    SELECT * 
                    FROM [VISA_TRANSACTIONS] 
                    ORDER BY [ID_NO_INDEX] OFFSET @Offset ROWS FETCH NEXT @BatchSize ROWS ONLY";

                using (var selectCommand = new SqlCommand(query, connection))
                {
                    selectCommand.Parameters.AddWithValue("@Offset", offset);
                    selectCommand.Parameters.AddWithValue("@BatchSize", batchSize);

                    using (var reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result += reader.GetInt32(reader.GetOrdinal("AMOUNT"));
                        }

                        Console.WriteLine($"Batch of {batchSize} executed, offset = {offset}. Current sum: {result}");
                    }
                }
            }

            watch.Stop();

            Console.WriteLine($"\nFinal sum = {result}. Time: {watch.Elapsed.ToString(@"m\:ss\.fff")}\n\n");
        }
    }
}
