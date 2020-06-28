using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace CodingTask
{
    class Program
    {
        static void Main(string[] args)
        {
            // connects to database and inits it
            Database db = Database.GetConnection();
            db.Init();

            // starts the program
            ProgramControls.Start();
        }

        public static void SolveNewArrays()
        {
            Database db = Database.GetConnection();
            foreach (string array in JumpData.newArrays.ToList())
            {
                // checks if array already in database
                // if true, gets IsWinnable value from db
                // and skips unnecessary computations
                int isExisting = db.IsExisting(array);
                if (isExisting == 2)
                {
                    // if array is new, checks if end is reachable
                    // and inserts answer and array into database
                    int isWinnable = IsWinnable(ParseToInt(array));
                    db.Insert(array, isWinnable);

                    Console.WriteLine("Array: " + array);
                    Console.Write("Winnable? ");
                    if (isWinnable == 0) Console.Write("No\n");
                    else Console.Write("Yes\n");

                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Array: " + array);
                    Console.Write("Winnable? ");
                    if (isExisting == 0) Console.Write("No\n");
                    else if (isExisting == 1) Console.Write("Yes\n");

                    Console.WriteLine();
                }
                // clears temporary computation data for new array
                ClearJumpData();
            }
        }

        public static int IsWinnable(int[] array)
        {
            int lastIndex = array.Length - 1;

            // if array only has one element it's already in the end
            if (JumpData.currentIndex == lastIndex)
            {
                return 1;
            }

            while (JumpData.currentIndex != lastIndex)
            {
                JumpData.currentValue = (int)array.GetValue(JumpData.currentIndex);
                JumpData.indexAfterJump = JumpData.currentIndex + JumpData.currentValue;

                // if first element is negative you're stuck
                if ((JumpData.currentValue <= 0) && (JumpData.currentIndex == 0))
                {
                    return 0;
                }
                // if array can go past end it can reach end
                if (JumpData.indexAfterJump > lastIndex)
                {
                    return 1;
                }
                // end is reached
                if (JumpData.indexAfterJump == lastIndex)
                {
                    return 1;
                }

                int tempValue;
                int tempIndex = 0;
                int howManyStepsUntilEnd;
                int minSteps = lastIndex - JumpData.currentIndex;

                // takes subarray from current element to the furthest element you can reach with current value
                // checks which element can furthest
                for (int i = JumpData.indexAfterJump; i != JumpData.currentIndex; i--)
                {
                    tempValue = (int)array.GetValue(i);
                    howManyStepsUntilEnd = (lastIndex - i) - tempValue;
                    if (howManyStepsUntilEnd < minSteps)
                    {
                        minSteps = howManyStepsUntilEnd;
                        tempIndex = i;
                    }
                }

                if (tempIndex == JumpData.currentIndex)
                {
                    return 0;
                }

                // asigns new values
                JumpData.indexAfterJump = tempIndex;
                JumpData.valueAfterJump = (int)array.GetValue(JumpData.indexAfterJump);
                JumpData.currentIndex = JumpData.indexAfterJump;

                // nepersokes nulio nesakyk op
                // if value lower than zero returns 0, because if this values is choosen
                // it means you can't go past it
                if (JumpData.valueAfterJump <= 0)
                {
                    return 0;
                }

                // if array would get stuck in a loop ( 1 -1 ), it would jump back to the start.
                // currentIndex should already have a new value, if it's not, array is in the loop
                if (JumpData.currentIndex == 0)
                {
                    return 0;
                }
            }
            return 0;
        }

        public static void ReadArrays()
        {
            Console.WriteLine("Add arrays in new lines. Use values from 0 to 100.");
            Console.WriteLine("Press 'x' to stop.");
            Console.WriteLine();

            // reads new values until loop broken
            while (true)
            {
                string stringArray = Console.ReadLine().Trim();
                if (stringArray == "x") break;

                try
                {
                    // takes string input array, parses it to int array and puts in temporary list
                    ParseToInt(stringArray);
                    JumpData.newArrays.Add(stringArray);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Array is invalid. Please use only numbers.");
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("One of the elements is either too large or too small. Please try again.");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("One of the elements is either too large or too small. Please try again.");
                }
            }

            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine();

        }

        public static int[] ParseToInt(string array)
        {
            string[] splittedArray = array.Split();
            int[] intArray = new int[splittedArray.Length];

            for (int i = 0; i < splittedArray.Length; i++)
            {
                intArray[i] = int.Parse(splittedArray[i]);
                if (intArray[i] > 100)
                {
                    throw new OverflowException();
                }
            }
            return intArray;
        }

        // for easier debugging and seeing how array jumps through elements
        public static void WriteValues()
        {
            Console.WriteLine("Current index: " + JumpData.currentIndex);
            Console.WriteLine("Current value: " + JumpData.currentValue);
            Console.WriteLine("Index after jump: " + JumpData.indexAfterJump);
            Console.WriteLine("valueAfterJump: " + JumpData.valueAfterJump);
            Console.WriteLine();
        }

        public static void ClearJumpData()
        {
            JumpData.newArrays.Clear();
            JumpData.currentIndex = 0;
            JumpData.currentValue = 0;
            JumpData.indexAfterJump = 0;
            JumpData.valueAfterJump = 0;
        }

    }
}
