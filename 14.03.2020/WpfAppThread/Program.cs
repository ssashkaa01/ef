using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfAppThread
{
    class Program
    {
        public struct Params
        {
            public int delay { get; set; }
            public decimal max { get; set; }
        }

        static public void Fibonachi(object parameters)
        {
            Params param = (Params)parameters;

            decimal n1 = 1;
            decimal n2 = 1;
            decimal tmp;

            try
            {
                while (true)
                {
                    checked
                    {
                        tmp = n1 + n2;
                        n1 = n2;
                        n2 = tmp;
                    }
                    Thread.Sleep(param.delay);

                    if (tmp <= param.max)
                    {
                        Console.WriteLine("\t\t\t" + tmp);
                    }
                    else
                    {
                        return;
                    }

                    
                }
                
            }
            catch (OverflowException e)
            {

            }
        }

        static public void Factorial(object parameters)
        {
            Params param = (Params)parameters;

            int i = 0;
            decimal factorial = 1;
          
            try
            {
                while (true)
                {
                    factorial = 1;

                    checked
                    {
                        i++;

                        for(int n = 2; n < i; n++)
                        {
                            factorial *= n;
                        }
                    }
                    Thread.Sleep(param.delay);

                    if (factorial <= param.max)
                    {
                        Console.WriteLine("\t\t" + factorial);
                    }
                    else
                    {
                        return;
                    }
                    
                }

            }
            catch (OverflowException e)
            {

            }
        }

        static public void Simple(object parameters)
        {
            Params param = (Params)parameters;

            int i = 0;
            
            try
            {
                while (true)
                {
                    checked
                    {
                        i++;
                    }

                    if(IsSimple(i))
                    {
                        Thread.Sleep(param.delay);

                        if (i <= param.max)
                        {
                            Console.WriteLine(i);
                        }
                        else
                        {
                            return;
                        }
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


        static void Main(string[] args)
        {
            ParameterizedThreadStart ts1 = new ParameterizedThreadStart(Fibonachi);
            Thread t1 = new Thread(ts1);
            t1.IsBackground = true;

            ParameterizedThreadStart ts2 = new ParameterizedThreadStart(Factorial);
            Thread t2 = new Thread(ts2);
            t2.IsBackground = true;

            ParameterizedThreadStart ts3 = new ParameterizedThreadStart(Simple);
            Thread t3 = new Thread(ts3);
            t3.IsBackground = true;

            t1.Start((object)(new Params { delay = 500, max = 80000000 }));
            t2.Start((object)(new Params { delay = 500, max = 9999999999 }));
            t3.Start((object)(new Params { delay = 500, max = 1000 }));

            ConsoleKeyInfo input;
            do
            {
                input = Console.ReadKey();
               
                switch (input.KeyChar)
                {
                    case '1':
                        t1.Abort();
                        break;

                    case '2':
                        t2.Abort();
                        break;

                    case '3':
                        t3.Abort();
                        break;
                }

            } while (input.Key != ConsoleKey.Escape);

         
        }
    }
}
