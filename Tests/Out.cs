using System;

namespace Tests; 

internal static class Out {
    public static void Write(bool condition, int counter) {
        if (condition) {
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine($"[✔] Test #{counter.ToString()} passed");
        } else {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"[ ] Test #{counter.ToString()} failed");
        }
    }

    public static void Write(bool condition, int counter, string text) {
        if (condition) {
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine($"[✔] Test #{counter.ToString()} ({text}) passed");
        } else {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"[✔] Test #{counter.ToString()} ({text}) failed");
        }
    }
}