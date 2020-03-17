using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TanksLib
{
    public class Tank
    {
        private string Name { get; set; }
        private int Ammunition { get; set; }
        private int Armor { get; set; }
        private int Maneuverability { get; set; }
        static Random rnd = new Random();

        public Tank(string name)
        {
            var rand = new Random(name.GetHashCode());

            Name = name;
            Ammunition = rnd.Next(0, 100);
            Armor = rnd.Next(0, 100);
            Maneuverability = rnd.Next(0, 100);
        }

        public static bool operator ^(Tank t1, Tank t2)
        {
            int count = 0;

            if (t1.Ammunition > t2.Ammunition) count++;
            if (t1.Armor > t2.Armor) count++;
            if (t1.Maneuverability > t2.Maneuverability) count++;

            return count >= 2;
        }

        public override string ToString()
        {
            return $"Name: {Name} Ammunition: {Ammunition} Armor: {Armor} Maneuverability: {Maneuverability}";
        }
    }
}
