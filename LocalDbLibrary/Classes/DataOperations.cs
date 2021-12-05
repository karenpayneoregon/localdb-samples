using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using LocalDbLibrary.Models;

namespace LocalDbLibrary.Classes
{
    /// <summary>
    /// Code to create a new localDb SQL-Server database which is placed under the
    /// current executable folder, one level done in a folder named Data.
    ///
    /// <see cref="DetachDatabase"/> is only needed if there is a need to recreate the
    /// database during development.
    /// </summary>
    public class DataOperations
    {
        /// <summary>
        /// Folder where database will reside
        /// </summary>
        public const string DB_DIRECTORY = "Data";

        /// <summary>
        /// Name of database
        /// </summary>
        public const string DB_NAME      = "AppData";

        /// <summary>
        /// Connection string for <see cref="DB_NAME"/>
        /// </summary>
        private const string _connectionStringDb = 
            "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=" + DB_NAME + ";Integrated Security=True";

        /// <summary>
        /// Get list of database names
        /// </summary>
        /// <returns></returns>
        public static async Task<List<string>> LocalDatabaseNames()
        {

            List<string> list = new();

            return await Task.Run(async () =>
            {
                
                await using var cn = new SqlConnection(_connectionStringDb);

                await using var cmd = new SqlCommand($"SELECT name FROM sys.databases WHERE Name NOT IN ('master','model','msdb') ORDER BY Name;", cn);

                cn.Open();
                var reader = await cmd.ExecuteReaderAsync();

                while (reader.Read())
                {
                    list.Add(reader.GetString(0));
                }

                return list;

            });
            
        }

        #region Read operations
        /// <summary>
        /// Read Customer data into a <see cref="DataTable"/>
        /// </summary>
        public static DataTable ReadDataTable()
        {
            DataTable table = new DataTable();

            using var cn = new SqlConnection(_connectionStringDb);
            using var cmd = new SqlCommand($"SELECT Identifier, CompanyName, ContactName  FROM Customer", cn);
            
            cn.Open();

            table.Load(cmd.ExecuteReader());
            
            return table;
        }

        /// <summary>
        /// Read <see cref="Customer"/> into a list
        /// </summary>
        public static List<Customer> ReadList()
        {
            List<Customer> list = new();

            using var cn = new SqlConnection(_connectionStringDb);
            using var cmd = new SqlCommand($"SELECT Identifier, CompanyName, ContactName  FROM Customer", cn);

            cn.Open();

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {

                list.Add(new Customer()
                {
                    Identifier = reader.GetInt32(0), 
                    CompanyName = reader.GetString(1), 
                    ContactName = reader.GetString(2)
                });

            }

            return list;
        }
        #endregion

        /// <summary>
        /// Add a few customer records to present
        /// </summary>
        private static void PopulateCustomers()
        {
            List<string> insertList = new()
            {
                "INSERT INTO Customer (CompanyName,ContactName) VALUES ('Alfreds Futterkiste', 'Maria Anders')",
                "INSERT INTO Customer (CompanyName,ContactName) VALUES ('Princesa Isabel Vinhos', 'Isabel de Castro')",
                "INSERT INTO Customer (CompanyName,ContactName) VALUES ('Bottom-Dollar Markets', 'Elizabeth Lincoln')"
            };

            using var cn = new SqlConnection( _connectionStringDb );
            using var cmd = new SqlCommand() { Connection = cn };

            cn.Open();

            foreach (var customer in insertList)
            {
                cmd.CommandText = customer;
                cmd.ExecuteNonQuery();
            }

        }

        /// <summary>
        /// Create database in Data folder under the application folder
        /// ASSUMES Data folder exists
        ///
        /// In the calling project there is a post build event to create the folder if not exists
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> CreateDatabase()
        {

            string connectionStringMaster = $"Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True";

            string dbName = DB_NAME;

            // ReSharper disable once AssignNullToNotNullAttribute
            string outputFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), DB_DIRECTORY);

            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            string mdfFilename = $"{dbName}.mdf";
            string dbFileName = Path.Combine(outputFolder, mdfFilename);
            string logFileName = Path.Combine(outputFolder, $"{dbName}_log.ldf");

            string createTableCommand = @"
            CREATE TABLE [dbo].[Customer](
	            [Identifier] [int] IDENTITY(1,1) NOT NULL,
	            [CompanyName] [nvarchar](255) NULL,
	            [ContactName] [nvarchar](255) NULL,
             CONSTRAINT [Customer$PrimaryKey] PRIMARY KEY CLUSTERED 
            (
	            [Identifier] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY]
            ";

            try
            {

                if (File.Exists(dbFileName))
                {
                    
                    if (await DetachDatabase(dbFileName, logFileName) == false)
                    {
                        return false;
                    }
                }

                
                // ReSharper disable once UseAwaitUsing
                using var cn = new SqlConnection( connectionStringMaster );

                // ReSharper disable once UseAwaitUsing
                using var cmd = new SqlCommand($"CREATE DATABASE {dbName} ON (NAME = N'{dbName}', FILENAME = '{dbFileName}')", cn);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                cn.ConnectionString = _connectionStringDb;
                cmd.CommandText = createTableCommand;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                await Task.Delay(500);

                PopulateCustomers();

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// Drop database and delete physical files
        /// </summary>
        /// <param name="dbName">database name including path</param>
        /// <param name="logFile">log name including path</param>
        /// <returns>success</returns>
        public static async Task<bool> DetachDatabase(string dbName, string logFile)
        {

            return await Task.Run(async () =>
            {
                try
                {
                    string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True";

                    if (File.Exists(dbName))
                    {
                        // ReSharper disable once UseAwaitUsing
                        using var cn = new SqlConnection(connectionString);

                        cn.Open();

                        var cmd = cn.CreateCommand();

                        cmd.CommandText = 
                            $"ALTER DATABASE {Path.GetFileNameWithoutExtension(dbName)}" + 
                            " SET SINGLE_USER WITH ROLLBACK IMMEDIATE";

                        await cmd.ExecuteNonQueryAsync();
                        cmd.CommandText = $"exec sp_detach_db '{Path.GetFileNameWithoutExtension(dbName)}'";
                        await cmd.ExecuteNonQueryAsync();
                        cn.Close();

                        if (File.Exists(dbName))
                        {
                            File.Delete(dbName);
                        }

                        if (File.Exists(logFile))
                        {
                            File.Delete(logFile);
                        }

                        return true;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            });

        }
    }
}