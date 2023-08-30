using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp.SelectInBatch
{
    public class GenerateInserts
    {
        private void ResetTable(string dbName)
        {
            var clearScript = @"
                DELETE FROM [VISA_TRANSACTIONS]; 
                DBCC CHECKIDENT('[VISA_TRANSACTIONS]', RESEED, 0);";
            Common.RunSqlNonQuery(dbName, clearScript);
        }

        private string CreateRowInsertScript(int count, ref int id, ref int idNoIndex)
        {
            var sb = new StringBuilder();

            sb.Append("SET IDENTITY_INSERT [VISA_TRANSACTIONS] ON;");
            for (int i = 1; i <= count; i++)
            {
                var title = Common.GetRandomString(15);
                var amount = Common.GetRandomNumber(1, 5);

                sb.Append($"INSERT INTO [VISA_TRANSACTIONS] (ID, ID_NO_INDEX, TITLE, AMOUNT) VALUES ({id}, {idNoIndex}, '{title}', {amount});");

                id += Common.GetRandomNumber(1, 3);
                idNoIndex += Common.GetRandomNumber(1, 3);
            }
            sb.Append("SET IDENTITY_INSERT [VISA_TRANSACTIONS] OFF;");

            return sb.ToString();
        }

        /// <summary>
        /// IDs are inserted in sequence. Increment is a random value (not always 1).
        /// ID_NO_INDEXes are inserted the same way as IDs, but they don't always match (mostly don't).
        /// TITLE is random.
        /// AMOUNT is random. Check the sum manually with SQL COUNT().
        /// </summary>
        public void RunV2()
        {
            var dbName = "VisaIssueControl";

            var batchSize = 50_000;
            var maxRowCount = 1_000_000;

            var id = 1;
            var idNoIndex = 1;

            ResetTable(dbName);

            using (var connection = new SqlConnection(Common.GetConnectionString(dbName)))
            {
                connection.Open();

                for (int i = 0; i < maxRowCount / batchSize; i++)
                {
                    var script = CreateRowInsertScript(batchSize, ref id, ref idNoIndex);
                    Common.RunSqlNonQueryNoClose(connection, script);
                }

                connection.Close();
            }
        }

        public void Run()
        {
            var dbName = "VisaIssueControl";

            var firstBatchCount = 100_000;
            var secondBatchCount = 60_000;
            var thirdBatchCount = 80_000;

            ResetTable(dbName);

            var sb = new StringBuilder();
            for (int i = 1; i <= firstBatchCount; i++)
            {
                var idNoIndex = i;
                var title = Common.GetRandomString(4);
                var amount = Common.GetRandomNumber(1, 5);

                sb.Append($"INSERT INTO [VISA_TRANSACTIONS] (ID_NO_INDEX, TITLE, AMOUNT) VALUES ({idNoIndex}, '{title}', {amount});");
            }
            Common.RunSqlNonQuery(dbName, sb.ToString());



            sb = new StringBuilder();
            sb.Append("SET IDENTITY_INSERT [VISA_TRANSACTIONS] ON;");
            for (int i = 1; i <= secondBatchCount; i++)
            {
                var id = firstBatchCount + secondBatchCount + thirdBatchCount + 50 + i;
                var idNoIndex = id;
                var title = Common.GetRandomString(5);
                var amount = Common.GetRandomNumber(1, 5);

                sb.Append($"INSERT INTO [VISA_TRANSACTIONS] (ID, ID_NO_INDEX, TITLE, AMOUNT) VALUES ({id}, {idNoIndex}, '{title}', {amount});");
            }
            sb.Append("SET IDENTITY_INSERT [VISA_TRANSACTIONS] OFF;");
            Common.RunSqlNonQuery(dbName, sb.ToString());



            sb = new StringBuilder();
            sb.Append("SET IDENTITY_INSERT [VISA_TRANSACTIONS] ON;");
            for (int i = 1; i <= thirdBatchCount; i++)
            {
                var id = firstBatchCount + i;
                var idNoIndex = id;
                var title = Common.GetRandomString(6);
                var amount = Common.GetRandomNumber(1, 5);

                sb.Append($"INSERT INTO [VISA_TRANSACTIONS] (ID, ID_NO_INDEX, TITLE, AMOUNT) VALUES ({id}, {idNoIndex}, '{title}', {amount});");
            }
            sb.Append("SET IDENTITY_INSERT [VISA_TRANSACTIONS] OFF;");
            Common.RunSqlNonQuery(dbName, sb.ToString());
        }
    }
}
