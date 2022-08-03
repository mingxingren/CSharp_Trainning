using System;

namespace MultiThread
{
    class Program {
        static void Main(string[] args) {
            MultiThread thd = new MultiThread();
            Task.Run(thd.Run);
            Thread.Sleep(100);
            thd.Wait();
        }
    }

    class MultiThread {
        private readonly Object m_mutex = new Object();

        public MultiThread() {

        }

        public void Run() {
            lock(m_mutex) {
                Console.WriteLine("Thread sleep 3 seconds");
                Thread.Sleep(1000 * 3);
            }
            Console.WriteLine("release mutex");
        }

        public void Wait() {
            Console.WriteLine("Try to get mutex");
            lock(m_mutex) {
                Console.WriteLine("Get mutex!");
            }
        }
    }    
}
