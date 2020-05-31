using System;
using System.IO;
using System.Collections.Generic;
/* Write a function that takes an array of numbers as input, and outputs an array of 
*  numbers where there are no duplicates, and
*  the original order of the numbers is preserved.
*/
namespace StringManipulation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("DocuPhase Ordered Number Sequence Engineering Code Quiz");
            if (args.Length == 0)
            {
                Console.WriteLine("\nPlease enter a parameter which is a filepath relative to the program.cs file. For example, \"./test.txt\" or \"../../test.txt\"\n");
                return;
            }
            Console.WriteLine("To start program press enter.");
            Console.ReadLine();
            /* Get path to file test.txt to get to the current working directory I had to 
            *  go up 3 directories for using visual studios docuPhaseQ4/bin/Debug/netcoreapp3.1/test.txt to access
            *  it in debugging mode. In the command line you can use the current directory
            */
            string pathToFile = Path.Combine(Directory.GetCurrentDirectory(), $"{args[0]}");

            try
            {
                // Create a list to add onto
                List<int> numberList = new List<int>();

                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader text_stream = new StreamReader(pathToFile))
                {
                    string line;
                    // Read and display lines from the file until the end of
                    // the file is reached.
                    while ((line = text_stream.ReadLine()) != null)
                    {   
                        // Convert string to int
                        int lineInt = Convert.ToInt32(line);
                        // Get the list to an array to use methods
                        int[] numbersToArray = numberList.ToArray();
                        // Make sure the array isn't empty or go to else block
                        if(numbersToArray.Length != 0) 
                        {   
                            // Check if the int is the same as the last number on the list
                            if(lineInt != numbersToArray[numbersToArray.Length - 1]) {
                                // Add to end of list
                                numberList.Add(lineInt);
                            }
                            
                        }
                        else
                        {
                            // Add to end of list
                            numberList.Add(lineInt);
                        }
                    }
                }
                Console.WriteLine($"Return Array: [{string.Join(", ", numberList.ToArray())}]");
                Console.Write("\n");
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read: \nPlease enter a parameter which is a filepath relative to the program.cs file. For example, \"./test.txt\" or \"../../test.txt\"\n");
            }

        }
    }
}
