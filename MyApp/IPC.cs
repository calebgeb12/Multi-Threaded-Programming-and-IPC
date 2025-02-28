namespace MyApp {
  
    
    class IPC {
        private static List<Consumer> consumerList = new List<Consumer>();
        private static int numberOfConsumers = 100;
        static void Main(string[] args) {
            /**
                user inputs to determine...
                    1) number of consumers (always one producer) but up to a max
                    2) number of 

                use user inputs can perform the following...
                    1) print the current queue, specifying which index to print
            **/

            Console.WriteLine("working?");
            //create consumers, adding them to the consumerList at their corresponding indexes (id - 1 b/c id starts at 1)
            for (int i = 0; i < numberOfConsumers; i++) {
                Consumer newConsumer = new Consumer();
                consumerList.Add(newConsumer);

                Thread recieveThread = new Thread(newConsumer.recieve);
            }

        }
    }

    class Producer {

    }

    class Mailbox {        
        //implement a buffer that implements mutex lock, deadlock not needed
        Queue<int> buffer = new Queue<int>();   

        

    }

    class Consumer {
        
        /**
            potential race condition: the id was initially incremented by one whenenver the 'Consumer' constructor was called, where a static varaible called 'numberOfConsumers'
                would increment by one each call and be set as the the current consumer's id, but this would result in a race condition it was accessed concurrently by 
                multiple consumers. As a result, the static variable 'numberOfConsumers' is instead put in a Mutex class that locks, only allowing one variable to access it at a time
        **/
        
        private int id;
        private List<int> localMemory = new List<int>();

        public Consumer() {
            this.id = NumberOfConsumersMutexLock.getId(); //mutex lock implementation called
            NumberOfConsumersMutexLock.release();
        }     

        public void recieve() {
            //request access to the buffer, requires mutex lock (might also need to implement stalling when there is nothing in the buffer)
            //recieve val and add to localMemory
        }

        public void printLocalMemory() {
            string joinedList = string.Join(", ", localMemory);
            Console.WriteLine("Consumer " + this.id + " localMemory: [" + joinedList + "]");
        } 
    }
    
}