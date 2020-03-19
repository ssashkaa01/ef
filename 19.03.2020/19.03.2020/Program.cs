using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _19._03._2020
{
    class FibonacciCounter
    {
      
         AutoResetEvent[] pool = { new AutoResetEvent(true), new AutoResetEvent(false), new AutoResetEvent(false) };

        private int n1 { get; set; }
        private int n2 { get; set; }
        private int evenCount { get; set; }
       
        public FibonacciCounter()
        {
            n1 = 1;
            n2 = 1;
            evenCount = 0;       
        }

        public void Fibonachi1()
        {
            int tmp;
           
            for (int i = 0; i < 10; i++)
            {
                pool[0].WaitOne();
                lock (this)
                {

                    tmp = n1 + n2;
                    n1 = n2;
                    n2 = tmp;

                    Console.WriteLine("\t" + tmp);
                }
                pool[0].Reset();
                pool[1].Set();
            }
        }

        public void Fibonachi2()
        {
            int tmp;

            for (int i = 0; i < 10; i++)
            {
                pool[1].WaitOne();
                lock (this)
                {

                    tmp = n1 + n2;
                    n1 = n2;
                    n2 = tmp;

                    Console.WriteLine("\t\t" + tmp);
                }
                pool[1].Reset();
                pool[2].Set();
            }
        }

        public void Counter()
        {
            int c = 0;

            for (int i = 0; i < 10; i++)
            {
                pool[2].WaitOne();

                lock (this){
                    
                    c = 0;
                    if (n1 % 2 == 0) c++;
                    if (n2 % 2 == 0) c++;

                    Console.WriteLine("\t\t\t" + c);
                }

                pool[2].Reset();
                pool[0].Set();
                
            }
        }
    }

    class Program
    {

        static void Main(string[] args)
        {
            FibonacciCounter fc = new FibonacciCounter();

            ThreadStart ts1 = new ThreadStart(fc.Fibonachi1);
            Thread t1 = new Thread(ts1);
            t1.IsBackground = true;

            ThreadStart ts2 = new ThreadStart(fc.Fibonachi2);
            Thread t2 = new Thread(ts2);
            t2.IsBackground = true;

            ThreadStart ts3 = new ThreadStart(fc.Counter);
            Thread t3 = new Thread(ts3);
            t3.IsBackground = true;

            t1.Start();
            t2.Start();
            t3.Start();

            Console.ReadKey();
        }
    }
}
