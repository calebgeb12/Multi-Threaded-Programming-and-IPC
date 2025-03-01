using System;
using System.Threading;
using System.Collections.Generic;

namespace MyApp {


    class BankBalanceMutexLock {
        public static volatile int locked = 0; // 0 = unlocked, 1 = locked
        public static int bankBalance = 0;

        //only one bank account can add to the bankbalance at a time
        public static void add(int amount) {
            while (Interlocked.Exchange(ref locked, 1) == 1) {
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
            while (Interlocked.Exchange(ref locked, 1) == 1) {
                Thread.Yield(); 
            }
            
            return bankBalance;
        }

        public static void release() {
            Console.WriteLine("RELEASED");
            Interlocked.Exchange(ref locked, 0);
        }
    } 

    class NumberOfConsumersMutexLock {
        public static volatile int locked = 0; // 0 = unlocked, 1 = locked
        public static int numberOfConsumers = 0;
        
        public static int getId() {
            while (Interlocked.Exchange(ref locked, 1) == 1) {
                Thread.Yield(); 
            }

            // Console.WriteLine("ACQUIRED---");
            numberOfConsumers++;
            return numberOfConsumers;
        }

        public static void release() {
            // Console.WriteLine("RELEASED");
            Interlocked.Exchange(ref locked, 0);
        }
    }

    //implement queue lock here
    class BufferMutexLock {
        public static volatile int locked = 0; // 0 = unlocked, 1 = locked
        private static int maxBuffer = 20;
        private static Queue<int> buffer = new Queue<int>();  

        public static void sendData(int data) {
            while (Interlocked.Exchange(ref locked, 1) == 1 && buffer.Count == maxBuffer) { //sender has to wait until buffer is freed up
                Thread.Yield();
            }

            Console.WriteLine("SENT DATA---");
            buffer.Enqueue(data);
            release();
        }

        public static int recieveData() {
            while (Interlocked.Exchange(ref locked, 1) == 1) {
                Thread.Yield(); 
            }


            //means the buffer is empty, return -1 which acts like "null"
            if (buffer.Count == 0) {
                Console.WriteLine("RECIEVED <BAD> DATA---");
                return -1;
            }

            Console.WriteLine("RECIEVED <GOOD> DATA---");
            return buffer.Dequeue();
        }

        public static void release() {
            // Console.WriteLine("RELEASED");
            Interlocked.Exchange(ref locked, 0);
        }

    }
}