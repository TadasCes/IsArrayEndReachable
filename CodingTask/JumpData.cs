using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTask
{
    class JumpData
    {
        // temps data. Cleared after arrays inserted to database
        public static List<string> newArrays = new List<string>();
        public static int currentIndex = 0;
        public static int currentValue = 0;
        public static int indexAfterJump = 0;
        public static int valueAfterJump = 0;
    }
}
