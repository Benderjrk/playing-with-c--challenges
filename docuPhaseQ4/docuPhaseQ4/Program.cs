using System;
using System.IO;

namespace MinMaxStack
{
    internal class Stack
    {
        static readonly int _overflowLimit = 500;
        int topNode;
        int[] stack = new int[_overflowLimit];


        public Stack()
        {
            topNode = -1;
        }
        
        internal int Pop()
        {
            if (topNode < 0)
            {
                Console.Write("\nStack Underflow\n");
                return 0;
            }
            else
            {
                int value = stack[topNode--];
                return value;
            }
        }
        internal bool Push(int data)
        {
            if (topNode >= _overflowLimit)
            {
                Console.Write("\nStack Overflow\n");
                return false;
            }
            else
            {   
                // must do ++topNode because topNode++ wont work
                stack[++topNode] = data;
                return true;
            }
        }

        internal void Peek()
        {
            if (topNode < 0)
            {
                Console.Write("\nStack Underflow\n");
                return;
            }
            else
                Console.Write($"\nThe top element of Stack is: {stack[topNode]}\n");
                return;
        }

        internal bool IsEmpty()
        {
            if (topNode < 0)
            {
                Console.WriteLine("\nEmpty Stack\n");
                return true;
            }
            else
            {
                Console.WriteLine($"\nStack has {_overflowLimit - topNode} nodes before overflow\n");
                return false;
            }
        }
        internal void Min()
        {
            int minNumber = 500;
            for (int i = topNode; i >= 0; i--)
            {
                if (minNumber > stack[i])
                {
                    minNumber = stack[i];
                }
            }
            Console.Write($"\nThe lowest number in the stack is: {minNumber}\n");
            return;

        }

        internal void Max()
        {
            int maxNumber = -1;
            for (int i = topNode; i >= 0; i--)
            {
                if (maxNumber < stack[i])
                {
                    maxNumber = stack[i];
                }
            }
            Console.Write($"\nThe highest number in the stack is: {maxNumber}\n");
            return;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "DocuPhase Min/Max Stack Engineering Code Quiz";
            if (args.Length == 0)
            {
                Console.WriteLine("\nPlease enter a parameter which is a filepath relative to the program.cs file. For example, \"./test.txt\" or \"../../test.txt\"\n");
                return;
            }
            Console.WriteLine("\nTo start program press enter.\n");
            Console.ReadLine();

            // Get path to file
            string pathToFile = Path.Combine(Directory.GetCurrentDirectory(), $"{args[0]}");
            
            
            Stack myStack = new Stack();

            myStack.IsEmpty();

            try
            {

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
                        myStack.Push(lineInt);

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read: \nPlease enter a parameter which is a filepath relative to the program.cs file. \nFor example, \"./test.txt\" or \"../../test.txt\"\n");
                return;
            }

            myStack.Peek();
            myStack.Max();
            myStack.Min();
            Console.WriteLine($"\npopped {myStack.Pop()} from Stack\n");
            myStack.IsEmpty();
        }
    }
}