using System;
using System.Threading;
using System.Collections.Generic;

namespace MyApp {

    class Program {
        private static int maxAccounts = 10;
        private static int threadExecutionTime = 500;
        private static List<string> ownerNames = new List<string> {
            "John", "Alice", "Bob", "Emma", "David", "Sophia", "Michael", "Olivia", "James", "Liam",
            "Noah", "Isabella", "William", "Mia", "Benjamin", "Charlotte", "Lucas", "Amelia", "Henry", "Evelyn",
            "Alexander", "Harper", "Elijah", "Abigail", "Daniel", "Ella", "Matthew", "Scarlett", "Aiden", "Grace",
            "Samuel", "Chloe", "Joseph", "Victoria", "Sebastian", "Riley", "Jack", "Aria", "Owen", "Lily",
            "Nathan", "Avery", "Caleb", "Zoey", "Isaac", "Penelope", "Luke", "Layla", "Dylan", "Hannah"
        };

        static void Main(string[] args) {
            Thread mainThread = Thread.CurrentThread;
            mainThread.Name = "Main Thread";

            createAccounts();

        }

        //creates accounts and starts a thread for each of their non-static methods (deposit and withdraw) 
        static void createAccounts() {
            Random rand = new Random();
            for (int i = 0; i < 30; i++) {
                int balance = rand.Next(3000, 10000); //randomly creates amount
                BankAccount newAccount = new BankAccount(ownerNames[i], balance);
                int depositAmount = rand.Next(50, 100);
                int withdrawalAmount = rand.Next(200, 400);
                Thread depositThread = new Thread(() => newAccount.deposit(depositAmount));
                Thread withdrawThread = new Thread(() => newAccount.withdraw(withdrawalAmount));

                depositThread.Start();
                withdrawThread.Start();
            }
        }
    }

    class BankAccount {
        private string owner;
        private int balance;
        private static int threadExecutionTime = 100;
        private static int numberOfAccounts = 0;
        private int accountNumber;
        

        public BankAccount(string owner, int balance) {
            this.owner = owner;
            this.balance = balance;
            numberOfAccounts++;
            accountNumber = numberOfAccounts;
            // Console.WriteLine(owner + " has account number " + accountNumber);
        }

        public void deposit(int amount) {
            for (int i = 0; i < 10; i++) {
                balance += amount;
                BankBalanceMutexLock.add(amount);
                // Console.WriteLine(owner + " has deposited " + "< " + amount + " >" + " and now has " + " < " + balance + " >");
                Thread.Sleep(threadExecutionTime);
            }
        }

        public void withdraw(int amount) {
            for (int i = 0; i < 10; i++) {
                //top thread
                if (amount > balance) {
                    balance -= amount;
                    BankBalanceMutexLock.add(-amount);
                    // Console.WriteLine(owner + " has gone BANKRUPT and now has " + balance);
                    return;
                }

                balance -= amount;
                BankBalanceMutexLock.add(-amount);
                // Console.WriteLine(owner + " has withdrawn " + "< " + amount + " >" + " and now has " + " < " + balance + " >");
                Thread.Sleep(threadExecutionTime);
            }
        }

        /**deadlock example: if accounts 1 and 2 try transferring to each other at the same time, they will cause a deadlock. They will both lock their own accounts 
            so that they are not modified elsewhere to prevent race conditions. This will cause them to wait for each other indefinitely, causing deadlock
        **/ 
        public static void BadDeadlockExample(BankAccount source, BankAccount destination, int amount) {
            lock(source) { //lock the source so that its amount is not modified somewhere else at the same time (causing a race condition)
                lock (destination) {
                    if (source.getBalance() > amount) { //lock the dest. for the same reason stated above
                        source.setBalance(source.balance - amount);
                        destination.setBalance(destination.balance + amount); //transferring won't occur if source and dest try transferring at the same time!!
                    }
                }
            }
        }  

        /**
            this method improves upon the bad deadlock example above, since it locks in a predetermined order, and the source and destination don't lock at the same time,
                causing them to wait on each other. Instead, the account with a smaller account number is locked first, then the other account is locked, preventing
                deadlock.

                Furthermore, to implement the timeout mechanism, Monitor usage has been used instead of lock which is. According to ChatGPT, the lock mechanism is 
                syntaxic sugar. It looks nicer, but it doesn't have the full range of functionality since it is abstracted  
        **/

        public static void GoodDeadlockExample(BankAccount source, BankAccount destination, int amount) {
            int timeoutDuration = 5;
            
            //determines order in which source and dest are locked based off their account number 
            BankAccount first = source;
            BankAccount second = destination;

            if (source.accountNumber > destination.accountNumber) {
                first = destination;
                second = source;
            }

            //attempts to lock by entering, but it will timeout after x seconds
            if (Monitor.TryEnter(first, TimeSpan.FromSeconds(timeoutDuration))) {
                try {
                    // Attempt to lock the second account with a timeout
                    if (Monitor.TryEnter(second, TimeSpan.FromSeconds(timeoutDuration))) {
                        try {
                            if (source.getBalance() >= amount) {
                                source.setBalance(source.balance - amount);
                                destination.setBalance(destination.balance + amount);
                            }
                        } finally {
                            Monitor.Exit(second);  // Always release the second lock
                        }
                    } else {
                        // Handle the case where the second lock could not be acquired within the timeout
                        Console.WriteLine("Failed to acquire lock on second account within timeout.");
                    }
                } finally {
                    Monitor.Exit(first);  // Always release the first lock
                }
            } else {
                // Handle the case where the first lock could not be acquired within the timeout
                Console.WriteLine("Failed to acquire lock on first account within timeout.");
            }
        }

 
        public int getBalance() {
            return this.balance;
        }

        public void setBalance(int amount) {
            this.balance = amount;
        }
    }
}