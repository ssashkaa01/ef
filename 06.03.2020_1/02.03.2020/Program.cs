using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02._03._2020
{
    class Program
    {
        static void Main(string[] args)
        {
            ModelBarbers ctx = new ModelBarbers();

            ctx.Positions.Add(new Position()
            {
                 Name = "test"
            });

            ctx.SaveChanges();
        }
    }
}
