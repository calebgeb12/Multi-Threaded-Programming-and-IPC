using System;
using System.Threading;

namespace MyFirstProgram;

class Program {
    static void Main(string[] args) {
        Thread mainThread = Thread.currentThread;
        mainThread.Name = "Main Thread";
        Console.WriteLine(mainThread.Name + " is complete")

        CountDown(10);
        CountUp(10);

    }

    public static void CountDown(int n) {
        for (int i = n; i >= 0; i++) {
            Console.WriteLine("timer 1: " + i);
        }

        Console.WriteLine("testing");
    }

    public static void CountDown(int n) {
        for (int i = 0; i < n; i++) {
            Console.WriteLine("timer 2: " + i);
        }
    }
}