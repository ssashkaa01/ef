using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Logics
    {
        DBSet ctx = new DBSet();

        public IEnumerable<Shop> GetShops()
        {
             return ctx.Shops.Local.ToList();
        }

        public IEnumerable<Worker> GetWorkers()
        {
            return ctx.Workers.Local.ToList();
        }

        public IEnumerable<Product> GetProducts()
        {
            return ctx.Products.Local.ToList();
        }

        public IEnumerable<Director> GetDirectors()
        {
            return ctx.Directors.Local.ToList();
        }

      
    }
}
