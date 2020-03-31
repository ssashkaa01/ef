using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class DALClass
    {
        private Model _ctx;

        public DALClass()
        {
            _ctx = new Model();
        }

        public IQueryable<Street> GetStreets(int index)
        {
            return _ctx.Streets.Where(s => s.Index == index);
        }

        public void AddStreet(string name, int index)
        {
            _ctx.Streets.Add(new Street() { Index = index, Name = name });
            _ctx.SaveChanges();
        }
    }
}
