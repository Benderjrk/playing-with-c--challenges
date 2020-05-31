using System;
using System.IO;
/* Write a function to compact a string in place */
namespace StringManipulation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("DocuPhase String Manipulation Engineering Code Quiz");
            if (args.Length == 0)
            {
                Console.WriteLine("\nPlease enter a parameter which is a filepath relative to the program.cs file. For example, \"./test.txt\" or \"../../test.txt\"\n");
                return;
            }
            Console.WriteLine("To start program press enter.");
            Console.ReadLine();

            /* Get path to file test.txt to get to the current working directory I had to 
             * go up 3 directories for using visual studios docuPhaseQ1/bin/Debug/netcoreapp3.1/test.txt to access
             * it in debugging mode. In the command line you can use the current directory
             */
            string pathToFile = Path.Combine(Directory.GetCurrentDirectory(), $"{args[0]}");
            string text = "";
            try
            {
                text = System.IO.File.ReadAllText(pathToFile);
            }
            catch(Exception e)
            {
                Console.Write("Enter a parameter which is a filepath relative to the program.cs file. For example, \"./test.txt\" or \"../../test.txt\"\n");
            }
            Console.WriteLine($"The text string you picked was: {text}");

            // Replace whitespace in the string.
            string removedWhitespace = text.Replace(" ", "");
            // Create Final String.
            string finalString = "";

            // Loop through the string to check characters.
            for (int i = 0; i < removedWhitespace.Length; i++)
            {
                // Is the index Before the last one
                if (i < removedWhitespace.Length - 1)
                {
                    // is the value at index the same as the next index value
                    if (removedWhitespace[i] != removedWhitespace[i + 1])
                    {
                        // add the string at index to the end of the finalString
                        finalString += removedWhitespace[i];
                    }
                }
                // It the index the last one
                else if (i == removedWhitespace.Length - 1)
                {
                    // add the final index to the end of the finalString
                    finalString += removedWhitespace[i];
                }
            }
            // Print out the manipulated string
            Console.Write("\n");
            Console.Write(finalString);
            Console.Write("\n");
            return;
        }
    }
}
