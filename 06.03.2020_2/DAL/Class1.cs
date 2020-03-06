using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Class1
    {
        public void Test()
        {
            using (DBSet ctx = new DBSet())
            {
                ctx.WorkersTypes.Add(new WorkerType() { Name = "Casir", IsStaticSalary = true });
                ctx.SaveChanges();
            }
        }
    }
}
