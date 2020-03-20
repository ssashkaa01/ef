using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _20._03._2020
{
    class Threads
    {

        public ManualResetEvent[] pool = { new ManualResetEvent(false), new ManualResetEvent(false), new ManualResetEvent(false) };
        
        private int countFibonachi { get; set; }
        private int countFactorial { get; set; }
        private int countSimple { get; set; }

        private int iFibonachi { get; set; }
        private int iFactorial { get; set; }
        private int iSimple { get; set; }

        public Threads()
        {
            countFactorial = 0;
            countFibonachi = 0;
            countSimple = 0;
            iFactorial = 0;
            iFibonachi = 0;
            iSimple = 0;
            
        }

        public void Fibonachi()
        {
            decimal n1 = 1;
            decimal n2 = 1;
            decimal tmp;

            try
            {
                while(countFactorial <= 15)
                {
                    pool[0].WaitOne();
                    lock (this)
                    {

                        checked
                        {
                            tmp = n1 + n2;
                            n1 = n2;
                            n2 = tmp;
                        }


                        iFibonachi++;
                        countFibonachi++;


                        if (tmp <= 999999999999)
                        {
                            Console.WriteLine("\t" + tmp);
                        }
                        else
                        {
                            return;
                        }

                        pool[0].Set();
                    }
                }

            }
            catch (OverflowException e)
            {

            }
        }

        public void Factorial()
        {
            int i = 0;
            decimal factorial = 1;

            try
            {
                while (countFactorial <= 15)
                {
                    pool[1].WaitOne();
                    lock (this)
                    {

                        factorial = 1;

                        checked
                        {
                            i++;

                            for (int n = 2; n < i; n++)
                            {
                                factorial *= n;
                            }
                        }


                        iFactorial++;
                        countFactorial++;

                        if (factorial <= 9999999999999)
                        {
                            Console.WriteLine("\t\t" + factorial);
                        }
                        else
                        {
                            return;
                        }

                        pool[1].Set();
                    }
                }

            }
            catch (OverflowException e)
            {

            }
        }

        public void Simple()
        {
            int i = 0;

            try
            {
                while (countFactorial <= 15)
                {
                    pool[2].WaitOne();
                    lock (this)
                    {
                        checked
                        {
                            i++;
                        }


                        iSimple++;
                        countSimple++;


                        if (IsSimple(i))
                        {
                            if (i <= 99)
                            {
                                Console.WriteLine("\t\t\t\t" + i);
                            }
                            else
                            {
                                return;
                            }
                        }

                        pool[2].Set();
                    }
                    
                }

            }
            catch (OverflowException e)
            {

            }
        }

        static bool IsSimple(int i, int c = 2)
        {
            if (Math.Sqrt(i) + 1 < c) return true;
            else if (i % c == 0) return false;
            else return IsSimple(i, ++c);
        }

        public void Result()
        {
            pool[0].Set();
            pool[1].Set();
            pool[2].Set();

            while (countFactorial <= 15)
            {
            
                //Console.WriteLine("Wait");

                ManualResetEvent.WaitAll(pool);

                lock (this)
                {
                    Thread.Sleep(1000);
                   
                    pool[0].Reset();
                    pool[1].Reset();
                    pool[2].Reset();

                    Console.WriteLine($"\t\t\t\t\t\t{countFibonachi} {countFactorial} {countSimple}");

                    countFactorial = 0;
                    countFibonachi = 0;
                    countSimple = 0;
                }
                
            }
        }
    }

    class Program
    {

        static void Main(string[] args)
        {
            Threads fc = new Threads();

            ThreadStart ts1 = new ThreadStart(fc.Fibonachi);
            Thread t1 = new Thread(ts1);
            t1.IsBackground = true;

            ThreadStart ts2 = new ThreadStart(fc.Factorial);
            Thread t2 = new Thread(ts2);
            t2.IsBackground = true;

            ThreadStart ts3 = new ThreadStart(fc.Simple);
            Thread t3 = new Thread(ts3);
            t3.IsBackground = true;

            ThreadStart ts4 = new ThreadStart(fc.Result);
            Thread t4 = new Thread(ts4);
            t4.IsBackground = true;

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();

            Console.ReadKey();
        }
    }
}
