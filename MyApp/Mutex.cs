using System;
using System.Threading;
using System.Collections.Generic;

namespace MyApp {


    class BankBalanceMutexLock {
        public static volatile int locked = 0; // 0 = unlocked, 1 = locked
        public static int bankBalance = 0;

        //only one bank account can add to the bankbalance at a time
        public static void add(int amount) {
            while (Interlocked.Exchange(ref locked, 1) == 1) 
            {
                // Busy-wait until unlocked
                Thread.Yield(); // Hint to the scheduler to reduce CPU 
            }

            Console.WriteLine("ACQUIRED---");
            int pevAmount = bankBalance;
            bankBalance += amount;
            Console.WriteLine(pevAmount + " + " + amount + " = " + bankBalance);
            release();
        }

        //getting the balance is an exclusive operation
        public static int getBalance() {
            while (Interlocked.Exchange(ref locked, 1) == 1) 
            {
                // Busy-wait until unlocked
                Thread.Yield(); // Hint to the scheduler to reduce CPU 
            }
            
            return bankBalance;
        }

        public static void release() {
            Console.WriteLine("RELEASED");
            Interlocked.Exchange(ref locked, 0);
        }
    } 
}