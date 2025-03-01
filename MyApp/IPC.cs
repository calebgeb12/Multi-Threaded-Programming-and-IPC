namespace MyApp {
  
    
    class IPC {
        private static List<Consumer> consumerList = new List<Consumer>();
        private static int numberOfConsumers = 5;
        static void Main(string[] args) {
            //only one producer and many consumers
            Producer producer = new Producer();
            Thread sendThread = new Thread(producer.send);
            sendThread.Start();

            //create consumers, adding them to the consumerList at their corresponding indexes (id - 1 b/c id starts at 1)
            for (int i = 0; i < numberOfConsumers; i++) {
                Consumer newConsumer = new Consumer();
                consumerList.Add(newConsumer);

                Thread recieveThread = new Thread(newConsumer.recieve);
                recieveThread.Start();
            }

        }
    }

    class Producer {
        private int sendTime = 500;
        private int minNum = 5;
        private int maxNum = 250;
        public Producer() {

        }

        //tries to acquire critical section (queue) and add data to it
        public void send() {
            Random rand = new Random();
            int num = rand.Next(minNum, maxNum);
            BufferMutexLock.sendData(num);
            Thread.Sleep(sendTime);
            send();
        }
    }

    class Mailbox {        
        /**this 'Mailbox' class doesn't actually serve any purpose. It only exists concetually as a "mailbox" as the actual buffer 
            that contains data is implemented in a BufferMutexLock **/
    }

    class Consumer {
        
        /**
            potential race condition: the id was initially incremented by one whenenver the 'Consumer' constructor was called, where a static varaible called 'numberOfConsumers'
                would increment by one each call and be set as the the current consumer's id, but this would result in a race condition it was accessed concurrently by 
                multiple consumers. As a result, the static variable 'numberOfConsumers' is instead put in a Mutex class that locks, only allowing one variable to access it at a time
        **/

        private int id;
        private List<int> localMemory = new List<int>();
        private int maxMemory = 10;
        private int checkTime = 1000;

        public Consumer() {
            this.id = NumberOfConsumersMutexLock.getId(); //mutex lock implementation called
            NumberOfConsumersMutexLock.release();
        }     

        //request access to the buffer, requires mutex lock (might also need to implement stalling when there is nothing in the buffer)
        //recieve val and add to localMemory
        public void recieve() {
            // Console.WriteLine(this.id);
            if (localMemory.Count < maxMemory) {
                int data = BufferMutexLock.recieveData();
                BufferMutexLock.release();
                if (data != -1) {
                    localMemory.Add(data);
                }
            }

            else {
                printLocalMemory();
                return;
            }

            Thread.Sleep(checkTime);
            recieve();
        }

        public void printLocalMemory() {
            string joinedList = string.Join(", ", localMemory);
            Console.WriteLine("Consumer " + this.id + " localMemory: [" + joinedList + "]");
        } 
    }
    
}