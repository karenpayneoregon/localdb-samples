﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LocalDbLibrary.Classes;

namespace LocalDbLibraryConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Debug.WriteLine("Dropping NorthWind");
            DataOperations.DropNorthWindDatabase();
            Debug.WriteLine("Creating/populating");

            var (result, exception) = await DataOperations.CreateNorthWindDatabase();
            if (result == SqlResult.Success)
            {
                Debug.WriteLine("Finished");
            }else if (result == SqlResult.AlreadyExists)
            {
                Debug.WriteLine("Database already exists");
            }else if (result == SqlResult.Failed)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        /// <summary>
        /// If database exists, read else create and read
        /// </summary>
        /// <returns></returns>
        private static async Task Version1()
        {
            // get names of databases under master
            var databaseNames = await DataOperations.LocalDatabaseNames();
            // check if our database exists
            var databaseExists = databaseNames.Any(name => name == DataOperations.APP_DATA_DB_NAME);

            if (databaseExists)
            {
                ReadCustomers();
            }
            else
            {
                if (await DataOperations.CreateDatabase())
                {
                    ReadCustomers();
                }
                else
                {
                    Debug.WriteLine("Failed");
                }
            }
        }

        /// <summary>
        /// Always create the database then read
        /// </summary>
        /// <returns></returns>
        private static async Task Version2()
        {
            if (await DataOperations.CreateDatabase())
            {
                ReadCustomers();
            }
            else
            {
                Debug.WriteLine("Failed");
            }
        }

        /// <summary>
        /// Read data from Customer table
        /// </summary>
        private static void ReadCustomers()
        {
            var customers = DataOperations.ReadList();

            foreach (var customer in customers)
            {
                Debug.WriteLine($"{customer.Identifier,-4:D2}{customer.CompanyName,-25}{customer.ContactName}");
            }
        }

        [ModuleInitializer]
        public static void Init1()
        {
            Console.Title = "Code sample: create database";
        }
    }
}
