using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace biosim {
    public static class Debug {
        static Debug() {
        }

        public static void PrintWarning(string text) {
#if DEBUG
            ConsoleColor currColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("WARNING: ");
            Console.ForegroundColor = currColor;
            Console.WriteLine(text);
#endif
        }

        public static void PrintError(string text) {
#if DEBUG
            ConsoleColor currColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("ERROR: ");
            Console.ForegroundColor = currColor;
            Console.WriteLine(text);
#endif
        }

        public static void PrintInfo(string text) {
#if DEBUG
            ConsoleColor currColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("INFO: ");
            Console.ForegroundColor = currColor;
            Console.WriteLine(text);
#endif
        }

    }
}
