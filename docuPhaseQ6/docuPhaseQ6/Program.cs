using System;
using System.IO;
using System.Data.SqlClient;
using MySql.Data.MySqlClient; 
using System.Collections.Generic;
using System.Configuration;

/* Where the first line of the file has the name of a database table
*  The second line has the columns of the table, and
*  Lines 3 through N have the data lines for that table
*  All data is comma separated, and
*  Results in the following:
*   Once the file is parsed
*     it should create a database table with the proper columns and data types
*     as well as insert the data from the file into those columns.
*  Validation or processing errors MUST NOT cause the program to crash: 
*     you should be able to log an entry into a log file, and continue processing the file.
*/


namespace DataProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Data Processing Engineering Code Quiz";
            if (args.Length == 0)
            {
                Console.WriteLine("\nPlease enter a parameter which is a filepath relative to the program.cs file. For example, \"./test.txt\" or \"../../test.txt\"\n");
                return;
            }
            Console.WriteLine("To start program press enter.");
            Console.ReadLine();

            /* Get path to file test.txt to get to the current working directory I had to 
             * go up 3 directories for using visual studios docuPhaseQ4/bin/Debug/netcoreapp3.1/test.txt to access
             * it in debugging mode. In the command line you can use the current directory
             */

            string pathToFile = Path.Combine(Directory.GetCurrentDirectory(), $"{args[0]}");
            string tableTitle = "";
            string[] columns = {};
            List<string> columnData = new List<string>();
            // To setup the insert query later on will collect the column names in the loop to get column data.
            string insertQueryColumnSetup = "";
            string connectionString = "";

            /*
            * This Try block is to take in that file data and sort them into:
            *   tableTitle
            *   colums 
            *   columnData in a list
            */
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader text_stream = new StreamReader(pathToFile))
                {
                    string line;
                    // Knowing what row of the file we are on
                    int whatRow = 0;
                    // Read and display lines from the file until the end of
                    // the file is reached.
                    while ((line = text_stream.ReadLine()) != null)
                    {   
                        // If first row it is the title
                        if(whatRow == 0)
                        {
                            whatRow++;
                            tableTitle = line;
                        }
                        // If second row it is the column names
                        else if (whatRow == 1)
                        {
                            whatRow++;
                            columns = line.Split(", ");
                        }
                        // All proceeding rows will be collections of data per column
                        else
                        {
                            columnData.Add(line);
                        }
                        
                    }
                }

                Console.Write($"\n Table Name to add: {tableTitle} \n");
                Console.Write($"\n Table Columns to add: [{string.Join(", ", columns)}]\n");
                Console.Write(columns.Length);
                int columnIndex = 0;
                foreach (string element in columnData)
                {
                    Console.Write($"\n Data Column {columnIndex}  to add: {element} \n");
                    columnIndex++;
                }     
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read: \nPlease enter a parameter which is a filepath relative to the program.cs file. For example, \"./test.txt\" or \"../../test.txt\"\n");
                return;
            }
            
            /*
            * This Try block is to setup the MySQL config file from App.config:
            */
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["MyContext"].ConnectionString;
            }
            catch(Exception e)
            {
                ErrorLogging(e);
                Console.Write("connectionString Failing\n\n\n");
                return;
            }

            /*
            * Adding the data from the file to the actual database
            */
            try
            {
                setupAndRunTableTitleColumnQuery(tableTitle, insertQueryColumnSetup, connectionString, columns);
                setupAndRunColumnDataQuery(tableTitle, insertQueryColumnSetup, connectionString, columnData);
            }
            catch (Exception e)
            {
                ErrorLogging(e);
            }

            
        }
        private static void setupAndRunTableTitleColumnQuery(string tableTitle, string insertQueryColumnSetup, string connectionString, string[] columns)
        {
            try
            {
                // Make the query string by looping through the columns to get the exact column amount 
                string tableCreateQueryStart = $"CREATE TABLE `justin-code-exam`.`{tableTitle}` (";
                string tableCreateQueryEnd = "";
                bool isFirstColumn = true;
                foreach (string col in columns)
                {
                    if (isFirstColumn == true)
                    {
                        isFirstColumn = false;
                        string columnQuery = $"`{col}` VARCHAR(20) UNIQUE";
                        tableCreateQueryEnd = $",PRIMARY KEY (`{col}`));";
                        tableCreateQueryStart += columnQuery;

                        // Creating a query list of the columns to use in inserting the data into them later on 
                        string addingToInsertQuery = $"`{col}`";
                        insertQueryColumnSetup += addingToInsertQuery;
                    }
                    else
                    {
                        string columnQuery = $",`{col}` VARCHAR(20) NULL";
                        tableCreateQueryStart += columnQuery;

                        // Creating a query list of the columns to use in inserting the data into them later on 
                        string addingToInsertQuery = $",`{col}`";
                        insertQueryColumnSetup += addingToInsertQuery;
                    }
                }
                string tableCreateQueryFull = tableCreateQueryStart + tableCreateQueryEnd;
                // Run the query to make the table with the columns
                runQueryCommand(tableCreateQueryFull, connectionString);
            }
            catch (Exception e)
            {
                ErrorLogging(e);
            }
        }
        private static void setupAndRunColumnDataQuery(string tableTitle, string insertQueryColumnSetup, string connectionString, List<string> columnData)
        {
            try
            {
                foreach (string items in columnData) // Loop through List with foreach
                {

                    string middleQuery = ") VALUES (";
                    string endQuery = ");";
                    string[] itemsList = items.Split(", ");
                    bool firstInLoop = true;
                    foreach (string item in itemsList) // Loop through List with foreach
                    {
                        if (firstInLoop == true)
                        {
                            firstInLoop = false;
                            middleQuery += $"'{item}'";
                        }
                        else
                        {
                            middleQuery += $",'{item}'";
                        }
                    }
                    string queryTableString = $"INSERT INTO `{tableTitle}`(";
                    string rowQueryFull = queryTableString + insertQueryColumnSetup + middleQuery + endQuery;
                    runQueryCommand(rowQueryFull, connectionString);
                }
            }
            catch (Exception e)
            {
                ErrorLogging(e);
            }
        }
        private static void runQueryCommand(string queryString, string connectionString)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(queryString, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch  (Exception e)
            {
                ErrorLogging(e);
            }
        }
        /*
        * Created a Error log function to send errors to error.txt
        */
        public static void ErrorLogging(Exception ex)
        {
            try
            {
                string errorFile = Path.Combine(Directory.GetCurrentDirectory(), "./error.txt");
                if (!File.Exists(errorFile))
                {
                    File.Create(errorFile).Dispose();
                }
                using (StreamWriter sw = File.AppendText(errorFile))
                {
                    sw.WriteLine("=============Error Logging ===========");
                    sw.WriteLine("Start: " + DateTime.Now);
                    sw.WriteLine("Error Message: " + ex.Message);
                    sw.WriteLine("Stack Trace: " + ex.StackTrace);
                    sw.WriteLine("===========End============= " + DateTime.Now);

                }
                Console.Write("\nError logged to error.txt\n");
            }
            catch  (Exception e)
            {
                Console.Write(e.Message);
            }
        }
    }
}
