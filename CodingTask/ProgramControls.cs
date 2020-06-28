using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;

namespace CodingTask
{
    class ProgramControls
    {

        public static void Start()
        {
            Console.WriteLine("Hello.");
            Console.WriteLine();
            Menu();
        }

        public static void Menu()
        {
            Database db = Database.GetConnection();

            Console.WriteLine("What would you want to do? Write a number");
            while (true)
            {
                // recursive menu. Works until exit options chosen
                Console.WriteLine("1. Add arrays");
                Console.WriteLine("2. Check existing arrays");
                Console.WriteLine("3. Remove all existing arrays");
                Console.WriteLine("4. Exit");

                try
                {
                    int input = int.Parse(Console.ReadLine());
                    switch (input)
                    {
                        case 1:
                            Console.Clear();
                            Program.ReadArrays();
                            Program.SolveNewArrays();
                            Menu();
                            break;

                        case 2:
                            Console.Clear();
                            db.PrintValues();
                            Menu();
                            break;

                        case 3:
                            db.ClearTable();
                            Console.Clear();
                            Console.WriteLine("All arrays deleted");
                            Console.WriteLine();
                            Menu();
                            break;

                        case 4:
                            Environment.Exit(0);
                            break;

                        default:
                            Console.WriteLine("Invalid input. Choose an option from menu");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Choose an option from menu");
                }
            }
        }

    }
}
