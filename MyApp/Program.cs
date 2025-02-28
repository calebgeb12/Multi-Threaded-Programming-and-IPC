using System;
using System.Threading;

namespace MyFirstProgram;

class Program {
    static void Main(string[] args) {
        Thread mainThread = Thread.CurrentThread;
        mainThread.Name = "Main Thread";
        Console.WriteLine(mainThread.Name + " is complete");

        Thread thread1 = new Thread(CountDown);
        Thread thread2 = new Thread(CountUp);
        thread1.Start();
        thread2.Start();

        CountDown();
        CountUp();
    }

    public static void CountDown() {
        int n = 10;
        for (int i = n; i >= 0; i--) {
            Console.WriteLine("timer 1: " + i);
            Thread.Sleep(100);
        }
    }

    public static void CountUp() {
        int n = 10;
        for (int i = 0; i < n; i++) {
            Console.WriteLine("timer 2: " + i);
                Thread.Sleep(100);
        }
    }
}