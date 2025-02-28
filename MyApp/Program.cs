using System;
using System.Threading;
using System.Collections.Generic;

namespace MyFirstProgram;

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
            // Thread.Sleep(threadExecutionTime);

            withdrawThread.Start();
            // Thread.Sleep(threadExecutionTime);
        }
    }


}

class BankAccount {
    private string owner;
    private int balance;
    private static int threadExecutionTime = 10;

    public BankAccount(string owner, int balance) {
        this.owner = owner;
        this.balance = balance;
    }

    public void deposit(int amount) {
        for (int i = 0; i < 10; i++) {
            balance += amount;
            // Console.WriteLine(owner + " has deposited " + "< " + amount + " >" + " and now has " + " < " + balance + " >");
            BankBalanceMutexLock.add(amount);
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
            // Console.WriteLine(owner + " has withdrawn " + "< " + amount + " >" + " and now has " + " < " + balance + " >");
            BankBalanceMutexLock.add(-amount);
            Thread.Sleep(threadExecutionTime);
        }
    }

    //mutex lock section

    public int getBalance() {
        return this.balance;
    }
}

class BankBalanceMutexLock
{
    public static volatile int locked = 0; // 0 = unlocked, 1 = locked
    public static int bankBalance = 0;

    //only one bank account can add to the bankbalance at a time
    public static void add(int amount)
    {
        while (Interlocked.Exchange(ref locked, 1) == 1) 
        {
            // Busy-wait until unlocked
            Thread.Yield(); // Hint to the scheduler to reduce CPU 
        }

        Console.WriteLine("ACQUIRED---");
        bankBalance += amount;
        release();
    }

    //getting the balance is an exclusive operation
    public static int getBalance()
    {
        while (Interlocked.Exchange(ref locked, 1) == 1) 
        {
            // Busy-wait until unlocked
            Thread.Yield(); // Hint to the scheduler to reduce CPU 
        }
        
        return bankBalance;
    }

    public static void release()
    {
        Console.WriteLine("RELEASED");
        Interlocked.Exchange(ref locked, 0);
    }
}
